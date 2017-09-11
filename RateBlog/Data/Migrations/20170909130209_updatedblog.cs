using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RateBlog.Migrations
{
    public partial class updatedblog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BriefText",
                table: "BlogArticles",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "ArticleHeader",
                table: "BlogArticles",
                newName: "Description");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateTime",
                table: "BlogArticles",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Title",
                table: "BlogArticles",
                newName: "BriefText");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "BlogArticles",
                newName: "ArticleHeader");

            migrationBuilder.AlterColumn<string>(
                name: "DateTime",
                table: "BlogArticles",
                nullable: true,
                oldClrType: typeof(DateTime));
        }
    }
}
