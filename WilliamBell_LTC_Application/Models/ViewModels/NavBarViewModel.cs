using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WilliamBell_LTC_Application.Models.ViewModels
{
    public class NavBarViewModel
    {
        public List<Category> Categories { get; set; }
        public List<ReviewCategory> ReviewCategories { get; set; }
        public List<Genre> Genres { get; set; }
        public PopularViewModel PopularReview { get; set; }
        public PopularViewModel PopularBlog { get; set; }

    }

    public class PopularViewModel
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Image { get; set; }
    }
}