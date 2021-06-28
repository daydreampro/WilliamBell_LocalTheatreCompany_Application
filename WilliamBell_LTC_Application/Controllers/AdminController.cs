using System;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using WilliamBell_LTC_Application.Models.ViewModels;
using WilliamBell_LTC_Application.Models;
using WilliamBell_LTC_Application.Models.DAL;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Threading.Tasks;
using System.Web;
using System.IO;

namespace WilliamBell_LTC_Application.Controllers

{
    //MODERATOR AND ADMIN!
    [Authorize(Roles = "Admin, Moderator")]
    public partial class AdminController : Controller
    {
        private  LTCContext db = new LTCContext();

        public ActionResult AllCases()
        {
            //get the cases that are unasigned
            var cases = db.Cases.Include(c=>c.ReportedComments).Include(c=>c.ReportedPosts).Where(c => !c.IsSolved && String.IsNullOrEmpty(c.ModeratorId)).ToList();

            var model = new List<CasesListViewModel>();

            foreach(Case c in cases)
            {
                //get all the reasons for the reports from the case id
                var reports = db.Reports.Include(r=>r.Reasons).Where(r => r.CaseId == c.CaseId).ToList();
                //get the reasons! make it distinct as to not repeat ourselves
                var reasons = reports.SelectMany(r => r.Reasons).Distinct().Select(r => r.Reason).ToList();

                model.Add(new CasesListViewModel
                {
                    CaseId = c.CaseId,
                    ReportedItemsCount = c.ReportedComments.Count + c.ReportedPosts.Count, 
                    ReportReasons = reasons,
                    ReportsCount = reports.Count,
                    Username = db.Users.Find(c.UserId).UserName
                }) ;
            }
            ViewBag.Count = cases.Count;

            return View(model);
        }

        public ActionResult MyOpenCases()
        {
            var modId = User.Identity.GetUserId();

            var cases = db.Cases.Include(c=>c.ReportedComments).Include(c=> c.ReportedPosts).Where(c => !c.IsSolved && c.ModeratorId.Equals(modId)).ToList();

            var model = new List<CasesListViewModel>();

            foreach (Case c in cases)
            {
                //get all the reasons for the reports from the case id
                var reports = db.Reports.Include(r => r.Reasons).Where(r => r.CaseId == c.CaseId).ToList();
                var reasons = reports.SelectMany(r => r.Reasons).Distinct().Select(r => r.Reason).ToList();

                model.Add(new CasesListViewModel
                {
                    CaseId = c.CaseId,
                    ReportedItemsCount = c.ReportedComments.Count + c.ReportedPosts.Count,
                    ReportReasons = reasons,
                    ReportsCount = reports.Count,
                    Username = db.Users.Find(c.UserId).UserName
                });
            }

            ViewBag.Count = cases.Count;

            return View(model);
        }

        public ActionResult MyClosedCases()
        {
            var modId = User.Identity.GetUserId();

            var cases = db.Cases.Where(c => c.IsSolved == true && c.ModeratorId.Equals(modId)).ToList();

            var model = new List<ClosedCasesListViewModel>();

            foreach (Case c in cases)
            {
                
                //var reports = db.Reports.Include(r => r.Reasons).Where(r => r.CaseId == c.CaseId).ToList();
                //var reasons = reports.SelectMany(r => r.Reasons).Distinct().Select(r => r.Reason).ToList();

                string verdict = "Guilty";
                if (!c.IsGuilty)
                {
                    verdict = "Innocent";
                }

                model.Add(new ClosedCasesListViewModel
                {
                    CaseId = c.CaseId,
                    Username = db.Users.Find(c.UserId).UserName,
                    Reason = c.Reason,
                    Verdict = verdict
                });
            }

            ViewBag.Count = cases.Count;

            return View(model);
        }

        public ActionResult AllReports(string type)
        {
            //get comments that have been reported and are not asigned a case/ are pending
            var reportedComments = db.Comments.Include(c => c.Reports).Where(c => c.Reports.Count > 0 && c.Reports.Any(r=> r.Status == Status.Pending) && (c.Reports.Any(rs=>rs.CaseId <= 0) || c.Reports.Any(rs=>rs.CaseId == null))).ToList();

            //get posts that have been reported and are not asigned a case/ are pending
            var reportedPosts = db.Posts.Include(p => p.Reports).Where(c => c.Reports.Count > 0 && c.Reports.Any(r => r.Status == Status.Pending) && (c.Reports.Any(rs => rs.CaseId <= 0) || c.Reports.Any(rs => rs.CaseId == null))).ToList();

            //add all reported items to the viewmodel
            var model = new List<AllReportsViewModel>();
            if(string.IsNullOrEmpty(type)||type.Equals("Comment"))
            {
                foreach (Comment c in reportedComments)
                {
                    //use a dictonary to get the reasons only once
                    Dictionary<int, string> reasons =  db.ReportReasons.Where(rr=>rr.Reports.Any(r => r.CommentId == c.CommentID)).ToDictionary(rr=>rr.ReportReasonId, rr=>rr.Reason);

                    model.Add(new AllReportsViewModel
                    {
                        ReportItemId = c.CommentID,
                        Username = db.Users.Find(c.UserID).Email,
                        ReportItemType = "Comment",
                        Title = c.Content,
                        ReportCount = c.Reports.Count,
                        ReportReasons = reasons.Select(r=>r.Value).ToList()
                    });
                }

            }
            if (string.IsNullOrEmpty(type) || type.Equals("Post"))
            {
                foreach (Post p in reportedPosts)
                {
                    Dictionary<int, string> reasons = db.ReportReasons.Where(rr => rr.Reports.Any(r => r.PostId == p.PostId)).ToDictionary(rr => rr.ReportReasonId, rr => rr.Reason);

                    model.Add(new AllReportsViewModel
                    {
                        ReportItemId = p.PostId,
                        Username = db.Users.Find(p.AuthorId).Email,
                        ReportItemType = "Post",
                        Title = p.Title,
                        ReportCount = p.Reports.Count,
                        ReportReasons = reasons.Select(r => r.Value).ToList()
                    });
                }
            }
            //just to say if there are no reports display a message in the view
            ViewBag.Count = model.Count;

            return View(model);
        }

