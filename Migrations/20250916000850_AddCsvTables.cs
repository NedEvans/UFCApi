using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AzureAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddCsvTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EventsCsv",
                columns: table => new
                {
                    EventId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EventName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    EventDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EventCity = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EventState = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EventCountry = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventsCsv", x => x.EventId);
                });

            migrationBuilder.CreateTable(
                name: "FightersCsv",
                columns: table => new
                {
                    FighterId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FighterFName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FighterLName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FighterNickname = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FighterHeightCm = table.Column<double>(type: "float", nullable: false),
                    FighterWeightLbs = table.Column<double>(type: "float", nullable: false),
                    FighterReachCm = table.Column<double>(type: "float", nullable: false),
                    FighterStance = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FighterDob = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FighterW = table.Column<int>(type: "int", nullable: false),
                    FighterL = table.Column<int>(type: "int", nullable: false),
                    FighterD = table.Column<int>(type: "int", nullable: false),
                    FighterNcDq = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FightersCsv", x => x.FighterId);
                });

            migrationBuilder.CreateTable(
                name: "FightsCsv",
                columns: table => new
                {
                    FightId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EventId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Referee = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Fighter1Id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Fighter2Id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    WinnerId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NumRounds = table.Column<int>(type: "int", nullable: false),
                    TitleFight = table.Column<bool>(type: "bit", nullable: true),
                    WeightClass = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Result = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ResultDetails = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FinishRound = table.Column<int>(type: "int", nullable: false),
                    FinishTime = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    TimeFormat = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Scores1 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Scores2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FightsCsv", x => x.FightId);
                    table.ForeignKey(
                        name: "FK_FightsCsv_EventsCsv_EventId",
                        column: x => x.EventId,
                        principalTable: "EventsCsv",
                        principalColumn: "EventId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FightsCsv_FightersCsv_Fighter1Id",
                        column: x => x.Fighter1Id,
                        principalTable: "FightersCsv",
                        principalColumn: "FighterId");
                    table.ForeignKey(
                        name: "FK_FightsCsv_FightersCsv_Fighter2Id",
                        column: x => x.Fighter2Id,
                        principalTable: "FightersCsv",
                        principalColumn: "FighterId");
                    table.ForeignKey(
                        name: "FK_FightsCsv_FightersCsv_WinnerId",
                        column: x => x.WinnerId,
                        principalTable: "FightersCsv",
                        principalColumn: "FighterId");
                });

            migrationBuilder.CreateTable(
                name: "RoundsCsv",
                columns: table => new
                {
                    FightId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FighterId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Round = table.Column<int>(type: "int", nullable: false),
                    Knockdowns = table.Column<int>(type: "int", nullable: false),
                    StrikesAtt = table.Column<int>(type: "int", nullable: false),
                    StrikesSucc = table.Column<int>(type: "int", nullable: false),
                    HeadStrikesAtt = table.Column<int>(type: "int", nullable: false),
                    HeadStrikesSucc = table.Column<int>(type: "int", nullable: false),
                    BodyStrikesAtt = table.Column<int>(type: "int", nullable: false),
                    BodyStrikesSucc = table.Column<int>(type: "int", nullable: false),
                    LegStrikesAtt = table.Column<int>(type: "int", nullable: false),
                    LegStrikesSucc = table.Column<int>(type: "int", nullable: false),
                    DistanceStrikesAtt = table.Column<int>(type: "int", nullable: false),
                    DistanceStrikesSucc = table.Column<int>(type: "int", nullable: false),
                    GroundStrikesAtt = table.Column<int>(type: "int", nullable: false),
                    GroundStrikesSucc = table.Column<int>(type: "int", nullable: false),
                    ClinchStrikesAtt = table.Column<int>(type: "int", nullable: false),
                    ClinchStrikesSucc = table.Column<int>(type: "int", nullable: false),
                    TotalStrikesAtt = table.Column<int>(type: "int", nullable: false),
                    TotalStrikesSucc = table.Column<int>(type: "int", nullable: false),
                    TakedownAtt = table.Column<int>(type: "int", nullable: false),
                    TakedownSucc = table.Column<int>(type: "int", nullable: false),
                    SubmissionAtt = table.Column<int>(type: "int", nullable: false),
                    Reversals = table.Column<int>(type: "int", nullable: false),
                    CtrlTime = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoundsCsv", x => new { x.FightId, x.FighterId, x.Round });
                    table.ForeignKey(
                        name: "FK_RoundsCsv_FightersCsv_FighterId",
                        column: x => x.FighterId,
                        principalTable: "FightersCsv",
                        principalColumn: "FighterId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoundsCsv_FightsCsv_FightId",
                        column: x => x.FightId,
                        principalTable: "FightsCsv",
                        principalColumn: "FightId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EventsCsv_EventDate",
                table: "EventsCsv",
                column: "EventDate");

            migrationBuilder.CreateIndex(
                name: "IX_FightersCsv_FighterFName",
                table: "FightersCsv",
                column: "FighterFName");

            migrationBuilder.CreateIndex(
                name: "IX_FightersCsv_FighterLName",
                table: "FightersCsv",
                column: "FighterLName");

            migrationBuilder.CreateIndex(
                name: "IX_FightsCsv_EventId",
                table: "FightsCsv",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_FightsCsv_Fighter1Id",
                table: "FightsCsv",
                column: "Fighter1Id");

            migrationBuilder.CreateIndex(
                name: "IX_FightsCsv_Fighter2Id",
                table: "FightsCsv",
                column: "Fighter2Id");

            migrationBuilder.CreateIndex(
                name: "IX_FightsCsv_WinnerId",
                table: "FightsCsv",
                column: "WinnerId");

            migrationBuilder.CreateIndex(
                name: "IX_RoundsCsv_FighterId",
                table: "RoundsCsv",
                column: "FighterId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoundsCsv");

            migrationBuilder.DropTable(
                name: "FightsCsv");

            migrationBuilder.DropTable(
                name: "EventsCsv");

            migrationBuilder.DropTable(
                name: "FightersCsv");
        }
    }
}
