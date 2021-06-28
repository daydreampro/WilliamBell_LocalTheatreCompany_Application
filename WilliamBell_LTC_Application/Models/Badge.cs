using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WilliamBell_LTC_Application.Models
{
    public class Badge
    {
        public int BadgeId { get; set; }
        [Required]
        public string BadgeName { get; set; }
        [Required]
        public string Description { get; set; }
        [Required(ErrorMessage ="Image is required")]
        public string ImageLocation { get; set; }

        public ICollection<User> Users { get; set; }
    }
}