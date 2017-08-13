using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RateBlog.Data.Migrations
{
    public partial class updatedFeedback : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RateDateTime",
                table: "Feedback",
                newName: "FeedbackDateTime");

            migrationBuilder.RenameColumn(
                name: "FeedbackText",
                table: "Feedback",
                newName: "FeedbackGood");

            migrationBuilder.AlterColumn<int>(
                name: "Anbefaling",
                table: "Feedback",
                nullable: false,
                oldClrType: typeof(bool),
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "BasedOnFacebook",
                table: "Feedback",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "BasedOnInstagram",
                table: "Feedback",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "BasedOnSnapchat",
                table: "Feedback",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "BasedOnTwitch",
                table: "Feedback",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "BasedOnTwitter",
                table: "Feedback",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "BasedOnWebsite",
                table: "Feedback",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "BasedOnYoutube",
                table: "Feedback",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "FeedbackBetter",
                table: "Feedback",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BasedOnFacebook",
                table: "Feedback");

            migrationBuilder.DropColumn(
                name: "BasedOnInstagram",
                table: "Feedback");

            migrationBuilder.DropColumn(
                name: "BasedOnSnapchat",
                table: "Feedback");

            migrationBuilder.DropColumn(
                name: "BasedOnTwitch",
                table: "Feedback");

            migrationBuilder.DropColumn(
                name: "BasedOnTwitter",
                table: "Feedback");

            migrationBuilder.DropColumn(
                name: "BasedOnWebsite",
                table: "Feedback");

            migrationBuilder.DropColumn(
                name: "BasedOnYoutube",
                table: "Feedback");

            migrationBuilder.DropColumn(
                name: "FeedbackBetter",
                table: "Feedback");

            migrationBuilder.RenameColumn(
                name: "FeedbackGood",
                table: "Feedback",
                newName: "FeedbackText");

            migrationBuilder.RenameColumn(
                name: "FeedbackDateTime",
                table: "Feedback",
                newName: "RateDateTime");

            migrationBuilder.AlterColumn<bool>(
                name: "Anbefaling",
                table: "Feedback",
                nullable: true,
                oldClrType: typeof(int));
        }
    }
}
