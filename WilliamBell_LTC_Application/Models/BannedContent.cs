using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WilliamBell_LTC_Application.Models
{
    public class BannedContent
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Please input a word or phrase you would like banned!",AllowEmptyStrings = false)]
        public string Content { get; set; }
    }
}