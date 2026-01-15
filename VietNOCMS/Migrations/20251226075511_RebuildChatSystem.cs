using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietNOCMS.Migrations
{
    /// <inheritdoc />
    public partial class RebuildChatSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 1,
                column: "CreateAt",
                value: new DateTime(2025, 12, 26, 14, 55, 10, 606, DateTimeKind.Local).AddTicks(7180));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 2,
                column: "CreateAt",
                value: new DateTime(2025, 12, 26, 14, 55, 10, 606, DateTimeKind.Local).AddTicks(7215));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 3,
                column: "CreateAt",
                value: new DateTime(2025, 12, 26, 14, 55, 10, 606, DateTimeKind.Local).AddTicks(7217));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 4,
                column: "CreateAt",
                value: new DateTime(2025, 12, 26, 14, 55, 10, 606, DateTimeKind.Local).AddTicks(7219));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 5,
                column: "CreateAt",
                value: new DateTime(2025, 12, 26, 14, 55, 10, 606, DateTimeKind.Local).AddTicks(7221));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 12, 26, 14, 55, 10, 344, DateTimeKind.Local).AddTicks(5415), "$2a$11$xTRfAn/.ycVu10IjNxUYJ.HpwOyu/BQjUc431RkZPzBSIs6tI9eXu" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 12, 26, 14, 55, 10, 477, DateTimeKind.Local).AddTicks(3299), "$2a$11$ZLN.uK8KZr87sQytV1GTvOERJrC7VivVvFhn.cR.QVswxLoAF10Ue" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 3,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 12, 26, 14, 55, 10, 606, DateTimeKind.Local).AddTicks(5967), "$2a$11$BYh2SwVlVLECOjThEK2gG.vtWATWjV4F.7dP6MIcrj32alvmCSbZu" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 1,
                column: "CreateAt",
                value: new DateTime(2025, 12, 26, 14, 42, 33, 901, DateTimeKind.Local).AddTicks(2810));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 2,
                column: "CreateAt",
                value: new DateTime(2025, 12, 26, 14, 42, 33, 901, DateTimeKind.Local).AddTicks(2832));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 3,
                column: "CreateAt",
                value: new DateTime(2025, 12, 26, 14, 42, 33, 901, DateTimeKind.Local).AddTicks(2834));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 4,
                column: "CreateAt",
                value: new DateTime(2025, 12, 26, 14, 42, 33, 901, DateTimeKind.Local).AddTicks(2835));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 5,
                column: "CreateAt",
                value: new DateTime(2025, 12, 26, 14, 42, 33, 901, DateTimeKind.Local).AddTicks(2836));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 12, 26, 14, 42, 33, 640, DateTimeKind.Local).AddTicks(3019), "$2a$11$aFH0.jrjYXaFlx9.2DG0I.PQyrjRkfpuWovRqeODXtig0zMwpx9su" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 12, 26, 14, 42, 33, 774, DateTimeKind.Local).AddTicks(777), "$2a$11$I.Xy2n/grCc4rEcgnXtllu.HykNIHuaVzgmmSH9jauHmqcUDExVPW" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 3,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 12, 26, 14, 42, 33, 901, DateTimeKind.Local).AddTicks(1872), "$2a$11$AP1r.L2UBLyaVp10ciYuAO3X1Vg4N2rTvJVnhQDjw9RSzADji9iky" });
        }
    }
}
