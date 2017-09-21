﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using RateBlog.Data;

namespace RateBlog.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20170919190308_updatedDatetime")]
    partial class updatedDatetime
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

                    b.Property<DateTime>("BirthDay");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<DateTime>("Created");

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("Gender");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("Name");

                    b.Property<bool>("NewsLetter");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<int>("Postnummer");

                    b.Property<byte[]>("ProfilePicture");

                    b.Property<string>("SecurityStamp");

                    b.Property<DateTime>("TermsAndConditions");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("RateBlog.Models.BlogArticle", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<byte[]>("ArticlePicture");

                    b.Property<string>("ArticleText");

                    b.Property<string>("Author");

                    b.Property<string>("Categories");

                    b.Property<DateTime>("DateTime");

                    b.Property<string>("Description");

                    b.Property<byte[]>("IndexPicture");

                    b.Property<bool>("Publish");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.ToTable("BlogArticles");
                });

            modelBuilder.Entity("RateBlog.Models.BlogComment", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ApplicationUserId")
                        .IsRequired();

                    b.Property<string>("BlogArticleId")
                        .IsRequired();

                    b.Property<string>("Comment");

                    b.Property<DateTime>("DateTime");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationUserId");

                    b.HasIndex("BlogArticleId");

                    b.ToTable("BlogComments");
                });

            modelBuilder.Entity("RateBlog.Models.BlogRating", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ApplicationUserId")
                        .IsRequired();

                    b.Property<string>("BlogArticleId")
                        .IsRequired();

                    b.Property<int>("Rate");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationUserId");

                    b.HasIndex("BlogArticleId");

                    b.ToTable("BlogRatings");
                });

            modelBuilder.Entity("RateBlog.Models.Category", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Category");
                });

            modelBuilder.Entity("RateBlog.Models.EmailNotification", b =>
                {
                    b.Property<string>("Id");

                    b.Property<bool>("FeedbackUpdate");

                    b.Property<bool>("NewsLetter");

                    b.HasKey("Id");

                    b.ToTable("EmailNotifications");
                });

            modelBuilder.Entity("RateBlog.Models.Feedback", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Anbefaling");

                    b.Property<string>("Answer");

                    b.Property<DateTime>("AnswerDateTime");

                    b.Property<string>("ApplicationUserId");

                    b.Property<bool>("BasedOnFacebook");

                    b.Property<bool>("BasedOnInstagram");

                    b.Property<bool>("BasedOnSnapchat");

                    b.Property<bool>("BasedOnTwitch");

                    b.Property<bool>("BasedOnTwitter");

                    b.Property<bool>("BasedOnWebsite");

                    b.Property<bool>("BasedOnYoutube");

                    b.Property<string>("FeedbackBetter");

                    b.Property<DateTime>("FeedbackDateTime");

                    b.Property<string>("FeedbackGood");

                    b.Property<string>("InfluenterId");

                    b.Property<int>("Interaktion");

                    b.Property<bool>("IsAnswerRead");

                    b.Property<bool>("IsRead");

                    b.Property<int>("Kvalitet");

                    b.Property<int>("Opførsel");

                    b.Property<int>("Troværdighed");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationUserId");

                    b.HasIndex("InfluenterId");

                    b.ToTable("Feedback");
                });

            modelBuilder.Entity("RateBlog.Models.FeedbackReport", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ApplicationUserId");

                    b.Property<DateTime>("DateTime");

                    b.Property<string>("Description")
                        .HasMaxLength(200);

                    b.Property<string>("FeedbackId");

                    b.Property<bool>("IsRead");

                    b.Property<string>("Reason");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationUserId");

                    b.HasIndex("FeedbackId");

                    b.ToTable("FeedbackReports");
                });

            modelBuilder.Entity("RateBlog.Models.Influencer", b =>
                {
                    b.Property<string>("Id");

                    b.Property<string>("Alias")
                        .IsRequired();

                    b.Property<bool>("IsApproved");

                    b.Property<string>("ProfileText");

                    b.Property<string>("Url");

                    b.HasKey("Id");

                    b.ToTable("Influencer");
                });

            modelBuilder.Entity("RateBlog.Models.InfluencerCategory", b =>
                {
                    b.Property<string>("InfluencerId");

                    b.Property<string>("CategoryId");

                    b.HasKey("InfluencerId", "CategoryId");

                    b.HasIndex("CategoryId");

                    b.ToTable("InfluencerCategory");
                });

            modelBuilder.Entity("RateBlog.Models.InfluencerPlatform", b =>
                {
                    b.Property<string>("InfluencerId");

                    b.Property<string>("PlatformId");

                    b.Property<string>("Link");

                    b.HasKey("InfluencerId", "PlatformId");

                    b.HasIndex("PlatformId");

                    b.ToTable("InfluencerPlatform");
                });

            modelBuilder.Entity("RateBlog.Models.Platform", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Platform");
                });

            modelBuilder.Entity("RateBlog.Models.Vote", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Active");

                    b.Property<DateTime>("DateTime");

                    b.Property<string>("InfluencerId")
                        .IsRequired();

                    b.Property<string>("Title")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("InfluencerId");

                    b.ToTable("Votes");
                });

            modelBuilder.Entity("RateBlog.Models.VoteAnswer", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ApplicationUserId")
                        .IsRequired();

                    b.Property<string>("VoteQuestionId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("ApplicationUserId");

                    b.HasIndex("VoteQuestionId");

                    b.ToTable("VoteAnswers");
                });

            modelBuilder.Entity("RateBlog.Models.VoteQuestion", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Question")
                        .IsRequired();

                    b.Property<string>("VoteId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("VoteId");

                    b.ToTable("VoteQuestions");
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

            modelBuilder.Entity("RateBlog.Models.BlogComment", b =>
                {
                    b.HasOne("RateBlog.Models.ApplicationUser", "ApplicationUser")
                        .WithMany()
                        .HasForeignKey("ApplicationUserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("RateBlog.Models.BlogArticle", "BlogArticle")
                        .WithMany("BlogComments")
                        .HasForeignKey("BlogArticleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("RateBlog.Models.BlogRating", b =>
                {
                    b.HasOne("RateBlog.Models.ApplicationUser", "ApplicationUser")
                        .WithMany()
                        .HasForeignKey("ApplicationUserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("RateBlog.Models.BlogArticle", "BlogArticle")
                        .WithMany("BlogRatings")
                        .HasForeignKey("BlogArticleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("RateBlog.Models.EmailNotification", b =>
                {
                    b.HasOne("RateBlog.Models.ApplicationUser", "ApplicationUser")
                        .WithOne("EmailNotification")
                        .HasForeignKey("RateBlog.Models.EmailNotification", "Id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("RateBlog.Models.Feedback", b =>
                {
                    b.HasOne("RateBlog.Models.ApplicationUser", "ApplicationUser")
                        .WithMany("Ratings")
                        .HasForeignKey("ApplicationUserId");

                    b.HasOne("RateBlog.Models.Influencer", "Influenter")
                        .WithMany("Ratings")
                        .HasForeignKey("InfluenterId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("RateBlog.Models.FeedbackReport", b =>
                {
                    b.HasOne("RateBlog.Models.ApplicationUser", "ApplicationUser")
                        .WithMany()
                        .HasForeignKey("ApplicationUserId");

                    b.HasOne("RateBlog.Models.Feedback", "Feedback")
                        .WithMany()
                        .HasForeignKey("FeedbackId");
                });

            modelBuilder.Entity("RateBlog.Models.Influencer", b =>
                {
                    b.HasOne("RateBlog.Models.ApplicationUser", "ApplicationUser")
                        .WithMany()
                        .HasForeignKey("Id")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("RateBlog.Models.InfluencerCategory", b =>
                {
                    b.HasOne("RateBlog.Models.Category", "Category")
                        .WithMany("InfluenterKategori")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("RateBlog.Models.Influencer", "Influencer")
                        .WithMany("InfluenterKategori")
                        .HasForeignKey("InfluencerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("RateBlog.Models.InfluencerPlatform", b =>
                {
                    b.HasOne("RateBlog.Models.Influencer", "Influencer")
                        .WithMany("InfluenterPlatform")
                        .HasForeignKey("InfluencerId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("RateBlog.Models.Platform", "Platform")
                        .WithMany("InfluenterPlatform")
                        .HasForeignKey("PlatformId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("RateBlog.Models.Vote", b =>
                {
                    b.HasOne("RateBlog.Models.Influencer", "Influencer")
                        .WithMany()
                        .HasForeignKey("InfluencerId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("RateBlog.Models.VoteAnswer", b =>
                {
                    b.HasOne("RateBlog.Models.ApplicationUser", "ApplicationUser")
                        .WithMany()
                        .HasForeignKey("ApplicationUserId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("RateBlog.Models.VoteQuestion", "VoteQuestion")
                        .WithMany("VoteAnswers")
                        .HasForeignKey("VoteQuestionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("RateBlog.Models.VoteQuestion", b =>
                {
                    b.HasOne("RateBlog.Models.Vote", "Vote")
                        .WithMany("VoteQuestions")
                        .HasForeignKey("VoteId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
