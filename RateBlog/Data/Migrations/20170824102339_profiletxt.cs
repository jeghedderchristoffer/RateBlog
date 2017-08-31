using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RateBlog.Migrations
{
    public partial class profiletxt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfileText",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "ProfileText",
                table: "Influencer",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfileText",
                table: "Influencer");

            migrationBuilder.AddColumn<string>(
                name: "ProfileText",
                table: "AspNetUsers",
                nullable: true);
        }
    }
}
