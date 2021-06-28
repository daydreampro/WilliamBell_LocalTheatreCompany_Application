using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using WilliamBell_LTC_Application.Models;
using WilliamBell_LTC_Application.Models.DAL;
using WilliamBell_LTC_Application.Models.ViewModels;

namespace WilliamBell_LTC_Application.Controllers
{
    
    public class DashboardController : Controller
    {

        private LTCContext db = new LTCContext();


        [Authorize(Roles = "User")]
        public ActionResult Dashboard()
        {
            var user = db.Users.Find(User.Identity.GetUserId());

            
            if(user == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var noti = 0;
            try
            {
                noti = db.Notifications.Where(p => p.UserId.Equals(user.Id) && !p.Seen).Count();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                noti = 0;
            }

            var model = new UserDashboardViewModel
            {
                Badges = user.Badges.ToList(),
                Notifications = noti
            };

            return View(model);
        }

        [Authorize(Roles = "Author")]
        public ActionResult AuthorDashboard()
        {
            var user = db.Users.Find(User.Identity.GetUserId());

            ApplicationUserManager userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            var roles = userManager.GetRoles(user.Id).ToList();
            //getting a list of role names
            var roleToType = new List<string>();
            string rt = "";
            foreach (var r in roles)
            {
                switch (r)
                {
                    case "Reviewer":
                        rt = "Review";
                        roleToType.Add(rt);
                        break;
                    case "Blogger":
                        rt = "Blog";
                        roleToType.Add(rt);
                        break;
                    case "Reporter":
                        rt = "News";
                        roleToType.Add(rt);
                        break;
                    default:
                        rt = "";
                        break;
                }
            }
            var subclasstypes = Assembly.GetAssembly(typeof(Post)).GetTypes().Where(p => p.IsSubclassOf(typeof(Post))).ToList();
            //put them into a select list
            //and filter it based on users role by comparing roltotype and class names
            var subClassList = subclasstypes.Select(x => new SelectListItem()
            {
                Value = x.Name.ToString(),
                Text = x.Name.ToString()
            }).Where(i => roleToType.Any(r => i.Text.Equals(r)));

            if(subClassList.Count() <= 0)
            {
                ViewBag.TypesCount = 0;
            }

            var publishedPostCount = db.Posts.Where(p => (p.AuthorId.Equals(user.Id) || p.EditorId.Equals(user.Id)) && p.Published == true).Count();
            var draftPostCount = db.Posts.Where(p => (p.AuthorId.Equals(user.Id) || p.EditorId.Equals(user.Id)) && p.Draft == true).Count();
            var pendingPostCount = db.Posts.Where(p => (p.AuthorId.Equals(user.Id) || p.EditorId.Equals(user.Id)) && !p.Draft && !p.Published).Count();

            var model = new AuthorDashboardViewModel
            {
                PublishedPostsCount = publishedPostCount,
                PendingPostsCount = pendingPostCount,
                DraftPostsCount = draftPostCount,
                Types = subClassList,
                ClassCount = (byte)subClassList.Count()
            };

            
            return PartialView("~/Views/Dashboard/_AuthorDashboard.cshtml",model);
        }

        public ActionResult EditorDashboard()
        {
            //editing the look of posts
            //and confirming them for publishing
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);

            //posts published by this editor
            var completedEdits = db.Posts.Where(p => p.EditorId.Equals(userId) && p.Published).Count();
            //posts awating publishing and have no editor
            var pendingPosts = db.Posts.Where(p => string.IsNullOrEmpty(p.EditorId) && !p.Published && !p.Draft).Count();
            //posts awating publishing and are owned by the curret editor
            var draftPosts = db.Posts.Where(p => p.EditorId.Equals(userId) && !p.Published && !p.Draft).Count();

            var model = new EditorDashboardViewModel
            {
                CompletedPosts = completedEdits,
                DraftPosts = draftPosts,
                PendingPosts = pendingPosts
            };

            return PartialView("~/Views/Dashboard/_EditorDashboard.cshtml", model);
        }

        public ActionResult ModeratorDashboard()
        {
            //moderator has admin rights to check comments and posts,
            //block people and suspend
            var modId = User.Identity.GetUserId();

            var cases = db.Cases.ToList();

            var model = new ModeratorDashboardViewModel
            {
                AllPendingCasesCount = db.Cases.Where(c => !c.IsSolved && String.IsNullOrEmpty(c.ModeratorId)).Count(),
                MyOpenCasesCount = db.Cases.Where(c => c.ModeratorId.Equals(modId) && !c.IsSolved).Count(),
                MyClosedCasesCount = db.Cases.Where(c => c.ModeratorId.Equals(modId) && c.IsSolved).Count(),
            };

            return PartialView("~/Views/Dashboard/_ModeratorDashboard.cshtml",model);
        }

        
        public ActionResult AdminDashboard()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            //Edit users
            //promote users
            var model = new AdminDashboardViewModel
            {
                CategoriesCount = db.Categories.Count() + db.ReviewCategories.Count() + db.Genres.Count()
            };

            //CRUD
            //categories crud
            return PartialView("~/Views/Dashboard/_AdminDashboard.cshtml", model);
        }

    }
}