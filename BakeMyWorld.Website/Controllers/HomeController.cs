using BakeMyWorld.Website.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace BakeMyWorld.Website.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult VisitUs()
        {
            return View();
        }

        public IActionResult LocationHire()
        {
            return View();
        }

        public IActionResult AboutUs()
        {
            return View();
        }

        public IActionResult OurStory()
        {
            return View();
        }

        public IActionResult Press()
        {
            return View();
        }

        public IActionResult BakingClasses()
        {
            return View();
        }

        public IActionResult PublicClasses()
        {
            return View();
        }

        public IActionResult PrivateClasses()
        {
            return View();
        }
        public IActionResult SpecialClasses()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
