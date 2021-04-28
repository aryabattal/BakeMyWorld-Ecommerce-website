using BakeMyWorld.Website.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BakeMyWorld.Website.Models.Domain
{
    public class Cart
    {
        public IDictionary<int, CartItem> Items { get; set; } 
            = new Dictionary<int, CartItem>();

        public void AddCake(Cake cake)
        {
            Items.TryGetValue(cake.Id, out CartItem cartItem);

            if (cartItem == null)
            {
                cartItem = new CartItem { Cake = cake };
                Items.Add(cake.Id, cartItem);
            }

            cartItem.Quantity++;
        }

        public double CalculatePrice()
        {
            var totalPrice = 0.0;

            foreach (var item in Items)
            {
                totalPrice += ((double)item.Value.Cake.Price * item.Value.Quantity);
            }

            return totalPrice;
        }

        public class CartItem
        {
            public Cake Cake { get; set; }
            public int Quantity { get; set; }

            public decimal TotalAmount => Quantity * Cake.Price;
            
        }

        public class Cake
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public Uri ImageUrl { get; set; }
            public decimal Price { get; set; }
        }
    }

    
}
