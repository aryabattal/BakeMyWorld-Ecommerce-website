using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BakeMyWorld.Website.Data.Entities
{
    public class Order
    {
        public int Id { get; protected set; }
        public Customer Customer { get; set; }

        [Required]
        public DateTime CreatedAt { get; protected set; } = DateTime.Now;

        public ICollection<OrderLine> OrderLines { get; set; }
            = new List<OrderLine>();
    }
}