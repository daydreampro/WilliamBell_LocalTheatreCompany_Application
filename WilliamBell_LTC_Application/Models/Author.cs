using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WilliamBell_LTC_Application.Models
{
    //can also be an editor!
    public class Author : User
    {
        public DateTime? StaffSince { get; set; }
        public string SmallBio { get; set; }

        //navs
        public virtual ICollection<Post> Posts { get; set; }

        
    }
}