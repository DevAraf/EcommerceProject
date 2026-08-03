using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace VendorEcommerceProject.Migrations
{
    /// <inheritdoc />
    public partial class SeedProductStatuses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ProductStatuses",
                columns: new[] { "ProductStatusId", "CreatedAt", "CreatedBy", "DeletedAt", "DeletedBy", "DisplayName", "ModifiedAt", "ModifiedBy", "Name" },
                values: new object[,]
                {
                    { 1L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "Pending Approval", null, null, "Pending" },
                    { 2L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "Approved", null, null, "Approved" },
                    { 3L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "Rejected", null, null, "Rejected" },
                    { 4L, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, null, "Blocked by Admin", null, null, "Blocked" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ProductStatuses",
                keyColumn: "ProductStatusId",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "ProductStatuses",
                keyColumn: "ProductStatusId",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "ProductStatuses",
                keyColumn: "ProductStatusId",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "ProductStatuses",
                keyColumn: "ProductStatusId",
                keyValue: 4L);
        }
    }
}
