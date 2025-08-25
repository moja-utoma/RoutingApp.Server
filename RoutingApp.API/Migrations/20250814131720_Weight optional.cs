using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoutingApp.API.Migrations
{
    /// <inheritdoc />
    public partial class Weightoptional : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Weight",
                table: "Points",
                type: "decimal(6,1)",
                precision: 6,
                scale: 1,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(6,1)",
                oldPrecision: 6,
                oldScale: 1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Weight",
                table: "Points",
                type: "decimal(6,1)",
                precision: 6,
                scale: 1,
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(6,1)",
                oldPrecision: 6,
                oldScale: 1,
                oldNullable: true);
        }
    }
}
