using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tund11.Auth.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPyhaKylaline : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pyhad",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nimetus = table.Column<string>(type: "TEXT", nullable: false),
                    Kuupaev = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pyhad", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Kylalised",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Nimi = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    OnKutse = table.Column<bool>(type: "INTEGER", nullable: false),
                    PyhaId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kylalised", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Kylalised_Pyhad_PyhaId",
                        column: x => x.PyhaId,
                        principalTable: "Pyhad",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Kylalised_PyhaId",
                table: "Kylalised",
                column: "PyhaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Kylalised");

            migrationBuilder.DropTable(
                name: "Pyhad");
        }
    }
}
