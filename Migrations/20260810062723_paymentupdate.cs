using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VendorEcommerceProject.Migrations
{
    /// <inheritdoc />
    public partial class paymentupdate : Migration
    {
        /// <inheritdoc />
        //protected override void Up(MigrationBuilder migrationBuilder)
        //{
        //    migrationBuilder.AddColumn<long>(
        //        name: "PaymentMethodId",
        //        table: "Orders",
        //        type: "bigint",
        //        nullable: false,
        //        defaultValue: 0L);

        //    migrationBuilder.CreateIndex(
        //        name: "IX_Orders_PaymentMethodId",
        //        table: "Orders",
        //        column: "PaymentMethodId");

        //    migrationBuilder.AddForeignKey(
        //        name: "FK_Orders_PaymentMethods_PaymentMethodId",
        //        table: "Orders",
        //        column: "PaymentMethodId",
        //        principalTable: "PaymentMethods",
        //        principalColumn: "PaymentMethodId",
        //        onDelete: ReferentialAction.Restrict);
        //}
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "PaymentMethodId",
                table: "Orders",
                type: "bigint",
                nullable: true);

            //migrationBuilder.InsertData(
            //    table: "PaymentMethods",
            //    columns: new[] { "PaymentMethodId", "Name", "IsActive" },
            //    values: new object[,]
            //    {
            //{ 1L, "Cash On Delivery", true },
            //{ 2L, "bKash", true },
            //{ 3L, "Nagad", true },
            //{ 4L, "Card", true }
            //    });
            migrationBuilder.InsertData(
    table: "PaymentMethods",
    columns: new[] { "PaymentMethodId", "Name", "IsActive", "CreatedAt" },
    values: new object[,]
    {
        { 1L, "Cash On Delivery", true, new DateTime(2026, 8, 10, 0, 0, 0, DateTimeKind.Utc) },
        { 2L, "bKash", true, new DateTime(2026, 8, 10, 0, 0, 0, DateTimeKind.Utc) },
        { 3L, "Nagad", true, new DateTime(2026, 8, 10, 0, 0, 0, DateTimeKind.Utc) },
        { 4L, "Card", true, new DateTime(2026, 8, 10, 0, 0, 0, DateTimeKind.Utc) }
    });

            migrationBuilder.Sql(
                "UPDATE Orders SET PaymentMethodId = 1 WHERE PaymentMethodId IS NULL");

            migrationBuilder.AlterColumn<long>(
                name: "PaymentMethodId",
                table: "Orders",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_PaymentMethodId",
                table: "Orders",
                column: "PaymentMethodId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_PaymentMethods_PaymentMethodId",
                table: "Orders",
                column: "PaymentMethodId",
                principalTable: "PaymentMethods",
                principalColumn: "PaymentMethodId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_PaymentMethods_PaymentMethodId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_PaymentMethodId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "PaymentMethodId",
                table: "Orders");
        }
    }
}
