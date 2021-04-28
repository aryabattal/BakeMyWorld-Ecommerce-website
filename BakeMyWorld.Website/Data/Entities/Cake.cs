using Highscores.Website.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BakeMyWorld.Website.Data.Entities
{
    public class Cake
    {
        public Cake(string name, string description, Uri imageUrl, decimal price)
        {
            Name = name;
            Description = description;
            ImageUrl = imageUrl;
            Price = price;
            UrlSlug = name.Slugify();
        }

        public Cake(int id, string name, string description, Uri imageUrl, decimal price)
            : this(name, description, imageUrl, price)
        {
            Id = id;
        }

        public Cake(string name, string description, Uri imageUrl, decimal price, string urlSlug)
        {
            Name = name;
            Description = description;
            ImageUrl = imageUrl;
            Price = price;
            UrlSlug = urlSlug;
        }

        public Cake(int id, string name, string description, Uri imageUrl, decimal price, string urlSlug)
            : this(name, description, imageUrl, price, urlSlug)
        {
            Id = id;
        }

        public int Id { get; protected set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public Uri ImageUrl { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Required]
        [MaxLength(50)]
        public string UrlSlug { get; protected set; }

        public ICollection<Category> Categories { get; set; }
            = new List<Category>();

        public ICollection<Corporate> Corporates { get; set; }
        = new List<Corporate>();
    }
}
