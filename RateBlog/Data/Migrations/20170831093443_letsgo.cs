using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RateBlog.Migrations
{
    public partial class letsgo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmailNotification",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    FeedbackUpdate = table.Column<bool>(nullable: false),
                    NewsLetter = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailNotification", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmailNotification");
        }
    }
}
