using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VendorEcommerceProject.Migrations
{
    /// <inheritdoc />
    public partial class varrientUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ParentVariantId",
                table: "ProductVariants",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProductVariants_ParentVariantId",
                table: "ProductVariants",
                column: "ParentVariantId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductVariants_ProductVariants_ParentVariantId",
                table: "ProductVariants",
                column: "ParentVariantId",
                principalTable: "ProductVariants",
                principalColumn: "ProductVariantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductVariants_ProductVariants_ParentVariantId",
                table: "ProductVariants");

            migrationBuilder.DropIndex(
                name: "IX_ProductVariants_ParentVariantId",
                table: "ProductVariants");

            migrationBuilder.DropColumn(
                name: "ParentVariantId",
                table: "ProductVariants");
        }
    }
}
