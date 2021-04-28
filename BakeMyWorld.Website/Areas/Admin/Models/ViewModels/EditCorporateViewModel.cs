using System;
using System.ComponentModel.DataAnnotations;

namespace BakeMyWorld.Website.Areas.Admin.Models.ViewModels
{
    public class EditCorporateViewModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Display(Name = "Image URL")]
        public Uri ImageUrl { get; set; }
    }
}
