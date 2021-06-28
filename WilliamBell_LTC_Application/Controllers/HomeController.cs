using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using WilliamBell_LTC_Application.Models;
using WilliamBell_LTC_Application.Models.ViewModels;

namespace WilliamBell_LTC_Application.Controllers
{
    public class HomeController : Controller
    {
        private Models.DAL.LTCContext db = new Models.DAL.LTCContext();


        public ActionResult Index()
        {
            var feature = db.Posts.OfType<News>().Where(p => p.DatePosted <= DateTime.Now && p.Published)
                .OrderByDescending(p => p.DatePosted).FirstOrDefault();
            var featureType = feature.GetType().Name;
            featureType = featureType.Substring(0, featureType.LastIndexOf("_"));

            var featureReview = db.Posts.OfType<Review>()
                .Where(p=>p.DatePosted <= DateTime.Now && p.Published)
                .OrderByDescending(p=>p.DatePosted).FirstOrDefault();

            var featureBlog= db.Posts.OfType<Blog>()
                .Where(p => p.DatePosted <= DateTime.Now && p.Published)
                .OrderByDescending(p => p.DatePosted).FirstOrDefault();

            //get the sub classes of POST to list
            var subclasstypes = Assembly.GetAssembly(typeof(Post)).GetTypes().Where(p => p.IsSubclassOf(typeof(Post))).ToList();
            //put them into a select list
            var subClassList = subclasstypes.ToDictionary(x => x.GUID.ToString(), x => x.Name);
            //    Select(x => new DIc<string,string>()
            //{
            //    Key = x.GUID.ToString(),
            //    Value = x.Name
            //});
            //send it to the view as a select list
            ViewBag.SubClasses = subClassList;

            var model = new HomeViewModel
            {
                
                FeaturePost = new PostInformationViewModel
                {
                    PostId = feature.PostId,
                    PostType = featureType,
                    Controller = "News",
                    PublishDate = feature.DatePosted.ToString("D"),
                    Synopsis = TextTruncate(feature.Synopsis,150),
                    Title = feature.Title,
                    Image = feature.FeatureImagePath
                },
                FeatureReview = new PostInformationViewModel
                {
                    PostId = featureReview.PostId,
                    PostType = "Review",
                    Controller = "Reviews",
                    PublishDate = featureReview.DatePosted.ToString("D"),
                    Synopsis = TextTruncate(featureReview.Synopsis,150),
                    Title = featureReview.Title,
                    Image = featureReview.FeatureImagePath
                }, 
                FeatureBlog = new PostInformationViewModel
                {
                    PostId = featureBlog.PostId,
                    PostType = "Blog",
                    Controller = "Blogs",
                    PublishDate = featureBlog.DatePosted.ToString("D"),
                    Synopsis = TextTruncate(featureBlog.Synopsis,150),
                    Title = featureBlog.Title,
                    Image = featureBlog.FeatureImagePath
                }
            };

            return View(model);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public string TextTruncate(string str, int length)
        {
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length), "Length must be >= 0");
            }

            if (str == null)
            {
                return null;
            }

            int maxLength = Math.Min(str.Length, length);
            return str.Substring(0, maxLength) + "...";
        }
    }
}