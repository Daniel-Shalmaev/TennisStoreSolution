﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TennisStoreServer.Migrations
{
    /// <inheritdoc />
    public partial class addcategorynametosubcategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CategoryName",
                table: "Subcategories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CategoryName",
                table: "Subcategories");
        }
    }
}
