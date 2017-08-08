using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RateBlog.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Models
{
    public class SeedKategoriData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {

                if (context.Category.ToList().Count != 0)
                {
                    return;   // DB has been seeded
                }

                context.Category.AddRange(
                     new Category
                     {
                         Name = "Lifestyle"
                     }, 
                     new Category
                     {
                         Name = "DIY"
                     },
                     new Category
                     {
                         Name = "VLOG"
                     },
                     new Category
                     {
                         Name = "Beauty"
                     }, 
                     new Category
                     {
                         Name = "Gaming"
                     },
                     new Category
                     {
                         Name = "Entertainment"
                     },
                     new Category
                     {
                         Name = "Food"
                     },
                     new Category
                     {
                         Name = "Fashion"
                     }, 
                     new Category
                     {
                         Name = "Mommy"
                     }

                );
                context.SaveChanges();

            }


        }
    }
}
