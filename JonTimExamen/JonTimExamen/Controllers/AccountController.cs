using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using JonTimExamen.Data;
using JonTimExamen.Models;
using JonTimExamen.RequestObjects;


namespace JonTimExamen.Controllers
{
    public class AccountController : Controller
    {
        private WebDbContext db;
        private UserManager<Employee> userManager;
        private SignInManager<Employee> signInManager;
        private RoleManager<IdentityRole> roleManager;


        public AccountController(WebDbContext db, UserManager<Employee> userManager,
            SignInManager<Employee> signInManager, RoleManager<IdentityRole> roleManager)
        {
            this.db = db;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;

            db.SaveChanges();
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestObject request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var result = await signInManager.PasswordSignInAsync(request.Username, request.Password, false, false);

            if (!result.Succeeded)
            {
                return View();
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequestObject request)
        {
            Employee employee = new Employee
            {
                UserName = request.Username,
            };

            IdentityResult result = await userManager.CreateAsync(employee, request.Password);

            if (!result.Succeeded)
            {
                return View();
            }

            return RedirectToAction("Login");
        }

    }
}
