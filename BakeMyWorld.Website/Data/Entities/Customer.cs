using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BakeMyWorld.Website.Data.Entities
{
    public class Customer
    {
        public Customer(string firstName, string lastName, string email, string address)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Address = address;
        }

        public int Id { get; protected set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; protected set; }
        
        [Required]
        [MaxLength(50)]
        public string LastName { get; protected set; }

        [Required]
        [MaxLength(50)]
        public string Email { get; protected set; }

        [Required]
        [MaxLength(200)]
        public string Address { get; protected set; }

        public ICollection<Order> Orders { get; set; }
            = new List<Order>();
    }
}
