using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RateBlog.Data.Migrations
{
    public partial class influenterplatform : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Influenter_Platform_PlatformForeignKey",
                table: "Influenter");

            migrationBuilder.DropForeignKey(
                name: "FK_Platform_Influenter_InfluenterId",
                table: "Platform");

            migrationBuilder.DropIndex(
                name: "IX_Platform_InfluenterId",
                table: "Platform");

            migrationBuilder.DropIndex(
                name: "IX_Influenter_PlatformForeignKey",
                table: "Influenter");

            migrationBuilder.DropColumn(
                name: "InfluenterId",
                table: "Platform");

            migrationBuilder.DropColumn(
                name: "PlatformForeignKey",
                table: "Influenter");

            migrationBuilder.DropColumn(
                name: "PlatformId",
                table: "Influenter");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Platform",
                newName: "PlatformId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Influenter",
                newName: "InfluenterId");

            migrationBuilder.CreateTable(
                name: "InfluenterPlatform",
                columns: table => new
                {
                    InfluenterId = table.Column<int>(nullable: false),
                    PlatformId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InfluenterPlatform", x => new { x.InfluenterId, x.PlatformId });
                    table.ForeignKey(
                        name: "FK_InfluenterPlatform_Influenter_InfluenterId",
                        column: x => x.InfluenterId,
                        principalTable: "Influenter",
                        principalColumn: "InfluenterId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InfluenterPlatform_Platform_PlatformId",
                        column: x => x.PlatformId,
                        principalTable: "Platform",
                        principalColumn: "PlatformId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InfluenterPlatform_PlatformId",
                table: "InfluenterPlatform",
                column: "PlatformId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InfluenterPlatform");

            migrationBuilder.RenameColumn(
                name: "PlatformId",
                table: "Platform",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "InfluenterId",
                table: "Influenter",
                newName: "Id");

            migrationBuilder.AddColumn<int>(
                name: "InfluenterId",
                table: "Platform",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PlatformForeignKey",
                table: "Influenter",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PlatformId",
                table: "Influenter",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Platform_InfluenterId",
                table: "Platform",
                column: "InfluenterId");

            migrationBuilder.CreateIndex(
                name: "IX_Influenter_PlatformForeignKey",
                table: "Influenter",
                column: "PlatformForeignKey");

            migrationBuilder.AddForeignKey(
                name: "FK_Influenter_Platform_PlatformForeignKey",
                table: "Influenter",
                column: "PlatformForeignKey",
                principalTable: "Platform",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Platform_Influenter_InfluenterId",
                table: "Platform",
                column: "InfluenterId",
                principalTable: "Influenter",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
