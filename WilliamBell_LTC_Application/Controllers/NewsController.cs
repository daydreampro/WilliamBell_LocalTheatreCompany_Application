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
    public class NewsController : Controller
    {
        private LTCContext db = new LTCContext();
        public ActionResult AllNews(DateTime? date )
        {
            var news = db.Posts.OfType<News>().Where(n => n.Published == true)
                .OrderByDescending(p => p.DatePosted).ToList();

            var model = new List<NewsListViewModel>();

            var dates = new Dictionary<string, DateTime>();
            var now = DateTime.Now;
            foreach (var n in news)
            {
                if (n.DatePosted <= now)
                {
                    var monthYear = n.DatePosted.ToString("MMMM yyyy");
                    if (!dates.Keys.Contains(monthYear))
                    {
                        dates.Add(monthYear, new DateTime(n.DatePosted.Year, n.DatePosted.Month, 1));
                    }

                }
            }

            if (date != null)
            {
                news = news.Where(n => n.DatePosted > date && n.DatePosted < date.Value.AddMonths(1)).ToList();
            }

            
            foreach(var n in news)
            {
                if (n.DatePosted <= now)
                {
                    
                    model.Add(new NewsListViewModel
                    {
                        AuthorName = n.Author.FirstName + " " + n.Author.LastName,
                        AuthorId = n.AuthorId,
                        Image = n.FeatureImagePath,
                        Content = n.PostContent.Substring(n.PostContent.IndexOf("<p>"),n.PostContent.IndexOf("</p>")) + "...",
                        NumberOfComments = new PostsController().CommentsCount(n.PostId),
                        Posted = n.DatePosted.ToString(),
                        Title = n.Title,
                        PostId = n.PostId
                    }); 
                }
                
            }
            ViewBag.Dates = dates;
            return View(model);
        }

        public ActionResult ViewNews(int postId)
        {
            var news = db.Posts.OfType<News>().Single(p => p.PostId == postId);
            if (news == null)
            {
                return RedirectToAction("AllNews");
            }

            var coms = news.Comments;

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
                if (news.Votes.Select(v => v.UserId).Contains(userId))
                {
                    postVote = news.Votes.OfType<PostVote>().Single(v => v.UserId.Equals(userId) && v.PostId == news.PostId);
                }
            }
            var model = new NewsDetailsViewModel
            {
                AuthorId = news.AuthorId,
                AuthorName = news.Author.FirstName + " " + news.Author.LastName,
                Content = news.PostContent,
                Image = news.FeatureImagePath,
                Locked = news.PostLocked,
                Posted = news.DatePosted.ToString(),
                PostId = (int) postId,
                Title = news.Title,
                Comments = comments,
                PostScore = news.Votes.Where(v => v.UpVote).Count() - news.Votes.Where(v => !v.UpVote).Count(),
                UserVote = postVote
            };
            return View(model);
        }
    }
}