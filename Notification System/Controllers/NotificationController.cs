//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Notification_System.Hubs;
//using Notification_System.Models;
//using TableDependency.SqlClient;

//namespace Notification_System.Controllers
//{
//    public class NotificationController : Controller
//    {

//        SqlTableDependency<Notification> tableDependency;

//        NotificationHub notificationhub;
//        SignalRContext signalRContext;

//        public NotificationController(SignalRContext signalRContext)
//        {
//            this.signalRContext = signalRContext;
//        }

//        public IActionResult Index()
//        {
            
//            var notifications = signalRContext.Notifications.ToList(); // Fetch notifications from database
//            return View(notifications);
//        }
//    }
//}
