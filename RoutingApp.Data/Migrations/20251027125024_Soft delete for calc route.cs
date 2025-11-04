using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoutingApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class Softdeleteforcalcroute : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeletedAt",
                table: "CalculatedRoutes",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "CalculatedRoutes",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "CalculatedRoutes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "CalculatedRoutes");
        }
    }
}
