using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RateBlog.Migrations
{
    public partial class AddedByteArray : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "ArticlePicture",
                table: "Blog",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Categories",
                table: "Blog",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ArticlePicture",
                table: "Blog");

            migrationBuilder.DropColumn(
                name: "Categories",
                table: "Blog");
        }
    }
}
