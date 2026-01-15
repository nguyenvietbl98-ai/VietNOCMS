using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietNOCMS.Migrations
{
    /// <inheritdoc />
    public partial class AddWalletAndBalance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Balance",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "Wallet",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false),
                    Amount = table.Column<decimal>(type: "TEXT", nullable: false),
                    Type = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wallet", x => x.Id);
                });

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
                columns: new[] { "Balance", "CreatedAt", "PasswordHash" },
                values: new object[] { 0m, new DateTime(2025, 12, 18, 9, 33, 58, 857, DateTimeKind.Local).AddTicks(2698), "$2a$11$Gn2oBrSwvXGcGm6Y7vsQ.eeTsqnEp7BouoUO1EBuXQXhRvH8/RgwO" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                columns: new[] { "Balance", "CreatedAt", "PasswordHash" },
                values: new object[] { 0m, new DateTime(2025, 12, 18, 9, 33, 59, 68, DateTimeKind.Local).AddTicks(5811), "$2a$11$lPwXlLUnwhgO4TTAkIku0O7FqztIIJNeWIRWcbm.YDd4oivpjrLIi" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 3,
                columns: new[] { "Balance", "CreatedAt", "PasswordHash" },
                values: new object[] { 0m, new DateTime(2025, 12, 18, 9, 33, 59, 293, DateTimeKind.Local).AddTicks(3690), "$2a$11$01b9UsjDMSUqLmys5mAtCeW7iPGqxo3uvXmZ6NLlIMvx5u7tjqFFy" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Wallet");

            migrationBuilder.DropColumn(
                name: "Balance",
                table: "Users");

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
    }
}
