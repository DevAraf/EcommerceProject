using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace VendorEcommerceProject.Migrations
{
    /// <inheritdoc />
    public partial class SeedOrderStatuses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "OrderStatuses",
                columns: new[] { "OrderStatusId", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "ModifiedAt", "ModifiedBy", "Name" },
                values: new object[,]
                {
                    { 1L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, null, "Pending" },
                    { 2L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, null, "Processing" },
                    { 3L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, null, "Shipped" },
                    { 4L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, null, "Delivered" },
                    { 5L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, null, null, "Cancelled" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "OrderStatuses",
                keyColumn: "OrderStatusId",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "OrderStatuses",
                keyColumn: "OrderStatusId",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "OrderStatuses",
                keyColumn: "OrderStatusId",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "OrderStatuses",
                keyColumn: "OrderStatusId",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "OrderStatuses",
                keyColumn: "OrderStatusId",
                keyValue: 5L);
        }
    }
}
