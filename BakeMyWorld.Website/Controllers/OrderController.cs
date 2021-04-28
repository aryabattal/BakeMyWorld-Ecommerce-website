using BakeMyWorld.Website.Data;
using BakeMyWorld.Website.Data.Entities;
using BakeMyWorld.Website.Extensions;
using BakeMyWorld.Website.Models.Domain;
using BakeMyWorld.Website.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BakeMyWorld.Website.Controllers
{
    
    public class OrderController : Controller
    {
        private readonly BakeMyWorldContext context;

        public OrderController(BakeMyWorldContext context)
        {
            this.context = context;
        }

        [Route("/checkout")]
        public IActionResult Checkout()
        {
            var viewModel = new CheckOutViewModel
            {
                Cart = HttpContext.Session.Get<Cart>("Cart") ?? new Cart()
            };
             
            return View(viewModel);
        }

        [HttpPost]
        [Route ("/checkout", Name="checkout")]
        public IActionResult Checkout(CheckOutViewModel viewModel)
        {
            if(!ModelState.IsValid)
            {
                viewModel.Cart = HttpContext.Session.Get<Cart>("Cart");
                return View(viewModel);
            }

            var customer = new Customer(
                viewModel.FirstName,
                viewModel.LastName,
                viewModel.Email,
                viewModel.Address);

            var cart = HttpContext.Session.Get<Cart>("Cart");

            var order = new Order
            {
                OrderLines = cart
                    .Items
                    .Values.Select(cartItem => new OrderLine(cartItem.Cake.Id, cartItem.Quantity))
                    .ToList()
            };

            customer.Orders.Add(order);
            context.Customers.Add(customer);
            context.SaveChanges();

            HttpContext.Session.Remove("Cart");
                
            return RedirectToAction(nameof(Confirmation));
        }

        public IActionResult Confirmation()
        {
            return View();
        }
    }
}
