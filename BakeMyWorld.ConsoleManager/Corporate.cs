using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BakeMyWorld.ConsoleManager
{
    class Corporate
    {

        public Corporate(int id, string name)
        {
            Id = id;
            Name = name;
       
        }
        public Corporate(string name, Uri imageUrl)
        {
            Name = name;
            ImageUrl = imageUrl;
        }

        public Corporate(int id, string name, Uri imageUrl)
        {
            Id = id;
            Name = name;
            ImageUrl = imageUrl;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public Uri ImageUrl { get; set; }

        public override string ToString()
        {
            return ($"Name: {this.Name}" +"\n"+ $" Image Url: {this.ImageUrl}");

        }
    }
}
