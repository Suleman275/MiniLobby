using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniLobby.Migrations
{
    /// <inheritdoc />
    public partial class addedhostmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsHostMigrationEnabled",
                table: "Lobbies",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsHostMigrationEnabled",
                table: "Lobbies");
        }
    }
}
