using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RateBlog.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Models
{
    public class SeedPlatformData
    {

        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {

                if (context.Platform.ToList().Count != 0)
                {
                    return;   // DB has been seeded
                }

                context.Platform.AddRange(
                     new Platform
                     {

                         Name = "YouTube"
                     },

                     new Platform
                     {
                         Name = "Instagram",
                     },

                     new Platform
                     {
                         Name = "SnapChat",
                     },

                   new Platform
                   {

                       Name = "Facebook",
                   },

                   new Platform
                   {

                       Name = "Twitch"
                   },
                   new Platform
                   {
                       Name = "Twitter"
                   },
                   new Platform
                   {
                       Name = "Website"
                   }

                );
                context.SaveChanges();

            }


        }


    }
}


