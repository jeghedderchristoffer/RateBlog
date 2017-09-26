using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Bestfluence.Migrations
{
    public partial class YoutubeDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Country",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Country", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "YoutubeData",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Comments = table.Column<int>(nullable: false),
                    Dislike = table.Column<int>(nullable: false),
                    Engagement = table.Column<int>(nullable: false),
                    FemaleViews = table.Column<int>(nullable: false),
                    InfluencerId = table.Column<string>(nullable: true),
                    Likes = table.Column<int>(nullable: false),
                    MaleViews = table.Column<int>(nullable: false),
                    Subcribers = table.Column<int>(nullable: false),
                    Views = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YoutubeData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_YoutubeData_Influencer_InfluencerId",
                        column: x => x.InfluencerId,
                        principalTable: "Influencer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "YoutubeAgeGroup",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    EighteenTothirtyfour = table.Column<int>(nullable: false),
                    FiftyfiveToSixtyfive = table.Column<int>(nullable: false),
                    FortyFiveTofiftyfive = table.Column<int>(nullable: false),
                    SixtyfivePlus = table.Column<int>(nullable: false),
                    ThirteenToSeventeen = table.Column<int>(nullable: false),
                    ThirtyfiveTofortyfour = table.Column<int>(nullable: false),
                    YoutubeDataId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YoutubeAgeGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_YoutubeAgeGroup_YoutubeData_YoutubeDataId",
                        column: x => x.YoutubeDataId,
                        principalTable: "YoutubeData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "YoutubeCountry",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Count = table.Column<int>(nullable: false),
                    CountryId = table.Column<string>(nullable: true),
                    YoutubeDataId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YoutubeCountry", x => x.Id);
                    table.ForeignKey(
                        name: "FK_YoutubeCountry_Country_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Country",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_YoutubeCountry_YoutubeData_YoutubeDataId",
                        column: x => x.YoutubeDataId,
                        principalTable: "YoutubeData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_YoutubeAgeGroup_YoutubeDataId",
                table: "YoutubeAgeGroup",
                column: "YoutubeDataId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_YoutubeCountry_CountryId",
                table: "YoutubeCountry",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_YoutubeCountry_YoutubeDataId",
                table: "YoutubeCountry",
                column: "YoutubeDataId");

            migrationBuilder.CreateIndex(
                name: "IX_YoutubeData_InfluencerId",
                table: "YoutubeData",
                column: "InfluencerId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "YoutubeAgeGroup");

            migrationBuilder.DropTable(
                name: "YoutubeCountry");

            migrationBuilder.DropTable(
                name: "Country");

            migrationBuilder.DropTable(
                name: "YoutubeData");
        }
    }
}
