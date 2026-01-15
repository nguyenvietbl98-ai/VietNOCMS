using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietNOCMS.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCollaboratorFeature : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AgreedSalary",
                table: "CourseCollaborator",
                newName: "ResponseAt");

            migrationBuilder.AddColumn<bool>(
                name: "IsRecruiting",
                table: "Courses",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "CourseCollaborator",
                type: "TEXT",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 1,
                column: "CreateAt",
                value: new DateTime(2025, 12, 26, 16, 0, 45, 66, DateTimeKind.Local).AddTicks(3871));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 2,
                column: "CreateAt",
                value: new DateTime(2025, 12, 26, 16, 0, 45, 66, DateTimeKind.Local).AddTicks(3908));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 3,
                column: "CreateAt",
                value: new DateTime(2025, 12, 26, 16, 0, 45, 66, DateTimeKind.Local).AddTicks(3910));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 4,
                column: "CreateAt",
                value: new DateTime(2025, 12, 26, 16, 0, 45, 66, DateTimeKind.Local).AddTicks(3912));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 5,
                column: "CreateAt",
                value: new DateTime(2025, 12, 26, 16, 0, 45, 66, DateTimeKind.Local).AddTicks(3916));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 12, 26, 16, 0, 44, 782, DateTimeKind.Local).AddTicks(7326), "$2a$11$/AeYFLMR3.UbolLqvBn4hudFUZ0p.tzajRnmbOBECEHNcB14SZdo6" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 12, 26, 16, 0, 44, 929, DateTimeKind.Local).AddTicks(97), "$2a$11$FQoApJz/FhOHUCsCQP6wpOZKvjxBEZNtmDEsMoxj8AciucqZkJxUC" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 3,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 12, 26, 16, 0, 45, 66, DateTimeKind.Local).AddTicks(1868), "$2a$11$eQrT91QGyZp27crs1cGFOOX3J.KplmKCcA0v45AgZTSGUMB6542Xe" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRecruiting",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "Message",
                table: "CourseCollaborator");

            migrationBuilder.RenameColumn(
                name: "ResponseAt",
                table: "CourseCollaborator",
                newName: "AgreedSalary");

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
    }
}
