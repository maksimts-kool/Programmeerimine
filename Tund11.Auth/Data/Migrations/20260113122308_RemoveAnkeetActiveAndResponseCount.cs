using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Tund11.Auth.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveAnkeetActiveAndResponseCount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OnAktiivne",
                table: "Ankeetid");

            migrationBuilder.DropColumn(
                name: "VastuseteArv",
                table: "Ankeetid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "OnAktiivne",
                table: "Ankeetid",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "VastuseteArv",
                table: "Ankeetid",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
