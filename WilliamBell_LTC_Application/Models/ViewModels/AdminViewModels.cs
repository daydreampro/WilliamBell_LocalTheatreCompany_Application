using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WilliamBell_LTC_Application.Models.ViewModels
{
    public class AdminDashViewModel
    {
        public int PostCount { get; set; }
        public int MyCaseCount { get; set; }
        public int AllCases { get; set; }
    }

    public class AllReportsViewModel
    {
        public int ReportItemId { get; set; }
        public string ReportItemType { get; set; }
        public List<string> ReportReasons { get; set; }
        public int ReportCount { get; set; }
        public string Username { get; set; }
        public string Title { get; set; }
    }

    public class CasesListViewModel
    {
        public int CaseId { get; set; }
        public string Username { get; set; }
        public int ReportsCount { get; set; }
        public int ReportedItemsCount { get; set; }
        public List<string> ReportReasons { get; set; }
    }

    public class ClosedCasesListViewModel
    {
        public int CaseId { get; set; }
        public string Username { get; set; }
        public string Verdict { get; set; }
        public string Reason { get; set; }
    }

    public class CaseDetailsViewModel
    {
        public int CaseId { get; set; }
        public List<CaseReportItemDetailsViewModel> ReportsItems  { get; set; }
        public User Offender { get; set; }
        public int NumberOfItems { get; set; }
        public int NumberOfReports { get; set; }
        public bool Owned { get; set; }
    }
    public class ClosedCaseDetailsViewModel
    {
        public int CaseId { get; set; }
        public List<CaseReportItemDetailsViewModel> ReportsItems  { get; set; }
        public User Offender { get; set; }
        public int NumberOfItems { get; set; }
        public int NumberOfReports { get; set; }
        public string Verdict { get; set; }
        public string Reason { get; set; }
    }

    public class CaseReportItemDetailsViewModel
    {
        public int ReportItemId { get; set; }
        public string Type { get; set; }
        public List<ReportDetailViewModel> Reports { get; set; }
        public List<Case> PreviousCases { get; set; }
        public string Content { get; set; }
        public int ReportCount { get; set; }
    }

    public class ReportItemDetailsViewModel
    {
        public int ReportItemId { get; set; }
        public List<ReportDetailViewModel> Reports { get; set; }
        public List<Case> PreviousCases { get; set; }
        public string Content { get; set; }
        public User Offender { get; set; }
        public int ReportCount { get; set; }
        public string Type { get; set; }
        public string Cont { get; set; }
        public int? ParentPostId { get; set; }
    }

    public class ReportDetailViewModel
    {
        public int ReportId { get; set; }
        public List<string> Reasons { get; set; }
        public string ExtraInfo { get; set; }
    }

    public class AllCategoriesViewModel
    {
        public int CatId { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
    }

    public class AllUsersViewModel
    {

    }

    public class AllStaffViewModel
    {
        public ICollection<StaffViewModel> Staff { get; set; }
        public Dictionary<string, string> AllRoles { get; set; }
    }

    public class StaffViewModel
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string JoinDate { get; set; }
        public ICollection<string> StaffRoles { get; set; }

    }

    public class CreateStaffViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        

        public List<AuthorRolesViewModel> Roles { get; set; }

        public string Type { get; set; }
    }

    public class AuthorRolesViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsSelected { get; set; }
    }

    public class BannedPhrasesListViewModel
    {
        public int Id { get; set; }
        public string Phrase { get; set; }
        public string Type { get; set; }
    }

}