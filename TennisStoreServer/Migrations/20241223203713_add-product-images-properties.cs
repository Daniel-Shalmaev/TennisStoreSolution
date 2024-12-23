using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TennisStoreServer.Migrations
{
    /// <inheritdoc />
    public partial class addproductimagesproperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Base64Img",
                table: "Products",
                newName: "Image1");

            migrationBuilder.AddColumn<string>(
                name: "Image2",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image3",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image4",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image5",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image6",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image7",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image8",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image2",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Image3",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Image4",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Image5",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Image6",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Image7",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Image8",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "Image1",
                table: "Products",
                newName: "Base64Img");
        }
    }
}
