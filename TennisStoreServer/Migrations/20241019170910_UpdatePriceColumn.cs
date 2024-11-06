using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TennisStoreServer.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePriceColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Products",
                type: "decimal(18, 2)", // Fixed the closing parenthesis
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)"); // Fixed the space for consistency
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Products",
                type: "decimal(18, 2)", // Fixed the closing parenthesis
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 2)"); // Fixed the space for consistency
        }
    }
}
