using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AzureAPI.Migrations
{
    /// <inheritdoc />
    public partial class RemoveFighterFromFightClass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fights_Fighters_BlueCornerId",
                table: "Fights");

            migrationBuilder.DropForeignKey(
                name: "FK_Fights_Fighters_RedCornerId",
                table: "Fights");

            migrationBuilder.DropIndex(
                name: "IX_Fights_BlueCornerId",
                table: "Fights");

            migrationBuilder.DropIndex(
                name: "IX_Fights_RedCornerId",
                table: "Fights");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Fights_BlueCornerId",
                table: "Fights",
                column: "BlueCornerId");

            migrationBuilder.CreateIndex(
                name: "IX_Fights_RedCornerId",
                table: "Fights",
                column: "RedCornerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Fights_Fighters_BlueCornerId",
                table: "Fights",
                column: "BlueCornerId",
                principalTable: "Fighters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Fights_Fighters_RedCornerId",
                table: "Fights",
                column: "RedCornerId",
                principalTable: "Fighters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
