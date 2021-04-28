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
    public class CategoriesController : Controller
    {
        private readonly BakeMyWorldContext context;

        public CategoriesController(BakeMyWorldContext context)
        {
            this.context = context;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            return View(await context.Categories.ToListAsync());
        }

        // GET /Categories/sour
        [Route("/categories/{urlSlug}", Name = "categorydetails")]
        public async Task<IActionResult> Details(string urlSlug)
        {
            if (urlSlug == "")
            {
                return NotFound();
            }

            ViewBag.Categories = context.Categories;
            
            var category = await context.Categories
                .Include(m => m.Cakes)
                .FirstOrDefaultAsync(m => m.UrlSlug == urlSlug);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }
    }
}
