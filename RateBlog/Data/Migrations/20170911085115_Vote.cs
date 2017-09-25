using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Bestfluence.Migrations
{
    public partial class Vote : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Votes",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    InfluencerId = table.Column<string>(nullable: false),
                    Title = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Votes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Votes_Influencer_InfluencerId",
                        column: x => x.InfluencerId,
                        principalTable: "Influencer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VoteQuestions",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Question = table.Column<string>(nullable: false),
                    VoteId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoteQuestions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VoteQuestions_Votes_VoteId",
                        column: x => x.VoteId,
                        principalTable: "Votes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "VoteAnswers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    ApplicationUserId = table.Column<string>(nullable: false),
                    VoteQuestionId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoteAnswers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VoteAnswers_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VoteAnswers_VoteQuestions_VoteQuestionId",
                        column: x => x.VoteQuestionId,
                        principalTable: "VoteQuestions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Votes_InfluencerId",
                table: "Votes",
                column: "InfluencerId");

            migrationBuilder.CreateIndex(
                name: "IX_VoteAnswers_ApplicationUserId",
                table: "VoteAnswers",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_VoteAnswers_VoteQuestionId",
                table: "VoteAnswers",
                column: "VoteQuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_VoteQuestions_VoteId",
                table: "VoteQuestions",
                column: "VoteId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VoteAnswers");

            migrationBuilder.DropTable(
                name: "VoteQuestions");

            migrationBuilder.DropTable(
                name: "Votes");
        }
    }
}
