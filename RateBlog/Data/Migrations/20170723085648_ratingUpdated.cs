using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RateBlog.Data.Migrations
{
    public partial class ratingUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Aktivitet",
                table: "Rating");

            migrationBuilder.DropColumn(
                name: "Orginalitet",
                table: "Rating");

            migrationBuilder.DropColumn(
                name: "SprogBrug",
                table: "Rating");

            migrationBuilder.RenameColumn(
                name: "TidFulgt",
                table: "Rating",
                newName: "Opførsel");

            migrationBuilder.RenameColumn(
                name: "Review",
                table: "Rating",
                newName: "Feedback");

            migrationBuilder.AddColumn<bool>(
                name: "Anbefaling",
                table: "Rating",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Anbefaling",
                table: "Rating");

            migrationBuilder.RenameColumn(
                name: "Opførsel",
                table: "Rating",
                newName: "TidFulgt");

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
                name: "Orginalitet",
                table: "Rating",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SprogBrug",
                table: "Rating",
                nullable: false,
                defaultValue: 0);
        }
    }
}
