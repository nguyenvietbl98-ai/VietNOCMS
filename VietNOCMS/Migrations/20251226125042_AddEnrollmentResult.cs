using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietNOCMS.Migrations
{
    /// <inheritdoc />
    public partial class AddEnrollmentResult : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CompletedAt",
                table: "Enrollments",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "FinalScore",
                table: "Enrollments",
                type: "REAL",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCertified",
                table: "Enrollments",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Rank",
                table: "Enrollments",
                type: "TEXT",
                nullable: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompletedAt",
                table: "Enrollments");

            migrationBuilder.DropColumn(
                name: "FinalScore",
                table: "Enrollments");

            migrationBuilder.DropColumn(
                name: "IsCertified",
                table: "Enrollments");

            migrationBuilder.DropColumn(
                name: "Rank",
                table: "Enrollments");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 1,
                column: "CreateAt",
                value: new DateTime(2025, 12, 26, 16, 3, 43, 302, DateTimeKind.Local).AddTicks(1591));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 2,
                column: "CreateAt",
                value: new DateTime(2025, 12, 26, 16, 3, 43, 302, DateTimeKind.Local).AddTicks(1626));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 3,
                column: "CreateAt",
                value: new DateTime(2025, 12, 26, 16, 3, 43, 302, DateTimeKind.Local).AddTicks(1628));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 4,
                column: "CreateAt",
                value: new DateTime(2025, 12, 26, 16, 3, 43, 302, DateTimeKind.Local).AddTicks(1630));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 5,
                column: "CreateAt",
                value: new DateTime(2025, 12, 26, 16, 3, 43, 302, DateTimeKind.Local).AddTicks(1631));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 12, 26, 16, 3, 43, 38, DateTimeKind.Local).AddTicks(4039), "$2a$11$0FEqz.l74ktr5HJszKSPheB6eAA4fnLMKeIujFOAF6fW2nkkF.THy" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 12, 26, 16, 3, 43, 173, DateTimeKind.Local).AddTicks(3134), "$2a$11$R9SX9.lmG/teEUtj3prlm.99fEul6QfCq7FWaZUN2qs.Knw2p3dPG" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 3,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 12, 26, 16, 3, 43, 302, DateTimeKind.Local).AddTicks(340), "$2a$11$Jov0pa.rBcxPQSPbOiRJYeGx4FOq3DEBjlecIu3IyqeCn2jR9EwHu" });
        }
    }
}
