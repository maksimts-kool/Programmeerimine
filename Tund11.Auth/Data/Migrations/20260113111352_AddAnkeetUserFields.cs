using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tund11.Auth.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAnkeetUserFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Epost",
                table: "Ankeetid",
                type: "TEXT",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "KesOsaleb",
                table: "Ankeetid",
                type: "TEXT",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "PyhaId",
                table: "Ankeetid",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Ankeetid_PyhaId",
                table: "Ankeetid",
                column: "PyhaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ankeetid_Pyhad_PyhaId",
                table: "Ankeetid",
                column: "PyhaId",
                principalTable: "Pyhad",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ankeetid_Pyhad_PyhaId",
                table: "Ankeetid");

            migrationBuilder.DropIndex(
                name: "IX_Ankeetid_PyhaId",
                table: "Ankeetid");

            migrationBuilder.DropColumn(
                name: "Epost",
                table: "Ankeetid");

            migrationBuilder.DropColumn(
                name: "KesOsaleb",
                table: "Ankeetid");

            migrationBuilder.DropColumn(
                name: "PyhaId",
                table: "Ankeetid");
        }
    }
}
