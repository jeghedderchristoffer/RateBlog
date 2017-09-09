using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RateBlog.Migrations
{
    public partial class emailSettingsCascade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
                name: "FK_EmailNotifications_AspNetUsers_Id",
                table: "EmailNotifications",
                column: "Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmailNotifications_AspNetUsers_Id",
                table: "EmailNotifications");
        }
    }
}
