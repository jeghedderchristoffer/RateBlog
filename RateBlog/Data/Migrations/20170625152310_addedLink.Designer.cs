using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using RateBlog.Data;

namespace RateBlog.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20170625152310_addedLink")]
    partial class addedLink
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("RateBlog.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<DateTime?>("Birth");

                    b.Property<string>("City");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<int?>("InfluenterId");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("Name");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("ProfileText");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("InfluenterId")
                        .IsUnique();

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("RateBlog.Models.Influenter", b =>
                {
                    b.Property<int>("InfluenterId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Alias")
                        .IsRequired();

                    b.HasKey("InfluenterId");

                    b.ToTable("Influenter");
                });

            modelBuilder.Entity("RateBlog.Models.InfluenterKategori", b =>
                {
                    b.Property<int>("InfluenterId");

                    b.Property<int>("KategoriId");

                    b.HasKey("InfluenterId", "KategoriId");

                    b.HasIndex("KategoriId");

                    b.ToTable("InfluenterKategori");
                });

            modelBuilder.Entity("RateBlog.Models.InfluenterPlatform", b =>
                {
                    b.Property<int>("InfluenterId");

                    b.Property<int>("PlatformId");

                    b.Property<string>("Link");

                    b.HasKey("InfluenterId", "PlatformId");

                    b.HasIndex("PlatformId");

                    b.ToTable("InfluenterPlatform");
                });

            modelBuilder.Entity("RateBlog.Models.InfluenterRating", b =>
                {
                    b.Property<int>("InfluenterId");

                    b.Property<int>("RatingId");

                    b.HasKey("InfluenterId", "RatingId");

                    b.HasIndex("RatingId");

                    b.ToTable("InfluenterRating");
                });

            modelBuilder.Entity("RateBlog.Models.Kategori", b =>
                {
                    b.Property<int>("KategoriId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("KategoriNavn");

                    b.HasKey("KategoriId");

                    b.ToTable("Kategori");
                });

            modelBuilder.Entity("RateBlog.Models.Platform", b =>
                {
                    b.Property<int>("PlatformId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("PlatformNavn");

                    b.HasKey("PlatformId");

                    b.ToTable("Platform");
                });

            modelBuilder.Entity("RateBlog.Models.Rating", b =>
                {
                    b.Property<int>("RatingId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Feedback");

                    b.Property<int>("KommerUd");

                    b.Property<int>("Kvalitet");

                    b.Property<int>("Sprog");

                    b.Property<int>("Troværdighed");

                    b.HasKey("RatingId");

                    b.ToTable("Rating");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Claims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("RateBlog.Models.ApplicationUser")
                        .WithMany("Claims")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("RateBlog.Models.ApplicationUser")
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Users")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("RateBlog.Models.ApplicationUser")
                        .WithMany("Roles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("RateBlog.Models.ApplicationUser", b =>
                {
                    b.HasOne("RateBlog.Models.Influenter", "Influenter")
                        .WithOne("ApplicationUser")
                        .HasForeignKey("RateBlog.Models.ApplicationUser", "InfluenterId");
                });

            modelBuilder.Entity("RateBlog.Models.InfluenterKategori", b =>
                {
                    b.HasOne("RateBlog.Models.Influenter", "Influenter")
                        .WithMany("InfluenterKategori")
                        .HasForeignKey("InfluenterId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("RateBlog.Models.Kategori", "Kategori")
                        .WithMany("InfluenterKategori")
                        .HasForeignKey("KategoriId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("RateBlog.Models.InfluenterPlatform", b =>
                {
                    b.HasOne("RateBlog.Models.Influenter", "Influenter")
                        .WithMany("InfluenterPlatform")
                        .HasForeignKey("InfluenterId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("RateBlog.Models.Platform", "Platform")
                        .WithMany("InfluenterPlatform")
                        .HasForeignKey("PlatformId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("RateBlog.Models.InfluenterRating", b =>
                {
                    b.HasOne("RateBlog.Models.Influenter", "Influenter")
                        .WithMany("InfluenterRating")
                        .HasForeignKey("InfluenterId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("RateBlog.Models.Rating", "Rating")
                        .WithMany("InfluenterRating")
                        .HasForeignKey("RatingId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
