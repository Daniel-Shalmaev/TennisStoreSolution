using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TennisStoreServer.Migrations
{
    /// <inheritdoc />
    public partial class addproductlongdescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LongDescription",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LongDescription",
                table: "Products");
        }
    }
}
