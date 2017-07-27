using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace RateBlog.Data.Migrations
{
    public partial class rasmus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateIndex(
                name: "IX_EkspertRating_ApplicationUserId",
                table: "EkspertRating",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_EkspertRating_InfluenterId",
                table: "EkspertRating",
                column: "InfluenterId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EkspertRating");
        }
    }
}
