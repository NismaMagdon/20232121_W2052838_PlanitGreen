using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _20232121_W2052838_PlanitGreen.Migrations
{
    /// <inheritdoc />
    public partial class BadgeEntityUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BadgeCategory",
                table: "Badge",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "BonusEcoPoints",
                table: "Badge",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BadgeCategory",
                table: "Badge");

            migrationBuilder.DropColumn(
                name: "BonusEcoPoints",
                table: "Badge");
        }
    }
}
