using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Notification_System.Hubs;

using Notification_System.Models;


[Route("api/[controller]")]
public class NotificationController : Controller
{

    private readonly SignalRContext _signalRContext;
    private readonly IHubContext<NotificationHub> _hubContext;

    public NotificationController(SignalRContext signalRContext, IHubContext<NotificationHub> hubContext)
    {
        _signalRContext = signalRContext;
        _hubContext = hubContext;
    }

    //// GET: api/notification/index/{username}
    // GET: api/notification/index/{username}
    [HttpGet("index/{username}")]
    public async Task<IActionResult> Index(string username)
    {
        var notifications = await _signalRContext.Notifications
            .Where(n => n.Username == username)
            .OrderByDescending(n => n.NotificationDateTime)
            .ToListAsync();

        if (notifications == null || notifications.Count == 0)
        {
            return View(new List<Notification>());
        }

        return View(notifications);
    }


    // GET: /Notification/Index/{username}
    //public async Task<IActionResult> Index()
    //{
    //    // Retrieve notifications for the specified username
    //    var notifications = await _signalRContext.Notifications
    //        .OrderByDescending(n => n.NotificationDateTime)
    //        .ToListAsync();
    //    if (notifications == null || notifications.Count == 0)
    //    {
    //        return NotFound(); // Return 404 Not Found if no notifications found
    //    }
    //    return View(notifications); // Return the Index view with notifications
    //}

    [HttpGet("Create")]
    public IActionResult Create()
    {
        return View();
    }

    //public IActionResult Create() { return View();}
    [HttpPost("Create")]
    public async Task<IActionResult> Create([FromBody] NotificationCreateModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var notification = new Notification
            {
                Message = model.Message,
                Username = model.Username,
                MessageType = model.MessageType,
                NotificationDateTime = model.NotificationDateTime
            };

            _signalRContext.Notifications.Add(notification);
            await _signalRContext.SaveChangesAsync();

            if (notification.MessageType == "All")
            {
                await _hubContext.Clients.All.SendAsync("ReceivedNotification", model.Message, model.MessageType, model.Username, model.NotificationDateTime);
            }
            else 
            {
                var hubConnections = _signalRContext.HubConnections.Where(con => con.Username == model.Username).ToList();
                foreach (var hubConnection in hubConnections)
                {
                    await _hubContext.Clients.Client(hubConnection.ConnectionId).SendAsync("ReceivedPersonalNotification", model.Message, model.MessageType, model.Username, model.NotificationDateTime);
                }
            }

            return Ok(new { success = true });
        }
        catch (DbUpdateException ex)
        {
            // Check for specific database update errors
            var errorMessage = "An error occurred while saving changes to the database.";
            if (ex.InnerException != null)
            {
                errorMessage += " Inner exception: " + ex.InnerException.Message;
            }
            return StatusCode(500, new { success = false, error = errorMessage });
        }
        catch (Exception ex)
        {
            // Handle other unexpected errors
            return StatusCode(500, new { success = false, error = "An unexpected error occurred: " + ex.Message, innerError = ex.InnerException?.Message });
        }
    }
}

