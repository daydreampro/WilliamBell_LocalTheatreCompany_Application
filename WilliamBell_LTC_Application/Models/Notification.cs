using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WilliamBell_LTC_Application.Models
{
    public class Notification
    {
        public int NotificationId { get; set; }
        /// <summary>
        /// message to be displayed for notification
        /// </summary>
        public string Message { get; set; }
        /// <summary>
        /// if the user has seen but not removed it from bell notifications
        /// </summary>
        public bool Seen { get; set; }
        /// <summary>
        /// if the user has removed it from bell notifications
        /// </summary>
        public bool RemoveFromBell { get; set; }
        /// <summary>
        /// the date it should be automatically removed from bell notifications
        /// should not affect view all notifications
        /// </summary>
        public DateTime? DateToRemoveBellNotification { get; set; }
        /// <summary>
        /// datetime it was created
        /// </summary>
        public DateTime CreatedAt { get; set; }
        /// <summary>
        /// the raw URL to send the user to
        /// </summary>
        public string Destination { get; set; }

        //navs
        /// <summary>
        /// THe user the notification is owned by
        /// </summary>
        [ForeignKey("User")]
        public virtual string UserId { get; set; }
        public User User { get; set; }
    }

}