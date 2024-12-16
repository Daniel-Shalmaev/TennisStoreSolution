using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TennisStoreServer.Migrations
{
    /// <inheritdoc />
    public partial class addsubnameproduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SubName",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubName",
                table: "Products");
        }
    }
}
