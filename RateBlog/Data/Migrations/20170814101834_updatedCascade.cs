﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Bestfluence.Migrations
{
    public partial class updatedCascade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feedback_Influencer_InfluenterId",
                table: "Feedback");

            migrationBuilder.AddForeignKey(
                name: "FK_Feedback_Influencer_InfluenterId",
                table: "Feedback",
                column: "InfluenterId",
                principalTable: "Influencer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feedback_Influencer_InfluenterId",
                table: "Feedback");

            migrationBuilder.AddForeignKey(
                name: "FK_Feedback_Influencer_InfluenterId",
                table: "Feedback",
                column: "InfluenterId",
                principalTable: "Influencer",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
