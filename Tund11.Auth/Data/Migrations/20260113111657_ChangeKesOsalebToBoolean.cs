using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tund11.Auth.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeKesOsalebToBoolean : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KesOsaleb",
                table: "Ankeetid");

            migrationBuilder.AddColumn<bool>(
                name: "OnOsaleb",
                table: "Ankeetid",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OnOsaleb",
                table: "Ankeetid");

            migrationBuilder.AddColumn<string>(
                name: "KesOsaleb",
                table: "Ankeetid",
                type: "TEXT",
                maxLength: 500,
                nullable: false,
                defaultValue: "");
        }
    }
}
