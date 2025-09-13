using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AzureAPI.Migrations
{
    /// <inheritdoc />
    public partial class catchup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte>(
                name: "CardType",
                table: "Fights",
                type: "tinyint",
                nullable: false,
                defaultValue: (byte)0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CardType",
                table: "Fights");
        }
    }
}
