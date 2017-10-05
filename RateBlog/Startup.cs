using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Bestfluence.Data;
using Bestfluence.Models;
using Bestfluence.Services;
using Microsoft.AspNetCore.Http;
using Bestfluence.Repository;
using System.Globalization;
using Bestfluence.Services.Interfaces;
using Bestfluence.Helper;
using Microsoft.AspNetCore.Authentication.OAuth;
using System.Security.Claims;
using Bestfluence.Services.Interfaces;
using Bestfluence.Services;
using Microsoft.AspNetCore.Identity;

namespace Bestfluence
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            if (env.IsDevelopment())
            {
                // For more details on using the user secret store see https://go.microsoft.com/fwlink/?LinkID=532709
                builder.AddUserSecrets<Startup>();
            }

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.User.RequireUniqueEmail = true;

                // Skift login path
                options.Cookies.ApplicationCookie.LoginPath = new PathString();
            })
                .AddErrorDescriber<CustomIdentityErrorDescriber>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();


            services.AddRouting(options => options.LowercaseUrls = true);

            services.AddMemoryCache();
            services.AddSession();
            services.AddMvc();

            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();

            services.AddTransient<IInfluencerRepository, InfluencerRepository>();
            services.AddTransient<IRepository<Platform>, Repository<Platform>>();
            services.AddTransient<IRepository<Category>, Repository<Category>>();
            services.AddTransient<IRepository<Feedback>, Repository<Feedback>>();
            services.AddTransient<IRepository<FeedbackReport>, Repository<FeedbackReport>>();
            services.AddTransient<IRepository<EmailNotification>, Repository<EmailNotification>>();
            services.AddTransient<IRepository<BlogArticle>, Repository<BlogArticle>>();

            services.AddTransient<IInfluencerService, InfluencerService>();
            services.AddTransient<IAdminService, AdminService>();
            services.AddTransient<IFeedbackService, FeedbackService>();
            services.AddTransient<IBlogService, BlogService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            // Skal bruges til at seperere double's decimaler med . og ikke , //
            var cultureInfo = new CultureInfo("da-DK");
            cultureInfo.NumberFormat.NumberDecimalSeparator = ".";
            CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
            CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

            app.UseStaticFiles();

            app.UseIdentity();

            app.UseSession();

            app.UseFacebookAuthentication(new FacebookOptions()
            {
                AppId = Configuration["Authentication:Facebook:AppId"],
                AppSecret = Configuration["Authentication:Facebook:AppSecret"],
                //AppId = "175669099670909", Facebook Test 
                //AppSecret = "803adc0f1857df807529b191cb51ce23", Facebook Test
                SaveTokens = true,
                Scope =
                {
                    "user_birthday",
                    "public_profile",
                    "user_location"
                },
                Fields =
                {
                    "birthday", //User's DOB  
                    "picture", //User Profile Image  
                    "name", //User Full Name  
                    "email", //User Email  
                    "gender", //user's Gender  
                    "location"
                },
            });



            app.UseGoogleAuthentication(new GoogleOptions()
            {
                ClientId = Configuration["Authentication:Google:ClientId"],
                ClientSecret = Configuration["Authentication:Google:ClientSecret"],
                Scope =
                {
                    "https://www.googleapis.com/auth/plus.login",
                    "https://www.googleapis.com/auth/plus.me"
                },
                Events = new OAuthEvents
                {
                    OnCreatingTicket = context =>
                    {
                        // Extract the "language" property from the JSON payload returned by
                        // the user profile endpoint and add a new "urn:language" claim.
                        var gender = context.User.Value<string>("gender");
                        var birthday = context.User.Value<string>("birthday");

                        if (!string.IsNullOrEmpty(gender))
                        {
                            context.Identity.AddClaim(new Claim(ClaimTypes.Gender, gender));
                        }
                        if (!string.IsNullOrEmpty(birthday))
                        {
                            context.Identity.AddClaim(new Claim(ClaimTypes.DateOfBirth, birthday));
                        }

                        return Task.FromResult(0);
                    }
                },
                SaveTokens = true
            });

            SeedData(app); 

            // Add external authentication middleware below. To configure them please see https://go.microsoft.com/fwlink/?LinkID=532715

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                   name: "root",
                   template: "{id}",
                   defaults: new { controller = "Influencer", action = "Profile" });

                routes.MapRoute(
                   name: "blog",
                   template: "Blog/{id}",
                   defaults: new { controller = "Blog", action = "Article" });
            });

        }

        public static void SeedData(IApplicationBuilder app)
        {
            using (var context = app.ApplicationServices.GetRequiredService<ApplicationDbContext>())
            {
                if (context.Platform.Count() == 0)
                {
                    context.Platform.Add(new Platform() { Name = "Twitch" });
                    context.Platform.Add(new Platform() { Name = "Facebook" });
                    context.Platform.Add(new Platform() { Name = "Website" });
                    context.Platform.Add(new Platform() { Name = "SnapChat" });
                    context.Platform.Add(new Platform() { Name = "Twitter" });
                    context.Platform.Add(new Platform() { Name = "YouTube" });
                    context.Platform.Add(new Platform() { Name = "Instagram" });
                    context.Platform.Add(new Platform() { Name = "SecondYouTube" });
                    context.SaveChanges();
                }

                if(context.Category.Count() == 0)
                {
                    context.Category.Add(new Category() { Name = "Gaming" });
                    context.Category.Add(new Category() { Name = "Personal" });
                    context.Category.Add(new Category() { Name = "Interests" });
                    context.Category.Add(new Category() { Name = "Entertainment" });
                    context.Category.Add(new Category() { Name = "Fashion" });
                    context.Category.Add(new Category() { Name = "Lifestyle" });
                    context.Category.Add(new Category() { Name = "Beauty" });
                    context.SaveChanges();
                }

                if(context.Roles.Count() == 0)
                {
                    context.Roles.Add(new IdentityRole() { Name = "Influencer", NormalizedName = "INFLUENCER" });
                    context.Roles.Add(new IdentityRole() { Name = "Admin", NormalizedName = "ADMIN" });
                    context.SaveChanges(); 
                }

            }
        }

    }

}
