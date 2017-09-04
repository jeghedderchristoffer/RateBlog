using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RateBlog.Migrations
{
    public partial class datetimeonreport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReportFeedback",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ApplicationUserId = table.Column<string>(nullable: true),
                    Description = table.Column<string>(maxLength: 200, nullable: true),
                    Discrimination = table.Column<bool>(nullable: false),
                    FeedbackId = table.Column<string>(nullable: true),
                    IsRead = table.Column<bool>(nullable: false),
                    LanguageUse = table.Column<bool>(nullable: false),
                    Other = table.Column<bool>(nullable: false),
                    ReportedDateTime = table.Column<DateTime>(nullable: false),
                    Spam = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportFeedback", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReportFeedback_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ReportFeedback_Feedback_FeedbackId",
                        column: x => x.FeedbackId,
                        principalTable: "Feedback",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReportFeedback_ApplicationUserId",
                table: "ReportFeedback",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportFeedback_FeedbackId",
                table: "ReportFeedback",
                column: "FeedbackId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReportFeedback");
        }
    }
}
