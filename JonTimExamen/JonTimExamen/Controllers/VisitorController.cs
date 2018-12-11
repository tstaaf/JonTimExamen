using JonTimExamen.Models;
using JonTimExamen.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Drawing;
using System.Linq;
using Microsoft.AspNetCore.Razor.TagHelpers;
using ZXing.QrCode;
using System.Drawing;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Identity;
using System.IO;
using Microsoft.EntityFrameworkCore;
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

            return View("Index");
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
        [Authorize]
        public IActionResult History(Visitor visitor)
            {
                List<Visitor> model = db.Visitor
                .ToList();

                return View(model);
            }
        }
    }
