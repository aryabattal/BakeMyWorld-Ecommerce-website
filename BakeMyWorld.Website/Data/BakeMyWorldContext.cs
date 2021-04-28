using BakeMyWorld.Website.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BakeMyWorld.Website.Data
{
    public class BakeMyWorldContext : DbContext 
    {
        public DbSet<Cake> Cakes { get; set; } 
        public DbSet<Category> Categories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Corporate> Corporates { get; set; }

        public BakeMyWorldContext(DbContextOptions<BakeMyWorldContext> options)
            : base(options)
        {

        }
    }
}
