using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RateBlog.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RateBlog.Models
{
    public class SeedInfluentData
    {

        public static void Initialize(IServiceProvider serviceProvider)
        {
            //using (var context = new ApplicationDbContext(
            //    serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            //{

            //    if (context.Influenter.ToList().Count != 0)
            //    {
            //        return;   // DB has been seeded
            //    }

            //    context.Influenter.AddRange(
            //         new Influenter
            //         {
            //             Fornavn = "Cecilia",
            //             Efternavn = "Demant",
            //             Alias = "Cecilia Demant",
            //             Alder = 23,
            //             Links = "https://www.youtube.com/user/NordicBeautySecretsD",
            //             Profiltekst = "Jeg hedder..... og går meget op i ........."
            //         },

            //         new Influenter
            //         {
            //             Fornavn = "Sandra",
            //             Efternavn = "Willer",
            //             Alias = "Sandra Willer",
            //             Alder = 20,
            //             Links = "http://nouw.com/blogbysandra",
            //             Profiltekst = "Jeg hedder..... og går meget op i ........."
            //         },

            //         new Influenter
            //         {
            //             Fornavn = "Rasmus",
            //             Efternavn = "Brohave",
            //             Alias = "Rasmus Brohave",
            //             Alder = 22,
            //             Links = "https://www.youtube.com/user/RasmusBrohave",
            //             Profiltekst = "Jeg hedder..... og går meget op i ........."
            //         },

            //       new Influenter
            //       {
            //           Fornavn = "Louise",
            //           Efternavn = "Bjerre",
            //           Alias = "LouLiving",
            //           Alder = 22,
            //           Links = "https://www.youtube.com/channel/UCkKoWgJbERdvqhXDFgJ7akw/featured",
            //           Profiltekst = "Heysan, jeg hedder Louise - jeg har ingen humor og så elsker jeg pizza :')"
            //       },

            //       new Influenter
            //       {
            //           Fornavn = "Hamsa",
            //           Efternavn = "Yassin",
            //           Alias = "Hamsa Yassin",
            //           Alder = 18,
            //           Links = "",
            //           Profiltekst = "Homo"
            //       }

            //    );
            //    context.SaveChanges();

            //}


        }
    }
}


