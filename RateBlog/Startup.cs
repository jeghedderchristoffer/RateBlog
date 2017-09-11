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
using RateBlog.Data;
using RateBlog.Models;
using RateBlog.Services;
using Microsoft.AspNetCore.Http;
using RateBlog.Repository;
using System.Globalization;
using RateBlog.Services.Interfaces;
using RateBlog.Helper;

namespace RateBlog
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
                //AppId = System.Environment.GetEnvironmentVariable("Authentication:Facebook:AppId"),
                //AppSecret = System.Environment.GetEnvironmentVariable("Authentication:Facebook:AppSecret"),
                AppId = Configuration["Authentication:Facebook:AppId"],
                AppSecret = Configuration["Authentication:Facebook:AppSecret"],
                SaveTokens = true,
                Scope =
                {
                    //"user_birthday",
                    "public_profile"
                },
                Fields = 
                {
                    "birthday", //User's DOB  
                    "picture", //User Profile Image  
                    "name", //User Full Name  
                    "email", //User Email  
                    "gender", //user's Gender  
                }
            });



            app.UseGoogleAuthentication(new GoogleOptions()
            {
                //ClientId = System.Environment.GetEnvironmentVariable("Authentication:Google:ClientId"),
                //ClientSecret = System.Environment.GetEnvironmentVariable("Authentication:Google:ClientSecret"),
                ClientId = Configuration["Authentication:Google:ClientId"],
                ClientSecret = Configuration["Authentication:Google:ClientSecret"],
                Scope =
                {
                    "https://www.googleapis.com/auth/plus.login"
                },
                SaveTokens = true              
            });

            // Add external authentication middleware below. To configure them please see https://go.microsoft.com/fwlink/?LinkID=532715

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

        }

    }

}
