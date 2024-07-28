using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniLobby.Migrations
{
    /// <inheritdoc />
    public partial class updatememberschema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_LobbyMembers",
                table: "LobbyMembers");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "LobbyMembers",
                newName: "MemberId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LobbyMembers",
                table: "LobbyMembers",
                columns: new[] { "CurrentLobbyId", "MemberId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_LobbyMembers",
                table: "LobbyMembers");

            migrationBuilder.RenameColumn(
                name: "MemberId",
                table: "LobbyMembers",
                newName: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LobbyMembers",
                table: "LobbyMembers",
                column: "Id");
        }
    }
}
