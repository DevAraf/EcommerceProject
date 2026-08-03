using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VendorEcommerceProject.Migrations
{
    /// <inheritdoc />
    public partial class RenameVariantPrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AdditionalPrice",
                table: "ProductVariants",
                newName: "Price");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Price",
                table: "ProductVariants",
                newName: "AdditionalPrice");
        }
    }
}
