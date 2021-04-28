using BakeMyWorld.Website.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BakeMyWorld.Website.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SearchResultController : Controller
    {
        private readonly BakeMyWorldContext context;

        public SearchResultController(BakeMyWorldContext context)
        {
            this.context = context;
        }


        // GET: /search?q=cake
        [Route("Admin/search")]
        public IActionResult Index(string q)
        {
            var cakes = context.Cakes.Where(c => c.Name.Contains(q));
            
            return View(cakes);
        }
    }
}
