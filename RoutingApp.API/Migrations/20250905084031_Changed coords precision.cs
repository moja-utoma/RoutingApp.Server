using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoutingApp.API.Migrations
{
    /// <inheritdoc />
    public partial class Changedcoordsprecision : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Longitude",
                table: "Warehouses",
                type: "decimal(17,14)",
                precision: 17,
                scale: 14,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(9,6)",
                oldPrecision: 9,
                oldScale: 6);

            migrationBuilder.AlterColumn<decimal>(
                name: "Latitude",
                table: "Warehouses",
                type: "decimal(17,14)",
                precision: 17,
                scale: 14,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(8,6)",
                oldPrecision: 8,
                oldScale: 6);

            migrationBuilder.AlterColumn<decimal>(
                name: "Longitude",
                table: "DeliveryPoints",
                type: "decimal(17,14)",
                precision: 17,
                scale: 14,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(9,6)",
                oldPrecision: 9,
                oldScale: 6);

            migrationBuilder.AlterColumn<decimal>(
                name: "Latitude",
                table: "DeliveryPoints",
                type: "decimal(17,14)",
                precision: 17,
                scale: 14,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(8,6)",
                oldPrecision: 8,
                oldScale: 6);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Longitude",
                table: "Warehouses",
                type: "decimal(9,6)",
                precision: 9,
                scale: 6,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(17,14)",
                oldPrecision: 17,
                oldScale: 14);

            migrationBuilder.AlterColumn<decimal>(
                name: "Latitude",
                table: "Warehouses",
                type: "decimal(8,6)",
                precision: 8,
                scale: 6,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(17,14)",
                oldPrecision: 17,
                oldScale: 14);

            migrationBuilder.AlterColumn<decimal>(
                name: "Longitude",
                table: "DeliveryPoints",
                type: "decimal(9,6)",
                precision: 9,
                scale: 6,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(17,14)",
                oldPrecision: 17,
                oldScale: 14);

            migrationBuilder.AlterColumn<decimal>(
                name: "Latitude",
                table: "DeliveryPoints",
                type: "decimal(8,6)",
                precision: 8,
                scale: 6,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(17,14)",
                oldPrecision: 17,
                oldScale: 14);
        }
    }
}
