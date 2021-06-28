using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WilliamBell_LTC_Application.Models.ViewModels
{
    public class EditCommentViewModel
    {
        public int CommentId { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }
    }

    public class DeleteCommentViewModel
    {
        public int CommentId { get; set; }
        public string Message { get; set; }
        public bool Success { get; set; }
        public bool Removing { get; set; }
    }
    public class CommentViewModel
    {
        public int CommentId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string CommentContet { get; set; }
        public DateTime DateTime { get; set; }
        public int ChildrenCommentsCount { get; set; }
        public int Padding { get; set; }
        public int Score { get; set; }
        public int UpVotes { get; set; }
        public int DownVotes { get; set; }
        public bool DELETED { get; set; }
        public CommentVote UserVote { get; set; }
        
    }

    public class AddCommentViewModel
    {
        public int Id { get; set; }
        public int? PostId { get; set; }
        public int? CommentId { get; set; }
        public string UserId { get; set; }
        public string Comment { get; set; }
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
    }
}