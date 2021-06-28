using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WilliamBell_LTC_Application.Models.ViewModels
{
    public class PublicProfileViewModel
    {
        public string UserId { get; set; }
        public string DisplayName { get; set; }
        public string Biography { get; set; }
        public string Image { get; set; }
        public List<Badge> Badges { get; set; }
        public List<string> Roles { get; set; }
    }
    public class ProfilePostsViewModel
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string PostType { get; set; }
        public string Controller { get; set; }
    }

    public class UserDashboardViewModel
    {
        public int Notifications { get; set; }
        public List<Badge> Badges { get; set; }
    }
    public class AuthorDashboardViewModel
    {
        public int PublishedPostsCount { get; set; }
        public int DraftPostsCount { get; set; }
        public int PendingPostsCount { get; set; }
        public IEnumerable<SelectListItem> Types { get; set; }
        public string TypeId { get; set; }
        public byte ClassCount { get; set; }
    }
    public class ModeratorDashboardViewModel
    {
        public int MyOpenCasesCount { get; set; }
        public int AllPendingCasesCount { get; set; }
        public int MyClosedCasesCount { get; set; }

    }
    public class EditorDashboardViewModel
    {
        public int DraftPosts { get; set; }
        public int PendingPosts { get; set; }
        public int CompletedPosts { get; set; }
    }

    public class AdminDashboardViewModel
    {
        public int CategoriesCount { get; set; }
    }

    public class MiniProfileViewModel
    {
        public string UserId { get; set; }
        public string DisplayName { get; set; }
        public string Image { get; set; }
        public List<Badge> Badges { get; set; }
    }

    public class AuthorCreditViewModel
    {
        public string AuthorId { get; set; }
        public string AuthorName { get; set; }
        public string Image { get; set; }
        public string SmallBio { get; set; }
        public List<Badge> Badges { get; set; }
    }
}