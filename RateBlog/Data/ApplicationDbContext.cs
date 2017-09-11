
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RateBlog.Models;
using Microsoft.EntityFrameworkCore.Metadata;

namespace RateBlog.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Influencer> Influencer { get; set; }
        public DbSet<Platform> Platform { get; set; }
        public DbSet<Feedback> Feedback { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<InfluencerPlatform> InfluencerPlatform { get; set; }
        public DbSet<InfluencerCategory> InfluencerCategory { get; set; }
        public DbSet<FeedbackReport> FeedbackReports { get; set; }
        public DbSet<EmailNotification> EmailNotifications { get; set; }
        public DbSet<BlogArticle> BlogArticles { get; set; } 

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<InfluencerPlatform>()
                .HasKey(t => new { t.InfluencerId, t.PlatformId });

            builder.Entity<InfluencerPlatform>()
                .HasOne(pt => pt.Influencer)
                .WithMany(p => p.InfluenterPlatform)
                .HasForeignKey(pt => pt.InfluencerId);

            builder.Entity<InfluencerPlatform>()
                .HasOne(pt => pt.Platform)
                .WithMany(t => t.InfluenterPlatform)
                .HasForeignKey(pt => pt.PlatformId);

            builder.Entity<InfluencerCategory>()
                .HasKey(t => new { t.InfluencerId, t.CategoryId });

            builder.Entity<InfluencerCategory>()
                .HasOne(pt => pt.Influencer)
                .WithMany(p => p.InfluenterKategori)
                .HasForeignKey(pt => pt.InfluencerId);

            builder.Entity<InfluencerCategory>()
                .HasOne(pt => pt.Category)
                .WithMany(t => t.InfluenterKategori)
                .HasForeignKey(pt => pt.CategoryId);

            builder.Entity<Influencer>()
                .HasMany(x => x.Ratings)
                .WithOne(p => p.Influenter)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<EmailNotification>()
                .HasOne(x => x.ApplicationUser)
                .WithOne(x => x.EmailNotification)
                .OnDelete(DeleteBehavior.Cascade); 


            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
