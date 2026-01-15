using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tund11.Auth.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveOnKutseFromKylaline : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OnKutse",
                table: "Kylalised");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "OnKutse",
                table: "Kylalised",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }
    }
}
