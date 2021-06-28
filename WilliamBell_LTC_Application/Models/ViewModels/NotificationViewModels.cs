using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WilliamBell_LTC_Application.Models.ViewModels
{
    public class NotificationsViewModel
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string CreatedAt { get; set; }
        public string Destination { get; set; }
        public bool Seen { get; set; }
    }
}