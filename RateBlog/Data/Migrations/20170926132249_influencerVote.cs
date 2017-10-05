using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Bestfluence.Migrations
{
    public partial class influencerVote : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Votes_InfluencerId",
                table: "Votes");

            migrationBuilder.CreateIndex(
                name: "IX_Votes_InfluencerId",
                table: "Votes",
                column: "InfluencerId",
                unique: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Votes_InfluencerId",
                table: "Votes");

            migrationBuilder.CreateIndex(
                name: "IX_Votes_InfluencerId",
                table: "Votes",
                column: "InfluencerId");
        }
    }
}
