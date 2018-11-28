using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using JonTimExamen.Data;
using JonTimExamen.Models;


namespace WebTentamen.Controllers
{
    public class AccountController : Controller
    {
        private WebDbContext db;
        private UserManager<Employee> userManager;
        private SignInManager<Employee> signInManager;
        private RoleManager<IdentityRole> roleManager;

        public AccountController(WebDbContext db, UserManager<Employee> userManager, SignInManager<Employee> signInManager, RoleManager<IdentityRole> roleManager)
        {
            this.db = db;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }
    }
}
