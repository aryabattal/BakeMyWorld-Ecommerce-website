using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BakeMyWorld.Website.Data;

namespace BakeMyWorld.Website.Controllers
{
    public class CorporatesController : Controller
    {
        private readonly BakeMyWorldContext context;

        public CorporatesController(BakeMyWorldContext context)
        {
            this.context = context;
        }

        // GET: Corporates
        public async Task<IActionResult> Index()
        {
            return View(await context.Corporates.ToListAsync());
        }

        // GET /Corporates/corporatedetails
        [Route("/corporates/{urlSlug}", Name = "corporatedetails")]
        public async Task<IActionResult> Details(string urlSlug)
        {
            if (urlSlug == "")
            {
                return NotFound();
            }

            ViewBag.Corporates = context.Corporates;

            var corporate = await context.Corporates
                .Include(m => m.Cakes)
                .FirstOrDefaultAsync(m => m.UrlSlug == urlSlug);

            if (corporate == null)
            {
                return NotFound();
            }

            return View(corporate);
        }
    }
}
