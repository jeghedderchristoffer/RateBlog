using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RateBlog.Data.Migrations
{
    public partial class UpdatedRatingAgain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Sprog",
                table: "Rating",
                newName: "TidFulgt");

            migrationBuilder.RenameColumn(
                name: "KommerUd",
                table: "Rating",
                newName: "Orginalitet");

            migrationBuilder.RenameColumn(
                name: "Feedback",
                table: "Rating",
                newName: "Review");

            migrationBuilder.AddColumn<int>(
                name: "Aktivitet",
                table: "Rating",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Interaktion",
                table: "Rating",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Aktivitet",
                table: "Rating");

            migrationBuilder.DropColumn(
                name: "Interaktion",
                table: "Rating");

            migrationBuilder.RenameColumn(
                name: "TidFulgt",
                table: "Rating",
                newName: "Sprog");

            migrationBuilder.RenameColumn(
                name: "Review",
                table: "Rating",
                newName: "Feedback");

            migrationBuilder.RenameColumn(
                name: "Orginalitet",
                table: "Rating",
                newName: "KommerUd");
        }
    }
}
