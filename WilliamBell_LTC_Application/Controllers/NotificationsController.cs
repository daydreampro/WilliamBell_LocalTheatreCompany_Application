using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WilliamBell_LTC_Application.Models;
using WilliamBell_LTC_Application.Models.DAL;
using WilliamBell_LTC_Application.Models.ViewModels;

namespace WilliamBell_LTC_Application.Controllers
{
    [Authorize(Roles = "User")]
    public class NotificationsController : Controller
    {
        private LTCContext db = new LTCContext();


        public ActionResult GetBellUserNotifications()
        {
            var userId = User.Identity.GetUserId();
            var notifications = db.Notifications.Where(n => n.UserId.Equals(userId)).ToList().OrderBy(n=> n.Seen);

            var model = new List<NotificationsViewModel>();

            foreach (Notification n in notifications)
            {
                //if it has been removed from bell notifications we dont want it
                if (!n.RemoveFromBell)
                {
                    //buiuld the created at string
                    string created = n.CreatedAt.ToString("dd MMM HH:mm");
                    if(n.CreatedAt.Date == DateTime.Now.Date)
                    {
                        created = "Today at " + n.CreatedAt.ToString("HH:mm");
                    }else if(n.CreatedAt.Date > n.CreatedAt.Date.AddDays(14))
                    {
                        created = n.CreatedAt.ToString("dd MMM");
                    }

                    //if it has been seen check date
                    if (n.Seen)
                    {
                        if ((n.DateToRemoveBellNotification != null) && (n.DateToRemoveBellNotification > DateTime.Now))
                        {
                            model.Add(new NotificationsViewModel
                            {
                                Id = n.NotificationId,
                                Message = n.Message,
                                Destination = n.Destination,
                                Seen = n.Seen,
                                CreatedAt = created
                            });
                        }
                    }
                    else
                    {
                        model.Add(new NotificationsViewModel
                        {
                            Id = n.NotificationId,
                            Message = n.Message,
                            Destination = n.Destination,
                            Seen = n.Seen,
                            CreatedAt = created
                        });
                    }
                }

            }
            //get the count of new notifications (i.e. not seen notifications)
            ViewBag.NewNotificationCount = db.Notifications.Where(n => n.UserId.Equals(userId) && n.Seen == false).ToList().Count;

            return PartialView("~/Views/Shared/Profile/_Notifications.cshtml", model);
        }

        public JsonResult NotificationSeen(int id)
        {
            var noti = db.Notifications.Find(id);

            noti.Seen = true;
            noti.DateToRemoveBellNotification = DateTime.Now.AddDays(7);

            db.Entry(noti).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            var model = new NotificationsViewModel
            {
                Id = noti.NotificationId,
                Destination = noti.Destination,
                Message = noti.Message,
                Seen = noti.Seen,
                CreatedAt = noti.CreatedAt.ToString("dd MMM HH:mm")
            };


            return Json(model, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AllNotifications()
        {
            var userId = User.Identity.GetUserId();
            if(userId == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var user = db.Users.Find(userId);
            if(user == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var notifications = db.Notifications.Where(n => n.UserId.Equals(userId));

            return View(notifications);
        }
    }
}