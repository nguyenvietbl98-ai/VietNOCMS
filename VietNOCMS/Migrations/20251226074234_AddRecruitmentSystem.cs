using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietNOCMS.Migrations
{
    /// <inheritdoc />
    public partial class AddRecruitmentSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AgreedSalary",
                table: "CourseCollaborators",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "CourseCollaborators",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Conversations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Type = table.Column<string>(type: "TEXT", nullable: false),
                    CourseId = table.Column<int>(type: "INTEGER", nullable: true),
                    User1Id = table.Column<int>(type: "INTEGER", nullable: false),
                    User2Id = table.Column<int>(type: "INTEGER", nullable: false),
                    LastMessageAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conversations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RecruitmentPosts",
                columns: table => new
                {
                    PostId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CourseId = table.Column<int>(type: "INTEGER", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Requirement = table.Column<string>(type: "TEXT", nullable: false),
                    SalaryRange = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecruitmentPosts", x => x.PostId);
                    table.ForeignKey(
                        name: "FK_RecruitmentPosts_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "CourseId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChatMessages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ConversationId = table.Column<int>(type: "INTEGER", nullable: false),
                    SenderId = table.Column<int>(type: "INTEGER", nullable: false),
                    Content = table.Column<string>(type: "TEXT", nullable: false),
                    SentAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsRead = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChatMessages_Conversations_ConversationId",
                        column: x => x.ConversationId,
                        principalTable: "Conversations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RecruitmentApplications",
                columns: table => new
                {
                    ApplicationId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PostId = table.Column<int>(type: "INTEGER", nullable: false),
                    ApplicantId = table.Column<int>(type: "INTEGER", nullable: false),
                    Message = table.Column<string>(type: "TEXT", nullable: false),
                    AppliedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Status = table.Column<string>(type: "TEXT", nullable: false),
                    RecruitmentPostPostId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecruitmentApplications", x => x.ApplicationId);
                    table.ForeignKey(
                        name: "FK_RecruitmentApplications_RecruitmentPosts_RecruitmentPostPostId",
                        column: x => x.RecruitmentPostPostId,
                        principalTable: "RecruitmentPosts",
                        principalColumn: "PostId");
                    table.ForeignKey(
                        name: "FK_RecruitmentApplications_Users_ApplicantId",
                        column: x => x.ApplicantId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_ConversationId",
                table: "ChatMessages",
                column: "ConversationId");

            migrationBuilder.CreateIndex(
                name: "IX_RecruitmentApplications_ApplicantId",
                table: "RecruitmentApplications",
                column: "ApplicantId");

            migrationBuilder.CreateIndex(
                name: "IX_RecruitmentApplications_RecruitmentPostPostId",
                table: "RecruitmentApplications",
                column: "RecruitmentPostPostId");

            migrationBuilder.CreateIndex(
                name: "IX_RecruitmentPosts_CourseId",
                table: "RecruitmentPosts",
                column: "CourseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChatMessages");

            migrationBuilder.DropTable(
                name: "RecruitmentApplications");

            migrationBuilder.DropTable(
                name: "Conversations");

            migrationBuilder.DropTable(
                name: "RecruitmentPosts");

            migrationBuilder.DropColumn(
                name: "AgreedSalary",
                table: "CourseCollaborators");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "CourseCollaborators");

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
        }
    }
}
