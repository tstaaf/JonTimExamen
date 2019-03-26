using JonTimExamen.Models;
using JonTimExamen.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace JonTimExamen.Controllers
{
    public class VisitorController : Controller
    {
        private WebDbContext db;
        private readonly IHostingEnvironment _environment;

        public VisitorController(WebDbContext db, IHostingEnvironment hostingEnvironment)
        {
            _environment = hostingEnvironment;
            this.db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CheckOut()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CheckIn(Visitor visitor, string name)
        {

            db.Visitor.Add(visitor);
            visitor.CheckInTime = DateTime.Now;

            byte[] buffer = new byte[5];
            Random r = new Random();
            r.NextBytes(buffer);
            visitor.RandomNumber = BitConverter.ToString(buffer);

            ViewBag.visitor = visitor;


            var pictures = HttpContext.Request.Form.Files;
            if (pictures != null)
            {
                foreach (var picture in pictures)
                {
                    if (picture.Length > 0)
                    {
                        var pictureName = picture.FileName;
                        var uniquePictureName = Convert.ToString(Guid.NewGuid());
                        var pictureExtension = Path.GetExtension(pictureName);

                        var finalPictureName = string.Concat(uniquePictureName, pictureExtension);
                        var filePath = Path.Combine(_environment.WebRootPath, "visitorPhotos") + $@"\{finalPictureName}";

                        if (!string.IsNullOrEmpty(filePath))
                        {
                            StoreInFolder(picture, filePath);
                        }

                        // var pictureBytes = System.IO.File.ReadAllBytes(filePath);
                        //if(pictureBytes != null)
                        //{
                        //    StoreInDatabase(pictureBytes);
                        //}
                    }
                }
            }
            db.SaveChanges();

            return View("QrView");
        }

        private void StoreInFolder(IFormFile picture, string pictureName)
        {
            using (FileStream fs = System.IO.File.Create(pictureName))
            {
                picture.CopyTo(fs);
                fs.Flush();
            }
        }

        [HttpPost]
        public IActionResult CheckOut(Visitor visitor)
        {
            string rnum = Request.Form["RandomNumberInput"];
            ViewData["rnum"] = rnum;
            // var visitorId = db.Visitor.OrderByDescending(v => v.RandomNumber).Select(v => v.RandomNumber).FirstOrDefault();
            var itemToEdit = db.Visitor.SingleOrDefault(x => x.RandomNumber == rnum);

            if (itemToEdit.CheckInTime > itemToEdit.CheckOutTime)
            {
                itemToEdit.CheckOutTime = DateTime.Now;
            }

            else
            {
                return RedirectToAction("Index");
            }


            db.SaveChanges();

            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("CurrentVisitors");
            }


            return RedirectToAction("Index", "Home");
        }
        [Authorize]
        public IActionResult History(Visitor visitor)
        {
            List<Visitor> model = db.Visitor
                .ToList();

            return View(model);
        }


        [Authorize]
        public IActionResult CurrentVisitors(Visitor visitor)
        {
            List<Visitor> model = db.Visitor
            .ToList();

            return View(model);
        }

        public IActionResult Print()
        {
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Capture()
        {
            return View();
        }
    }
}
