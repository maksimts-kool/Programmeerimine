using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tund11.Auth.Data.Migrations
{
    /// <inheritdoc />
    public partial class ReplacePealkiriKirjeldusWithNimi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Kirjeldus",
                table: "Ankeetid");

            migrationBuilder.RenameColumn(
                name: "Pealkiri",
                table: "Ankeetid",
                newName: "Nimi");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Nimi",
                table: "Ankeetid",
                newName: "Pealkiri");

            migrationBuilder.AddColumn<string>(
                name: "Kirjeldus",
                table: "Ankeetid",
                type: "TEXT",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");
        }
    }
}
