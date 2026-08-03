using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VendorEcommerceProject.Migrations
{
    /// <inheritdoc />
    public partial class RemoveVendorFromProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Commissions_Orders_OrderId",
                table: "Commissions");

            migrationBuilder.DropForeignKey(
                name: "FK_Commissions_Vendors_VendorId",
                table: "Commissions");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Vendors_VendorId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_VendorEarnings_Orders_OrderId",
                table: "VendorEarnings");

            migrationBuilder.DropForeignKey(
                name: "FK_VendorEarnings_Vendors_VendorId",
                table: "VendorEarnings");

            migrationBuilder.DropForeignKey(
                name: "FK_VendorPayments_Vendors_VendorId",
                table: "VendorPayments");

            migrationBuilder.DropForeignKey(
                name: "FK_VendorReviews_AspNetUsers_UserId",
                table: "VendorReviews");

            migrationBuilder.DropForeignKey(
                name: "FK_VendorReviews_Vendors_VendorId",
                table: "VendorReviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Vendors_AspNetUsers_UserId",
                table: "Vendors");

            migrationBuilder.DropForeignKey(
                name: "FK_VendorSettings_Vendors_VendorId",
                table: "VendorSettings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VendorSettings",
                table: "VendorSettings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Vendors",
                table: "Vendors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VendorReviews",
                table: "VendorReviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VendorPayments",
                table: "VendorPayments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VendorEarnings",
                table: "VendorEarnings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Commissions",
                table: "Commissions");

            migrationBuilder.RenameTable(
                name: "VendorSettings",
                newName: "VendorSetting");

            migrationBuilder.RenameTable(
                name: "Vendors",
                newName: "Vendor");

            migrationBuilder.RenameTable(
                name: "VendorReviews",
                newName: "VendorReview");

            migrationBuilder.RenameTable(
                name: "VendorPayments",
                newName: "VendorPayment");

            migrationBuilder.RenameTable(
                name: "VendorEarnings",
                newName: "VendorEarning");

            migrationBuilder.RenameTable(
                name: "Commissions",
                newName: "Commission");

            migrationBuilder.RenameIndex(
                name: "IX_VendorSettings_VendorId",
                table: "VendorSetting",
                newName: "IX_VendorSetting_VendorId");

            migrationBuilder.RenameIndex(
                name: "IX_Vendors_UserId",
                table: "Vendor",
                newName: "IX_Vendor_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_VendorReviews_VendorId",
                table: "VendorReview",
                newName: "IX_VendorReview_VendorId");

            migrationBuilder.RenameIndex(
                name: "IX_VendorReviews_UserId",
                table: "VendorReview",
                newName: "IX_VendorReview_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_VendorPayments_VendorId",
                table: "VendorPayment",
                newName: "IX_VendorPayment_VendorId");

            migrationBuilder.RenameIndex(
                name: "IX_VendorEarnings_VendorId",
                table: "VendorEarning",
                newName: "IX_VendorEarning_VendorId");

            migrationBuilder.RenameIndex(
                name: "IX_VendorEarnings_OrderId",
                table: "VendorEarning",
                newName: "IX_VendorEarning_OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_Commissions_VendorId",
                table: "Commission",
                newName: "IX_Commission_VendorId");

            migrationBuilder.RenameIndex(
                name: "IX_Commissions_OrderId",
                table: "Commission",
                newName: "IX_Commission_OrderId");

            migrationBuilder.AlterColumn<long>(
                name: "VendorId",
                table: "Products",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VendorSetting",
                table: "VendorSetting",
                column: "VendorSettingId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Vendor",
                table: "Vendor",
                column: "VendorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VendorReview",
                table: "VendorReview",
                column: "VendorReviewId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VendorPayment",
                table: "VendorPayment",
                column: "VendorPaymentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VendorEarning",
                table: "VendorEarning",
                column: "VendorEarningId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Commission",
                table: "Commission",
                column: "CommissionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Commission_Orders_OrderId",
                table: "Commission",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Commission_Vendor_VendorId",
                table: "Commission",
                column: "VendorId",
                principalTable: "Vendor",
                principalColumn: "VendorId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Vendor_VendorId",
                table: "Products",
                column: "VendorId",
                principalTable: "Vendor",
                principalColumn: "VendorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vendor_AspNetUsers_UserId",
                table: "Vendor",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VendorEarning_Orders_OrderId",
                table: "VendorEarning",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VendorEarning_Vendor_VendorId",
                table: "VendorEarning",
                column: "VendorId",
                principalTable: "Vendor",
                principalColumn: "VendorId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VendorPayment_Vendor_VendorId",
                table: "VendorPayment",
                column: "VendorId",
                principalTable: "Vendor",
                principalColumn: "VendorId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VendorReview_AspNetUsers_UserId",
                table: "VendorReview",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VendorReview_Vendor_VendorId",
                table: "VendorReview",
                column: "VendorId",
                principalTable: "Vendor",
                principalColumn: "VendorId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VendorSetting_Vendor_VendorId",
                table: "VendorSetting",
                column: "VendorId",
                principalTable: "Vendor",
                principalColumn: "VendorId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Commission_Orders_OrderId",
                table: "Commission");

            migrationBuilder.DropForeignKey(
                name: "FK_Commission_Vendor_VendorId",
                table: "Commission");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Vendor_VendorId",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Vendor_AspNetUsers_UserId",
                table: "Vendor");

            migrationBuilder.DropForeignKey(
                name: "FK_VendorEarning_Orders_OrderId",
                table: "VendorEarning");

            migrationBuilder.DropForeignKey(
                name: "FK_VendorEarning_Vendor_VendorId",
                table: "VendorEarning");

            migrationBuilder.DropForeignKey(
                name: "FK_VendorPayment_Vendor_VendorId",
                table: "VendorPayment");

            migrationBuilder.DropForeignKey(
                name: "FK_VendorReview_AspNetUsers_UserId",
                table: "VendorReview");

            migrationBuilder.DropForeignKey(
                name: "FK_VendorReview_Vendor_VendorId",
                table: "VendorReview");

            migrationBuilder.DropForeignKey(
                name: "FK_VendorSetting_Vendor_VendorId",
                table: "VendorSetting");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VendorSetting",
                table: "VendorSetting");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VendorReview",
                table: "VendorReview");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VendorPayment",
                table: "VendorPayment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_VendorEarning",
                table: "VendorEarning");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Vendor",
                table: "Vendor");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Commission",
                table: "Commission");

            migrationBuilder.RenameTable(
                name: "VendorSetting",
                newName: "VendorSettings");

            migrationBuilder.RenameTable(
                name: "VendorReview",
                newName: "VendorReviews");

            migrationBuilder.RenameTable(
                name: "VendorPayment",
                newName: "VendorPayments");

            migrationBuilder.RenameTable(
                name: "VendorEarning",
                newName: "VendorEarnings");

            migrationBuilder.RenameTable(
                name: "Vendor",
                newName: "Vendors");

            migrationBuilder.RenameTable(
                name: "Commission",
                newName: "Commissions");

            migrationBuilder.RenameIndex(
                name: "IX_VendorSetting_VendorId",
                table: "VendorSettings",
                newName: "IX_VendorSettings_VendorId");

            migrationBuilder.RenameIndex(
                name: "IX_VendorReview_VendorId",
                table: "VendorReviews",
                newName: "IX_VendorReviews_VendorId");

            migrationBuilder.RenameIndex(
                name: "IX_VendorReview_UserId",
                table: "VendorReviews",
                newName: "IX_VendorReviews_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_VendorPayment_VendorId",
                table: "VendorPayments",
                newName: "IX_VendorPayments_VendorId");

            migrationBuilder.RenameIndex(
                name: "IX_VendorEarning_VendorId",
                table: "VendorEarnings",
                newName: "IX_VendorEarnings_VendorId");

            migrationBuilder.RenameIndex(
                name: "IX_VendorEarning_OrderId",
                table: "VendorEarnings",
                newName: "IX_VendorEarnings_OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_Vendor_UserId",
                table: "Vendors",
                newName: "IX_Vendors_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Commission_VendorId",
                table: "Commissions",
                newName: "IX_Commissions_VendorId");

            migrationBuilder.RenameIndex(
                name: "IX_Commission_OrderId",
                table: "Commissions",
                newName: "IX_Commissions_OrderId");

            migrationBuilder.AlterColumn<long>(
                name: "VendorId",
                table: "Products",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_VendorSettings",
                table: "VendorSettings",
                column: "VendorSettingId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VendorReviews",
                table: "VendorReviews",
                column: "VendorReviewId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VendorPayments",
                table: "VendorPayments",
                column: "VendorPaymentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_VendorEarnings",
                table: "VendorEarnings",
                column: "VendorEarningId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Vendors",
                table: "Vendors",
                column: "VendorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Commissions",
                table: "Commissions",
                column: "CommissionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Commissions_Orders_OrderId",
                table: "Commissions",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Commissions_Vendors_VendorId",
                table: "Commissions",
                column: "VendorId",
                principalTable: "Vendors",
                principalColumn: "VendorId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Vendors_VendorId",
                table: "Products",
                column: "VendorId",
                principalTable: "Vendors",
                principalColumn: "VendorId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VendorEarnings_Orders_OrderId",
                table: "VendorEarnings",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "OrderId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VendorEarnings_Vendors_VendorId",
                table: "VendorEarnings",
                column: "VendorId",
                principalTable: "Vendors",
                principalColumn: "VendorId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VendorPayments_Vendors_VendorId",
                table: "VendorPayments",
                column: "VendorId",
                principalTable: "Vendors",
                principalColumn: "VendorId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VendorReviews_AspNetUsers_UserId",
                table: "VendorReviews",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VendorReviews_Vendors_VendorId",
                table: "VendorReviews",
                column: "VendorId",
                principalTable: "Vendors",
                principalColumn: "VendorId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Vendors_AspNetUsers_UserId",
                table: "Vendors",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VendorSettings_Vendors_VendorId",
                table: "VendorSettings",
                column: "VendorId",
                principalTable: "Vendors",
                principalColumn: "VendorId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
