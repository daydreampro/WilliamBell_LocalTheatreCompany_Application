using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WilliamBell_LTC_Application.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class User : IdentityUser
    {
        //properties
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [DataType(DataType.ImageUrl)]
        public string UserImage { get; set; }
        public string  Biography { get; set; }
        public DateTime? JoinDate { get; set; }
        public int NumberOfWarnings { get; set; }
        public int NumberOfSuspension { get; set; }
        public int NumberOfBlockings { get; set; }
        public bool CommentsBlocked { get; set; }
        public bool AccountSuspended { get; set; }
        [DataType(DataType.Date)]
        public DateTime? SuspensionUntil { get; set; }
        [DataType(DataType.Date)]
        public DateTime? CommentsBlockedUntil { get; set; }

        //navs
        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<Case> Cases { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }

        public virtual ICollection<Badge> Badges { get; set; }

        public virtual ICollection<Vote> Votes { get; set; }


        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    
}