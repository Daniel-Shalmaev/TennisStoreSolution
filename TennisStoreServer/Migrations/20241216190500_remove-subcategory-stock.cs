using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TennisStoreServer.Migrations
{
    /// <inheritdoc />
    public partial class removesubcategorystock : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Stock",
                table: "Subcategories");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Stock",
                table: "Subcategories",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
