using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BakeMyWorld.Website.Areas.Admin.Models.ViewModels
{
    public class CreateCorporateViewModel
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Display(Name = "Image URL")]
        public Uri ImageUrl { get; set; }
    }

}

