using Notification_System.Hubs;
using Notification_System.Models;
using TableDependency.SqlClient;

namespace Notification_System.SubscribeTableDependencies
{
    public class SubscribeNotificationTableDependency : ISubscribeTableDependency
    {

        SqlTableDependency<Notification> tableDependency;

        NotificationHub notificationhub;

        public SubscribeNotificationTableDependency(NotificationHub notificationhub)
        {
            this.notificationhub = notificationhub;
        }


        //public Task SubscribeTableDependecy(string ConnectionString)
        //        {

        //            tableDependency = new SqlTableDependency<Notification>(ConnectionString);
        //            tableDependency.OnChanged += TableDependency_Onchanged;
        //            tableDependency.OnError += TableDependency_OnError;
        //            tableDependency.Start();    

        //        }

        public void SubscribeTableDependency(string connectionString)
        {
            tableDependency = new SqlTableDependency<Notification>(connectionString);
            tableDependency.OnChanged += TableDependency_OnChanged;
            tableDependency.OnError += TableDependency_OnError;
            tableDependency.Start();

        }

        //public void TableDependency_OnError(object Sender, TableDependency.SqlClient.Base.EventArgs.ErrorEventArgs e)
        //{
        //    Console.WriteLine($"{nameof(Notification)} SqlTableDependency error: {e.Error.Message}");
        //}


        //public async Task TableDependency_Onchanged(object sender , TableDependency.SqlClient.Base.EventArgs.RecordChangedEventArgs<Notification> e)
        //{
        //    if(e.ChangeType != TableDependency.SqlClient.Base.Enums.ChangeType.None)
        //    {
        //        var notification = e.Entity;
        //        if(notification.MessageType == "All")
        //        {
        //           await  notificationhub.SendNotificationToAll(notification.Message);
        //        }
        //        else if(notification.MessageType == "Personal")
        //        {
        //          await   notificationhub.SendNotificationPersonal(notification.Message, notification.Username);
        //        }
        //    }


        //}

        private void TableDependency_OnError(object sender, TableDependency.SqlClient.Base.EventArgs.ErrorEventArgs e)
        {
            Console.WriteLine($"{nameof(Notification)} SqlTableDependency error: {e.Error.Message}");
        }

        private async void TableDependency_OnChanged(object sender, TableDependency.SqlClient.Base.EventArgs.RecordChangedEventArgs<Notification> e)
        {
            if (e.ChangeType != TableDependency.SqlClient.Base.Enums.ChangeType.None)
            {
                var notification = e.Entity;
                if (notification.MessageType == "All")
                {
                    await notificationhub.SendNotificationToAll(notification.Message);
                }
                else if (notification.MessageType == "Personal")
                {
                    await notificationhub.SendNotificationPersonal(notification.Message, notification.Username);
                }
            }
        }



    }
}
