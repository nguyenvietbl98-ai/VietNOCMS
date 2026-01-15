using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietNOCMS.Migrations
{
    /// <inheritdoc />
    public partial class fixcin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CourseId",
                table: "Conversations",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Conversations",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 1,
                column: "CreateAt",
                value: new DateTime(2025, 12, 26, 15, 43, 10, 232, DateTimeKind.Local).AddTicks(2373));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 2,
                column: "CreateAt",
                value: new DateTime(2025, 12, 26, 15, 43, 10, 232, DateTimeKind.Local).AddTicks(2398));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 3,
                column: "CreateAt",
                value: new DateTime(2025, 12, 26, 15, 43, 10, 232, DateTimeKind.Local).AddTicks(2400));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 4,
                column: "CreateAt",
                value: new DateTime(2025, 12, 26, 15, 43, 10, 232, DateTimeKind.Local).AddTicks(2401));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 5,
                column: "CreateAt",
                value: new DateTime(2025, 12, 26, 15, 43, 10, 232, DateTimeKind.Local).AddTicks(2406));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 12, 26, 15, 43, 9, 959, DateTimeKind.Local).AddTicks(2427), "$2a$11$ilCBAXklga7mJ29VeWLwwOn4XH6hvcfHlbDcNmZKhZ8RWY4enh8fC" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 12, 26, 15, 43, 10, 100, DateTimeKind.Local).AddTicks(5463), "$2a$11$djEZbGDoGE6nIqJP/7FWuuIsl.8Ou0PR2Dww3EzilLwxoawGoigem" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 3,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 12, 26, 15, 43, 10, 232, DateTimeKind.Local).AddTicks(1534), "$2a$11$fGT1FLpcVKm1YlUcTCd7DOXVWNzby6R17BJCd9tXXFOD08so8bEL." });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "Conversations");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Conversations");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 1,
                column: "CreateAt",
                value: new DateTime(2025, 12, 26, 15, 40, 26, 512, DateTimeKind.Local).AddTicks(7158));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 2,
                column: "CreateAt",
                value: new DateTime(2025, 12, 26, 15, 40, 26, 512, DateTimeKind.Local).AddTicks(7235));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 3,
                column: "CreateAt",
                value: new DateTime(2025, 12, 26, 15, 40, 26, 512, DateTimeKind.Local).AddTicks(7237));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 4,
                column: "CreateAt",
                value: new DateTime(2025, 12, 26, 15, 40, 26, 512, DateTimeKind.Local).AddTicks(7238));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 5,
                column: "CreateAt",
                value: new DateTime(2025, 12, 26, 15, 40, 26, 512, DateTimeKind.Local).AddTicks(7240));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 12, 26, 15, 40, 26, 232, DateTimeKind.Local).AddTicks(3666), "$2a$11$UaRZO10dWkQstzuq/49Ihuox3sk3uGEod4DMryhsGrFj1wc8sZ0di" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 12, 26, 15, 40, 26, 377, DateTimeKind.Local).AddTicks(4431), "$2a$11$uDBlmJCANnm.jhni45JzyOJKV3oRjscpTqBTC263mOP35ZLFktffG" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 3,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 12, 26, 15, 40, 26, 512, DateTimeKind.Local).AddTicks(4012), "$2a$11$3X3fni15sGzxwabwEe.5AOQblnQoAyOy3zO/6Q6Ep6qeJkmaUxh7O" });
        }
    }
}
