using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BakeMyWorld.Website.Data;
using BakeMyWorld.Website.Data.Entities;

namespace BakeMyWorld.Website.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CustomersController : Controller
    {
        private readonly BakeMyWorldContext context;

        public CustomersController(BakeMyWorldContext context)
        {
            this.context = context;
        }

        // GET: Admin/Customers
        public async Task<IActionResult> Index()
        {
            return View(await context.Customers.ToListAsync());
        }

        // GET: Admin/Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await context.Customers
                .Include(c => c.Orders)
                .FirstOrDefaultAsync(m => m.Id == id);

            var order = await context.Orders
                .Include(o => o.OrderLines)
                .FirstOrDefaultAsync(m => m.Customer.Id == id);

            
            var cakeIds = new List<int>();
            foreach (var orderline in order.OrderLines)
            {
                cakeIds.Add(orderline.CakeId);
            }

            var cakes = new List<Cake>();
            foreach (var cakeId in cakeIds)
            {
                var cake = context.Cakes.FirstOrDefault(c => c.Id == cakeId);
                cakes.Add(cake);
            }


            ViewBag.Cakes = cakes;

            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Admin/Customers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await context.Customers
                .FirstOrDefaultAsync(m => m.Id == id);

            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Admin/Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await context.Customers.FindAsync(id);
            context.Customers.Remove(customer);
            await context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
            return context.Customers.Any(e => e.Id == id);
        }
    }
}
