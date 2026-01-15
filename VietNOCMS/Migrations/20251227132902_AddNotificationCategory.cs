using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietNOCMS.Migrations
{
    /// <inheritdoc />
    public partial class AddNotificationCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Notifications",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 1,
                column: "CreateAt",
                value: new DateTime(2025, 12, 27, 20, 29, 2, 146, DateTimeKind.Local).AddTicks(986));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 2,
                column: "CreateAt",
                value: new DateTime(2025, 12, 27, 20, 29, 2, 146, DateTimeKind.Local).AddTicks(1008));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 3,
                column: "CreateAt",
                value: new DateTime(2025, 12, 27, 20, 29, 2, 146, DateTimeKind.Local).AddTicks(1009));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 4,
                column: "CreateAt",
                value: new DateTime(2025, 12, 27, 20, 29, 2, 146, DateTimeKind.Local).AddTicks(1010));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 5,
                column: "CreateAt",
                value: new DateTime(2025, 12, 27, 20, 29, 2, 146, DateTimeKind.Local).AddTicks(1011));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 12, 27, 20, 29, 1, 889, DateTimeKind.Local).AddTicks(6478), "$2a$11$5hgU3NqCZicFVPVXu5Arve8AuAhD.Tq5ypi0GCFj9nFzl536a2wKC" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 12, 27, 20, 29, 2, 18, DateTimeKind.Local).AddTicks(6165), "$2a$11$aKY/w95GIgikxvEIfk3U5OwIDopcbeNyL6.qmckpR8oWKalpsI32S" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 3,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 12, 27, 20, 29, 2, 146, DateTimeKind.Local).AddTicks(167), "$2a$11$qeRRctgvM9i7n8tZq5XgeuQzJqMkJt2WCHJXLwfAAeZZ22ryxIr.u" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "Notifications");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 1,
                column: "CreateAt",
                value: new DateTime(2025, 12, 26, 19, 50, 42, 291, DateTimeKind.Local).AddTicks(3014));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 2,
                column: "CreateAt",
                value: new DateTime(2025, 12, 26, 19, 50, 42, 291, DateTimeKind.Local).AddTicks(3038));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 3,
                column: "CreateAt",
                value: new DateTime(2025, 12, 26, 19, 50, 42, 291, DateTimeKind.Local).AddTicks(3040));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 4,
                column: "CreateAt",
                value: new DateTime(2025, 12, 26, 19, 50, 42, 291, DateTimeKind.Local).AddTicks(3041));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 5,
                column: "CreateAt",
                value: new DateTime(2025, 12, 26, 19, 50, 42, 291, DateTimeKind.Local).AddTicks(3043));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 12, 26, 19, 50, 41, 991, DateTimeKind.Local).AddTicks(6413), "$2a$11$fDHPtxGbmJuKsaDlamDMS.gKD6ktwgkxWdqb57RRN2SewSPOGkiYG" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 12, 26, 19, 50, 42, 140, DateTimeKind.Local).AddTicks(8790), "$2a$11$Lju2yGGtiFCkHwODTmKvnuJpJw1a/6OsM1e4MY32FI5Fw3.as5P82" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 3,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 12, 26, 19, 50, 42, 291, DateTimeKind.Local).AddTicks(1996), "$2a$11$uw1IQgupCEAjLA3cdwYozeVZ9S7OJcMySCb5wFb9FImHG6DWBzzhi" });
        }
    }
}
