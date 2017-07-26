using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RateBlog.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Models
{
    public class SeedRoleData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {

                if (context.Roles.ToList().Count != 0)
                {
                    return;   // DB has been seeded
                }

                context.Roles.AddRange(
                    new IdentityRole
                    {
                        Name ="Ekspert"
                    },
                    new IdentityRole
                    {
                        Name="Admin"
                    }
                     

                );
                context.SaveChanges();

            }


        }
    }
}
