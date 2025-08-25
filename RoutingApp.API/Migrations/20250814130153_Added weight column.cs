using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RoutingApp.API.Migrations
{
    /// <inheritdoc />
    public partial class Addedweightcolumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "Points");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Points",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: false,
                defaultValue: "");
        }
    }
}
