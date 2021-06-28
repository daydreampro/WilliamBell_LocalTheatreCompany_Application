using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using WilliamBell_LTC_Application.Models;
using WilliamBell_LTC_Application.Models.ViewModels;
using WilliamBell_LTC_Application.Models.DAL;
using Microsoft.AspNet.Identity.Owin;
using System.Data.Entity;
using System.IO;
using System.Threading.Tasks;

namespace WilliamBell_LTC_Application.Controllers
{
    public class PostsController : Controller
    {
        private LTCContext db = new LTCContext();

        public ActionResult Archive(string typeId, string search)
        {
            //get the sub classes of POST to list
            var subclasstypes = Assembly.GetAssembly(typeof(Post)).GetTypes().Where(p => p.IsSubclassOf(typeof(Post))).ToList();
            //put them into a select list
            var subClassList = subclasstypes.Select(x => new SelectListItem()
            {
                Value = x.GUID.ToString(),
                Text = x.Name
            });
            //send it to the view as a select list
            ViewBag.SubClasses = subClassList;
            //send all other categories/genres
            ViewBag.BlogCats = db.Categories.ToList();
            ViewBag.ReviewCats = db.ReviewCategories.ToList();
            ViewBag.Genres = db.Genres.ToList();

            //create a non dynamic list of type posts
            var nonDynamic = db.Posts.ToList().Where(p => p.Published==true).OrderByDescending(p => p.DatePosted);
            //cast the posts to a dynamic list for filtering
            IList<dynamic> posts = nonDynamic.Select(x => (dynamic)x).ToList();
            
            //filters posts to based on the type selected, using black magic
            if (typeId != null)
            {
                Type subClass = subclasstypes.Where(sc => sc.GUID.ToString().Equals(typeId)).FirstOrDefault();

                MethodInfo method = typeof(Queryable).GetMethod("OfType");
                MethodInfo generic = method.MakeGenericMethod(new Type[] { subClass });

                var results = (IEnumerable<dynamic>)generic.Invoke(null, new dynamic[] { db.Posts.Where(p=>p.Published) });
                posts = results.ToList();
            }

            //filter the posts based on a search parametre
            if (!string.IsNullOrEmpty(search))
            {
                posts = posts.Where(p => p.Title.Contains(search)).ToList();
            }
            

            //finally load the view model with the posts
            var model = new List<ArchiveListViewModel>();
            //check agains teh release date
            var now = DateTime.Now;

            foreach (var p in posts)
            {
                //some janky anit fuck-upery
                //trying to make sure if the controller name is both plural and singular (many News posts and one news post)
                //to trim the controller name

                //could extend for more difficult plurals (categories vs category)
                string type = p.GetType().Name.Substring(0, p.GetType().Name.LastIndexOf("_"));
                string cont = type + "s";

                if (cont[cont.LastIndexOf("s")-1] == 's')
                {
                    cont = cont.Substring(0, cont.LastIndexOf("s"));
                }
                if(p.DatePosted <= now)
                {
                    model.Add(new ArchiveListViewModel
                    {
                        AuthorName = p.Author.FirstName + " " + p.Author.LastName,
                        Image = p.FeatureImagePath,
                        Synopsis = TextTruncate(p.Synopsis,150),
                        NumberOfComments = CommentsCount(p.PostId),
                        Posted = p.DatePosted.ToString(),
                        Title = p.Title,
                        PostId = p.PostId,
                        PostType = type,
                        ControllerName = cont
                    });
                }
                
            }
            //doing some pagnation stuff
            ViewBag.PostsCount = model.Count();

            return View(model);
        }

        [Authorize(Roles = "Admin, Author,Editor")]
        public ActionResult CreateReview(int? postId)
        {
            var gens = db.Genres.ToList();
            var revCats = db.ReviewCategories.ToList();

            var model = new CreateReviewViewModel
            {
                Genres = gens,
                ReviewCategories = revCats,
                LastSaved = "Not Saved Yet"
            };

            if (postId != null)
            {
                var post = db.Posts.OfType<Review>().Include(p=>p.ReviewCategories).Include(p=>p.Genres).Single(p => p.PostId == postId);
                if (post != null)
                {
                    model.PostId = (int)postId;
                    model.Title = post.Title;
                    model.Synopsis = post.Synopsis;
                    model.FeatureImage = post.FeatureImagePath;
                    model.Content = post.PostContent;
                    model.Score = post.Score;
                    model.LastSaved = post.DatePosted.ToString("f") ?? "Not saved yet";
                    //send the selected genres and cats back to be highlighted
                    model.SelectedCategories = post.ReviewCategories.Select(rc => rc.Id).ToArray();
                    model.SelectedGenres = post.Genres.Select(g => g.GenreId).ToArray();
                }
            }


            return View(model);
        }

