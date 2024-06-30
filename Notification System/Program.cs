using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Notification_System.Hubs;
using Notification_System.MiddleWare;
using Notification_System.Models;
using Notification_System.Repos;
using Notification_System.SubscribeTableDependencies;

namespace Notification_System
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddSignalR();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                               .AllowAnyMethod()
                               .AllowAnyHeader();
                    });
            });

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<SignalRContext>(options =>
            options.UseSqlServer(connectionString), ServiceLifetime.Singleton);
            //        builder.Services.AddDbContext<SignalRContext>(options =>
            //options.UseSqlServer(connectionString), ServiceLifetime.Scoped);
            //        builder.Services.AddDbContext<SignalRContext>(options =>
            //options.UseSqlServer(connectionString));


            //Dependency Injection
            builder.Services.AddSingleton<UserRepo>();
            builder.Services.AddSingleton<IHttpContextAccessor,HttpContextAccessor>();
            builder.Services.AddSingleton<NotificationHub>();
            builder.Services.AddSingleton<SubscribeNotificationTableDependency>();
            //builder.Services.AddSingleton<ISubscribeTableDependency>(new SubscribeNotificationTableDependency(notificationHub));


            //Session
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.UseSession();

            app.MapHub<NotificationHub>("/notificationhub");
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=account}/{action=SignIn}/{id?}");

            app.UseSqlTableDependency<SubscribeNotificationTableDependency>(connectionString);

         
            app.Run();
        }
    }
}
