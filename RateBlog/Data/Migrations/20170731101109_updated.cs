using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace RateBlog.Data.Migrations
{
    public partial class updated : Migration
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

            migrationBuilder.AddColumn<byte[]>(
                name: "ImageFile",
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

            migrationBuilder.AddColumn<string>(
                name: "ProfileText",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Influenter",
                columns: table => new
                {
                    InfluenterId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Alias = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Influenter", x => x.InfluenterId);
                });

            migrationBuilder.CreateTable(
                name: "Kategori",
                columns: table => new
                {
                    KategoriId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    KategoriNavn = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kategori", x => x.KategoriId);
                });

            migrationBuilder.CreateTable(
                name: "Platform",
                columns: table => new
                {
                    PlatformId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    PlatformNavn = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Platform", x => x.PlatformId);
                });

            migrationBuilder.CreateTable(
                name: "EkspertRating",
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
                    table.PrimaryKey("PK_EkspertRating", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EkspertRating_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EkspertRating_Influenter_InfluenterId",
                        column: x => x.InfluenterId,
                        principalTable: "Influenter",
                        principalColumn: "InfluenterId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Rating",
                columns: table => new
                {
                    RatingId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Anbefaling = table.Column<bool>(nullable: true),
                    Answer = table.Column<string>(nullable: true),
                    ApplicationUserId = table.Column<string>(nullable: true),
                    Feedback = table.Column<string>(nullable: true),
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
                    table.PrimaryKey("PK_Rating", x => x.RatingId);
                    table.ForeignKey(
                        name: "FK_Rating_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Rating_Influenter_InfluenterId",
                        column: x => x.InfluenterId,
                        principalTable: "Influenter",
                        principalColumn: "InfluenterId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InfluenterKategori",
                columns: table => new
                {
                    InfluenterId = table.Column<int>(nullable: false),
                    KategoriId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InfluenterKategori", x => new { x.InfluenterId, x.KategoriId });
                    table.ForeignKey(
                        name: "FK_InfluenterKategori_Influenter_InfluenterId",
                        column: x => x.InfluenterId,
                        principalTable: "Influenter",
                        principalColumn: "InfluenterId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InfluenterKategori_Kategori_KategoriId",
                        column: x => x.KategoriId,
                        principalTable: "Kategori",
                        principalColumn: "KategoriId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InfluenterPlatform",
                columns: table => new
                {
                    InfluenterId = table.Column<int>(nullable: false),
                    PlatformId = table.Column<int>(nullable: false),
                    Link = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InfluenterPlatform", x => new { x.InfluenterId, x.PlatformId });
                    table.ForeignKey(
                        name: "FK_InfluenterPlatform_Influenter_InfluenterId",
                        column: x => x.InfluenterId,
                        principalTable: "Influenter",
                        principalColumn: "InfluenterId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InfluenterPlatform_Platform_PlatformId",
                        column: x => x.PlatformId,
                        principalTable: "Platform",
                        principalColumn: "PlatformId",
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
                name: "IX_EkspertRating_ApplicationUserId",
                table: "EkspertRating",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_EkspertRating_InfluenterId",
                table: "EkspertRating",
                column: "InfluenterId");

            migrationBuilder.CreateIndex(
                name: "IX_InfluenterKategori_KategoriId",
                table: "InfluenterKategori",
                column: "KategoriId");

            migrationBuilder.CreateIndex(
                name: "IX_InfluenterPlatform_PlatformId",
                table: "InfluenterPlatform",
                column: "PlatformId");

            migrationBuilder.CreateIndex(
                name: "IX_Rating_ApplicationUserId",
                table: "Rating",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Rating_InfluenterId",
                table: "Rating",
                column: "InfluenterId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Influenter_InfluenterId",
                table: "AspNetUsers",
                column: "InfluenterId",
                principalTable: "Influenter",
                principalColumn: "InfluenterId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Influenter_InfluenterId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "EkspertRating");

            migrationBuilder.DropTable(
                name: "InfluenterKategori");

            migrationBuilder.DropTable(
                name: "InfluenterPlatform");

            migrationBuilder.DropTable(
                name: "Rating");

            migrationBuilder.DropTable(
                name: "Kategori");

            migrationBuilder.DropTable(
                name: "Platform");

            migrationBuilder.DropTable(
                name: "Influenter");

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
                name: "ImageFile",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "InfluenterId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ProfileText",
                table: "AspNetUsers");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_UserId",
                table: "AspNetUserRoles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName");
        }
    }
}
