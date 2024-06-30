using System.ComponentModel.DataAnnotations;

namespace Notification_System.Models
{
    public class NotificationCreateModel
    {
        [Required(ErrorMessage = "Message is required")]
        [StringLength(1000)]
        public string Message { get; set; }

        [Required(ErrorMessage = "MessageType is required")]
        [StringLength(50)]
        public string MessageType { get; set; }

        [Required(ErrorMessage = "NotificationDateTime is required")]
        public DateTime NotificationDateTime { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [StringLength(50)]
        public string Username { get; set; }
    }
}
