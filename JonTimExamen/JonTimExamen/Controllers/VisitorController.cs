using JonTimExamen.Models;
using JonTimExamen.Data;
using Microsoft.AspNetCore.Mvc;
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

        [HttpPost]
        public IActionResult Create(Visitor visitor)
        {
            db.Visitor.Add(visitor);

            return RedirectToAction("Index");
        }
    }
}