using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TennisStoreServer.Migrations
{
    /// <inheritdoc />
    public partial class AddBrandSubcategoryRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BrandSubcategory_Brands_BrandsId",
                table: "BrandSubcategory");

            migrationBuilder.DropForeignKey(
                name: "FK_BrandSubcategory_Subcategories_SubcategoriesId",
                table: "BrandSubcategory");

            migrationBuilder.RenameColumn(
                name: "SubcategoriesId",
                table: "BrandSubcategory",
                newName: "SubcategoryId");

            migrationBuilder.RenameColumn(
                name: "BrandsId",
                table: "BrandSubcategory",
                newName: "BrandId");

            migrationBuilder.RenameIndex(
                name: "IX_BrandSubcategory_SubcategoriesId",
                table: "BrandSubcategory",
                newName: "IX_BrandSubcategory_SubcategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_BrandSubcategory_Brands_BrandId",
                table: "BrandSubcategory",
                column: "BrandId",
                principalTable: "Brands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BrandSubcategory_Subcategories_SubcategoryId",
                table: "BrandSubcategory",
                column: "SubcategoryId",
                principalTable: "Subcategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BrandSubcategory_Brands_BrandId",
                table: "BrandSubcategory");

            migrationBuilder.DropForeignKey(
                name: "FK_BrandSubcategory_Subcategories_SubcategoryId",
                table: "BrandSubcategory");

            migrationBuilder.RenameColumn(
                name: "SubcategoryId",
                table: "BrandSubcategory",
                newName: "SubcategoriesId");

            migrationBuilder.RenameColumn(
                name: "BrandId",
                table: "BrandSubcategory",
                newName: "BrandsId");

            migrationBuilder.RenameIndex(
                name: "IX_BrandSubcategory_SubcategoryId",
                table: "BrandSubcategory",
                newName: "IX_BrandSubcategory_SubcategoriesId");

            migrationBuilder.AddForeignKey(
                name: "FK_BrandSubcategory_Brands_BrandsId",
                table: "BrandSubcategory",
                column: "BrandsId",
                principalTable: "Brands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BrandSubcategory_Subcategories_SubcategoriesId",
                table: "BrandSubcategory",
                column: "SubcategoriesId",
                principalTable: "Subcategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
