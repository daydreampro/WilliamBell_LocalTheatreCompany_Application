using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WilliamBell_LTC_Application.Models
{
    public class Vote
    {
        [Key]
        public int VoteId { get; set; }
        public bool UpVote { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        public User User { get; set; }

    }

    public class CommentVote : Vote
    {
        //nav
        [ForeignKey("Comment")]
        public int CommentId { get; set; }
        public Comment Comment { get; set; }
    }
    public class PostVote : Vote
    {
        //nav
        [ForeignKey("Post")]
        public int PostId { get; set; }
        public Post Post { get; set; }
    }
}

