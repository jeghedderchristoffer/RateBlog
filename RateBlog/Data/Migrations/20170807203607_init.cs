using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace RateBlog.Data.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AspNetUserRoles_UserId",
                table: "AspNetUserRoles");

            migrationBuilder.DropIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles");

            migrationBuilder.AddColumn<DateTime>(
                name: "Birth",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InfluenterId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "ProfilePicture",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfileText",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Influencer",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Alias = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Influencer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Platform",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Platform", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExpertFeedback",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Anbefaling = table.Column<bool>(nullable: true),
                    AnbefalingString = table.Column<string>(nullable: true),
                    ApplicationUserId = table.Column<string>(nullable: true),
                    BestfluenceFeedback = table.Column<string>(nullable: true),
                    BestfluenceFeedbackString = table.Column<string>(nullable: true),
                    InfluenterId = table.Column<int>(nullable: false),
                    Interaktion = table.Column<int>(nullable: false),
                    InteraktionString = table.Column<string>(nullable: true),
                    Kvalitet = table.Column<int>(nullable: false),
                    KvalitetString = table.Column<string>(nullable: true),
                    OffentligFeedback = table.Column<string>(nullable: true),
                    OffentligFeedbackString = table.Column<string>(nullable: true),
                    Opførsel = table.Column<int>(nullable: false),
                    OpførselString = table.Column<string>(nullable: true),
                    RateDateTime = table.Column<DateTime>(nullable: false),
                    Troværdighed = table.Column<int>(nullable: false),
                    TroværdighedString = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpertFeedback", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExpertFeedback_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExpertFeedback_Influencer_InfluenterId",
                        column: x => x.InfluenterId,
                        principalTable: "Influencer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Feedback",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Anbefaling = table.Column<bool>(nullable: true),
                    Answer = table.Column<string>(nullable: true),
                    ApplicationUserId = table.Column<string>(nullable: true),
                    FeedbackText = table.Column<string>(nullable: true),
                    InfluenterId = table.Column<int>(nullable: false),
                    Interaktion = table.Column<int>(nullable: false),
                    IsAnswerRead = table.Column<bool>(nullable: false),
                    IsRead = table.Column<bool>(nullable: false),
                    Kvalitet = table.Column<int>(nullable: false),
                    Opførsel = table.Column<int>(nullable: false),
                    RateDateTime = table.Column<DateTime>(nullable: false),
                    Troværdighed = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feedback", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Feedback_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Feedback_Influencer_InfluenterId",
                        column: x => x.InfluenterId,
                        principalTable: "Influencer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InfluencerCategory",
                columns: table => new
                {
                    InfluencerId = table.Column<int>(nullable: false),
                    CategoryId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InfluencerCategory", x => new { x.InfluencerId, x.CategoryId });
                    table.ForeignKey(
                        name: "FK_InfluencerCategory_Category_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InfluencerCategory_Influencer_InfluencerId",
                        column: x => x.InfluencerId,
                        principalTable: "Influencer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InfluencerPlatform",
                columns: table => new
                {
                    InfluencerId = table.Column<int>(nullable: false),
                    PlatformId = table.Column<int>(nullable: false),
                    Link = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InfluencerPlatform", x => new { x.InfluencerId, x.PlatformId });
                    table.ForeignKey(
                        name: "FK_InfluencerPlatform_Influencer_InfluencerId",
                        column: x => x.InfluencerId,
                        principalTable: "Influencer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InfluencerPlatform_Platform_PlatformId",
                        column: x => x.PlatformId,
                        principalTable: "Platform",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_InfluenterId",
                table: "AspNetUsers",
                column: "InfluenterId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExpertFeedback_ApplicationUserId",
                table: "ExpertFeedback",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpertFeedback_InfluenterId",
                table: "ExpertFeedback",
                column: "InfluenterId");

            migrationBuilder.CreateIndex(
                name: "IX_Feedback_ApplicationUserId",
                table: "Feedback",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Feedback_InfluenterId",
                table: "Feedback",
                column: "InfluenterId");

            migrationBuilder.CreateIndex(
                name: "IX_InfluencerCategory_CategoryId",
                table: "InfluencerCategory",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_InfluencerPlatform_PlatformId",
                table: "InfluencerPlatform",
                column: "PlatformId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Influencer_InfluenterId",
                table: "AspNetUsers",
                column: "InfluenterId",
                principalTable: "Influencer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Influencer_InfluenterId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "ExpertFeedback");

            migrationBuilder.DropTable(
                name: "Feedback");

            migrationBuilder.DropTable(
                name: "InfluencerCategory");

            migrationBuilder.DropTable(
                name: "InfluencerPlatform");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropTable(
                name: "Influencer");

            migrationBuilder.DropTable(
                name: "Platform");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_InfluenterId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "Birth",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "City",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "InfluenterId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ProfilePicture",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ProfileText",
                table: "AspNetUsers");


            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName");
        }
    }
}
