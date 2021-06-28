using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using WilliamBell_LTC_Application.Models;
using WilliamBell_LTC_Application.Models.DAL;
using WilliamBell_LTC_Application.Models.ViewModels;
using Microsoft.AspNet.Identity;

namespace WilliamBell_LTC_Application.Controllers
{
    public class BlogsController : Controller
    {
        private LTCContext db = new LTCContext();

        public ActionResult AllBlogs(int? categoryId, int[] categories)
        {
            //to check if it is released get the date time now
            var now = DateTime.Now;
            //get alll teh posts that are published - include their categories to filter
            var posts = db.Posts.OfType<Blog>()
                .Where(b => b.Published == true)
                .Include(b=>b.Votes)
                .Include(b => b.Categories).OrderByDescending(p=>p.DatePosted).ToList();

            //filtering
            if (categories != null)
            {
                posts = posts.Where(p => categories.Any(cid => p.Categories.Any(c => c.CategoryId == cid))).ToList();
            }
            else if(categoryId != null)
            {
                posts = posts.Where(p => p.Categories.Any(cid => cid.CategoryId == categoryId)).ToList();
            }

            var model = new List<BlogListViewModel>();
            //for each post (if the date posted is before or eqaul to todays datetime) add it to the viewmodel
            foreach(var b in posts)
            {
                if(b.DatePosted <= now)
                {
                    model.Add(new BlogListViewModel
                    {
                        Title = b.Title,
                        Image = b.FeatureImagePath,
                        AuthorName = b.Author.FirstName + " " + b.Author.LastName,
                        Synopsis = TextTruncate(b.Synopsis, 200),
                        Posted = b.DatePosted.ToString(),
                        PostId = b.PostId,
                        NumberOfComments = new PostsController().CommentsCount(b.PostId),
                        PostVotes = b.Votes.Where(v=>v.UpVote).Count()- b.Votes.Where(v=>!v.UpVote).Count()
                    });
                }
                
            }

            ViewBag.Categories = db.Categories.ToList();

            //if we are filtering the page return a partial to render as apposesd to an entire list!
            if (this.Request.IsAjaxRequest())
            {
                return PartialView("~/Views/Blogs/_FilterBlogs.cshtml", model);
            }

            return View(model);
        }

        public ActionResult ViewBlog(int postId)
        {
            var blog = db.Posts.OfType<Blog>().Include(p=>p.Categories).Include(p=>p.Comments).Single(p=> p.PostId == postId);
            if(blog == null)
            {
                return RedirectToAction("AllBlogs");
            }
            var coms = blog.Comments;

            var comments = new List<CommentViewModel>();
            //get the logged in user
            var userId = User.Identity.GetUserId();
            foreach (Comment c in coms)
            {
                //count of extra sub comments
                var count = db.Comments.Find(c.CommentID).Replies.Count;
                //get the username
                var username = db.Users.Find(c.UserID).UserName;

                //USER Voted checking
                
                //if the user is logged in, get their vote
                CommentVote vote = null;
                if (!string.IsNullOrEmpty(userId))
                {
                    if (c.Votes.Select(v => v.UserId).Contains(userId))
                    {
                        vote = db.Votes.OfType<CommentVote>().Single(v => v.UserId.Equals(userId) && v.CommentId == c.CommentID);
                    }
                }
                var up = c.Votes.Where(v => v.UpVote).Count();
                var down = c.Votes.Where(v => !v.UpVote).Count();

                //create the comment
                comments.Add(new CommentViewModel
                {
                    CommentId = c.CommentID,
                    UserId = c.UserID,
                    UserName = username,
                    CommentContet = c.Content,
                    DateTime = c.EditedAt ?? c.DatePosted,
                    ChildrenCommentsCount = count,
                    Padding = 0,
                    UpVotes = up,
                    DownVotes = down,
                    Score = up - down,
                    DELETED = c.DELTED,
                    UserVote = vote
                });
            }
            //USER Post Voted checking
            //if the user is logged in, get their vote
            PostVote postVote = null;
            if (!string.IsNullOrEmpty(userId))
            {
                if (blog.Votes.Select(v => v.UserId).Contains(userId))
                {
                    postVote = blog.Votes.OfType<PostVote>().Single(v => v.UserId.Equals(userId) && v.PostId == blog.PostId);
                }
            }
            var model = new BlogDetailsViewModel()
            {
                PostId = (int)postId,
                AuthorId = blog.AuthorId,
                Title = blog.Title,
                Image = blog.FeatureImagePath,
                Content = blog.PostContent,
                AuthorName = blog.Author.FirstName + " " + blog.Author.LastName,
                Posted = blog.DatePosted.ToString(),
                Comments = comments,
                Locked = blog.PostLocked,
                Categories = blog.Categories.ToList(),
                PostScore = blog.Votes.Where(v => v.UpVote).Count() - blog.Votes.Where(v => !v.UpVote).Count(),
                UserVote = postVote
            };

            return View(model);
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