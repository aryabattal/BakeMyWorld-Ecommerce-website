using BakeMyWorld.Website.Data;
using BakeMyWorld.Website.Extensions;
using BakeMyWorld.Website.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BakeMyWorld.Website.Controllers
{
    public class CartsController : Controller
    {
        private readonly BakeMyWorldContext context;

        public CartsController(BakeMyWorldContext context)
        {
            this.context = context;
        }
        
        public IActionResult Index()
        {
            var cart = HttpContext.Session.Get<Cart>("Cart") ?? new Cart();

            return View(cart);
        }

        [Route("/cart/add", Name = "AddToCart")]
        [HttpPost]
        public IActionResult AddToCart(int cakeId)
        {
            var cake = context.Cakes.FirstOrDefault(c => c.Id == cakeId);

            var cartCake = new Cart.Cake
            {
                Id = cake.Id,
                Name = cake.Name,
                ImageUrl = cake.ImageUrl,
                Price = cake.Price
            };

            var cart = HttpContext.Session.Get<Cart>("Cart") ?? new Cart();
            
            cart.AddCake(cartCake);

            HttpContext.Session.Set("Cart", cart);
            
            return RedirectToAction(nameof(Index));
        }
    }
}