        public ActionResult ViewClosedCase(int caseId)
        {
            //get the case
            var viewCase = db.Cases.Single(c => c.CaseId == caseId);
            db.Entry(viewCase).Reload();

            //check the moderator of the case
            var caseModeratorId = viewCase.ModeratorId;
            //if the moderator owns this case, check the current mod/admin trying to access it!
            if (!string.IsNullOrEmpty(caseModeratorId))
            {
                var currentModeratorId = User.Identity.GetUserId();
                if (!caseModeratorId.Equals(currentModeratorId))
                {
                    return RedirectToAction("Dashboard", "Dashboard");
                }
            }

            //get a list of the cases reported comments and posts
            var reportedComments = viewCase.ReportedComments;
            var reportedPosts = viewCase.ReportedPosts;
            //ge the offender
            var offender = db.Users.Find(viewCase.UserId);

            var reportsCount = 0;
            //create the individial reported items models into a list for the case view model
            var reportedItemModel = new List<CaseReportItemDetailsViewModel>();
            //go through both comments and posts to add their relavent information
            foreach (Comment c in reportedComments)
            {
                db.Entry(c).Reload();
                //for each one get their reports
                //and teh reports reasons
                //and stick them in their viewmodel to add to the individual reported item vm
                var reports = db.Reports.Include(r => r.Reasons).Where(r => r.CommentId == c.CommentID).ToList();
                reportsCount += reports.Count;
                var reportsModel = new List<ReportDetailViewModel>();
                foreach (Report r in reports)
                {
                    var reasons = r.Reasons.Distinct().Select(rr => rr.Reason).ToList();
                    reportsModel.Add(new ReportDetailViewModel
                    {
                        ReportId = r.ReportId,
                        ExtraInfo = string.IsNullOrEmpty(r.ExtraInformation) ? "None" : r.ExtraInformation,
                        Reasons = reasons
                    });
                }
                reportedItemModel.Add(new CaseReportItemDetailsViewModel
                {
                    Content = c.Content,
                    Reports = reportsModel,
                    PreviousCases = db.Cases.Include(cc => cc.ReportedComments).Include(cc => cc.ReportedPosts).Where(cc => cc.UserId.Equals(offender.Id) && cc.IsGuilty).ToList(),
                    ReportItemId = c.CommentID,
                    ReportCount = reportsModel.Count,
                    Type = "Comment"
                });
            }

            //same as above but for posts
            foreach (Post p in reportedPosts)
            {
                db.Entry(p).Reload();

                var reports = db.Reports.Include(r => r.Reasons).Where(r => r.PostId == p.PostId).ToList();
                reportsCount += reports.Count;
                var reportsModel = new List<ReportDetailViewModel>();
                foreach (Report r in reports)
                {
                    var reasons = r.Reasons.Distinct().Select(rr => rr.Reason).ToList();
                    reportsModel.Add(new ReportDetailViewModel
                    {
                        ReportId = r.ReportId,
                        ExtraInfo = string.IsNullOrEmpty(r.ExtraInformation) ? "None" : r.ExtraInformation,
                        Reasons = reasons
                    });
                }
                reportedItemModel.Add(new CaseReportItemDetailsViewModel
                {
                    Content = p.PostContent,
                    Reports = reportsModel,
                    PreviousCases = db.Cases.Include(cc => cc.ReportedComments).Include(cc => cc.ReportedPosts).Where(cc => cc.UserId.Equals(offender.Id) && cc.IsGuilty).ToList(),
                    ReportItemId = p.PostId,
                    ReportCount = reportsModel.Count,
                    Type = "Post"
                });
            }

            //the main view model to add
            var model = new ClosedCaseDetailsViewModel
            {
                ReportsItems = reportedItemModel,
                CaseId = caseId,
                Offender = offender,
                NumberOfItems = reportedItemModel.Count,
                NumberOfReports = reportsCount,
                Verdict = viewCase.IsSolved ? "Innocent" : "Guilty",
                Reason = viewCase.Reason
            };

            return View(model);
        }

        /// <summary>
        ///has all the reported comments/posts 
        ///can view all information and reports
        ///can pass judgment
        /// </summary>
        public ActionResult ViewCase(int caseId)
        {
            //get the case
            var viewCase = db.Cases.Single(c => c.CaseId == caseId);

            //check the moderator of the case
            var owned = false;
            var caseModeratorId = viewCase.ModeratorId;
            //if the moderator owns this case, check the current mod/admin trying to access it!
            if (!string.IsNullOrEmpty(caseModeratorId))
            {
                var currentModeratorId = User.Identity.GetUserId();
                if (!caseModeratorId.Equals(currentModeratorId))
                {
                    return RedirectToAction("Dashboard", "Dashboard");
                }
                owned = true;
            }
            //get a list of the cases reported comments and posts
            var reportedComments = viewCase.ReportedComments;
            var reportedPosts = viewCase.ReportedPosts;
            //ge the offender
            var offender = db.Users.Find(viewCase.UserId);

            var reportsCount = 0;
            //create the individial reported items models into a list for the case view model
            var reportedItemModel = new List<CaseReportItemDetailsViewModel>();
            //go through both comments and posts to add their relavent information
            foreach(Comment c in reportedComments)
            {
                //for each one get their reports
                //and teh reports reasons
                //and stick them in their viewmodel to add to the individual reported item vm
                var reports = db.Reports.Include(r => r.Reasons).Where(r => r.CommentId == c.CommentID).ToList();
                reportsCount += reports.Count;
                var reportsModel = new List<ReportDetailViewModel>();
                foreach(Report r in reports)
                {
                    var reasons = r.Reasons.Distinct().Select(rr => rr.Reason).ToList();
                    reportsModel.Add(new ReportDetailViewModel
                    {
                        ReportId = r.ReportId,
                        ExtraInfo = string.IsNullOrEmpty(r.ExtraInformation) ? "None" : r.ExtraInformation,
                        Reasons = reasons
                    });
                }
                reportedItemModel.Add(new CaseReportItemDetailsViewModel
                {
                    Content = c.Content,
                    Reports = reportsModel,
                    PreviousCases = db.Cases.Include(cc => cc.ReportedComments).Include(cc => cc.ReportedPosts).Where(cc => cc.UserId.Equals(offender.Id) && cc.IsGuilty).ToList(),
                    ReportItemId = c.CommentID,
                    ReportCount = reportsModel.Count,
                    Type = "Comment"
                });
            }

            //same as above but for posts
            foreach (Post p in reportedPosts)
            {
                var reports = db.Reports.Include(r => r.Reasons).Where(r => r.PostId == p.PostId).ToList();
                reportsCount += reports.Count;
                var reportsModel = new List<ReportDetailViewModel>();
                foreach (Report r in reports)
                {
                    var reasons = r.Reasons.Distinct().Select(rr => rr.Reason).ToList();
                    reportsModel.Add(new ReportDetailViewModel
                    {
                        ReportId = r.ReportId,
                        ExtraInfo = string.IsNullOrEmpty(r.ExtraInformation) ? "None" : r.ExtraInformation,
                        Reasons = reasons
                    });
                }
                reportedItemModel.Add(new CaseReportItemDetailsViewModel
                {
                    Content = p.PostContent,
                    Reports = reportsModel,
                    PreviousCases = db.Cases.Include(cc => cc.ReportedComments).Include(cc => cc.ReportedPosts).Where(cc => cc.UserId.Equals(offender.Id) && cc.IsGuilty).ToList(),
                    ReportItemId = p.PostId,
                    ReportCount = reportsModel.Count,
                    Type = "Post"
                });
            }

            //the main view model to add
            var model = new CaseDetailsViewModel
            {
                ReportsItems = reportedItemModel,
                CaseId = caseId,
                Offender = offender,
                NumberOfItems = reportedItemModel.Count,
                NumberOfReports = reportsCount,
                Owned = owned
            };

            return View(model);
        }

