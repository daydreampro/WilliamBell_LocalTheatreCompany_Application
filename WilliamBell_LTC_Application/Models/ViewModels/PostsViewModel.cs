using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WilliamBell_LTC_Application.Models.ViewModels
{
    public class ReviewListViewModel 
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string AuthorName { get; set; }
        public string Synopsis { get; set; }
        public string Image { get; set; }
        public string Posted { get; set; }
        public int NumberOfComments { get; set; }
        public int PostVotes { get; set; }
    }

    public class ReviewDetailsViewModel
    {
        public int PostId { get; set; }
        public string Image { get; set; }
        public string Title { get; set; }
        public string AuthorId { get; set; }
        public string AuthorName { get; set; }
        public string Content { get; set; }
        public decimal Rating { get; set; }
        public List<ReviewCategory> Categories { get; set; }
        public List<Genre> Genres { get; set; }
        public string Posted { get; set; }
        public bool Locked { get; set; }
        public List<CommentViewModel> Comments { get; set; }
        public int PostScore { get; set; }
        public PostVote UserVote { get; set; }
    }

    public class BlogListViewModel 
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string AuthorName { get; set; }
        public string Synopsis { get; set; }
        public string Image { get; set; }
        public string Posted { get; set; }
        public int NumberOfComments { get; set; }
        public int PostVotes { get; set; }
    }

    public class BlogDetailsViewModel
    {
        public int PostId { get; set; }
        public string Image { get; set; }
        public string Title { get; set; }
        public string AuthorId { get; set; }
        public string AuthorName { get; set; }
        public string Content { get; set; }
        public string Posted { get; set; }
        public bool Locked { get; set; }
        public List<CommentViewModel> Comments { get; set; }
        public List<Category> Categories { get; set; }
        public int PostScore { get; set; }
        public PostVote UserVote { get; set; }
    }

    public class NewsListViewModel
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string AuthorName { get; set; }
        public string AuthorId { get; set; }
        public string Content { get; set; }
        public string Image { get; set; }
        public string Posted { get; set; }
        public int NumberOfComments { get; set; }
    }

    public class NewsDetailsViewModel
    {
        public int PostId { get; set; }
        public string Image { get; set; }
        public string Title { get; set; }
        public string AuthorId { get; set; }
        public string AuthorName { get; set; }
        public string Content { get; set; }
        public string Posted { get; set; }
        public bool Locked { get; set; }
        public List<CommentViewModel> Comments { get; set; }
        public int PostScore { get; set; }
        public PostVote UserVote { get; set; }
    }

    public class ArchiveListViewModel
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string AuthorName { get; set; }
        public string Synopsis { get; set; }
        public string Image { get; set; }
        public string Posted { get; set; }
        public int NumberOfComments { get; set; }
        public string PostType { get; set; }
        public string ControllerName { get; set; }
        public bool IsFlagged { get; set; }
    }

    public class CreateReviewViewModel
    {
        public int? PostId { get; set; }
        public string Title { get; set; }
        public string Synopsis { get; set; }
        [AllowHtml]
        public string Content { get; set; }
        public string FeatureImage { get; set; }
        public decimal Score { get; set; }
        public string LastSaved { get; set; }
        public IList<Genre> Genres { get; set; }
        public int[] SelectedGenres { get; set; }
        public IList<ReviewCategory> ReviewCategories { get; set; }
        public int[] SelectedCategories { get; set; }
    }
    public class CreateBlogViewModel
    {
        public int? PostId { get; set; }
        public string Title { get; set; }
        public string Synopsis { get; set; }
        [AllowHtml]
        public string Content { get; set; }
        public string FeatureImage { get; set; }
        public string LastSaved { get; set; }
        public IList<Category> Categories { get; set; }
        public int[] SelectedCategories { get; set; }
    }
    public class CreateNewsViewModel
    {
        public int? PostId { get; set; }
        public string Title { get; set; }
        public string Synopsis { get; set; }
        [AllowHtml]
        public string Content { get; set; }
        [DataType(DataType.ImageUrl)]
        public string FeatureImage { get; set; }
        public string LastSaved { get; set; }
    }

    public class PostSaveViewModel
    {
        public int PostId { get; set; }
        public string TimeSaved { get; set; }
        public bool Success { get; set; }
    }

    public class PostInformationViewModel
    {
        public int PostId { get; set; }
        public string PostType { get; set; }
        public string Controller { get; set; }
        public string Title { get; set; }
        public string Synopsis { get; set; }
        public string PublishDate { get; set; }
        public string Image { get; set; }
    }


}