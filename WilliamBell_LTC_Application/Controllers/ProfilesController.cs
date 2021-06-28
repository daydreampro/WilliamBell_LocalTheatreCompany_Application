using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using WilliamBell_LTC_Application.Models.ViewModels;
using WilliamBell_LTC_Application.Models.DAL;
using WilliamBell_LTC_Application.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WilliamBell_LTC_Application.Controllers
{
    public partial class ProfilesController : Controller
    {
        private LTCContext db = new LTCContext();


        public ActionResult ViewProfile(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Index", "Home");
            }
            var user = db.Users
                .Include(u=>u.Badges)
                .Single(u=>u.Id.Equals(userId));
            if(user == null)
            {
                return RedirectToAction("Index", "Home");
            }
            List<string> userRoles = new List<string>();

            using (var userManager = new UserManager<User>(new UserStore<User>(db)))
            {
                userRoles = userManager.GetRoles(user.Id).ToList();
            }

            var model = new PublicProfileViewModel
            {
                UserId = userId,
                DisplayName = user.FirstName + " " + user.LastName,
                Biography = user.Biography,
                Image = user.UserImage,
                Badges = user.Badges.ToList(),
                Roles = userRoles
            };


            return View(model);
        }

        public ActionResult ViewOwnProfile()
        {
            var userId = User.Identity.GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Index", "Home");
            }
            var user = db.Users.Find(userId);
            if(user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("ViewProfile","Profiles",new { userId = userId } );
        }

        public ActionResult EditProfile(string userId)
        {
            return View();
        }

        public PartialViewResult GetUsersPosts(string userId)
        {
            var user = db.Users.OfType<Author>()
                .Include(u => u.Posts)
                .Single(u => u.Id.Equals(userId));
            if (user == null)
            {

            }
            var posts = user.Posts
                .Where(p => p.Published)
                .OrderByDescending(p => p.DatePosted);

            //var model = new List<ArchiveListViewModel>();

            var model = from p in posts
                        select new ProfilePostsViewModel
                        {
                            Title = p.Title,
                            PostId = p.PostId,
                            PostType = p.GetType().Name.Contains("Review") ? "Review" : p.GetType().Name.Contains("Blog") ? "Blog" : "News",
                            Controller = p.GetType().Name.Contains("Review") ? "Reviews" : p.GetType().Name.Contains("Blog") ? "Blogs" : "News"
                        };

            return PartialView("~/Views/Profiles/_ProfileUserPosts.cshtml",model.ToList());
        }

        public ActionResult GetMiniProfile(string userId,int commentId, bool? deleted)
        {
            if(userId == null)
            {
                //return error
            }

            var user = db.Users.Find(userId);

            string displayName = user.UserName;

            if(user is Author)
            {
                displayName = user.FirstName + " " + user.LastName;
            }

            var model = new MiniProfileViewModel
            {
                UserId = userId,
                DisplayName = displayName,
                Image = user.UserImage,
                Badges = user.Badges.ToList()
            };

            ViewBag.CommentId = commentId;
            ViewBag.Deleted = deleted ?? false;
            
            return PartialView("~/Views/Shared/Profile/_MiniProfile.cshtml", model);
        }

        public PartialViewResult GetAuthorCredit(string authorId)
        {
            var author = db.Users.OfType<Author>().Include(a=>a.Badges).Single(a=>a.Id.Equals(authorId));
            if(author == null)
            {

            }

            var model = new AuthorCreditViewModel
            {
                AuthorId = authorId,
                AuthorName = author.FirstName + " " + author.LastName,
                Image = author.UserImage,
                SmallBio = author.SmallBio,
                Badges = author.Badges.ToList()
            };

            return PartialView("~/Views/Shared/Profile/_AuthorCredit.cshtml", model);
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