        /// <summary>
        /// Allocates a moderator/admin to the case
        /// </summary>
        public JsonResult AcceptCase(int caseId)
        {
            var theCase = db.Cases.Find(caseId);
            if(theCase == null)
            {
                return Json(new { Success = false, Message = "Case does not exist!?" });
            }
            var modId = User.Identity.GetUserId();
            var mod = db.Admins.Find(modId);

            if(mod == null)
            {
                return Json(new { Success = false, Message = "You do not exist!?" });
            }

            if (!string.IsNullOrEmpty(theCase.ModeratorId))
            {
                return Json(new { Success = false, Message = "A moderator already owns this case!" });
            }

            theCase.Moderator = mod;
            db.Entry(theCase).State = EntityState.Modified;
            if (db.SaveChanges() > 0)
            {
                return Json(new { Success = true, Message = "You have taken the case!" });
            }
            return Json(new { Success = false, Message = "Something went wrong! Please try again later." });
        }

        /// <summary>
        /// Remove individual reported item (post/comment) from a generated case
        /// If it is the lsat item to be removed from a case it will nopt be allowed
        /// </summary>
        public JsonResult RemoveReportedItem(int reportItemId, string type, int caseId, string reason)
        {
            //find the case
            var theCase = db.Cases.Find(caseId);
            if(theCase == null)
            {
                return Json(new { Success = false, Message = "Cannot find case!" });
            }
            //check it is not the last reported item in  the case
            if(theCase.ReportedPosts.Count + theCase.ReportedComments.Count == 1)
            {
                return Json(new { Success = false, Message = "Cannot remove last report item, please finish the case by condeming or ignoring the case!" });
            }

            //make a new case to save
            var modId = User.Identity.GetUserId();
            var mod = db.Users.OfType<Admin>().Single(m => m.Id.Equals(modId));
            var user = db.Users.Find(theCase.UserId);
            var newCase = new Case
            {
                IsGuilty = false,
                IsSolved = true,
                Moderator = mod,
                Punsihment = null,
                Reason = reason,
                User = user
            };

            //find the reported item and "solve" the reports
            if (type.Equals("Comment"))
            {
                var comment = db.Comments.Include(c=>c.Reports).Single(c=>c.CommentID == reportItemId);
                if (comment == null)
                {
                    return Json(new { Success = false, Message = "Cannot find comment!" });
                }

                foreach(Report r in comment.Reports)
                {
                    r.Status = Status.Solved;
                    db.Entry(r).State = EntityState.Modified;
                }
                comment.Immune = true;
                theCase.ReportedComments.Remove(comment);
                db.Entry(theCase).State = EntityState.Modified;
                db.Entry(comment).State = EntityState.Modified;
                
                newCase.ReportedComments = new List<Comment> { comment };
                
            }
            else if(type.Equals("Post"))
            {
                var post = db.Posts.Include(p => p.Reports).Single(p => p.PostId == reportItemId);
                if (post == null)
                {
                    return Json(new { Success = false, Message = "Cannot find post!" });
                }
                foreach (Report r in post.Reports)
                {
                    r.Status = Status.Solved;
                    db.Entry(r).State = EntityState.Modified;
                }
                post.Immune = true;
                theCase.ReportedPosts.Remove(post);
                db.Entry(theCase).State = EntityState.Modified;
                db.Entry(post).State = EntityState.Modified;

                newCase.ReportedPosts = new List<Post> { post };

            }
            //add the new solved case
            db.Cases.Add(newCase);
            //try catch, should do this more!
            try
            {
                if (db.SaveChanges() > 0)
                {
                    return Json(new { Success = true, Message = "Success!" });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return Json(new { Success = false, Message = e.Message });
            }

            return Json(new { Success = false, Message = "Something went wrong!" });
        }

        /// <summary>
        /// Update the case to innocent
        /// </summary>
        /// <returns></returns>
        public JsonResult CaseVerdictInnocent(int caseId, string reason)
        {
            //delete all records ascociated with the case
            //and remove the case itself
            var theCase = db.Cases.Include(c => c.ReportedComments).Include(c => c.ReportedPosts).Single(c => c.CaseId == caseId);
            if(theCase == null)
            {
                // the case does not exist!?!
                return Json(new { Success = false, Message = "Case does not exist" });
            }

            //set the case as solved
            theCase.IsGuilty = false;
            theCase.IsSolved = true;
            theCase.Reason = reason;

            //delete reports PLEASE WORK!
            var commentReports = db.Reports.Where(r => theCase.ReportedComments.Any(rc => rc.CommentID == r.CommentId)).ToList();
            var postReports = db.Reports.Where(r => theCase.ReportedPosts.Any(rp => rp.PostId == r.PostId)).ToList();

            foreach (Report r in commentReports)
            {
                db.Reports.Remove(r);
            }
            foreach (Report r in postReports)
            {
                db.Reports.Remove(r);
            }

            db.Entry(theCase).State = EntityState.Modified;

            if(db.SaveChanges() > 0)
            {
                return Json(new { Success = true, Message = "Case Updated!" });
                //then redirect using Jqueryt to dashboard
            }

            return Json(new { Success = false, Message = "Something went wrong, please try again!" });
        }

        /// <summary>
        /// Update the case to guilty, give reasons and message to user, with optional punishment time (in added days from now)
        /// </summary>
        /// <returns></returns>
        public JsonResult CaseVerdictGuilty(int caseId, int punishment, string reason, string userMessage, int? numberOfDays)
        {
            //get the case and user
            
            //set the case to complete
            //set the outcome and reason
            //set the reports to solved

            //get the case 
            var theCase = db.Cases.Include(c=>c.ReportedComments).Include(c=>c.ReportedPosts).Single(c=>c.CaseId == caseId);
            if(theCase == null)
            {
                // the case does not exist!?!
                return Json(new { Success = false, Message = "Case does not exist" });
            }
            //get the user/offender
            var offender = db.Users.Find(theCase.UserId);
            if(offender == null)
            {
                return Json(new { Success = false , Message = "User not found"});
            }

            //PUNSIH THEM!
            if (!UserPunishment(offender, punishment, numberOfDays, userMessage))
            {
                return Json(new { Success = false, Message = "User punishment failure! Please try again later!" });
            }

            //get all teh reports
            var reports = db.Reports.Where(r => r.CaseId == caseId).ToList();
            foreach (Report r in reports)
            {
                r.Status = Status.Solved;
            }

            //delete reports PLEASE WORK!
            var reportedComments = theCase.ReportedComments.ToList();
            var reportedPosts = theCase.ReportedPosts.ToList();

            ////if it is a comment, delete it, if it is a post. push to draft and flag for change
            // all comments deleted/removed
            foreach(Comment c in reportedComments)
            {
                if (c.Replies.Count <= 0)
                {
                    //if the comment hgas no replies delete it safely
                    db.Comments.Remove(c);
                }
                else
                {
                    //if the comment has replies change the state of the comment
                    c.DELTED = true;
                    c.Content = "DELETED";

                    db.Entry(c).State = EntityState.Modified;
                }
            }
            //all posts taken down and put into draft
            foreach(Post p in reportedPosts)
            {
                p.Draft = true;
                p.Flagged = true;
                db.Entry(p).State = EntityState.Modified;
            }

            //update case
            theCase.Reason = reason;
            theCase.IsGuilty = true;
            theCase.IsSolved = true;

            //states
            db.Entry(theCase).State = EntityState.Modified;
            //db.Entry(offender).State = EntityState.Modified;

            //TO THINK:
            //posts and comments can be individually guilty and not each one will require "justice"
            //we should excuse or condem the reported items individually, or remove them from the case
            //this way we can punish the user and refrain from deleting post/comments that within the case
            //do not break rules!

            //save
            if (db.SaveChanges() > 0)
            {
                return Json(new { Success = true, Message = "Success!" });
            }

            return Json(new { Success = false, Message = "Something went wrong, please try again!" });
        }

        /// <summary>
        /// display the reported comment with reasons
        /// and allow mod to forgive or punish the user
        /// </summary>
        public ActionResult ViewReportedComment(int id)
        {
            //get the reported comment
            var comment = db.Comments.Include(c => c.Reports).Single(c => c.CommentID == id);
            if(comment == null)
            {
                return new HttpNotFoundResult();
            }
            //get offender
            var user = db.Users.Find(comment.UserID);
            //get any previous cases to display
            var previousCases = db.Cases.Include(c => c.ReportedComments).Include(c => c.ReportedPosts).Where(c => c.UserId.Equals(user.Id) && c.IsGuilty).ToList();

            //get the parrent comment by going through ( if necessary) the parent commments
            Comment parentComment = comment;
            while(parentComment.PostId < 1)
            {
                parentComment = db.Comments.Find(parentComment.ParentCommentId);
            }
            var parentPost = db.Posts.Find(parentComment.PostId);

            var t = parentPost.GetType().Name;

            //create the model
            var model = new ReportItemDetailsViewModel
            {
                Content = comment.Content,
                Offender = user,
                ReportItemId = id,
                ReportCount = comment.Reports.Count,
                Reports = new List<ReportDetailViewModel>(),
                PreviousCases = previousCases,
                Type = t.Contains("Rev") ? "Review" : t.Contains("Blog") ? "Blog" : "News",
                Cont = t.Contains("Rev") ? "Reviews" : t.Contains("Blog") ? "Blogs" : "News",
                ParentPostId = parentPost.PostId
            };
            //add all the reports to be viewed
            foreach(Report r in comment.Reports)
            {
                //get the report with reasons
                var report = db.Reports.Include(rr => rr.Reasons).Single(rr => rr.ReportId == r.ReportId);
                //get the reasons! make it distinct as to not repeat ourselves
                var reasons = report.Reasons.Distinct().Select(rr => rr.Reason).ToList();
                
                model.Reports.Add(new ReportDetailViewModel
                {
                    ReportId = r.ReportId,
                    Reasons = reasons,
                    ExtraInfo = string.IsNullOrEmpty(r.ExtraInformation) ? "None" : r.ExtraInformation
                });
            }

            return View(model);
        }

        /// <summary>
        /// The reported comment is guilty of violating the websites commenting/psoting condfitions 
        /// and is given a punishment dictated by the admin/mod
        /// </summary>
        [HttpPost]
        public JsonResult GuiltyReportedComment(int commentId,int punishment, string reason, string message, int numberOfDays)
        {
            //get comment
            var comment = db.Comments.Include(c => c.Reports).Include(c=>c.Replies).Single(c => c.CommentID == commentId);
            //user
            var user = db.Users.Find(comment.UserID);
            //cehck ocmment exists!
            if(comment == null)
            {
                return Json(new { Success = false, Message = "Cannot find comment." });
            }
            if(user == null)
            {
                return Json(new { Success = false, Message = "Cannot find user." });
            }
            //mark reports as complete
            var reports = comment.Reports;

            foreach(Report r in reports)
            {
                r.Status = Status.Solved;
            }
            if (!UserPunishment(user, punishment, numberOfDays, message)) 
            {
                return Json(new { Success = false, Message = "Something went wrong" });
            }

            //delete comment
            if (comment.Replies.Count <= 0)
            {
                //if the comment hgas no replies delete it safely
                db.Comments.Remove(comment);
            }
            else
            {
                //if the comment has replies change the state of the comment
                comment.DELTED = true;
                comment.Content = "DELETED";

                db.Entry(comment).State = EntityState.Modified;
            }

            

            //make a case to store the report and its outcome
            var modId = User.Identity.GetUserId();
            var mod = db.Users.OfType<Admin>().Single(u=> u.Id.Equals(modId));

            var aCase = new Case
            {
                IsGuilty = true,
                IsSolved = true,
                Reason = reason,
                Moderator = mod,
                User = user,
                Punsihment = (Punishment)punishment,
            };

            db.Cases.Add(aCase);

            if (db.SaveChanges() > 0)
            {
                return Json(new { Success = true, Message = "Report succesfully processed!" });
            }

            return Json(new { Success = false, Message = "Something went wrong! Please try again later!" });
        }
        /// <summary>
        /// The reported comment is innocent of violating the websites commenting/psoting condfitions 
        /// and is given immunity from further reports - perhaps a much larger limit
        /// </summary>
        [HttpPost]
        public JsonResult InnocentReportedComment(int commentId, string reason)
        {
            //get the comments
            var comment = db.Comments.Include(c => c.Reports).Single(c => c.CommentID == commentId);

            //get the admin/mod
            var modId = User.Identity.GetUserId();
            var mod = db.Users.OfType<Admin>().Single(m => m.Id.Equals(modId));
            var user = db.Users.Single(m => m.Id.Equals(comment.UserID));

            //check stuff
            if (comment == null)
            {
                return Json(new { Success = false, Message = "Comment does not exist?!" });
            }
            if (mod == null)
            {
                return Json(new { Success = false, Message = "You dont exist?!" });
            }
            if (user == null)
            {
                return Json(new { Success = false, Message = "The offender doesn't exist?!" });
            }

            var reports = comment.Reports;
            //create a case and solve it
            var theCase = new Case
            {
                IsGuilty = false,
                IsSolved = true,
                Moderator = mod,
                User = user,
                Reason = reason,
                ReportedComments = new List<Comment> { comment },
            };
            db.Cases.Add(theCase);
            //set the reports to sovled
            foreach(Report r in reports)
            {
                r.Status = Status.Solved;
                db.Entry(r).State = EntityState.Modified;
            }

            comment.Immune = true;

            db.Entry(comment).State = EntityState.Modified;

            if (db.SaveChanges() > 0)
            {
                return Json(new { Success = true, Message = "Comment Succesfully absolved!" });
            }
            else
            {
                return Json(new { Success = false, Message = "Failed to save in DB, please try again later!" });
            }
        }

        /// <summary>
        /// display the reported post with reasons
        /// </summary>
        public ActionResult ViewReportedPost(int id)
        {
            //TODO same as reported comment.. but slightly different
            //get the post
            var post = db.Posts.Include(p => p.Reports).Single(p => p.PostId == id);

            if (post == null)
            {
                return new HttpNotFoundResult();
            }
            //get offender
            var user = db.Users.Find(post.AuthorId);
            //get any previous cases to display
            var previousCases = db.Cases.Include(c => c.ReportedComments).Include(c => c.ReportedPosts).Where(c => c.UserId.Equals(user.Id) && c.IsGuilty).ToList();

            var t = post.GetType().Name;
            //create the model
            var model = new ReportItemDetailsViewModel
            {
                Content = post.PostContent,
                Offender = user,
                ReportItemId = id,
                ReportCount = post.Reports.Count,
                Reports = new List<ReportDetailViewModel>(),
                PreviousCases = previousCases,
                Type = t.Contains("Rev") ? "Review" : t.Contains("Blog") ? "Blog" : "News",
                Cont = t.Contains("Rev") ? "Reviews" : t.Contains("Blog") ? "Blogs" : "News"
            };
            //add all the reports to be viewed
            foreach (Report r in post.Reports)
            {
                //get the report with reasons
                var report = db.Reports.Include(rr => rr.Reasons).Single(rr => rr.ReportId == r.ReportId);
                //get the reasons! make it distinct as to not repeat ourselves
                var reasons = report.Reasons.Distinct().Select(rr => rr.Reason).ToList();

                model.Reports.Add(new ReportDetailViewModel
                {
                    ReportId = r.ReportId,
                    Reasons = reasons,
                    ExtraInfo = string.IsNullOrEmpty(r.ExtraInformation) ? "None" : r.ExtraInformation
                });
            }

            return View(model);
        }
        /// <summary>
        /// Condem the reported post and set its status to unpublished, draft and flagged for editing
        /// </summary>
        [HttpPost]
        public JsonResult GuiltyReportedPost(int postId, string reason, string message)
        {
            //get comment
            var post = db.Posts.Include(c => c.Reports).Single(c => c.PostId == postId);
            //user
            var author = db.Users.Find(post.AuthorId);
            //cehck ocmment exists!
            if (post == null)
            {
                return Json(new { Success = false, Message = "Cannot find post." });
            }
            if (author == null)
            {
                return Json(new { Success = false, Message = "Cannot find user." });
            }
            //mark reports as complete
            var reports = post.Reports;

            foreach (Report r in reports)
            {
                r.Status = Status.Solved;
            }

            //make a case to store the report and its outcome
            var modId = User.Identity.GetUserId();
            var mod = db.Users.OfType<Admin>().Single(u => u.Id.Equals(modId));

            var aCase = new Case
            {
                IsGuilty = true,
                IsSolved = true,
                Reason = reason,
                Moderator = mod,
                User = author,
                Punsihment = Punishment.WARNING,
            };

            db.Cases.Add(aCase);
            post.Flagged = true;
            post.Published = false;
            post.Draft = true;

            var postType = post.GetType().Name;
            if (postType.Contains("_"))
            {
                postType = postType.Substring(0, postType.LastIndexOf("_"));
            }
            //notification
            var noti = new Notification
            {
                CreatedAt = DateTime.Now,
                Message = "One of your posts: " + post.Title + " has been taken down for: " + message,
                User = author,
                //Destination = "/" + (postType.Equals("News") ? "News" : postType + "s") + "/View" + postType + "?postId=" + postId
                Destination = "/Author/MyDraftPosts"
            };

            db.Notifications.Add(noti);

            db.Entry(post).State = EntityState.Modified;

            if (db.SaveChanges() > 0)
            {
                return Json(new { Success = true, Message = "Report succesfully processed!" });
            }

            return Json(new { Success = false, Message = "Something went wrong! Please try again later!" });
        }
        /// <summary>
        /// The reported comment is innocent of violating the websites commenting/psoting condfitions 
        /// and is given immunity from further reports - perhaps a much larger limit
        /// </summary>
        [HttpPost]
        public JsonResult InnocentReportedPost(int postId, string reason)
        {
            //get the comments
            var post = db.Posts.Include(c => c.Reports).Single(c => c.PostId == postId);

            //get the admin/mod
            var modId = User.Identity.GetUserId();
            var mod = db.Users.OfType<Admin>().Single(m => m.Id.Equals(modId));
            var user = db.Users.Single(m => m.Id.Equals(post.AuthorId));

            //check stuff
            if (post == null)
            {
                return Json(new { Success = false, Message = "Comment does not exist?!" });
            }
            if (mod == null)
            {
                return Json(new { Success = false, Message = "You dont exist?!" });
            }
            if (user == null)
            {
                return Json(new { Success = false, Message = "The offender doesn't exist?!" });
            }

            var reports = post.Reports;
            //create a case and solve it
            var theCase = new Case
            {
                IsGuilty = false,
                IsSolved = true,
                Moderator = mod,
                User = user,
                Reason = reason,
                ReportedPosts = new List<Post> { post },
            };
            db.Cases.Add(theCase);
            //set the reports to sovled
            foreach (Report r in reports)
            {
                r.Status = Status.Solved;
                db.Entry(r).State = EntityState.Modified;
            }

            post.Immune = true;

            db.Entry(post).State = EntityState.Modified;

            if (db.SaveChanges() > 0)
            {
                return Json(new { Success = true, Message = "Comment Succesfully absolved!" });
            }
            else
            {
                return Json(new { Success = false, Message = "Failed to save in DB, please try again later!" });
            }
        }

        /// <summary>
        /// A method to exact JUSTICE! upon a user
        /// </summary>
        /// <param name="user"> the offender being punished</param>
        /// <param name="punishment">the type of punishment</param>
        /// <param name="numberOfDays">how long the punishment should last for</param>
        /// <param name="message">the message a user will recieve explaing what has happened</param>
        /// <returns></returns>
        private bool UserPunishment(User user, int punishment, int? numberOfDays, string message)
        {
            //depending on punishment exact it
            //Warning, notify the user of a warning and increment the number of warnings
            //Blocking, notify the user of comment ability blocking, why, and how long
            //Suspension, tell the user they are blocked, why, and, how long

            //notify user of outcome!
            var noti = new Notification()
            {
                User = user,
                CreatedAt = DateTime.Now,
                Destination = "#",
            };

            //punishment - move me to seperate method
            if (punishment == (int)Punishment.WARNING)
            {
                noti.Message = "You have recieved a warning!";
                user.NumberOfWarnings++;
            }
            else
            {
                //user manager to add roles
                UserManager<User> um = new UserManager<User>(new UserStore<User>(db));

                //if the mod has specified the number of days
                if (numberOfDays > 0)
                {
                    //if mod selected to comment block
                    if (punishment == (int)Punishment.BLOCKED)
                    {
                        noti.Message = "You have been blocked from commenting!";
                        user.NumberOfBlockings++;
                        user.CommentsBlocked = true;
                        user.CommentsBlockedUntil = DateTime.Now.AddDays((int)numberOfDays);

                        um.AddToRole(user.Id, "Silenced");
                    }
                    //if mod chose suspension
                    else if (punishment == (sbyte)Punishment.SUSPENDED)
                    {
                        //user wont get this message tho, since they are suspended
                        noti.Message = "You have been suspended!";
                        user.NumberOfSuspension++;
                        user.AccountSuspended = true;
                        user.SuspensionUntil = DateTime.Now.AddDays((int)numberOfDays);
                        um.AddToRole(user.Id, "Suspended");
                    }
                }
                else
                {
                    if (punishment == (int)Punishment.BLOCKED)
                    {
                        noti.Message = "You have been blocked from commenting!";
                        user.NumberOfBlockings++;
                        user.CommentsBlocked = true;

                        //generic automated time for blocking
                        var prevBlocks = user.NumberOfBlockings;
                        var days = 1;

                        if (prevBlocks == 1)
                        {
                            days = 1;
                        }
                        if (prevBlocks == 2)
                        {
                            days = 2;
                        }
                        if (prevBlocks == 3)
                        {
                            days = 4;
                        }
                        if (prevBlocks >= 4)
                        {
                            days = 6;
                        }

                        user.CommentsBlockedUntil = DateTime.Now.AddDays(days);
                        um.AddToRole(user.Id, "Silenced");
                    }
                    else if (punishment == (int)Punishment.SUSPENDED)
                    {
                        noti.Message = "You have been suspended!";
                        user.NumberOfSuspension++;
                        user.AccountSuspended = true;

                        var prevSus = user.NumberOfSuspension;
                        var days = 1;

                        if (prevSus == 1)
                        {
                            days = 1;
                        }
                        if (prevSus == 2)
                        {
                            days = 2;
                        }
                        if (prevSus == 3)
                        {
                            days = 4;
                        }
                        if (prevSus == 4)
                        {
                            days = 6;
                        }
                        if (prevSus >= 5)
                        {
                            for (int i = 5; i < prevSus; i++)
                            {
                                days++;
                            }
                        }

                        user.SuspensionUntil = DateTime.Now.AddDays(days);
                        um.AddToRole(user.Id, "Suspended");
                    }
                }
            }
            db.Entry(user).State = EntityState.Modified;

            //add the mods message to the notification
            noti.Message += "<p class='text-truncate'>Moderator Message: " + message + "</p>";

            db.Notifications.Add(noti);
            if (db.SaveChanges() > 0)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Get all users ( not staff)
        /// </summary>
        /// <returns></returns>
        public ActionResult AllUsers()
        {
            var allUsers = db.Users.ToList();
            //oofts, really shite i assume
            var users = new List<User>();
            foreach(User u in allUsers)
            {
                var type = u.GetType().Name;
                type = type.Substring(0, type.LastIndexOf("_"));
                if(!type.Equals("Author") && !type.Equals("Admin"))
                {
                    users.Add(u);
                }
            }
            return View(users);
        }

        
        /// <summary>
        /// Directly Punish the user, without a case or report!
        /// </summary>m>
        public ActionResult PunishUser(string userId, int punishment, int? numberOfDays, string message, string reason)
        {
            User user = db.Users.Find(userId);
            var moderatorId = User.Identity.GetUserId();
            var mod = db.Users.OfType<Admin>().Single(m => m.Id.Equals(moderatorId));
            //check stuff
            if(user == null)
            {
                return Json(new { Success = false, Message = "User was not found!" });
            }
            if(mod == null)
            {
                return Json(new { Success = false, Message = "Who are you! We can not find your records!" });
            }
            

            if(UserPunishment(user, punishment, numberOfDays, message))
            {
                //create a solved case documenting reason why this action was taken!
                var leCase = new Case
                {
                    Moderator = mod,
                    User = user,
                    IsGuilty = true,
                    IsSolved = true,
                    Punsihment = (Punishment)punishment,
                    Reason = reason
                };
                db.Cases.Add(leCase);
                if (db.SaveChanges() > 0)
                {
                    return Json(new { Success = true, Message = "Succes!" });
                }

            }

            return Json(new { Success = false, Message = "Something went wrong!" });
        }

        /// <summary>
        /// pull the post directly, call REPORTEDPOST method to condemn it
        /// </summary>
        public JsonResult PullPost(int postId, string reason, string message)
        {
            var post = db.Posts.Find(postId);
            if(post == null)
            {
                return Json(new { Success = false, Message = "Post not found!" });
            }

            return GuiltyReportedPost(postId, reason, message);

            //return Json(new { Success = false, Message = "Something went wrong!" });
        }

        //BANNED WORDS and PHRASES!
        /// <summary>
        /// Get all the banned words and phrases!
        /// </summary>
        public ActionResult AllBannedContent(string type)
        {
            var phrases = db.BannedContents.ToList();
            if (!string.IsNullOrEmpty(type))
            {
                if (type.Equals("word"))
                {
                    phrases = phrases.Where(p => !p.Content.Contains(" ")).ToList();
                }
                else
                {
                    phrases = phrases.Where(p => p.Content.Contains(" ")).ToList();
                }
            }
            var model = new List<BannedPhrasesListViewModel>();

            foreach (var p in phrases)
            {
                string phraseType = "word";
                if (p.Content.Contains(" "))
                {
                    phraseType = "phrase";
                }
                model.Add(new BannedPhrasesListViewModel
                {
                    Id = p.Id,
                    Phrase = p.Content,
                    Type = phraseType
                });

            }

            return View(model);
        }
        [HttpGet]
        public ActionResult CreateBannedContent()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateBannedContent(BannedContent model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            db.BannedContents.Add(model);
            db.SaveChanges();

            return RedirectToAction("AllBannedContent");
        }
        public ActionResult DeleteBannedContent(int phraseId)
        {
            var content = db.BannedContents.Find(phraseId);
            db.BannedContents.Remove(content);
            return RedirectToAction("AllBannedContent");
        }
    }

    //ADMIN ONLY
    [Authorize(Roles = "Admin, Moderator")]
    public partial class AdminController : Controller
    {
        //STAFF
        public ActionResult AllStaff()
        {
            //get all staff
            var staff = db.Users.OfType<Author>().Include(s => s.Roles).ToList().OrderBy(u=>u.StaffSince);
            //get roles
            var roles = db.Roles.Distinct().ToDictionary(r=>r.Id, r=>r.Name);
            //remove admin/mod roles as it takes more than just adding a role
            //remove author role as that will be automatic
            var adRole = roles.Single(r => r.Value.Equals("Admin")).Key.ToString();
            var modRole = roles.Single(r => r.Value.Equals("Moderator")).Key.ToString();
            var autRole = roles.Single(r => r.Value.Equals("Author")).Key.ToString();
            var userRole = roles.Single(r => r.Value.Equals("User")).Key.ToString();

            roles.Remove(adRole);
            roles.Remove(userRole);
            roles.Remove(modRole);
            roles.Remove(autRole);

            //needs VM
            var model = new AllStaffViewModel
            {
                Staff = new List<StaffViewModel>(),
                AllRoles = roles
            };
            
            foreach (Author u in staff)
            {
                using (var userManager = new UserManager<User>(new UserStore<User>(db)))
                {
                    var userRoles =  userManager.GetRoles(u.Id);
                    //if i wanted the ids of the roles, i could use this to a dictoary again
                    //var staffRoles = roles.Where(r => userRoles.Any(ur=>ur.Equals(r.Value))).Select(r=>r.Value).ToList();

                    model.Staff.Add(new StaffViewModel
                    {
                        UserId = u.Id,
                        Name = u.FirstName + " " + u.LastName,
                        Email = u.Email,
                        JoinDate = u.StaffSince != null ? u.StaffSince.Value.ToString("d") : "n/a",
                        StaffRoles = userRoles
                    });
                }

                
            }
            return View(model);
        }

        public async Task<JsonResult> AddRole(string userId, string roleId)
        {
            //check validility
            var user = db.Users.Include(u=>u.Badges).Single(u=>u.Id.Equals(userId));
            if(user == null)
            {
                return Json(new { Success = false, Message = "Staff member not found." });
            }
            var role = db.Roles.Find(roleId);
            if (role == null)
            {
                return Json(new { Success = false, Message = "Role was not found." });
            }

            using (var userManager = new UserManager<User>(new UserStore<User>(db)))
            {
                //check the user does nto already have the role
                if (userManager.GetRoles(userId).Contains(role.Name))
                {
                    return Json(new { Success = false, Message = "Staff member was found to already have this role!" });
                }
                //if it is an author role
                //add author role if needed
                if(role.Name.Equals("Reporter") ||role.Name.Equals("Blogger") ||role.Name.Equals("Reviewer"))
                {
                    if (!userManager.GetRoles(userId).Contains("Author"))
                    {
                        await userManager.AddToRoleAsync(userId, "Author");
                    }
                }
                //otherwise just add the role
                await userManager.AddToRoleAsync(userId, role.Name);
                db.Entry(user).State = EntityState.Modified;
            }

            try
            {
                if (await db.SaveChangesAsync() > 0)
                {
                    return Json(new { Success = true, Message = "Role " + role.Name + " succesfully added to "+user.FirstName + " " + user.LastName+"." });
                }
            }catch(Exception e)
            {
                return Json(new { Success = false, Message = "Something went wrong: "+e.Message+"!" });
            }
            return Json(new { Success = false, Message = "Something went wrong!" });
        }

        public async Task<JsonResult> RemoveRole(string userId, string roleId)
        {
            //check validility
            var user = db.Users.Include(u => u.Badges).Single(u => u.Id.Equals(userId));
            if (user == null)
            {
                return Json(new { Success = false, Message = "Staff member not found." });
            }
            var role = db.Roles.Find(roleId);
            if (role == null)
            {
                return Json(new { Success = false, Message = "Role was not found." });
            }

            using (var userManager = new UserManager<User>(new UserStore<User>(db)))
            {
                var userRoles = userManager.GetRoles(userId);
                //check the user does has the role
                if (!userRoles.Contains(role.Name))
                {
                    return Json(new { Success = false, Message = "Staff member does not have this role, or it has already been removed!" });
                }

                // remove the role
                await userManager.RemoveFromRoleAsync(userId, role.Name);

                userRoles = userManager.GetRoles(userId);

                //if it is the last author role they have, remove author role
                if (!userRoles.Contains("Reporter") && !userRoles.Contains("Reviewer") && !userRoles.Contains("Blogger") )
                {
                    await userManager.RemoveFromRoleAsync(userId, "Author");
                }

                db.Entry(user).State = EntityState.Modified;
            }

            try
            {
                if (await db.SaveChangesAsync() > 0)
                {
                    return Json(new { Success = true, Message = "Role " + role.Name + " succesfully added to " + user.FirstName + " " + user.LastName + "." });
                }
            }
            catch (Exception e)
            {
                return Json(new { Success = false, Message = "Something went wrong: " + e.Message + "!" });
            }
            return Json(new { Success = false, Message = "Something went wrong!" });

        }

        [HttpGet]
        public ActionResult CreateAuthor(string type = "Author")
        {
            //get author roles
            var roles = db.Roles.Distinct()
                .Where(r=>!r.Name.Equals("Moderator") && !r.Name.Equals("Admin") && !r.Name.Equals("User") && !r.Name.Equals("Author")
                && !r.Name.Equals("Suspended") && !r.Name.Equals("Silenced"))
                .ToList();
            var list = new List<AuthorRolesViewModel>();

            foreach (var r in roles)
            {
                list.Add(new AuthorRolesViewModel
                {
                    Id = r.Id,
                    Name = r.Name,
                    IsSelected = false
                });
            }
            return View(new CreateStaffViewModel { Roles = list , Type = type});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateAuthor(CreateStaffViewModel model)
        {
            if (ModelState.IsValid)
            {
                var author = new Author
                {
                    UserName = model.Email.Trim(),
                    Email = model.Email.Trim(),
                    FirstName = model.FirstName.Trim(),
                    LastName = model.LastName.Trim(),
                    JoinDate = DateTime.Now,
                    StaffSince = DateTime.Now,
                    AccountSuspended = false,
                    CommentsBlocked = false,
                    UserImage = "/Content/Images/Profiles/Demo/defaultauthor.png"
                };

                using (var userManager = new UserManager<User>(new UserStore<User>(db)))
                {
                    var result = await userManager.CreateAsync(author, model.Password);


                    if (result.Succeeded)
                    {
                        //add to default role
                        await userManager.AddToRoleAsync(author.Id, "User");
                        await userManager.AddToRoleAsync(author.Id, "Author");

                        //add to new roles
                        foreach(var r in model.Roles)
                        {
                            
                            if (r.IsSelected)
                            {
                                var role = db.Roles.Find(r.Id);
                                if(role != null)
                                {
                                    await userManager.AddToRoleAsync(author.Id, role.Name);
                                }
                            }
                        }
                        return RedirectToAction("AllStaff");
                        //success, redirect!
                    }
                }
            }
            // If we got this far, something failed, redisplay form
            return View(model);

        }

        [HttpGet]
        public ActionResult CreateAdmin(string type = "Moderator")
        {
            //get author roles
            var roles = db.Roles.Distinct()
                .Where(r => !r.Name.Equals("Moderator") && !r.Name.Equals("Admin") && !r.Name.Equals("User") && !r.Name.Equals("Author")
                && !r.Name.Equals("Suspended") && !r.Name.Equals("Silenced"))
                .ToList();
            var list = new List<AuthorRolesViewModel>();

            foreach (var r in roles)
            {
                list.Add(new AuthorRolesViewModel
                {
                    Id = r.Id,
                    Name = r.Name,
                    IsSelected = false
                });
            }
            return View(new CreateStaffViewModel { Roles = list, Type = type });
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<ActionResult> CreateAdmin(CreateStaffViewModel model)
        {
            if (ModelState.IsValid)
            {
                var admin = new Admin
                {
                    UserName = model.Email.Trim(),
                    Email = model.Email.Trim(),
                    FirstName = model.FirstName.Trim(),
                    LastName = model.LastName.Trim(),
                    JoinDate = DateTime.Now,
                    StaffSince = DateTime.Now,
                    AccountSuspended = false,
                    CommentsBlocked = false,
                    UserImage = "/Content/Images/Profiles/Demo/defaultadmin.png"
                };

                using (var userManager = new UserManager<User>(new UserStore<User>(db)))
                {
                    var result = await userManager.CreateAsync(admin, model.Password);


                    if (result.Succeeded)
                    {
                        //add to default role
                        await userManager.AddToRoleAsync(admin.Id, "User");
                        await userManager.AddToRoleAsync(admin.Id, model.Type);
                        
                        //add to new roles
                        foreach (var r in model.Roles)
                        {
                            bool authorAdded = false;
                            if (r.IsSelected)
                            {
                                var role = db.Roles.Find(r.Id);
                                if (role != null)
                                {
                                    await userManager.AddToRoleAsync(admin.Id, role.Name);
                                    if((role.Name.Equals("Blogger") || role.Name.Equals("Reviewer") || role.Name.Equals("Reporter")) && !authorAdded)
                                    {
                                        await userManager.AddToRoleAsync(admin.Id, "Author");
                                    }
                                }
                            }
                        }
                        return RedirectToAction("AllStaff");
                        //success, redirect!
                    }
                }
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //CATEGORIES
        /// <summary>
        /// get all categories for admin to create new or modify
        /// </summary>
        public ActionResult AllCategories(string type = "")
        {
            var categories = db.Categories.ToList();
            var revCats = db.ReviewCategories.ToList();
            var genres = db.Genres.ToList();

            var model = new List<AllCategoriesViewModel>();

            if (type.Equals("Category") || string.IsNullOrEmpty(type))
            {
                foreach (Category c in categories)
                {
                    model.Add(new AllCategoriesViewModel
                    {
                        CatId = c.CategoryId,
                        Type = "Blog Category",
                        Name = c.Name
                    });
                }
            }
            if (type.Equals("ReviewCategory") || string.IsNullOrEmpty(type))
            {
                foreach (ReviewCategory c in revCats)
                {
                    model.Add(new AllCategoriesViewModel
                    {
                        CatId = c.Id,
                        Type = "Review Category",
                        Name = c.Name
                    });
                }
            }
            if (type.Equals("Genre") || string.IsNullOrEmpty(type))
            {

                foreach (Genre c in genres)
                {
                    model.Add(new AllCategoriesViewModel
                    {
                        CatId = c.GenreId,
                        Type = "Genre",
                        Name = c.Name
                    });
                }
            }
            switch (type)
            {
                case "Genre":
                    ViewBag.Selected = "Genre";
                    break;
                case "Category":
                    ViewBag.Selected = "Category";
                    break;
                case "ReviewCategory":
                    ViewBag.Selected = "ReviewCategory";
                    break;
                default:
                    ViewBag.Selected = "";
                    break;
            }

            return View(model);
        }
        public ActionResult CreateCategory()
        {
            return Json(new { Success = false, Message = "Something went wrong!" });
        }
        public ActionResult EditCategory()
        {
            return Json(new { Success = false, Message = "Something went wrong!" });
        }
        public ActionResult DeleteCategory()
        {
            return Json(new { Success = false, Message = "Something went wrong!" });
        }

        //BADGES!
        [Authorize(Roles = "Admin")]
        public ActionResult AllBadges()
        {
            var badges = db.Badges.ToList();


            return View(badges);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public ActionResult CreateBadge()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateBadge([Bind(Include ="BadgeName,Description,ImageLocation")]Badge model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            db.Badges.Add(model);
            db.SaveChanges();

            if (!String.IsNullOrEmpty(model.ImageLocation))
            {
                HttpPostedFileBase ImageLocation = Request.Files["ImageLocation"];
                //add image
                if (ImageLocation != null && ImageLocation.ContentLength > 0)
                {
                    try
                    {
                        string extension = System.IO.Path.GetExtension(ImageLocation.FileName);
                        string fileName = model.BadgeName.ToLower() + "_"  + extension;
                        string path = Path.Combine(Server.MapPath(@"~/Content/Images/Badges/"), fileName);

                        if (System.IO.File.Exists(path))
                        {
                            System.IO.File.Delete(path);
                        }

                        ImageLocation.SaveAs(path);
                        model.ImageLocation = @"~/Content/Images/Badges/" + fileName;
                        db.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        string error = e.Message.ToString();
                        ViewBag.ImageError = e.Message;
                        return View(model);
                    }
                }
            }

            return RedirectToAction("AllBadges");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult EditBadge(int badgeId)
        {
            var badge = db.Badges.Find(badgeId);

            return View();
        }

        [Authorize(Roles = "Admin")]
        public ActionResult DeleteBadge(int badgeId)
        {
            var badge = db.Badges.Find(badgeId);

            return View();
        }


    }
}