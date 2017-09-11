using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RateBlog.Migrations
{
    public partial class blogAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BlogArticles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ArticleHeader = table.Column<string>(nullable: true),
                    ArticlePicture = table.Column<byte[]>(nullable: true),
                    ArticleText = table.Column<string>(nullable: true),
                    Author = table.Column<string>(nullable: true),
                    BriefText = table.Column<string>(nullable: true),
                    Categories = table.Column<string>(nullable: true),
                    DateTime = table.Column<string>(nullable: true),
                    IndexPicture = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogArticles", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlogArticles");
        }
    }
}
