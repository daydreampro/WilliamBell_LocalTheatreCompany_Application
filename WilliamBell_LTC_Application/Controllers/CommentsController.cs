using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using WilliamBell_LTC_Application.Models.ViewModels;
using WilliamBell_LTC_Application.Models;

namespace WilliamBell_LTC_Application.Controllers
{
    public class CommentsController : Controller
    {
        private Models.DAL.LTCContext db = new Models.DAL.LTCContext();

        [ChildActionOnly]
        public ActionResult GetComments(int commentId, int? padding)
        {
            //get the comments from db based on the comment we are at (comment ID)
            var comments = db.Comments.Include(c=>c.Votes).Where(c => c.ParentCommentId == commentId).ToList();

            //create a list of view model
            var coms = new List<CommentViewModel>();

            //populate list with view model comments
            foreach (Comment c in comments)
            {
                //count of extra sub comments
                var count = db.Comments.Find(c.CommentID).Replies.Count;
                //get the username
                var username = db.Users.Find(c.UserID).UserName;

                //USER Voted checking
                //get the logged in user
                var userId = User.Identity.GetUserId();
                //if the user is logged in, get their vote
                CommentVote vote = null;
                if (!string.IsNullOrEmpty(userId))
                {
                    if (c.Votes.Select(v => v.UserId).Contains(userId)) 
                    {
                        vote = db.Votes.OfType<CommentVote>().Single(v => v.UserId.Equals(userId) && v.CommentId == c.CommentID);
                    }
                }
                //get the score
                var up = c.Votes.Where(v => v.UpVote).Count();
                var down = c.Votes.Where(v => !v.UpVote).Count();
                //create the comment
                coms.Add(new CommentViewModel
                {
                    CommentId = c.CommentID,
                    UserId = c.UserID,
                    UserName = username,
                    CommentContet = c.Content,
                    DateTime = c.EditedAt ?? c.DatePosted,
                    ChildrenCommentsCount = count,
                    Padding = padding ?? 25,
                    UpVotes = up,
                    DownVotes = down,
                    Score = up - down,
                    DELETED = c.DELTED,
                    UserVote = vote
                });
            }

            //return the model to the partial view to be built
            return PartialView("~/Views/Shared/Comments/_SubComments.cshtml", coms);
        }

