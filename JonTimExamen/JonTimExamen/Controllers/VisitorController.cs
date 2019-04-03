using JonTimExamen.Models;
using JonTimExamen.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using System.IO;
using System.Runtime.CompilerServices;
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

        public string CalculateRandomNumber()
        {
            byte[] buffer = new byte[5];
            Random r = new Random();
            r.NextBytes(buffer);
            var randomNumber = BitConverter.ToString(buffer);

            return randomNumber;
        }

        [HttpPost]
        public IActionResult CheckIn(Visitor visitor)
        {

            db.Visitor.Add(visitor);
            visitor.CheckInTime = DateTime.Now;
            
            visitor.RandomNumber = CalculateRandomNumber();
            bool number = db.Visitor.Any(u => u.RandomNumber == visitor.RandomNumber);

            while(number == true)
            {
                visitor.RandomNumber = string.Empty;
                visitor.RandomNumber = CalculateRandomNumber();
            }

            ViewBag.visitor = visitor;

            db.SaveChanges();

            return View("Capture");
        }

        [HttpPost]
        public IActionResult Capture(Visitor visitor)
        {
            ViewBag.visitor = visitor;

            visitor = db.Visitor.OrderByDescending(d => d.CheckInTime).FirstOrDefault();

            var pictures = HttpContext.Request.Form.Files;

            if (pictures != null)
            {
                foreach (var picture in pictures)
                {
                    if (picture.Length > 0)
                    {
                        var pictureName = picture.FileName;
                        var uniquePictureName = visitor.RandomNumber;
                        var pictureExtension = Path.GetExtension(pictureName);

                        var finalPictureName = string.Concat(uniquePictureName, pictureExtension);
                        var filePath = Path.Combine(_environment.WebRootPath, "visitorPhotos") + $@"\{finalPictureName}";

                        if (!string.IsNullOrEmpty(filePath))
                        {
                            StoreInFolder(picture, filePath);
                        }

                        var imageBytes = System.IO.File.ReadAllBytes(filePath);

                        if (imageBytes != null)
                        {
                            string base64String = Convert.ToBase64String(imageBytes, 0, imageBytes.Length);
                            string imageUrl = string.Concat("data:image/jpg;base64,", base64String);
                            visitor.ImageBase64String = imageUrl;
                        }
                    }
                }
            }

            db.SaveChanges();
            return View("Capture");
        }

        public IActionResult QrView()
        {
            return View();
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

            if (itemToEdit != null && itemToEdit.CheckInTime > itemToEdit.CheckOutTime)
            {
                itemToEdit.CheckOutTime = DateTime.Now;
            }

            else
            {
                ViewBag.error = "Wrong visitor ID";
                return View("Checkout");
            }


            db.SaveChanges();

            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("CurrentVisitors");
            }


            return View("Thx");
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
    }
}
