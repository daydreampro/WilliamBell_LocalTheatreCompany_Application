using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WilliamBell_LTC_Application.Models
{
    public class Comment
    {
        public int CommentID { get; set; }
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }

        public DateTime DatePosted { get; set; }
        public DateTime? EditedAt { get; set; }
        //public int UpVotes { get; set; }
        //public int DownVotes { get; set; }
        public bool DELTED { get; set; }
        public bool Immune { get; set; }

        //navs
        [ForeignKey("User")]
        public virtual string UserID { get; set; }
        public User User { get; set; }

        //the post the comment is on
        [ForeignKey("Post")]
        public virtual int? PostId { get; set; }
        public Post Post { get; set; }

        //maybe the parent comment?
        [ForeignKey("ParentComment")]
        public virtual int? ParentCommentId { get; set; }
        public Comment ParentComment { get; set; }


        //reports a comment can get
        public virtual List<Report> Reports { get; set; }

        //list of replies a comment can get
        public virtual List<Comment> Replies { get; set; }

        public virtual ICollection<CommentVote> Votes { get; set; }


    }
}