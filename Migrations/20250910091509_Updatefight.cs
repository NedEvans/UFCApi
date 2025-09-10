using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AzureAPI.Migrations
{
    /// <inheritdoc />
    public partial class Updatefight : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fights_Events_EventId",
                table: "Fights");

            migrationBuilder.DropIndex(
                name: "IX_Fights_EventId",
                table: "Fights");

            migrationBuilder.AddColumn<byte[]>(
                name: "BlueBodyStrikes",
                table: "Fights",
                type: "BINARY(5)",
                fixedLength: true,
                maxLength: 5,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "BlueClinchStrikes",
                table: "Fights",
                type: "BINARY(5)",
                fixedLength: true,
                maxLength: 5,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "BlueDistanceStrikes",
                table: "Fights",
                type: "BINARY(5)",
                fixedLength: true,
                maxLength: 5,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "BlueGroundStrikes",
                table: "Fights",
                type: "BINARY(5)",
                fixedLength: true,
                maxLength: 5,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "BlueHeadStrikes",
                table: "Fights",
                type: "BINARY(5)",
                fixedLength: true,
                maxLength: 5,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "BlueKnockdowns",
                table: "Fights",
                type: "BINARY(5)",
                fixedLength: true,
                maxLength: 5,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "BlueLegStrikes",
                table: "Fights",
                type: "BINARY(5)",
                fixedLength: true,
                maxLength: 5,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<short>(
                name: "BlueOdds",
                table: "Fights",
                type: "SMALLINT",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<byte[]>(
                name: "BlueSubAttempts",
                table: "Fights",
                type: "BINARY(5)",
                fixedLength: true,
                maxLength: 5,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "BlueTakedowns",
                table: "Fights",
                type: "BINARY(5)",
                fixedLength: true,
                maxLength: 5,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<short>(
                name: "BlueTotalKnockdowns",
                table: "Fights",
                type: "SMALLINT",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "BlueTotalStrikes",
                table: "Fights",
                type: "SMALLINT",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "BlueTotalSubAttempts",
                table: "Fights",
                type: "SMALLINT",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "BlueTotalTakedowns",
                table: "Fights",
                type: "SMALLINT",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<bool>(
                name: "IsTitleFight",
                table: "Fights",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<byte[]>(
                name: "RedBodyStrikes",
                table: "Fights",
                type: "BINARY(5)",
                fixedLength: true,
                maxLength: 5,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "RedClinchStrikes",
                table: "Fights",
                type: "BINARY(5)",
                fixedLength: true,
                maxLength: 5,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "RedDistanceStrikes",
                table: "Fights",
                type: "BINARY(5)",
                fixedLength: true,
                maxLength: 5,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "RedGroundStrikes",
                table: "Fights",
                type: "BINARY(5)",
                fixedLength: true,
                maxLength: 5,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "RedHeadStrikes",
                table: "Fights",
                type: "BINARY(5)",
                fixedLength: true,
                maxLength: 5,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "RedKnockdowns",
                table: "Fights",
                type: "BINARY(5)",
                fixedLength: true,
                maxLength: 5,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "RedLegStrikes",
                table: "Fights",
                type: "BINARY(5)",
                fixedLength: true,
                maxLength: 5,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<short>(
                name: "RedOdds",
                table: "Fights",
                type: "SMALLINT",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<byte[]>(
                name: "RedSubAttempts",
                table: "Fights",
                type: "BINARY(5)",
                fixedLength: true,
                maxLength: 5,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "RedTakedowns",
                table: "Fights",
                type: "BINARY(5)",
                fixedLength: true,
                maxLength: 5,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<short>(
                name: "RedTotalKnockdowns",
                table: "Fights",
                type: "SMALLINT",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "RedTotalStrikes",
                table: "Fights",
                type: "SMALLINT",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "RedTotalSubAttempts",
                table: "Fights",
                type: "SMALLINT",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "RedTotalTakedowns",
                table: "Fights",
                type: "SMALLINT",
                nullable: false,
                defaultValue: (short)0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BlueBodyStrikes",
                table: "Fights");

            migrationBuilder.DropColumn(
                name: "BlueClinchStrikes",
                table: "Fights");

            migrationBuilder.DropColumn(
                name: "BlueDistanceStrikes",
                table: "Fights");

            migrationBuilder.DropColumn(
                name: "BlueGroundStrikes",
                table: "Fights");

            migrationBuilder.DropColumn(
                name: "BlueHeadStrikes",
                table: "Fights");

            migrationBuilder.DropColumn(
                name: "BlueKnockdowns",
                table: "Fights");

            migrationBuilder.DropColumn(
                name: "BlueLegStrikes",
                table: "Fights");

            migrationBuilder.DropColumn(
                name: "BlueOdds",
                table: "Fights");

            migrationBuilder.DropColumn(
                name: "BlueSubAttempts",
                table: "Fights");

            migrationBuilder.DropColumn(
                name: "BlueTakedowns",
                table: "Fights");

            migrationBuilder.DropColumn(
                name: "BlueTotalKnockdowns",
                table: "Fights");

            migrationBuilder.DropColumn(
                name: "BlueTotalStrikes",
                table: "Fights");

            migrationBuilder.DropColumn(
                name: "BlueTotalSubAttempts",
                table: "Fights");

            migrationBuilder.DropColumn(
                name: "BlueTotalTakedowns",
                table: "Fights");

            migrationBuilder.DropColumn(
                name: "IsTitleFight",
                table: "Fights");

            migrationBuilder.DropColumn(
                name: "RedBodyStrikes",
                table: "Fights");

            migrationBuilder.DropColumn(
                name: "RedClinchStrikes",
                table: "Fights");

            migrationBuilder.DropColumn(
                name: "RedDistanceStrikes",
                table: "Fights");

            migrationBuilder.DropColumn(
                name: "RedGroundStrikes",
                table: "Fights");

            migrationBuilder.DropColumn(
                name: "RedHeadStrikes",
                table: "Fights");

            migrationBuilder.DropColumn(
                name: "RedKnockdowns",
                table: "Fights");

            migrationBuilder.DropColumn(
                name: "RedLegStrikes",
                table: "Fights");

            migrationBuilder.DropColumn(
                name: "RedOdds",
                table: "Fights");

            migrationBuilder.DropColumn(
                name: "RedSubAttempts",
                table: "Fights");

            migrationBuilder.DropColumn(
                name: "RedTakedowns",
                table: "Fights");

            migrationBuilder.DropColumn(
                name: "RedTotalKnockdowns",
                table: "Fights");

            migrationBuilder.DropColumn(
                name: "RedTotalStrikes",
                table: "Fights");

            migrationBuilder.DropColumn(
                name: "RedTotalSubAttempts",
                table: "Fights");

            migrationBuilder.DropColumn(
                name: "RedTotalTakedowns",
                table: "Fights");

            migrationBuilder.CreateIndex(
                name: "IX_Fights_EventId",
                table: "Fights",
                column: "EventId");

            migrationBuilder.AddForeignKey(
                name: "FK_Fights_Events_EventId",
                table: "Fights",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