        [Authorize(Roles = "Admin, Author,Editor")]
        public ActionResult CreateBlog(int? postId)
        {
            var cats = db.Categories.ToList();

            var model = new CreateBlogViewModel
            {
                Categories = cats,
                LastSaved = "Not saved yet"
            };

            if(postId != null)
            {
                var post = db.Posts.OfType<Blog>().Include(p => p.Categories).Single(p => p.PostId == postId);

                if (post != null)
                {
                    model.PostId = (int)postId;
                    model.Title = post.Title;
                    model.Synopsis = post.Synopsis;
                    model.FeatureImage = post.FeatureImagePath;
                    model.Content = post.PostContent;
                    model.LastSaved = post.DatePosted.ToString("f") ?? "Not saved yet";
                    //send the selected genres and cats back to be highlighted
                    model.SelectedCategories = post.Categories.Select(rc => rc.CategoryId).ToArray();
                    
                }
            }

            return View(model);
        }

        [Authorize(Roles = "Admin, Author,Editor")]
        public ActionResult CreateNews(int? postId)
        {
            var model = new CreateNewsViewModel
            {
                LastSaved = "Not Saved Yet"
            };

            if(postId != null)
            {
                var post = db.Posts.OfType<News>().Single(p=> p.PostId == postId);
                if(post != null)
                {
                    model.PostId = (int)postId;
                    model.Title = post.Title;
                    model.Synopsis = post.Synopsis;
                    model.FeatureImage = post.FeatureImagePath;
                    model.Content = post.PostContent;
                    model.LastSaved = post.DatePosted.ToString("f") ?? "Not saved yet";
                }
            }
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Author,Editor")]
        public async Task<ActionResult> SaveNews(CreateNewsViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var author = db.Users.Find(User.Identity.GetUserId());
            if(author == null)
            {
                return Json(false);
            }

            var saveModel = new PostSaveViewModel
            {
                TimeSaved = DateTime.Now.ToString("f")
            };
            //if it hasnt already been saved
            if (model.PostId == null || model.PostId <= 0)
            {
                var news = new News
                {
                    Draft = true,
                    Title = model.Title,
                    DatePosted = DateTime.Now,
                    Author = author,
                    AuthorId = author.Id,
                    PostContent = model.Content,
                    Synopsis = model.Synopsis,
                    PostLocked = false,
                    Published = false,
                    Comments = null,
                };

                db.Posts.Add(news);

                if(await db.SaveChangesAsync() > 0)
                {
                    if (!String.IsNullOrEmpty(model.FeatureImage) || model.FeatureImage.Equals("undefined"))
                    {
                        HttpPostedFileBase ImagePath = Request.Files["FeatureImage"];
                        UploadImagePost(news.PostId, ImagePath);
                    }
                    else
                    {
                        news.FeatureImagePath = "~/Content/Images/Posts/Demo/dog_ate_img.jpeg";
                    }
                    db.Entry(news).State = EntityState.Modified;
                    if (await db.SaveChangesAsync() > 0)
                    {
                        saveModel.Success = true;
                        saveModel.PostId = news.PostId;
                    }
                }
            }
            else
            {
                var news = db.Posts.OfType<News>().Single(p=>p.PostId == model.PostId);

                if (news == null)
                {
                    return Json(saveModel, JsonRequestBehavior.AllowGet);
                }

                news.Synopsis = model.Synopsis;
                news.PostContent = model.Content;

                
                if (!String.IsNullOrEmpty(model.FeatureImage) || model.FeatureImage.Equals("undefined"))
                {
                    HttpPostedFileBase ImagePath = Request.Files["FeatureImage"];
                    UploadImagePost(news.PostId, ImagePath);
                }
                else
                {
                    news.FeatureImagePath = "~/Content/Images/Posts/Demo/dog_ate_img.jpeg";
                }

                news.Title = model.Title;

                news.DatePosted = DateTime.Now;

                db.Entry(news).State = EntityState.Modified;
                if (await db.SaveChangesAsync() > 0)
                {
                    saveModel.Success = true;
                    saveModel.PostId = news.PostId;
                }

            }

            return Json(saveModel, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [Authorize(Roles = "Admin, Author,Editor")]
        public async Task<ActionResult> SaveBlog(CreateBlogViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var author = db.Users.Find(User.Identity.GetUserId());
            if (author == null)
            {
                return Json(false);
            }

            var saveModel = new PostSaveViewModel
            {
                TimeSaved = DateTime.Now.ToString("f")
            };
            //if it hasnt already been saved
            if (model.PostId == null || model.PostId <= 0)
            {
                var blog = new Blog
                {
                    Draft = true,
                    Title = model.Title,
                    DatePosted = DateTime.Now,
                    Author = author,
                    AuthorId = author.Id,
                    PostContent = model.Content,
                    Synopsis = model.Synopsis,
                    PostLocked = false,
                    Published = false,
                    Categories = model.Categories,
                    Comments =  null,
                };

                db.Posts.Add(blog);
                if (await db.SaveChangesAsync() > 0)
                {
                    if (!String.IsNullOrEmpty(model.FeatureImage) || model.FeatureImage.Equals("undefined"))
                    {
                        HttpPostedFileBase ImagePath = Request.Files["FeatureImage"];
                        UploadImagePost(blog.PostId, ImagePath);
                    }
                    else
                    {
                        blog.FeatureImagePath = "~/Content/Images/Posts/Demo/dog_ate_img.jpeg";
                    }
                    db.Entry(blog).State = EntityState.Modified;
                    if (await db.SaveChangesAsync() > 0)
                    {
                        saveModel.Success = true;
                        saveModel.PostId = blog.PostId;
                    }
                }
            }
            else
            {
                var blog = db.Posts.OfType<Blog>().Single(p=>p.PostId == model.PostId);

                if (blog == null)
                {
                    return Json(saveModel, JsonRequestBehavior.AllowGet);
                }

                blog.Synopsis = model.Synopsis;
                blog.PostContent = model.Content;
                blog.Title = model.Title;
                blog.DatePosted = DateTime.Now;
                blog.Categories = model.Categories;

                if (!String.IsNullOrEmpty(model.FeatureImage) && !model.FeatureImage.Equals("undefined"))
                {
                    HttpPostedFileBase ImagePath = Request.Files["FeatureImage"];
                    UploadImagePost(blog.PostId, ImagePath);
                }
                else
                {
                    blog.FeatureImagePath = "~/Content/Images/Posts/Demo/dog_ate_img.jpeg";
                }

                db.Entry(blog).State = EntityState.Modified;
                if (await db.SaveChangesAsync() > 0)
                {
                    saveModel.Success = true;
                    saveModel.PostId = blog.PostId;
                }

            }

            return Json(saveModel, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Author,Editor")]
        public async Task<ActionResult> SaveReview(CreateReviewViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var author = db.Users.Find(User.Identity.GetUserId());
            if(author == null)
            {
                return Json(false);
            }

            var saveModel = new PostSaveViewModel
            {
                TimeSaved = DateTime.Now.ToString("f")
            };
            //if it hasnt already been saved
            if (model.PostId == null || model.PostId <= 0)
            {
                var review = new Review
                {
                    Draft = true,
                    Title = model.Title,
                    DatePosted = DateTime.Now,
                    Author = author,
                    AuthorId = author.Id,
                    PostContent = model.Content,
                    Synopsis = model.Synopsis,
                    PostLocked = false,
                    Published = false,
                    Score = model.Score,
                    Comments = null,
                };

                db.Posts.Add(review);
                if(await db.SaveChangesAsync() > 0)
                {
                    if (!String.IsNullOrEmpty(model.FeatureImage) && !model.FeatureImage.Equals("undefined"))
                    {
                        HttpPostedFileBase ImagePath = Request.Files["FeatureImage"];
                        if (ImagePath != null)
                        {
                            UploadImagePost(review.PostId, ImagePath);
                        }
                        else
                        {
                            review.FeatureImagePath = "~/Content/Images/Posts/Demo/dog_ate_img.jpeg";
                        }
                    }

                    db.Entry(review).State = EntityState.Modified;
                    if (await db.SaveChangesAsync() > 0)
                    {
                        saveModel.Success = true;
                        saveModel.PostId = review.PostId;
                    }
                }
            }
            else
            {
                var review = db.Posts.OfType<Review>().Single(p=> p.PostId == model.PostId);

                if (review == null)
                {
                    return Json(saveModel, JsonRequestBehavior.AllowGet);
                }

                review.Title = model.Title;
                review.Synopsis = model.Synopsis;
                review.PostContent = model.Content;
                review.Score = model.Score;
                review.DatePosted = DateTime.Now;

                if (!String.IsNullOrEmpty(model.FeatureImage) && !model.FeatureImage.Equals("undefined"))
                {
                    HttpPostedFileBase ImagePath = Request.Files["FeatureImage"];
                    UploadImagePost(review.PostId, ImagePath);
                }


                db.Entry(review).State = EntityState.Modified;
                if (await db.SaveChangesAsync() > 0)
                {
                    saveModel.Success = true;
                    saveModel.PostId = review.PostId;
                }

            }

            return Json(saveModel, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SaveReviewCategories(int postId, int[]catIds, int[] genIds)
        {
            //get the review
            Review rev = db.Posts.OfType<Review>().Include(r => r.Genres).Include(r=>r.ReviewCategories).Single(r => r.PostId == postId);

            if(rev == null)
            {
                return Json(new { success = false });
            }
            //get the reviews categories and delete the many to many records
            var currentRevs = rev.ReviewCategories.ToList();
            for(int i = 0; i < currentRevs.Count; i++) 
            {
                rev.ReviewCategories.Remove(currentRevs[i]);
            }
            //get the reviews genres and delete the many to many records
            var currentGenres = rev.Genres.ToList();
            for (int i = 0; i< currentGenres.Count;i++)
            {
                rev.Genres.Remove(currentGenres[i]);
            }
            //save the review with no categories/genres
            db.Entry(rev).State = EntityState.Modified;
            db.SaveChanges();

            //re add the new genres/categories and save the db
            if (genIds != null)
            {
                var genres = db.Genres.Where(g => genIds.Any(sg => sg == g.GenreId)).ToList();
                rev.Genres = new List<Genre>(genres);
            }
            if (catIds != null)
            {
                var cats = db.ReviewCategories.Where(c => catIds.Any(sc => sc == c.Id)).ToList();
                rev.ReviewCategories = new List<ReviewCategory>(cats);
            }

            db.Entry(rev).State = EntityState.Modified;

            if (db.SaveChanges() > 0)
            {
                return Json(new { success = true });
            }

            return Json(new { success = false });
        }
        public ActionResult SaveBlogCategories(int postId, int[]catIds)
        {
            //get the review
            Blog blog = db.Posts.OfType<Blog>().Include(r=>r.Categories).Single(r => r.PostId == postId);

            if(blog == null)
            {
                return Json(new { success = false });
            }
            //get the reviews categories and delete the many to many records
            var currentRevs = blog.Categories.ToList();
            for(int i = 0; i < currentRevs.Count; i++) 
            {
                blog.Categories.Remove(currentRevs[i]);
            }
            //save the review with no categories/genres
            db.Entry(blog).State = EntityState.Modified;
            //db.SaveChanges();

            
            if (catIds != null)
            {
                var cats = db.Categories.Where(c => catIds.Any(sc => sc == c.CategoryId)).ToList();

                blog.Categories = new List<Category>(cats);
            }

            db.Entry(blog).State = EntityState.Modified;

            if (db.SaveChanges() > 0)
            {
                return Json(new { success = true });
            }

            return Json(new { success = false });
        }

        //change this to individual hard coded delete methods
        public ActionResult DeletePost(int postId)
        {
            var post = db.Posts.Include(p => p.Comments).Single(p => p.PostId == postId);

            if(post == null)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }

            var type = post.GetType().Name;
            type = type.Substring(0,type.LastIndexOf("_"));

            //remove comments
            var comments = post.Comments.ToList();
            for(int i = 0; i < comments.Count; i++)
            {
                post.Comments.Remove(comments[i]);
            }

            switch (type)
            {
                case "Review":
                    //get the reviews categories and delete the many to many records
                    var rev = db.Posts.OfType<Review>().Include(r => r.Genres).Include(r => r.ReviewCategories).Single(p => p.PostId == postId);
                    var currentRevs = rev.ReviewCategories.ToList();
                    for (int i = 0; i < currentRevs.Count; i++)
                    {
                        rev.ReviewCategories.Remove(currentRevs[i]);
                    }
                    //get the reviews genres and delete the many to many records
                    var currentGenres = rev.Genres.ToList();
                    for (int i = 0; i < currentGenres.Count; i++)
                    {
                        rev.Genres.Remove(currentGenres[i]);
                    }
                    db.Posts.Remove(rev);
                    break;
                case "Blog":
                    //get the blogs categories and delete the many to many records
                    var blog = db.Posts.OfType<Blog>().Include(r => r.Categories).Single(p => p.PostId == postId);
                    var currentCats = blog.Categories.ToList();
                    for (int i = 0; i < currentCats.Count; i++)
                    {
                        blog.Categories.Remove(currentCats[i]);
                    }
                    db.Posts.Remove(blog);
                    break;
                case "News":
                    db.Posts.Remove((News)post);
                    break;
            }

            db.Posts.Remove(post);

            if (db.SaveChanges() > 0)
            {
                return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { Success = false });

        }

        /// <summary>
        /// Set the posts draft bool to false. Meaning the post is peninding publishing.
        /// </summary>
        public async Task<ActionResult> SubmitPost(int postId)
        {
            var post = db.Posts.Find(postId);
            if(post == null)
            {
                return Json(new { Success = false });
            }
            //var postType = post.GetType().Name.Substring(0, post.GetType().Name.LastIndexOf("_"));

            post.DatePosted = DateTime.Now;
            post.Draft = false;
            post.Published = false;
            
            db.Entry(post).State = EntityState.Modified;

            if(await db.SaveChangesAsync() > 0){
                return Json(new { Success = true });
            }

            return Json(new { Success = false });
        }
        
        /// <summary>
        /// Report the post
        /// </summary>
        public ActionResult ReportPost(int postId, int reasonId, string info)
        {
            //get the post and reports (that are not solved)
            var post = db.Posts.Include(c => c.Reports).Single(c => c.PostId == postId);

            if (post == null)
            {
                return Json(new { Success = false, Message = "Post does not exist" });
            }
            //check for immunity (comment has already been processed through report procedings)
            if (!post.Immune)
            {
                //check the number of reports the user has recieved through comments/posts
                //get user
                var user = db.Users.Find(post.AuthorId);
                if (user == null)
                {
                    return Json(new { Success = false, Message = "User does not exist" });
                }

                //get all the users asociated onging infractions, if any....
                //get the reported comments with an onoging report
                var allReportedComments = db.Comments.Include(c => c.Reports).Where(c => c.UserID.Equals(user.Id) && c.Reports.Any(r => r.Status != Status.Solved)).ToList();
                //from that get the reports to add to the case
                var allCommentReports = allReportedComments.SelectMany(r => r.Reports.Where(rr => rr.Status == Status.Pending || rr.Status == Status.Assigned)).ToList();

                //same for posts
                var allReportedPosts = db.Posts.Include(c => c.Reports).Where(c => c.AuthorId.Equals(user.Id) && c.Reports.Any(r => r.Status != Status.Solved)).ToList();
                var allPostReports = allReportedPosts.SelectMany(r => r.Reports.Where(rr => rr.Status == Status.Pending || rr.Status == Status.Assigned)).ToList();

                //add them together, to test whether the number of reports exceeds the amount needed to open a case!
                var reportsCount = allCommentReports.Count + allPostReports.Count;


                //can extend the reporting reasons to many.... just btw
                var report = new Report
                {
                    Post = post,
                    Reasons = new List<ReportReason> { db.ReportReasons.Find(reasonId) },
                    ExtraInformation = info,
                    Status = Status.Pending,
                };


                //if number of (current/not solved)reports is (20) or greater, open a case
                //the case points to the offending item (comment)
                //and the comment has all the reports attributed to it
                //plust one for the new report
                if (reportsCount + 1 >= 20)
                {
                    //find whether the a case exists and has not been solved
                    var existingCase = db.Cases.Include(ec => ec.ReportedComments)
                        .Where(ec => ec.UserId.Equals(user.Id) && !ec.IsSolved).SingleOrDefault();

                    //if there is no such case, open a new one - old cases can still be viewd through the closed cases

                    //and add any other comment/posts to the case as well
                    if (existingCase == null)
                    {
                        var opencase = new Case
                        {
                            IsGuilty = false,
                            IsSolved = false,
                            ReportedPosts = new List<Post> { post },
                            User = user,
                        };

                        //add any and all ongoing reported comments to add to the case
                        foreach (Comment c in allReportedComments)
                        {
                            //make sure the reports all point to this case now
                            foreach (Report r in c.Reports)
                            {
                                r.Case = opencase;
                                db.Entry(r).State = EntityState.Modified;
                            }
                            opencase.ReportedComments.Add(c);
                            db.Entry(c).State = EntityState.Modified;
                        }
                        //add any and all ongoing reported posts to add to the case
                        foreach (Post p in allReportedPosts)
                        {
                            //make sure the reports all point to this case now
                            foreach (Report r in p.Reports)
                            {
                                r.Case = opencase;
                                db.Entry(r).State = EntityState.Modified;
                            }
                            opencase.ReportedPosts.Add(p);
                            db.Entry(p).State = EntityState.Modified;
                        }

                        //add the new report also
                        report.Case = opencase;
                        //and assing all previous reports on this specific comment to the case
                        foreach (Report r in post.Reports)
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
                        if (!existingCase.ReportedPosts.Contains(post))
                        {
                            //if the comment is not already added (i.e. it is a new reported comment) add it to the case
                            existingCase.ReportedPosts.Add(post);
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
                    return Json(new { Success = true, Message = "Post Reported Succesfully" });
                }

                return Json(new { Success = false, Message = "Something went wrong! Please try again later" });
            }
            return Json(new { Success = true, Message = "Comment has immunity, shhhhhhh, dont tell anyone ;)" });
        }

        public JsonResult VotePost(int postId, bool upVote)
        {
            // get the comment and add a new vote to it, upvote or downvote
            var post = db.Posts.Include(c => c.Votes).Single(c => c.PostId == postId);
            if (post == null)
            {
                return Json(new { Success = false, Message = "Comment not found!" });
            }
            var userId = User.Identity.GetUserId();
            var user = db.Users.Find(userId);
            if (user == null)
            {
                return Json(new { Success = false, Message = "You were not found!" });
            }
            //check if the user has already voted on the comment
            foreach (PostVote v in post.Votes)
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
                            Score = post.Votes.Where(c => c.UpVote).Count() - post.Votes.Where(c => !c.UpVote).Count(),
                            Message = "Success!"
                        });
                    }
                    return Json(new { Success = false, Message = "User has already like/disliked the comment!" });
                }
            }

            //otherwise we can like/dislike
            post.Votes.Add(new PostVote { User = user, UpVote = upVote });

            db.Entry(post).State = EntityState.Modified;

            if (db.SaveChanges() > 0)
            {
                return Json(new
                {
                    Success = true,
                    Score = post.Votes.Where(c => c.UpVote).Count() - post.Votes.Where(c => !c.UpVote).Count(),
                    Message = "Success!"
                });
            }

            return Json(new { Success = false, Message = "Something went wrong!" });
        }

        private void UploadImagePost(int postId, HttpPostedFileBase ImagePath)
        {
            var post = db.Posts.Find(postId);
            var type = post.GetType().Name;
            //TAHEWJRFHWAFAWF
            if (type.Contains("_"))
            {
                type = type.Substring(0, type.LastIndexOf("_"));
            }
            if (ImagePath != null && ImagePath.ContentLength > 0)
            {
                try
                {
                    string extension = Path.GetExtension(ImagePath.FileName);
                    string fileName = post.Title.Trim().ToLower() + "_" + post.PostId + extension;
                    string path = Path.Combine(Server.MapPath(@"~/Content/Images/Posts/" + type + "/"), fileName);

                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }

                    ImagePath.SaveAs(path);
                    post.FeatureImagePath = @"~/Content/Images/Posts/" + type + "/" + fileName;
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    string error = e.Message.ToString();
                }
            }
        }


        public int CommentsCount(int postId)
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
        public int AddReplies(int commentId)
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
            return str.Substring(0, maxLength) +"...";
        }
    }
}