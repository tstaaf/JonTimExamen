using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using JonTimExamen.Models;
using JonTimExamen.Data;
using Microsoft.AspNetCore.Identity;

namespace JonTimExamen.Controllers
{
    public class HomeController : Controller
    {

        private WebDbContext db;
        private UserManager<Employee> userManager;
        private SignInManager<Employee> signInManager;
        private RoleManager<IdentityRole> roleManager;


        public HomeController(WebDbContext db, UserManager<Employee> userManager,
            SignInManager<Employee> signInManager, RoleManager<IdentityRole> roleManager)
        {
            this.db = db;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;

            db.SaveChanges();
        }


        public async Task<IActionResult> Index()
        {
            Employee employee = await userManager.GetUserAsync(User);
            return View(employee);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
