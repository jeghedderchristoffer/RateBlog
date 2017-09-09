using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RateBlog.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Models
{
    public class SeedEmailNotificationData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {

                if (context.EmailNotifications.Count() == context.Users.Count())
                {
                    return;   // DB has been seeded
                }

                foreach(var v in context.Users)
                {
                    context.EmailNotifications.Add(new EmailNotification() { NewsLetter = v.NewsLetter, FeedbackUpdate = true, Id = v.Id }); 
                }

                context.SaveChanges();
            }
        }
    }
}
