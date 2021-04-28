using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BakeMyWorld.Website.Data;
using BakeMyWorld.Website.Data.Entities;

namespace BakeMyWorld.Website.Controllers
{
    public class CakesController : Controller
    {
        private readonly BakeMyWorldContext context;

        public CakesController(BakeMyWorldContext context)
        {
            this.context = context;
        }

        // GET /cakes/chocolate-cake
        [Route("/cakes/{urlSlug}", Name = "cakedetails")]
        public async Task<IActionResult> Details(string urlSlug)
        {
            if (urlSlug == "")
            {
                return NotFound();
            }

            var cake = await context.Cakes
                .FirstOrDefaultAsync(m => m.UrlSlug == urlSlug);
            if (cake == null)
            {
                return NotFound();
            }

            return View(cake);
        }
    }
}
