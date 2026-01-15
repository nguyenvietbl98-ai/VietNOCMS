using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietNOCMS.Migrations
{
    /// <inheritdoc />
    public partial class Addreview : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InstructorReply",
                table: "Reviews",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ReplyAt",
                table: "Reviews",
                type: "TEXT",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 1,
                column: "CreateAt",
                value: new DateTime(2025, 12, 2, 15, 27, 44, 537, DateTimeKind.Local).AddTicks(7519));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 2,
                column: "CreateAt",
                value: new DateTime(2025, 12, 2, 15, 27, 44, 537, DateTimeKind.Local).AddTicks(7541));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 3,
                column: "CreateAt",
                value: new DateTime(2025, 12, 2, 15, 27, 44, 537, DateTimeKind.Local).AddTicks(7543));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 4,
                column: "CreateAt",
                value: new DateTime(2025, 12, 2, 15, 27, 44, 537, DateTimeKind.Local).AddTicks(7544));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 5,
                column: "CreateAt",
                value: new DateTime(2025, 12, 2, 15, 27, 44, 537, DateTimeKind.Local).AddTicks(7546));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 12, 2, 15, 27, 44, 281, DateTimeKind.Local).AddTicks(259), "$2a$11$n4LTcvfJT0wZDjpOCHKtvOJGIf5kIX5F623s6UPQhBeIcYkL8P7MS" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 12, 2, 15, 27, 44, 410, DateTimeKind.Local).AddTicks(3371), "$2a$11$uoGPgM/v.7H8XyGftEU8X.EZ8l3lAL.karzSej.Xir7jwBReJR5Ua" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 3,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 12, 2, 15, 27, 44, 537, DateTimeKind.Local).AddTicks(6755), "$2a$11$n12yVW9fecFc6SRV2.l4JucxPpx4Og.9sXqEwXSqq.wHOZ6XxlD6a" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InstructorReply",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "ReplyAt",
                table: "Reviews");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 1,
                column: "CreateAt",
                value: new DateTime(2025, 11, 29, 21, 16, 39, 143, DateTimeKind.Local).AddTicks(1157));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 2,
                column: "CreateAt",
                value: new DateTime(2025, 11, 29, 21, 16, 39, 143, DateTimeKind.Local).AddTicks(1171));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 3,
                column: "CreateAt",
                value: new DateTime(2025, 11, 29, 21, 16, 39, 143, DateTimeKind.Local).AddTicks(1173));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 4,
                column: "CreateAt",
                value: new DateTime(2025, 11, 29, 21, 16, 39, 143, DateTimeKind.Local).AddTicks(1174));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 5,
                column: "CreateAt",
                value: new DateTime(2025, 11, 29, 21, 16, 39, 143, DateTimeKind.Local).AddTicks(1175));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 11, 29, 21, 16, 38, 881, DateTimeKind.Local).AddTicks(3833), "$2a$11$4dCVB6O6RpNEl7jaCaXTgeW755fLAATfH.Nj76McbK..qq2Tm/X4u" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 11, 29, 21, 16, 39, 11, DateTimeKind.Local).AddTicks(3151), "$2a$11$a6tmdV2bTRjn8aKvbXnCm.PAXcgx5HA6ZYk7z1yIJY4Y0cc2nkE9W" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 3,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 11, 29, 21, 16, 39, 143, DateTimeKind.Local).AddTicks(468), "$2a$11$fAv0U4y6DVz/uZxjCf.BuOsmfLwycgsX0UK3WdhulfMsX2oEMDJG6" });
        }
    }
}
