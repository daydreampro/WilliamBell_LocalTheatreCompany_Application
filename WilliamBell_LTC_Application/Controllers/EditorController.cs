using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WilliamBell_LTC_Application.Models;
using WilliamBell_LTC_Application.Models.DAL;
using WilliamBell_LTC_Application.Models.ViewModels;

namespace WilliamBell_LTC_Application.Controllers
{
    [Authorize(Roles ="Editor")]
    public class EditorController : Controller
    {
        private LTCContext db = new LTCContext();

        /// <summary>
        /// View All PENDING Posts. Posts that need an editor to review and publish them
        /// Published == false, draft == false.
        /// </summary>
        /// <returns></returns>
        public ActionResult AllPendingPosts()
        {
            var userid = User.Identity.GetUserId();
            //get all pendings posts that do not have an editor
            var posts = db.Posts.Where(p => string.IsNullOrEmpty(p.EditorId) && !p.Published && !p.Draft).OrderByDescending(p => p.DatePosted).ToList();

            //veiw model
            var model = new List<ArchiveListViewModel>();


            foreach (var p in posts)
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
        /// As an editor accept the post to edit, review and eventually publish
        /// </summary>
        public ActionResult AcceptPost(int postId, string postType)
        {
            var post = db.Posts.Find(postId);

            if(post == null)
            {
                return RedirectToAction("AllPendingPosts");
            }

            var editorId = User.Identity.GetUserId();
            var editor = db.Users.Find(editorId);
            if(editor == null)
            {
                return RedirectToAction("AllPendingPosts");
            }

            post.Editor = editor;
            db.Entry(post).State = EntityState.Modified;

            try
            {
                if (db.SaveChanges() > 0)
                {
                    return RedirectToAction("Create"+postType,"Posts", new { postId =  postId });
                }
            }catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return RedirectToAction("AllPendingPosts");
            }
            return RedirectToAction("AllPendingPosts");
        }

       
        public ActionResult PublishPost(int postId, DateTime? releaseDate)
        {
            //get the post
            //get the editor of the post
            //if there is not one, make the current editor the editor
            //otherwise check that the current editor has permission to do this

            //set the post to published and not draft
            //set the release date

            //create notification for the post author

            //attempt save
            //return success and message
            var post = db.Posts.Find(postId);
            if (post == null)
            {
                return Json(new { Success = false, Message = "Could not find post!" });
            }
            var editor = db.Users.Find(post.EditorId);
            var currentEditorId = User.Identity.GetUserId();
            var currentEditor = db.Users.Find(currentEditorId);
            if (editor == null)
            {
                if (currentEditor == null)
                {
                    return Json(new { Success = false, Message = "You could not be found!?" });
                }
                post.Editor = currentEditor;
            }
            else
            {
                if (editor != currentEditor)
                {
                    return Json(new { Success = false, Message = "You do not have permission to do this!" });
                }
            }
            //update post
            post.Draft = false;
            post.Published = true;
            post.Flagged = false;
            post.DatePosted = releaseDate ?? DateTime.Now;

            db.Entry(post).State = EntityState.Modified;

            //create notification
            var type = post.GetType().Name;

            type = type.Substring(0, type.LastIndexOf("_"));
            var cont = type;
            if (!cont.Equals("News"))
            {
                cont += "s";
            }
            var author = db.Users.Find(post.AuthorId);
            if(author == null)
            {
                return Json(new { Success = false, Message = "Cannot find author, please try again later!" });
            }
            var noti = new Notification
            {
                CreatedAt = DateTime.Now,
                Message = "Your "+type+" '" + post.Title + "' has been accepted.<p>It will be published on " + post.DatePosted.ToString("d") + "</p>",
                User = author,
                Destination = "~/" + cont + "/View"+type+ "?postId=" + post.PostId
            };

            db.Notifications.Add(noti);

            try
            {
                if (db.SaveChanges() > 0)
                {
                    return Json(new { Success = true, Message = "Post published" });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return Json(new { Success = false, Message = "Something went wrong! \n" + e.Message });
            }


            return Json(new { Success = false, Message = "Something went wrong!" });
        }

        /// <summary>
        /// View all pending posts that the current editor owns
        /// </summary>
        /// <returns></returns>
        public ActionResult MyDraftPosts()
        {
            var editorId = User.Identity.GetUserId();

            var posts = db.Posts.Where(p => p.EditorId.Equals(editorId)
            && !p.Published && !p.Draft)
                .OrderByDescending(p => p.DatePosted)
                .ToList();


            var model = new List<ArchiveListViewModel>();

            foreach (var p in posts)
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
                    Image = p.FeatureImagePath,
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

        public JsonResult RejectPost(int postId, string reason)
        {
            //set the post back to draft and notify the user as to the reasons why
            var post = db.Posts.Find(postId);
            if (post == null)
            {
                return Json(new { Success = false, Message = "Post not found!" });
            }
            post.Draft = true;
            post.Flagged = true;

            //remove editor - might not need this, could alternatively send it back to the same editor
            //when the author "updates it" this way they alrady know what they are working with
            //EXTENSIONS : 
            //We could add a reason or notes to be attached to a post with regards to errors?
            post.Editor = null;
            post.EditorId = null;

            db.Entry(post).State = EntityState.Modified;

            var author = db.Users.Find(post.AuthorId);
            if (author == null)
            {
                return Json(new { Success = false, Message = "Author not found!" });
            }

            var noti = new Notification
            {
                User = author,
                CreatedAt = DateTime.Now,
                Destination = "/Author/MyDraftPosts",
                Message = "Your post: '"+post.Title+"' has been rejected for publishing.<h6>Reason:</h6><p>"+ reason +"</p>"
            };

            db.Notifications.Add(noti);

            try
            {
                if (db.SaveChanges() > 0)
                {
                    return Json(new { Success = true, Message = "Post rejected!" });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return Json(new { Success = false, Message = "Something went wrong: "+e.Message });
            }

            return Json(new { Success = false, Message = "Something went wrong!" });
        }
        /// <summary>
        /// View the logged in Editors PUBLISHED Posts.
        /// Published == true, draft == false.
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Author,Admin")]
        public ActionResult MyPublishedPosts()
        {
            var userid = User.Identity.GetUserId();
            var posts = db.Posts.Where(p => p.AuthorId.Equals(userid)).OrderByDescending(p => p.DatePosted).Where(p => p.Published).ToList();


            var model = new List<ArchiveListViewModel>();

            foreach (var p in posts)
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

        
    }
}