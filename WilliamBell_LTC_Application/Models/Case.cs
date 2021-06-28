using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WilliamBell_LTC_Application.Models
{
    public enum Punishment
    {
        WARNING = -1,
        BLOCKED = 0,
        SUSPENDED = 1
    }
    /// <summary>
    /// if a comment or post reaches a certain number of reports
    /// a case is opened for a admin to review
    /// </summary>
    public class Case
    {
        public int CaseId { get; set; }
        public bool IsSolved { get; set; }
        public bool IsGuilty { get; set; }
        public Punishment? Punsihment { get; set; }
        public string Reason { get; set; }

        //navs
        [ForeignKey("Moderator")]
        public string ModeratorId { get; set; }
        public Admin Moderator { get; set; }


        [ForeignKey("User")]
        public string UserId { get; set; }
        public User User { get; set; }

        public virtual ICollection<Comment> ReportedComments { get; set; }
        public virtual ICollection<Post> ReportedPosts { get; set; }

    }

}