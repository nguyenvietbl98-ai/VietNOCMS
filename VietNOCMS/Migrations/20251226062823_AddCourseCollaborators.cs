using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietNOCMS.Migrations
{
    /// <inheritdoc />
    public partial class AddCourseCollaborators : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CourseCollaborators",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CourseId = table.Column<int>(type: "INTEGER", nullable: false),
                    CollaboratorId = table.Column<int>(type: "INTEGER", nullable: false),
                    Role = table.Column<string>(type: "TEXT", nullable: false),
                    CanManageContent = table.Column<bool>(type: "INTEGER", nullable: false),
                    CanGrade = table.Column<bool>(type: "INTEGER", nullable: false),
                    InvitedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseCollaborators", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseCollaborators_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "CourseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CourseCollaborators_Users_CollaboratorId",
                        column: x => x.CollaboratorId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 1,
                column: "CreateAt",
                value: new DateTime(2025, 12, 26, 13, 28, 22, 703, DateTimeKind.Local).AddTicks(925));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 2,
                column: "CreateAt",
                value: new DateTime(2025, 12, 26, 13, 28, 22, 703, DateTimeKind.Local).AddTicks(961));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 3,
                column: "CreateAt",
                value: new DateTime(2025, 12, 26, 13, 28, 22, 703, DateTimeKind.Local).AddTicks(963));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 4,
                column: "CreateAt",
                value: new DateTime(2025, 12, 26, 13, 28, 22, 703, DateTimeKind.Local).AddTicks(965));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "CategoryId",
                keyValue: 5,
                column: "CreateAt",
                value: new DateTime(2025, 12, 26, 13, 28, 22, 703, DateTimeKind.Local).AddTicks(968));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 12, 26, 13, 28, 22, 418, DateTimeKind.Local).AddTicks(4799), "$2a$11$mjy.2ty8DuKvGVmQYdYs0uY7C3o9v.kA3z9f3yKSCitIz6xEviK5y" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 12, 26, 13, 28, 22, 557, DateTimeKind.Local).AddTicks(7435), "$2a$11$HOc2k8z1nqVzll/UglNjD.1bvIKvP7ykRWj2iM2xYVoP/kArz2vNK" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 3,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 12, 26, 13, 28, 22, 702, DateTimeKind.Local).AddTicks(9605), "$2a$11$uvCIlU5SywfANlJ09yfh1eYBj7UioqAQuOWuohTyc3ULkwTz53wxG" });

            migrationBuilder.CreateIndex(
                name: "IX_CourseCollaborators_CollaboratorId",
                table: "CourseCollaborators",
                column: "CollaboratorId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseCollaborators_CourseId",
                table: "CourseCollaborators",
                column: "CourseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseCollaborators");

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
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 12, 19, 13, 53, 29, 740, DateTimeKind.Local).AddTicks(6828), "$2a$11$nUQB3oXQKuraUlc5jaMI8.FzmjjzBB5/VihBcYBg49EQD8QwU.4bW" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 12, 19, 13, 53, 29, 874, DateTimeKind.Local).AddTicks(8040), "$2a$11$xuTwP7.FgmajItdV/vQMzOmKjPbOj/4i2EXCkCcc6wGEMdtBsbuXW" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: 3,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 12, 19, 13, 53, 30, 3, DateTimeKind.Local).AddTicks(2580), "$2a$11$f61lkto5pTUWXFo7Pf3UC.scpz4FXwhWAzslADD4KcdNcTz67.Qrm" });
        }
    }
}
