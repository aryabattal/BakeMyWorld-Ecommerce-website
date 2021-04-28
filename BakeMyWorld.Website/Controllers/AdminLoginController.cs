using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BakeMyWorld.Website.Controllers
{
    public class AdminLoginController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.AdminName = "Admin";
            ViewBag.Password = "123";
            return View();
        }

        [HttpPost]
        public IActionResult Login(string adminName, string password, string targetAdminName, string targetPassword)
        {
            if(adminName == targetAdminName && password == targetPassword)
            {
                return RedirectToAction("Index", "Categories", new { area = "Admin" });
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
