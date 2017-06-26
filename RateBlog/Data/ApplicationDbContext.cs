
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RateBlog.Models;


namespace RateBlog.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Influenter> Influenter { get; set; }
        public DbSet<Platform> Platform { get; set; }
        public DbSet<Rating> Rating { get; set; }
        public DbSet<Kategori> Kategori { get; set; }
        public DbSet<InfluenterPlatform> InfluenterPlatform { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<InfluenterPlatform>()
                .HasKey(t => new { t.InfluenterId, t.PlatformId });

            builder.Entity<InfluenterPlatform>()
                .HasOne(pt => pt.Influenter)
                .WithMany(p => p.InfluenterPlatform)
                .HasForeignKey(pt => pt.InfluenterId);

            builder.Entity<InfluenterPlatform>()
                .HasOne(pt => pt.Platform)
                .WithMany(t => t.InfluenterPlatform)
                .HasForeignKey(pt => pt.PlatformId);

            builder.Entity<InfluenterRating>()
                .HasKey(t => new { t.InfluenterId, t.RatingId });

            builder.Entity<InfluenterRating>()
                .HasOne(pt => pt.Influenter)
                .WithMany(p => p.InfluenterRating)
                .HasForeignKey(pt => pt.InfluenterId);

            builder.Entity<InfluenterRating>()
                .HasOne(pt => pt.Rating)
                .WithMany(t => t.InfluenterRating)
                .HasForeignKey(pt => pt.RatingId);

            builder.Entity<InfluenterKategori>()
                .HasKey(t => new { t.InfluenterId, t.KategoriId });

            builder.Entity<InfluenterKategori>()
                .HasOne(pt => pt.Influenter)
                .WithMany(p => p.InfluenterKategori)
                .HasForeignKey(pt => pt.InfluenterId);

            builder.Entity<InfluenterKategori>()
                .HasOne(pt => pt.Kategori)
                .WithMany(t => t.InfluenterKategori)
                .HasForeignKey(pt => pt.KategoriId);




            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
