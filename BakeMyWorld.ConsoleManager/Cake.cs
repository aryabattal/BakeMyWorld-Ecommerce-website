using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BakeMyWorld.ConsoleManager
{
    class Cake
    {
        public Cake()
        {

        }

        public Cake(string name, string description, Uri imageUrl, decimal price)
        {
            Name = name;
            Description = description;
            ImageUrl = imageUrl;
            Price = price;
        }

        public Cake(int id, string name, string description, Uri imageUrl, decimal price)
        {
            Id = id;
            Name = name;
            Description = description;
            ImageUrl = imageUrl;
            Price = price;
        }

        public Cake(string name, string description, Uri imageUrl, decimal price, int categoryId)
        {
            Name = name;
            Description = description;
            ImageUrl = imageUrl;
            Price = price;
            CategoryId = categoryId;
        }

        public Cake(int id, string name, string description, Uri imageUrl, decimal price, int categoryId)
        {
            Id = id;
            Name = name;
            Description = description;
            ImageUrl = imageUrl;
            Price = price;
            CategoryId = categoryId;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Uri ImageUrl { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }

        public override string ToString()
        {
            return $"\n  Name: {this.Name}" +
                   $"\n  Description: {this.Description}" +
                   $"\n  Image Url: {this.ImageUrl}" +
                   $"\n  Price: {this.Price}";
        }

    }
}
