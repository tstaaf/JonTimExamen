using JonTimExamen.Models;
using JonTimExamen.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;

namespace JonTimExamen.Controllers
{
    public class VisitorController : Controller
    {
        private WebDbContext db;

        public VisitorController(WebDbContext db)
        {
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
        public IActionResult CheckIn(Visitor visitor)
        {
            
            db.Visitor.Add(visitor);
            visitor.CheckInTime = DateTime.Now;

            byte[] buffer = new byte[5];
            Random r = new Random();
            r.NextBytes(buffer);
            visitor.RandomNumber = BitConverter.ToString(buffer);

            ViewBag.visitor = visitor;

            db.SaveChanges();

            return View("QrView");
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
    }
}
