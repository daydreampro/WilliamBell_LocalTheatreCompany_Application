using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using WilliamBell_LTC_Application.Models;
using WilliamBell_LTC_Application.Models.DAL;
using WilliamBell_LTC_Application.Models.ViewModels;

namespace WilliamBell_LTC_Application.Controllers
{
    [Authorize(Roles = "Author")]
    public class AuthorController : Controller
    {

        private LTCContext db = new LTCContext();

        /// <summary>
        /// View All Currently Logged in Authors PUBLISHED Posts.
        /// Published == true, draft == false.
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Author,Admin")]
        public ActionResult MyPublishedPosts()
        {
            var userid = User.Identity.GetUserId();
            var posts = db.Posts.Where(p => p.AuthorId.Equals(userid) || p.EditorId.Equals(userid)).OrderByDescending(p=>p.DatePosted).Where(p=>p.Published).ToList();


            var model = new List<ArchiveListViewModel>();

            foreach(var p in posts)
            {
                string type = p.GetType().Name.Substring(0, p.GetType().Name.LastIndexOf("_"));
                string cont = type + "s";

                if (cont[cont.LastIndexOf("s") - 1] == 's')
                {
                    cont = cont.Substring(0, cont.LastIndexOf("s"));
                }

                model.Add(new ArchiveListViewModel
                {
                    AuthorName = "",
                    Image = p.FeatureImagePath  ?? "~/Content/Images/Posts/Demo/dog_ate_img.jpeg",
                    Posted = p.DatePosted.ToString(),
                    PostId = p.PostId,
                    Synopsis = p.Synopsis,
                    Title = p.Title,
                    PostType = type,
                    NumberOfComments = new PostsController().CommentsCount(p.PostId),
                    ControllerName = cont
                });
            }

            ViewBag.PostsCount = model.Count;

            return View(model);
        }
        /// <summary>
        /// View All Currently Logged in Authors PENDING Posts.
        /// Published == false, draft == false.
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> MyPendingPosts()
        {
            var userid = User.Identity.GetUserId();
            var posts = db.Posts.Where(p => p.AuthorId.Equals(userid) || p.EditorId.Equals(userid)).OrderByDescending(p => p.DatePosted)
                .Where(p => !p.Published && !p.Draft).ToList();


            var model = new List<ArchiveListViewModel>();

            foreach (var p in posts)
            {
                await db.Entry(p).ReloadAsync();

                string type = p.GetType().Name.Substring(0, p.GetType().Name.LastIndexOf("_"));
                string cont = type + "s";

                if (cont[cont.LastIndexOf("s") - 1] == 's')
                {
                    cont = cont.Substring(0, cont.LastIndexOf("s"));
                }

                model.Add(new ArchiveListViewModel
                {
                    AuthorName = "",
                    Image = p.FeatureImagePath ?? "~/Content/Images/Posts/Demo/dog_ate_img.jpeg",
                    Posted = p.DatePosted.ToString(),
                    PostId = p.PostId,
                    Synopsis = p.Synopsis,
                    Title = p.Title,
                    PostType = type,
                    NumberOfComments = new PostsController().CommentsCount(p.PostId),
                    ControllerName = cont
                });
            }

            ViewBag.PostsCount = model.Count;

            return View(model);
        }

        /// <summary>
        /// View All Currently Logged in Authors DRAFT Posts.
        /// Published == false, draft == true.
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> MyDraftPosts()
        {
            var userid = User.Identity.GetUserId();
            var posts = db.Posts.Where(p => p.AuthorId.Equals(userid) || p.EditorId.Equals(userid)).OrderByDescending(p => p.DatePosted)
                .Where(p => !p.Published && p.Draft).ToList();


            var model = new List<ArchiveListViewModel>();

            foreach (var p in posts)
            {
                await db.Entry(p).ReloadAsync();
                string type = p.GetType().Name.Substring(0, p.GetType().Name.LastIndexOf("_"));
                string cont = type + "s";

                if (cont[cont.LastIndexOf("s") - 1] == 's')
                {
                    cont = cont.Substring(0, cont.LastIndexOf("s"));
                }

                model.Add(new ArchiveListViewModel
                {
                    AuthorName = "",
                    Image = p.FeatureImagePath,
                    Posted = p.DatePosted.ToString(),
                    PostId = p.PostId,
                    Synopsis = p.Synopsis,
                    Title = p.Title,
                    PostType = type,
                    NumberOfComments = new PostsController().CommentsCount(p.PostId),
                    ControllerName = cont,
                    IsFlagged = p.Flagged
                });
            }

            ViewBag.PostsCount = model.Count;

            return View(model);
        }
    }
}