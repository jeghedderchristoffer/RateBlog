using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RateBlog.Data.Migrations
{
    public partial class updatedUserInfluenter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Alder",
                table: "Influenter");

            migrationBuilder.DropColumn(
                name: "Efternavn",
                table: "Influenter");

            migrationBuilder.DropColumn(
                name: "Fornavn",
                table: "Influenter");

            migrationBuilder.DropColumn(
                name: "Profiltekst",
                table: "Influenter");

            migrationBuilder.AddColumn<int>(
                name: "InfluenterId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_InfluenterId",
                table: "AspNetUsers",
                column: "InfluenterId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Influenter_InfluenterId",
                table: "AspNetUsers",
                column: "InfluenterId",
                principalTable: "Influenter",
                principalColumn: "InfluenterId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Influenter_InfluenterId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_InfluenterId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "InfluenterId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "Alder",
                table: "Influenter",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Efternavn",
                table: "Influenter",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Fornavn",
                table: "Influenter",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Profiltekst",
                table: "Influenter",
                nullable: true);
        }
    }
}
