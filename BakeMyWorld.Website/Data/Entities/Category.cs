using Highscores.Website.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BakeMyWorld.Website.Data.Entities
{
    public class Category
    {
        public Category(int id, string name, Uri imageUrl)
        {
            Id = id;
            Name = name;
            ImageUrl = imageUrl;
            UrlSlug = name.Slugify();
        }

        public Category(string name, Uri imageUrl)
        {
            Name = name;
            ImageUrl = imageUrl;
            UrlSlug = name.Slugify();
        }

        public int Id { get; protected set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; protected set; }

        public Uri ImageUrl { get; protected set; }

        [Required]
        [MaxLength(50)]
        public string UrlSlug { get; protected set; }

        public ICollection<Cake> Cakes { get; protected set; }
            = new List<Cake>();

    }
}
