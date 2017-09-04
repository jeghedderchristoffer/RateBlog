using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RateBlog.Migrations
{
    public partial class treportinguseradded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TheUserWhoReportedId",
                table: "ReportFeedback",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReportFeedback_TheUserWhoReportedId",
                table: "ReportFeedback",
                column: "TheUserWhoReportedId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReportFeedback_AspNetUsers_TheUserWhoReportedId",
                table: "ReportFeedback",
                column: "TheUserWhoReportedId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReportFeedback_AspNetUsers_TheUserWhoReportedId",
                table: "ReportFeedback");

            migrationBuilder.DropIndex(
                name: "IX_ReportFeedback_TheUserWhoReportedId",
                table: "ReportFeedback");

            migrationBuilder.DropColumn(
                name: "TheUserWhoReportedId",
                table: "ReportFeedback");
        }
    }
}
