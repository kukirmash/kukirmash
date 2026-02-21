using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kukirmash.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddTextChannels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TextChannelEntity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(31)", maxLength: 31, nullable: false),
                    ServerId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TextChannelEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TextChannelEntity_Servers_ServerId",
                        column: x => x.ServerId,
                        principalTable: "Servers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TextChannelEntity_ServerId",
                table: "TextChannelEntity",
                column: "ServerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TextChannelEntity");
        }
    }
}
