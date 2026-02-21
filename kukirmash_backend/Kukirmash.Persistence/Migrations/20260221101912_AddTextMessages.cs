using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kukirmash.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddTextMessages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TextMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Text = table.Column<string>(type: "character varying(1023)", maxLength: 1023, nullable: false),
                    CreatedDateTimeUtc = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TextChannelId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TextMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TextMessages_TextChannels_TextChannelId",
                        column: x => x.TextChannelId,
                        principalTable: "TextChannels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TextMessages_Users_CreatorId",
                        column: x => x.CreatorId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TextMessages_CreatorId",
                table: "TextMessages",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_TextMessages_TextChannelId_CreatedDateTimeUtc",
                table: "TextMessages",
                columns: new[] { "TextChannelId", "CreatedDateTimeUtc" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TextMessages");
        }
    }
}
