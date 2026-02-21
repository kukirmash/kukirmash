using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kukirmash.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class TextChannelTableName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TextChannelEntity_Servers_ServerId",
                table: "TextChannelEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TextChannelEntity",
                table: "TextChannelEntity");

            migrationBuilder.RenameTable(
                name: "TextChannelEntity",
                newName: "TextChannels");

            migrationBuilder.RenameIndex(
                name: "IX_TextChannelEntity_ServerId",
                table: "TextChannels",
                newName: "IX_TextChannels_ServerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TextChannels",
                table: "TextChannels",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TextChannels_Servers_ServerId",
                table: "TextChannels",
                column: "ServerId",
                principalTable: "Servers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TextChannels_Servers_ServerId",
                table: "TextChannels");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TextChannels",
                table: "TextChannels");

            migrationBuilder.RenameTable(
                name: "TextChannels",
                newName: "TextChannelEntity");

            migrationBuilder.RenameIndex(
                name: "IX_TextChannels_ServerId",
                table: "TextChannelEntity",
                newName: "IX_TextChannelEntity_ServerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TextChannelEntity",
                table: "TextChannelEntity",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TextChannelEntity_Servers_ServerId",
                table: "TextChannelEntity",
                column: "ServerId",
                principalTable: "Servers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
