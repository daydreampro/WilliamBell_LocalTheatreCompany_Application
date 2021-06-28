using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WilliamBell_LTC_Application.Models.ViewModels;
using WilliamBell_LTC_Application.Models;

namespace WilliamBell_LTC_Application.Controllers
{
    public class NavbarController : Controller
    {
        private Models.DAL.LTCContext db = new Models.DAL.LTCContext();

        [ChildActionOnly]
        [OutputCache(Duration = 5 * 60)]
        public ActionResult NavBar()
        {
            var poprev = db.Posts.OfType<Review>().OrderBy(p => p.DatePosted).FirstOrDefault();
            var popblog = db.Posts.OfType<Blog>().OrderBy(p => p.DatePosted).FirstOrDefault();

            var model = new NavBarViewModel
            {
                ReviewCategories = db.ReviewCategories.ToList(),
                Categories = db.Categories.ToList(),
                Genres = db.Genres.ToList(),
                PopularReview = new PopularViewModel { PostId = poprev.PostId, Title = poprev.Title, Image = poprev.FeatureImagePath },
                PopularBlog = new PopularViewModel { PostId = popblog.PostId, Title = popblog.Title, Image = popblog.FeatureImagePath }
            };

            return PartialView("_NavBar", model);
        }
    }
}