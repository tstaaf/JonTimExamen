using JonTimExamen.Models;
using JonTimExamen.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;

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


            db.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult CheckOut(Visitor visitor)
        {

            string rnum = Request.Form["RandomNumberInput"];
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


            return RedirectToAction("Index");
        }
    }
}