using Microsoft.AspNetCore.SignalR;
using Notification_System.Models;

namespace Notification_System.Hubs
{
    public class NotificationHub:Hub
    {
        private readonly SignalRContext dbContext;

        public NotificationHub(SignalRContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task SendNotificationToAll(string message , string username ,DateTime notificationdate)
        {
            await Clients.All.SendAsync("ReceivedNotification", message ,username,notificationdate);
        }
        public async Task SendNotificationPersonal(string message, string username,DateTime notificationdate)
        {
            var hubConnections = dbContext.HubConnections.Where(con => con.Username == username).ToList();
            foreach (var hubConnection in hubConnections)
            {
                await Clients.Client(hubConnection.ConnectionId).SendAsync("ReceivedPersonalNotification", message, username,notificationdate);
            }
        }

        public override Task OnConnectedAsync()
        {
            Clients.Caller.SendAsync("OnConnected");
            return base.OnConnectedAsync();
        }

        public async Task SaveUserConnection(string username)
        {
            var connectionId = Context.ConnectionId;
            HubConnection hubConnection = new HubConnection
            {
                ConnectionId = connectionId,
                Username = username
            };

            dbContext.HubConnections.Add(hubConnection);
            await dbContext.SaveChangesAsync();
        }
        public override Task OnDisconnectedAsync(Exception? exception)
        {
            var hubConnection = dbContext.HubConnections.FirstOrDefault(con => con.ConnectionId == Context.ConnectionId);
            if (hubConnection != null)
            {
                dbContext.HubConnections.Remove(hubConnection);
                dbContext.SaveChangesAsync();
            }

            return base.OnDisconnectedAsync(exception);
        }

        private async Task SaveNotificationToDB(string message, string username, DateTime notificationdate)
        {
            var notification = new Notification
            {
                Message = message,
                Username = username,
                NotificationDateTime = notificationdate
            };

            dbContext.Notifications.Add(notification);
            await dbContext.SaveChangesAsync();
        }
    }
}
