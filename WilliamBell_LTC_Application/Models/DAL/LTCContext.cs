using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;

namespace WilliamBell_LTC_Application.Models.DAL
{
    public class LTCContext : IdentityDbContext<User>
    {
        public DbSet<Vote> Votes { get; set; }
        //tables
        //CATEGORIES!
        public DbSet<Category> Categories { get; set; }
        public DbSet<ReviewCategory> ReviewCategories { get; set; }
        public DbSet<Genre> Genres { get; set; }

        //POSTS!
        public DbSet<Post> Posts { get; set; }

        public DbSet<BannedContent> BannedContents { get; set; }
        public DbSet<Comment> Comments { get; set; }

        //REPORTS
        public DbSet<Report> Reports { get; set; }
        public DbSet<ReportReason> ReportReasons  { get; set; }
        public DbSet<Case> Cases { get; set; }

        //USERS!
        public DbSet<Author> Authors { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Badge> Badges { get; set; }
        public DbSet<Notification> Notifications { get; set; }

        public LTCContext(): base("LTC_WebAppV1", throwIfV1Schema: false)
        {
            Database.SetInitializer(new DBInitialiser());
        }

        public static LTCContext Create()
        {
            return new LTCContext();
        }

    }
}