using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace RateBlog.Data.Migrations
{
    public partial class added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AspNetUserRoles_UserId",
                table: "AspNetUserRoles");

            migrationBuilder.DropIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles");

            migrationBuilder.CreateTable(
                name: "Influenter",
                columns: table => new
                {
                    InfluenterId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Alder = table.Column<int>(nullable: true),
                    Alias = table.Column<string>(nullable: true),
                    Efternavn = table.Column<string>(nullable: true),
                    Fornavn = table.Column<string>(nullable: true),
                    Links = table.Column<string>(nullable: true),
                    Profiltekst = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Influenter", x => x.InfluenterId);
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
                name: "Rating",
                columns: table => new
                {
                    RatingId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Feedback = table.Column<string>(nullable: true),
                    KommerUd = table.Column<int>(nullable: false),
                    Kvalitet = table.Column<int>(nullable: false),
                    Sprog = table.Column<int>(nullable: false),
                    Troværdighed = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rating", x => x.RatingId);
                });

            migrationBuilder.CreateTable(
                name: "InfluenterPlatform",
                columns: table => new
                {
                    InfluenterId = table.Column<int>(nullable: false),
                    PlatformId = table.Column<int>(nullable: false)
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

            migrationBuilder.CreateTable(
                name: "InfluenterRating",
                columns: table => new
                {
                    InfluenterId = table.Column<int>(nullable: false),
                    RatingId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InfluenterRating", x => new { x.InfluenterId, x.RatingId });
                    table.ForeignKey(
                        name: "FK_InfluenterRating_Influenter_InfluenterId",
                        column: x => x.InfluenterId,
                        principalTable: "Influenter",
                        principalColumn: "InfluenterId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InfluenterRating_Rating_RatingId",
                        column: x => x.RatingId,
                        principalTable: "Rating",
                        principalColumn: "RatingId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InfluenterPlatform_PlatformId",
                table: "InfluenterPlatform",
                column: "PlatformId");

            migrationBuilder.CreateIndex(
                name: "IX_InfluenterRating_RatingId",
                table: "InfluenterRating",
                column: "RatingId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InfluenterPlatform");

            migrationBuilder.DropTable(
                name: "InfluenterRating");

            migrationBuilder.DropTable(
                name: "Platform");

            migrationBuilder.DropTable(
                name: "Influenter");

            migrationBuilder.DropTable(
                name: "Rating");

            migrationBuilder.DropIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles");

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
