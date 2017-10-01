using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Bestfluence.Migrations
{
    public partial class instastuff : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "City",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_City", x => x.Id);
                });

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
                name: "InstagramData",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    DayImpression = table.Column<int>(nullable: false),
                    DayReach = table.Column<int>(nullable: false),
                    FollowerCount = table.Column<int>(nullable: false),
                    InfluencerId = table.Column<string>(nullable: true),
                    MediaCount = table.Column<int>(nullable: false),
                    MonthImpression = table.Column<int>(nullable: false),
                    MonthReach = table.Column<int>(nullable: false),
                    WeekImpression = table.Column<int>(nullable: false),
                    WeekReach = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstagramData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InstagramData_Influencer_InfluencerId",
                        column: x => x.InfluencerId,
                        principalTable: "Influencer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InstagramAgeGroup",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Female13To17 = table.Column<int>(nullable: false),
                    Female18To24 = table.Column<int>(nullable: false),
                    Female25To34 = table.Column<int>(nullable: false),
                    Female35To44 = table.Column<int>(nullable: false),
                    Female45To55 = table.Column<int>(nullable: false),
                    Female55To64 = table.Column<int>(nullable: false),
                    Female65Plus = table.Column<int>(nullable: false),
                    InstagramDataId = table.Column<string>(nullable: true),
                    Male13To17 = table.Column<int>(nullable: false),
                    Male18To24 = table.Column<int>(nullable: false),
                    Male25To34 = table.Column<int>(nullable: false),
                    Male35To44 = table.Column<int>(nullable: false),
                    Male45To55 = table.Column<int>(nullable: false),
                    Male55To64 = table.Column<int>(nullable: false),
                    Male65Plus = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstagramAgeGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InstagramAgeGroup_InstagramData_InstagramDataId",
                        column: x => x.InstagramDataId,
                        principalTable: "InstagramData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InstagramCity",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    CityId = table.Column<string>(nullable: true),
                    Count = table.Column<int>(nullable: false),
                    InstagramDataId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstagramCity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InstagramCity_City_CityId",
                        column: x => x.CityId,
                        principalTable: "City",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InstagramCity_InstagramData_InstagramDataId",
                        column: x => x.InstagramDataId,
                        principalTable: "InstagramData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InstagramCountry",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Count = table.Column<int>(nullable: false),
                    CountryId = table.Column<string>(nullable: true),
                    InstagramDataId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstagramCountry", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InstagramCountry_Country_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Country",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InstagramCountry_InstagramData_InstagramDataId",
                        column: x => x.InstagramDataId,
                        principalTable: "InstagramData",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InstagramAgeGroup_InstagramDataId",
                table: "InstagramAgeGroup",
                column: "InstagramDataId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InstagramCity_CityId",
                table: "InstagramCity",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_InstagramCity_InstagramDataId",
                table: "InstagramCity",
                column: "InstagramDataId");

            migrationBuilder.CreateIndex(
                name: "IX_InstagramCountry_CountryId",
                table: "InstagramCountry",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_InstagramCountry_InstagramDataId",
                table: "InstagramCountry",
                column: "InstagramDataId");

            migrationBuilder.CreateIndex(
                name: "IX_InstagramData_InfluencerId",
                table: "InstagramData",
                column: "InfluencerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InstagramAgeGroup");

            migrationBuilder.DropTable(
                name: "InstagramCity");

            migrationBuilder.DropTable(
                name: "InstagramCountry");

            migrationBuilder.DropTable(
                name: "City");

            migrationBuilder.DropTable(
                name: "Country");

            migrationBuilder.DropTable(
                name: "InstagramData");
        }
    }
}
