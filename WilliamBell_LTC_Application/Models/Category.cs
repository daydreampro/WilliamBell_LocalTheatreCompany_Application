using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WilliamBell_LTC_Application.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }

        public ICollection<Blog> Blogs { get; set; }
    }

    public class Genre
    {
        public int GenreId { get; set; }
        public string Name { get; set; }

        public ICollection<Review> Reviews { get; set; }
    }
    /// <summary>
    /// eg: Stage, Film, Indie
    /// </summary>
    public class ReviewCategory
    {
        public int Id { get; set; }
        public string Name { get; set; }

        //navs
        public ICollection<Review> Reviews { get; set; }
    }
}