using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BakeMyWorld.Website.Data;
using BakeMyWorld.Website.Data.Entities;
using BakeMyWorld.Website.Areas.Admin.Models.ViewModels;

namespace BakeMyWorld.Website.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CakesController : Controller
    {
        private readonly BakeMyWorldContext context;

        public CakesController(BakeMyWorldContext context)
        {
            this.context = context;
        }

        // GET: Admin/Cakes
        public async Task<IActionResult> Index()
        {
            return View(await context.Cakes.ToListAsync());
        }

        // GET: Admin/Cakes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cake = await context.Cakes
                .FirstOrDefaultAsync(m => m.Id == id);

            if (cake == null)
            {
                return NotFound();
            }

            return View(cake);
        }

        // GET: Admin/Cakes/Create
        public IActionResult Create()
        {
            // Retrieve categories for dropdown box
            var categories = context.Categories.ToList()
                .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name.ToString() }).ToList();

            ViewBag.Categories = categories;

            return View();
        }

        // POST: Admin/Cakes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCakeViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                // Retrieve associated category (in case of unselection, category id will be set to default value "0")
                string categoryIdString = Request.Form["Categories"];
                bool categoryIdIsValid = int.TryParse(categoryIdString, out int categoryIdParsed);
                int categoryId = categoryIdIsValid ? categoryIdParsed : 0;
                var associatedCategory = context.Categories.Find(categoryId);

                var cake = new Cake(
                    viewModel.Name,
                    viewModel.Description,
                    viewModel.ImageUrl,
                    viewModel.Price
                    );

                if(associatedCategory != null) cake.Categories.Add(associatedCategory);
                
                context.Add(cake);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(viewModel);
        }

        // GET: Admin/Cakes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cake = await context.Cakes.FindAsync(id);

            if (cake == null)
            {
                return NotFound();
            }

            var viewModel = new EditCakeViewModel
            {
                Id = cake.Id,
                Name = cake.Name,
                Description = cake.Description,
                ImageUrl = cake.ImageUrl,
                Price = cake.Price
            };

            // Retrieve categories for dropdown box
            var categories = context.Categories.ToList()
                .Select(c => new SelectListItem { Value = c.Id.ToString(), Text = c.Name.ToString() }).ToList();

            ViewBag.Categories = categories;

            return View(viewModel);
        }

        // POST: Admin/Cakes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditCakeViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                // Retrieve associated category (in case of unselection, category id will be set to default value "1")
                string categoryIdString = Request.Form["Categories"];
                bool categoryIdIsValid = int.TryParse(categoryIdString, out int categoryIdParsed);
                int categoryId = categoryIdIsValid ? categoryIdParsed : 0;
                var associatedCategory = await context.Categories.FindAsync(categoryId);

                // Retrieve located cake and its categories
                var locatedCake = await context.Cakes.Include(c => c.Categories).FirstOrDefaultAsync(c => c.Id == viewModel.Id); 
                var locatedCakeCategories = locatedCake.Categories.ToList();

                locatedCake.Name = viewModel.Name;
                locatedCake.Description = viewModel.Description;
                locatedCake.ImageUrl = viewModel.ImageUrl;
                locatedCake.Price = viewModel.Price;

                // Context multiple tracking problem (context cannot track same ids simultaneously)
                //var cake = new Cake(
                 //   viewModel.Id,
                 //   viewModel.Name,
                 //   viewModel.Description,
                 //   viewModel.ImageUrl,
                 //   viewModel.Price
                 //   );

                if (associatedCategory != null &&
                    !locatedCakeCategories.Contains(associatedCategory))
                {
                    locatedCake.Categories.Add(associatedCategory);
                }
                        
               
                try
                {
                    context.Update(locatedCake);
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CakeExists(locatedCake.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }

        // GET: Admin/Cakes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cake = await context.Cakes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cake == null)
            {
                return NotFound();
            }

            return View(cake);
        }

        // POST: Admin/Cakes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cake = await context.Cakes.FindAsync(id);
            context.Cakes.Remove(cake);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CakeExists(int id)
        {
            return context.Cakes.Any(e => e.Id == id);
        }
    }
}
