using BakeMyWorld.Website.Areas.Admin.Models.ViewModels;
using BakeMyWorld.Website.Data;
using BakeMyWorld.Website.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BakeMyWorld.Website.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CorporatesController : Controller
    {
        private readonly BakeMyWorldContext context;

        public CorporatesController(BakeMyWorldContext context)
        {
            this.context = context;
        }

        //GET: /Admin/Corporates
        public async Task<IActionResult> Index()
        {
            return View(await context.Corporates.ToListAsync());
        }
        // GET: /Admin/Corporates/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var corporate = await context.Corporates
                .Include(m => m.Cakes)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (corporate == null)
            {
                return NotFound();
            }

            return View(corporate);
        }

        // GET: /Admin/Corporates/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/Corporates/Create
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCorporateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var corporate = new Corporate(viewModel.Name, viewModel.ImageUrl);

                context.Add(corporate);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(viewModel);
        }

        // GET: Admin/Corporates/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var corporate = await context.Corporates.FindAsync(id);

            if (corporate == null)
            {
                return NotFound();
            }

            var viewModel = new EditCorporateViewModel
            {
                Id = corporate.Id,
                Name = corporate.Name,
                ImageUrl = corporate.ImageUrl
            };

            return View(viewModel);
        }

        // POST: Admin/Corporates/Edit/5
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditCorporateViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var corporate = new Corporate(
                        viewModel.Id,
                        viewModel.Name,
                        viewModel.ImageUrl);
                try
                {
                    context.Update(corporate);
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CorporateExists(corporate.Id))
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

        // GET: Admin/Corporates/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var corporate = await context.Corporates
                .Include(m => m.Cakes)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (corporate == null)
            {
                return NotFound();
            }

            return View(corporate);
        }

        // POST: Admin/Corporates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var corporate = await context.Corporates.FindAsync(id);
            context.Corporates.Remove(corporate);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CorporateExists(int id)
        {
            return context.Corporates.Any(e => e.Id == id);
        }
    }
}
