using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EleaguesApp.API.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Leagues",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    Difficulty = table.Column<string>(nullable: true),
                    Format = table.Column<string>(nullable: true),
                    Rounds = table.Column<int>(nullable: false),
                    TeamFormat = table.Column<string>(nullable: true),
                    MatchTiming = table.Column<string>(nullable: true),
                    Powerplay = table.Column<string>(nullable: true),
                    NormalizeSkills = table.Column<string>(nullable: true),
                    RopeSettings = table.Column<string>(nullable: true),
                    PitchSettings = table.Column<string>(nullable: true),
                    WinPoints = table.Column<int>(nullable: false),
                    LostPoints = table.Column<int>(nullable: false),
                    ParticipationPoints = table.Column<int>(nullable: false),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    IsClosed = table.Column<bool>(nullable: false),
                    Winner = table.Column<string>(nullable: true),
                    IsGroupLeague = table.Column<bool>(nullable: false),
                    IsDefaultLeague = table.Column<bool>(nullable: false),
                    RulesFileName = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Leagues", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Username = table.Column<string>(nullable: true),
                    PasswordHash = table.Column<byte[]>(nullable: true),
                    PasswordSalt = table.Column<byte[]>(nullable: true),
                    KnownAs = table.Column<string>(nullable: true),
                    PSN = table.Column<string>(nullable: true),
                    DateOfBirth = table.Column<DateTime>(nullable: false),
                    Country = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    Mobile = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    IsAdmin = table.Column<bool>(nullable: false),
                    PhotoUrl = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LeagueId = table.Column<int>(nullable: false),
                    WinnerId = table.Column<int>(nullable: false),
                    OpponentId = table.Column<int>(nullable: false),
                    Stage = table.Column<string>(nullable: true),
                    Round = table.Column<int>(nullable: false),
                    Runs = table.Column<int>(nullable: false),
                    Overs = table.Column<decimal>(nullable: false),
                    Wickets = table.Column<int>(nullable: false),
                    Fifties = table.Column<int>(nullable: false),
                    Hundreds = table.Column<int>(nullable: false),
                    FiveWickets = table.Column<int>(nullable: false),
                    InningsHigh = table.Column<int>(nullable: false),
                    OpponentRuns = table.Column<int>(nullable: false),
                    OpponentOvers = table.Column<decimal>(nullable: false),
                    OpponentWickets = table.Column<int>(nullable: false),
                    OpponentFifties = table.Column<int>(nullable: false),
                    OpponentHundreds = table.Column<int>(nullable: false),
                    OpponentFiveWickets = table.Column<int>(nullable: false),
                    OpponentInningsHigh = table.Column<int>(nullable: false),
                    Result = table.Column<string>(nullable: true),
                    IsApproved = table.Column<bool>(nullable: false),
                    FileName = table.Column<string>(nullable: true),
                    WinnerGroup = table.Column<string>(nullable: true),
                    OpponentGroup = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedOn = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Games_Leagues_LeagueId",
                        column: x => x.LeagueId,
                        principalTable: "Leagues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Games_Users_OpponentId",
                        column: x => x.OpponentId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Games_Users_WinnerId",
                        column: x => x.WinnerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Games_LeagueId",
                table: "Games",
                column: "LeagueId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_OpponentId",
                table: "Games",
                column: "OpponentId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_WinnerId",
                table: "Games",
                column: "WinnerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "Leagues");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
