using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace _20232121_W2052838_PlanitGreen.Migrations
{
    /// <inheritdoc />
    public partial class AddTreesPlantedToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           
            migrationBuilder.AddColumn<int>(
                name: "TreesPlanted",
                table: "User",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TreesPlanted",
                table: "User");


        }
    }
}
