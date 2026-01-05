using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kukirmash.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddServerIcon : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IconPath",
                table: "Servers",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IconPath",
                table: "Servers");
        }
    }
}
