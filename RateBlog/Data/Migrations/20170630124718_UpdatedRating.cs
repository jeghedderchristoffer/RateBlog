using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RateBlog.Data.Migrations
{
    public partial class UpdatedRating : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Rating",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rating_ApplicationUserId",
                table: "Rating",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rating_AspNetUsers_ApplicationUserId",
                table: "Rating",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rating_AspNetUsers_ApplicationUserId",
                table: "Rating");

            migrationBuilder.DropIndex(
                name: "IX_Rating_ApplicationUserId",
                table: "Rating");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Rating");
        }
    }
}
