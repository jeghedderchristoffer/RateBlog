using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RateBlog.Data.Migrations
{
    public partial class isAnswerReadAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAnswerRead",
                table: "Rating",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<bool>(
                name: "Anbefaling",
                table: "EkspertRating",
                nullable: true,
                oldClrType: typeof(bool));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAnswerRead",
                table: "Rating");

            migrationBuilder.AlterColumn<bool>(
                name: "Anbefaling",
                table: "EkspertRating",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);
        }
    }
}
