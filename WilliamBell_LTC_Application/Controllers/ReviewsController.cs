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
    public class ReviewsController : CommentsController
    {
        private LTCContext db = new LTCContext();

        public ActionResult AllReviews(int? genreId, int? categoryId, int[] genreIds, int[] categoryIds)
        {
            //if no filters
            var reviews = db.Posts.OfType<Review>()
                .Where(r => r.Published==true)
                .Include(r => r.Genres).Include(r=>r.ReviewCategories)
                .Include(r=>r.Votes)
                .OrderByDescending(p => p.DatePosted).ToList();

            //filter it baby!
            if (genreIds != null)
            {
                reviews = reviews.Where(r => genreIds.Any(gid => r.Genres.Any(g => g.GenreId == gid))).ToList();
            }
            else if(genreId != null)
            {
                reviews = reviews.Where(r => r.Genres.Any(gid => gid.GenreId == genreId)).ToList();
            }
            if(categoryIds != null)
            {
                reviews = reviews.Where(r => categoryIds.Any(gid => r.ReviewCategories.Any(g => g.Id == gid))).ToList();
            }
            else if(categoryId != null)
            {
                reviews = reviews.Where(r => r.ReviewCategories.Any(cid => cid.Id == categoryId)).ToList();
            }

            List<ReviewListViewModel> revs = new List<ReviewListViewModel>();
            var now = DateTime.Now;

            foreach (Review r in reviews)
            {
                //chcek it is release date
                if(r.DatePosted <= now)
                {
                    revs.Add(new ReviewListViewModel
                    {
                        PostId = r.PostId,
                        Title = r.Title,
                        AuthorName = r.Author.FirstName + " " + r.Author.LastName,
                        Image = r.FeatureImagePath,
                        Posted = r.DatePosted.ToString(),
                        Synopsis = r.Synopsis,
                        NumberOfComments = CommentsCount(r.PostId),
                        PostVotes = r.Votes.Where(v=>v.UpVote).Count() - r.Votes.Where(v=>!v.UpVote).Count()
                    });
                }
                
            }
            //send gners and categories to the view
            ViewBag.Genres = db.Genres.ToList();
            ViewBag.Categories = db.ReviewCategories.ToList();

            //if we are filtering the page return a partial to render as apposesd to an entire list!
            if (this.Request.IsAjaxRequest())
            {
                return PartialView("~/Views/Reviews/_FilterReviews.cshtml",revs);
            }

            return View(revs);
        }

        public ActionResult ViewReview(int postId)
        {
            var review = db.Posts.OfType<Review>().Include(p=>p.ReviewCategories).Include(p=>p.Genres).Single(p=>p.PostId == postId);

            var coms = review.Comments;

            var comments = new List<CommentViewModel>();

            //get the logged in user
            var userId = User.Identity.GetUserId();

            foreach (Comment c in coms)
            {
                //count of extra sub comments
                var count = db.Comments.Find(c.CommentID).Replies.Count;
                //get the username
                var username = db.Users.Find(c.UserID).UserName;

                //USER Comment Voted checking
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
                    Score =  up- down,
                    DELETED = c.DELTED,
                    UserVote = vote,
                });
            }

            //USER Post Voted checking
            //if the user is logged in, get their vote
            PostVote postVote = null;
            if (!string.IsNullOrEmpty(userId))
            {
                if (review.Votes.Select(v => v.UserId).Contains(userId))
                {
                    postVote = review.Votes.OfType<PostVote>().Single(v => v.UserId.Equals(userId) && v.PostId == review.PostId);
                }
            }
            var model = new ReviewDetailsViewModel()
            {
                Rating = review.Score,
                PostId = postId,
                AuthorId = review.AuthorId,
                Title = review.Title,
                Image = review.FeatureImagePath,
                Content = review.PostContent,
                AuthorName = review.Author.FirstName + " " + review.Author.LastName,
                Posted = review.DatePosted.ToString("D"),
                Comments = comments,
                Locked = review.PostLocked,
                Genres = review.Genres.ToList(),
                Categories = review.ReviewCategories.ToList(),
                PostScore = review.Votes.Where(v=>v.UpVote).Count() - review.Votes.Where(v => !v.UpVote).Count(),
                UserVote = postVote
            };

            ViewBag.PostId = postId;

            return View(model);
        }

        private int CommentsCount(int postId)
        {
            int count = 0;

            var post = db.Posts.Find(postId);

            foreach (Comment c in post.Comments)
            {
                if (c.Replies.Count > 0)
                {
                    count += AddReplies(c.CommentID);
                }
                count++;
            }

            return count;
        }
        private int AddReplies(int commentId)
        {
            int count = 0;
            var comment = db.Comments.Find(commentId);

            foreach (Comment c in comment.Replies)
            {
                if (c.Replies.Count > 0)
                {
                    count += AddReplies(c.CommentID);
                }
                count++;
            }
            return count;
        }
    }
}