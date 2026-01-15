using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietNOCMS.Migrations
{
    /// <inheritdoc />
    public partial class AddBioToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Bio",
                table: "Users",
                type: "TEXT",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 1,
                column: "CreateAt",
                value: new DateTime(2025, 12, 19, 13, 53, 30, 3, DateTimeKind.Local).AddTicks(3393));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 2,
                column: "CreateAt",
                value: new DateTime(2025, 12, 19, 13, 53, 30, 3, DateTimeKind.Local).AddTicks(3412));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 3,
                column: "CreateAt",
                value: new DateTime(2025, 12, 19, 13, 53, 30, 3, DateTimeKind.Local).AddTicks(3414));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 4,
                column: "CreateAt",
                value: new DateTime(2025, 12, 19, 13, 53, 30, 3, DateTimeKind.Local).AddTicks(3415));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 5,
                column: "CreateAt",
                value: new DateTime(2025, 12, 19, 13, 53, 30, 3, DateTimeKind.Local).AddTicks(3416));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "Bio", "CreatedAt", "PasswordHash" },
                values: new object[] { null, new DateTime(2025, 12, 19, 13, 53, 29, 740, DateTimeKind.Local).AddTicks(6828), "$2a$11$nUQB3oXQKuraUlc5jaMI8.FzmjjzBB5/VihBcYBg49EQD8QwU.4bW" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                columns: new[] { "Bio", "CreatedAt", "PasswordHash" },
                values: new object[] { null, new DateTime(2025, 12, 19, 13, 53, 29, 874, DateTimeKind.Local).AddTicks(8040), "$2a$11$xuTwP7.FgmajItdV/vQMzOmKjPbOj/4i2EXCkCcc6wGEMdtBsbuXW" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 3,
                columns: new[] { "Bio", "CreatedAt", "PasswordHash" },
                values: new object[] { null, new DateTime(2025, 12, 19, 13, 53, 30, 3, DateTimeKind.Local).AddTicks(2580), "$2a$11$f61lkto5pTUWXFo7Pf3UC.scpz4FXwhWAzslADD4KcdNcTz67.Qrm" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bio",
                table: "Users");

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
    }
}
