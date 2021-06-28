using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WilliamBell_LTC_Application.Models
{
    /// <summary>
    /// parent class for all posts:
    /// BLOG and REVIEW and NEWS
    /// </summary>
    public abstract class Post
    {
        //props
        public int PostId { get; set; }
        //need to think about how to implement formatting of content:
        //either format is the same for each post type, or develop some sort of freedom for authors to format them selves
        
        public string Title { get; set; }
        
        public string Synopsis { get; set; }

        
        [Display(Name ="Post Content")]
        [DataType(DataType.MultilineText)]
        [AllowHtml]
        public string PostContent { get; set; }
        //date the content was posted
        [Display(Name = "Date Posted")]
        public DateTime DatePosted { get; set; }

        //is unfinished article
        //the false decleration means it is submitted for publishing
        public bool Draft { get; set; }
        //is published on the site
        public bool Published { get; set; }

        //lock the post from further comments
        public bool PostLocked { get; set; }

        //need to think about how to do images, for now feature image can go hear
        [Display(Name = "Feature Image")]
        public string FeatureImagePath { get; set; }

        //immune if wrongly persecuted by reports
        public bool Immune { get; set; }
        //Flagged if guilty of violations
        public bool Flagged { get; set; }


        //navs
        //owner of the post
        [ForeignKey("Author")]
        public virtual string AuthorId { get; set; }
        public virtual User Author { get; set; }

        //editor of post
        [ForeignKey("Editor")]
        public virtual string EditorId { get; set; }
        public virtual User Editor { get; set; }

        //comments
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Report> Reports { get; set; }

        public virtual ICollection<PostVote> Votes { get; set; }
    }

    public class Blog : Post
    {
        public ICollection<Category> Categories { get; set; }
    }

    public class News : Post
    {
        //somin maybe?
    }
    public class Review : Post
    {
        public decimal Score { get; set; }
        public ICollection<Genre> Genres { get; set; }
        public ICollection<ReviewCategory> ReviewCategories { get; set; }

    }
}