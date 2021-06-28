using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WilliamBell_LTC_Application.Models
{
    public class Admin : Author
    {
        ////navs
        ////posts made by this user
        //public List<Post> Posts { get; set; }

        public virtual ICollection<Case> AdminCases { get; set; }
    }
}