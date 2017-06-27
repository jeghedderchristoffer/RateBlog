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

                if (context.Kategori.ToList().Count != 0)
                {
                    return;   // DB has been seeded
                }

                context.Kategori.AddRange(
                     new Kategori
                     {
                         KategoriNavn = "Lifestyle"
                     }, 
                     new Kategori
                     {
                         KategoriNavn = "DIY"
                     },
                     new Kategori
                     {
                         KategoriNavn = "VLOG"
                     },
                     new Kategori
                     {
                         KategoriNavn = "Beauty"
                     }, 
                     new Kategori
                     {
                         KategoriNavn = "Gaming"
                     },
                     new Kategori
                     {
                         KategoriNavn = "Entertainment"
                     },
                     new Kategori
                     {
                         KategoriNavn = "Food"
                     },
                     new Kategori
                     {
                         KategoriNavn = "Fashion"
                     }, 
                     new Kategori
                     {
                         KategoriNavn = "Mommy"
                     }

                );
                context.SaveChanges();

            }


        }
    }
}
