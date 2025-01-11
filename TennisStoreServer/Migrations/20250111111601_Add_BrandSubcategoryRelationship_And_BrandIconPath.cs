using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TennisStoreServer.Migrations
{
    /// <inheritdoc />
    public partial class Add_BrandSubcategoryRelationship_And_BrandIconPath : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BrandIconPath",
                table: "Subcategories",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BrandSubcategory",
                columns: table => new
                {
                    BrandsId = table.Column<int>(type: "int", nullable: false),
                    SubcategoriesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BrandSubcategory", x => new { x.BrandsId, x.SubcategoriesId });
                    table.ForeignKey(
                        name: "FK_BrandSubcategory_Brands_BrandsId",
                        column: x => x.BrandsId,
                        principalTable: "Brands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BrandSubcategory_Subcategories_SubcategoriesId",
                        column: x => x.SubcategoriesId,
                        principalTable: "Subcategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BrandSubcategory_SubcategoriesId",
                table: "BrandSubcategory",
                column: "SubcategoriesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BrandSubcategory");

            migrationBuilder.DropColumn(
                name: "BrandIconPath",
                table: "Subcategories");
        }
    }
}
