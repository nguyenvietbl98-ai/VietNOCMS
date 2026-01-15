using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietNOCMS.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEnrollmentStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RejectionReason",
                table: "Enrollments",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Enrollments",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 1,
                column: "CreateAt",
                value: new DateTime(2025, 12, 18, 10, 35, 34, 863, DateTimeKind.Local).AddTicks(7843));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 2,
                column: "CreateAt",
                value: new DateTime(2025, 12, 18, 10, 35, 34, 863, DateTimeKind.Local).AddTicks(7869));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 3,
                column: "CreateAt",
                value: new DateTime(2025, 12, 18, 10, 35, 34, 863, DateTimeKind.Local).AddTicks(7871));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 4,
                column: "CreateAt",
                value: new DateTime(2025, 12, 18, 10, 35, 34, 863, DateTimeKind.Local).AddTicks(7872));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 5,
                column: "CreateAt",
                value: new DateTime(2025, 12, 18, 10, 35, 34, 863, DateTimeKind.Local).AddTicks(7873));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 12, 18, 10, 35, 34, 608, DateTimeKind.Local).AddTicks(2665), "$2a$11$nov3lUu9S.AGcWEgJ8pYvODzL/pnDlcZ6sSCFJSju2pxbpSNGBpqC" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 12, 18, 10, 35, 34, 737, DateTimeKind.Local).AddTicks(7360), "$2a$11$7kC4YcWFMDM34jJYcQYfIOheL3DHchoWhkhKbrQe8NlENlRJYpyhG" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 3,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 12, 18, 10, 35, 34, 863, DateTimeKind.Local).AddTicks(7014), "$2a$11$isHxWlyfav..INHTzJSS2Ow7mS8BD16W.oQrOHDH73GY.wxUVJ9oS" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RejectionReason",
                table: "Enrollments");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Enrollments");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 1,
                column: "CreateAt",
                value: new DateTime(2025, 12, 18, 9, 33, 59, 293, DateTimeKind.Local).AddTicks(4790));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 2,
                column: "CreateAt",
                value: new DateTime(2025, 12, 18, 9, 33, 59, 293, DateTimeKind.Local).AddTicks(4796));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 3,
                column: "CreateAt",
                value: new DateTime(2025, 12, 18, 9, 33, 59, 293, DateTimeKind.Local).AddTicks(4797));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 4,
                column: "CreateAt",
                value: new DateTime(2025, 12, 18, 9, 33, 59, 293, DateTimeKind.Local).AddTicks(4799));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 5,
                column: "CreateAt",
                value: new DateTime(2025, 12, 18, 9, 33, 59, 293, DateTimeKind.Local).AddTicks(4800));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 12, 18, 9, 33, 58, 857, DateTimeKind.Local).AddTicks(2698), "$2a$11$Gn2oBrSwvXGcGm6Y7vsQ.eeTsqnEp7BouoUO1EBuXQXhRvH8/RgwO" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 12, 18, 9, 33, 59, 68, DateTimeKind.Local).AddTicks(5811), "$2a$11$lPwXlLUnwhgO4TTAkIku0O7FqztIIJNeWIRWcbm.YDd4oivpjrLIi" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 3,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 12, 18, 9, 33, 59, 293, DateTimeKind.Local).AddTicks(3690), "$2a$11$01b9UsjDMSUqLmys5mAtCeW7iPGqxo3uvXmZ6NLlIMvx5u7tjqFFy" });
        }
    }
}
