using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Bestfluence.Migrations
{
    public partial class influenterreference : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_InstagramData_InfluencerId",
                table: "InstagramData");

            migrationBuilder.CreateIndex(
                name: "IX_InstagramData_InfluencerId",
                table: "InstagramData",
                column: "InfluencerId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_InstagramData_InfluencerId",
                table: "InstagramData");

            migrationBuilder.CreateIndex(
                name: "IX_InstagramData_InfluencerId",
                table: "InstagramData",
                column: "InfluencerId");
        }
    }
}
