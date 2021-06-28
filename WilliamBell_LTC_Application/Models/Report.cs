using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WilliamBell_LTC_Application.Models
{
    /// <summary>
    /// Statuses like:
    /// New,
    /// Ongoing,
    /// Resolved,
    /// Archived
    /// </summary>
    public enum Status
    {
        Pending = -1,
        Assigned = 0,
        Solved = 1
    }

    /// <summary>
    /// Reasons as to why something should be reported
    /// </summary>
    public class ReportReason
    {
        public int ReportReasonId { get; set; }
        public string Reason { get; set; }

        //[ForeignKey("Report")]
        //public virtual int ReportId { get; set; }
        //public virtual Report Report { get; set; }
        public virtual ICollection<Report> Reports { get; set; }
    }

    /// <summary>
    /// A report of either a comment or a post with a reason and extra information
    /// to be reviewed by admins
    /// </summary>
    public class Report
    {
        [Key]
        public int ReportId { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name ="Extra Information")]
        public string ExtraInformation { get; set; }

        public Status Status { get; set; }

        //navs
        //if it is attched to a comment
        [ForeignKey("Comment")]
        public virtual int? CommentId { get; set; }
        public virtual Comment Comment { get; set; }

        //if it is attached to a post
        [ForeignKey("Post")]
        public virtual int? PostId { get; set; }
        public virtual Post Post { get; set; }

        [ForeignKey("Case")]
        public virtual int? CaseId { get; set; }
        public virtual Case Case { get; set; }

        public virtual ICollection<ReportReason> Reasons { get; set; }

    }
}