        [HttpPost]
        public ActionResult AddComment(int postId, string content, string postPage)
        {
            //make the model
            var model = new AddCommentViewModel
            {
                Comment = content,
                PostId = postId
            };
            //check the content is not empty
            if (string.IsNullOrWhiteSpace(content))
            {
                model.Success = false;
                model.ErrorMessage = "Comment cannot be empty.";
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            //check for banned words?
            List<string> bannedWords = db.BannedContents.Select(b => b.Content).ToList();

            foreach (string s in bannedWords)
            {
                if (content.Contains(s))
                {
                    model.Success = false;
                    model.ErrorMessage = "Comment cannot contain profanities.";
                    return Json(model, JsonRequestBehavior.AllowGet);
                }
            }

            //check the model
            var post = db.Posts.Find(postId);
            //check the post exists
            if (post == null)
            {
                model.Success = false;
                model.ErrorMessage = "Post does not exist!";
                return Json(model, JsonRequestBehavior.AllowGet);
            }

            //get the current user leaving the comment
            var user = db.Users.Find(User.Identity.GetUserId());

            //make sure user exists
            if (user == null)
            {
                model.Success = false;
                model.ErrorMessage = "User does not exist!?";
                return Json(model, JsonRequestBehavior.AllowGet);
            }

            //create the comment
            var comment = new Comment
            {
                Content = content,
                DatePosted = DateTime.Now,
                //PostId = model.PostId,
                Post = post,
                User = user
            };

            //add and save the comment
            db.Comments.Add(comment);
            db.SaveChanges();

            //CREATE THE NOTIFICATION
            //get the id of the comment
            int id = comment.CommentID;
            //get the post owner to send notification too
            var postOwner = db.Users.Find(post.AuthorId);
            //create notification
            var notification = new Notification
            {
                User = postOwner,
                Seen = false,
                DateToRemoveBellNotification = null,
                CreatedAt = DateTime.Now,
                RemoveFromBell = false,
                Destination = postPage + "#commentHeading-" + id,
                Message = "User " + User.Identity.GetUserName() + " commented on your post!"
            };

            
            //save notification
            db.Notifications.Add(notification);
            db.SaveChanges();


            model.Success = true;
            model.Id = comment.CommentID;
            model.UserId = user.Id;
            return Json(model, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult AddReply(int parentId, string content, string postPage)
        {
            //make the model
            var model = new AddCommentViewModel
            {
                Comment = content,
                CommentId = parentId
            };
            //check the content is not empty
            if (string.IsNullOrWhiteSpace(content))
            {
                model.Success = false;
                model.ErrorMessage = "Comment cannot be empty.";
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            //check for banned words
            List<string> bannedWords = db.BannedContents.Select(b => b.Content).ToList();
            foreach (string s in bannedWords)
            {
                if (content.Contains(s))
                {
                    model.Success = false;
                    model.ErrorMessage = "Comment cannot contain profanities.";
                    return Json(model, JsonRequestBehavior.AllowGet);
                }
            }

            //check the model
            var parent = db.Comments.Find(parentId);
            //check the comment exists
            if (parent == null)
            {
                model.Success = false;
                model.ErrorMessage = "The parent comment could not be found, please refresh the page and try again.";
                return Json(model, JsonRequestBehavior.AllowGet);
            }

            //get the current user leaving the comment
            var user = db.Users.Find(User.Identity.GetUserId());

            //make sure user exists
            if (user == null)
            {
                model.Success = false;
                model.ErrorMessage = "Please login to make a comment!";
                return Json(model, JsonRequestBehavior.AllowGet);
            }

            //create the comment
            var comment = new Comment
            {
                Content = content,
                DatePosted = DateTime.Now,
                //PostId = model.PostId,
                ParentComment = parent,
                User = user
            };

            //add and save the comment
            db.Comments.Add(comment);
            db.SaveChanges();

            //CREATE THE NOTIFICATION
            //get the id of the comment
            int id = comment.CommentID;
            //get the post owner to send notification too
            var commentOwner = db.Users.Find(parent.UserID);
            //create notification
            var notification = new Notification
            {
                User = commentOwner,
                Seen = false,
                DateToRemoveBellNotification = null,
                CreatedAt = DateTime.Now,
                RemoveFromBell = false,
                Destination = postPage + "#commentHeading-" + id,
                Message = "User " + User.Identity.GetUserName() + " replied to your comment!"
            };
            //save notification
            db.Notifications.Add(notification);
            db.SaveChanges();


            //TODO: change to JSON
            model.Success = true;
            model.Id = comment.CommentID;
            model.UserId = user.Id;

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult AddNewComment(int? commentId, int? padding)
        {
            var comment = db.Comments.Find(commentId);
            //if padding is 0, it is an initial comment, and we want to add the padding ourselves
            if (padding == 0)
            {
                padding = 25;
            }
            if (comment == null)
            {
                //return an error
            }

            var model = new CommentViewModel
            {
                CommentId = (int)commentId,
                Padding = padding ?? 0,
                UserId = comment.UserID,
                DateTime = comment.EditedAt ?? comment.DatePosted,
                CommentContet = comment.Content,
                Score = 0,
            };

            return PartialView("~/Views/Shared/Comments/_NewComment.cshtml", model);
        }

        public JsonResult GetReportReasons()
        {
            var reasons = db.ReportReasons.Select(x => new SelectListItem
            {
                Text = x.Reason,
                Value = x.ReportReasonId.ToString()
            }).ToList();

            return Json(reasons);
        }

        public JsonResult ReportComment(int commentId, int reasonId, string info)
        {
            //get the comment and reports (that are not solved)
            var comment = db.Comments.Include(c => c.Reports).Single(c => c.CommentID == commentId);

            if (comment == null)
            {
                return Json(new { Success = false, Message = "Comment does not exist" });
            }
            //check for immunity (comment has already been processed through report procedings)
            if (!comment.Immune)
            {
                //check the number of reports the user has recieved through comments/posts
                //get user
                var user = db.Users.Find(comment.UserID);
                if (user == null)
                {
                    return Json(new { Success = false, Message = "User does not exist" });
                }

                //get all the users asociated onging infractions, if any....
                //get the reported comments with an onoging report
                var allReportedComments = db.Comments.Include(c=>c.Reports).Where(c => c.UserID.Equals(user.Id) && c.Reports.Any(r=> r.Status != Status.Solved)).ToList();
                //from that get the reports to add to the case
                var allCommentReports = allReportedComments.SelectMany(r => r.Reports.Where(rr=>rr.Status == Status.Pending||rr.Status == Status.Assigned)).ToList();

                //same for posts
                var allReportedPosts = db.Posts.Include(c => c.Reports).Where(c => c.AuthorId.Equals(user.Id) && c.Reports.Any(r => r.Status != Status.Solved)).ToList();
                var allPostReports = allReportedPosts.SelectMany(r => r.Reports.Where(rr => rr.Status == Status.Pending || rr.Status == Status.Assigned)).ToList();

                //add them together, to test whether the number of reports exceeds the amount needed to open a case!
                var reportsCount = allCommentReports.Count + allPostReports.Count;

                
                //can extend the reporting reasons to many.... just btw
                var report = new Report
                {
                    Comment = comment,
                    Reasons = new List<ReportReason> { db.ReportReasons.Find(reasonId) },
                    ExtraInformation = info,
                    Status = Status.Pending,
                };


                //if number of (current/not solved)reports is (5) or greater, open a case
                //the case points to the offending item (comment)
                //and the comment has all the reports attributed to it
                //plust one for the new report
                if (reportsCount + 1 >= 5)
                {
                    //find whether the a case exists and has not been solved
                    var existingCase = db.Cases.Include(ec => ec.ReportedComments)
                        .Where(ec => ec.UserId.Equals(user.Id) && !ec.IsSolved).SingleOrDefault();

                    //if there is no such case, open a new one - old cases can still be viewd through the closed cases
                    //TODO:
                    //and add any other comment/posts to the case as well
                    if (existingCase == null)
                    {
                        var opencase = new Case
                        {
                            IsGuilty = false,
                            IsSolved = false,
                            ReportedComments = new List<Comment> { comment },
                            User = user,
                        };
                        //add any and all ongoing reported comments to add to the case
                        foreach(Comment c in allReportedComments)
                        {
                            //make sure the reports all point to this case now
                            foreach(Report r in c.Reports)
                            {
                                r.Case = opencase;
                                db.Entry(r).State = EntityState.Modified;
                            }
                            opencase.ReportedComments.Add(c);
                            db.Entry(c).State = EntityState.Modified;
                        }
                        //add the new report also
                        report.Case = opencase;
                        //and assing all previous reports on this specific comment to the case
                        foreach (Report r in comment.Reports)
                        {
                            r.Case = opencase;
                            db.Entry(r).State = EntityState.Modified;
                        }

                        db.Cases.Add(opencase);
                    }
                    else
                    {
                        //if a case is open, the report is assinged to that case
                        report.Status = 0;
                        report.Case = existingCase;

                        //if a unsolved case exists, add the new offending item
                        //check if the comment is already added
                        if (!existingCase.ReportedComments.Contains(comment))
                        {
                            //if the comment is not already added (i.e. it is a new reported comment) add it to the case
                            existingCase.ReportedComments.Add(comment);
                            db.Entry(existingCase).State = EntityState.Modified;
                        }

                        //if there is a moderator/admin assinged to the case,
                        //notify that moderator
                        if (!String.IsNullOrEmpty(existingCase.ModeratorId))
                        {
                            db.Notifications.Add(new Notification
                            {
                                User = db.Users.Find(existingCase.ModeratorId),
                                Seen = false,
                                RemoveFromBell = false,
                                CreatedAt = DateTime.Now,
                                Destination = "/Admin/ViewCase?caseId=" + existingCase.CaseId.ToString(), //this works as long as it is @URL.Content
                                Message = "A new report has been added to one of your on-going cases."
                            });
                        }
                    }
                }

                db.Reports.Add(report);

                if (db.SaveChanges() > 0)
                {
                    return Json(new { Success = true, Message = "Comment Reported Succesfully" });
                }

                return Json(new { Success = false, Message = "Something went wrong! Please try again later" });
            }
            return Json(new { Success = true, Message = "Comment has immunity, shhhhhhh, dont tell anyone ;)" });
        }

        [HttpPost]
        public ActionResult EditComment(int id, string content)
        {
            var model = new EditCommentViewModel
            {
                CommentId = id,
                Message = "Comment Updated",
                Success = true
            };
            var comment = db.Comments.Find(id);
            if (comment == null)
            {
                model.Success = false;
                model.Message = "Something went wrong please try agian later!";
                return Json(model, JsonRequestBehavior.AllowGet);
            }

            //check the content is not empty
            if (string.IsNullOrWhiteSpace(content))
            {
                model.Success = false;
                model.Message = "Comment cannot be empty.";
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            //check for banned words
            List<string> bannedWords = db.BannedContents.Select(b => b.Content).ToList();
            foreach (string s in bannedWords)
            {
                if (content.Contains(s))
                {
                    model.Success = false;
                    model.Message = "Comment cannot contain profanities.";
                    return Json(model, JsonRequestBehavior.AllowGet);
                }
            }

            comment.Content = content;
            comment.EditedAt = DateTime.Now;

            db.Entry(comment).State = EntityState.Modified;
            db.SaveChanges();

            return Json(model, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteComment(int commentId)
        {
            var model = new DeleteCommentViewModel();

            var comment = db.Comments.Find(commentId);
            if (comment == null)
            {
                model.Success = false;
                model.Message = "Something went wrong, comment could not be found!";
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            if (comment.Replies.Count <= 0)
            {
                //if the comment hgas no replies delete it safely
                db.Comments.Remove(comment);
                db.SaveChanges();

                model.Removing = true;
            }
            else
            {
                //if the comment has replies change the state of the comment
                comment.DELTED = true;
                comment.Content = "DELETED";

                model.Removing = false;

                db.Entry(comment).State = EntityState.Modified;
                db.SaveChanges();
            }

            model.Success = true;
            model.CommentId = commentId;
            model.Message = "Comment Succesfully Deleted";

            return Json(model);
        }

        public JsonResult VoteComment(int commentId, bool upVote)
        {
            // get the comment and add a new vote to it, upvote or downvote
            var comment = db.Comments.Include(c=>c.Votes).Single(c=>c.CommentID == commentId);
            if(comment == null)
            {
                return Json(new { Success = false, Message = "Comment not found!" });
            }
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            if(user == null)
            {
                return Json(new { Success = false, Message = "You were not found!" });
            }
            //check if the user has already voted on the comment
            foreach(CommentVote v in comment.Votes)
            {
                if (v.UserId.Equals(userId))
                {
                    //change the vote!
                    v.UpVote = upVote;
                    db.Entry(v).State = EntityState.Modified;
                    if (db.SaveChanges() > 0)
                    {
                        return Json(new
                        {
                            Success = true,
                            Score = comment.Votes.Where(c => c.UpVote).Count() - comment.Votes.Where(c => !c.UpVote).Count(),
                            Message = "Success!"
                        });
                    }
                    return Json(new { Success = false, Message = "User has already like/disliked the comment!" });
                }
            }

            //otherwise we can like/dislike
            comment.Votes.Add(new CommentVote { User = user, UpVote = upVote });

            db.Entry(comment).State = EntityState.Modified;

            if (db.SaveChanges() > 0)
            {
                return Json(new { Success = true,
                    Score = comment.Votes.Where(c => c.UpVote).Count() - comment.Votes.Where(c => !c.UpVote).Count(),
                    Message = "Success!" });
            }

            return Json(new { Success = false,
                Message = "Something went wrong" });
        }
    }
}