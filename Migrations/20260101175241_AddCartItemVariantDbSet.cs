using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VendorEcommerceProject.Migrations
{
    /// <inheritdoc />
    public partial class AddCartItemVariantDbSet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItemVariant_CartItems_CartItemId",
                table: "CartItemVariant");

            migrationBuilder.DropForeignKey(
                name: "FK_CartItemVariant_ProductVariants_ProductVariantId",
                table: "CartItemVariant");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CartItemVariant",
                table: "CartItemVariant");

            migrationBuilder.RenameTable(
                name: "CartItemVariant",
                newName: "CartItemVariants");

            migrationBuilder.RenameIndex(
                name: "IX_CartItemVariant_ProductVariantId",
                table: "CartItemVariants",
                newName: "IX_CartItemVariants_ProductVariantId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CartItemVariants",
                table: "CartItemVariants",
                columns: new[] { "CartItemId", "ProductVariantId" });

            migrationBuilder.AddForeignKey(
                name: "FK_CartItemVariants_CartItems_CartItemId",
                table: "CartItemVariants",
                column: "CartItemId",
                principalTable: "CartItems",
                principalColumn: "CartItemId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CartItemVariants_ProductVariants_ProductVariantId",
                table: "CartItemVariants",
                column: "ProductVariantId",
                principalTable: "ProductVariants",
                principalColumn: "ProductVariantId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItemVariants_CartItems_CartItemId",
                table: "CartItemVariants");

            migrationBuilder.DropForeignKey(
                name: "FK_CartItemVariants_ProductVariants_ProductVariantId",
                table: "CartItemVariants");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CartItemVariants",
                table: "CartItemVariants");

            migrationBuilder.RenameTable(
                name: "CartItemVariants",
                newName: "CartItemVariant");

            migrationBuilder.RenameIndex(
                name: "IX_CartItemVariants_ProductVariantId",
                table: "CartItemVariant",
                newName: "IX_CartItemVariant_ProductVariantId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CartItemVariant",
                table: "CartItemVariant",
                columns: new[] { "CartItemId", "ProductVariantId" });

            migrationBuilder.AddForeignKey(
                name: "FK_CartItemVariant_CartItems_CartItemId",
                table: "CartItemVariant",
                column: "CartItemId",
                principalTable: "CartItems",
                principalColumn: "CartItemId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CartItemVariant_ProductVariants_ProductVariantId",
                table: "CartItemVariant",
                column: "ProductVariantId",
                principalTable: "ProductVariants",
                principalColumn: "ProductVariantId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
