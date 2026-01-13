using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tund11.Auth.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAnkeet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ankeetid",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Pealkiri = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Kirjeldus = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    LoomiseKupaev = table.Column<DateTime>(type: "TEXT", nullable: false),
                    OnAktiivne = table.Column<bool>(type: "INTEGER", nullable: false),
                    VastuseteArv = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ankeetid", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ankeetid");
        }
    }
}
