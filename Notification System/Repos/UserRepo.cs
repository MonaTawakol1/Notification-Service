using Microsoft.EntityFrameworkCore;
using Notification_System.Models;

namespace Notification_System.Repos
{
    public class UserRepo
    {
        private readonly SignalRContext dbContext;

        public UserRepo(SignalRContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<User> GetUserDetails(string username, string password)
        {
            return await dbContext.Users.FirstOrDefaultAsync(user => user.Username == username && user.Password == password);
        }
    }
}
 