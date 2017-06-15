using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RateBlog.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Models
{
    public class SeedData
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

                         PlatformNavn = "YouTube"
                     },

                     new Platform
                     {
                         PlatformNavn = "Instagram",
                     },

                     new Platform
                     {
                         PlatformNavn = "SnapChat",
                     },

                   new Platform
                   {

                       PlatformNavn = "Facebook",
                   },

                   new Platform
                   {

                       PlatformNavn = "LinkedIn"
                   }

                );
                context.SaveChanges();

            }


        }


    }
}
    

