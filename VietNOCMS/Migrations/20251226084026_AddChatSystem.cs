using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietNOCMS.Migrations
{
    /// <inheritdoc />
    public partial class AddChatSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseCollaborators_Courses_CourseId",
                table: "CourseCollaborators");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseCollaborators_Users_CollaboratorId",
                table: "CourseCollaborators");

            migrationBuilder.DropTable(
                name: "RecruitmentApplications");

            migrationBuilder.DropTable(
                name: "RecruitmentPosts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CourseCollaborators",
                table: "CourseCollaborators");

            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "Conversations");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Conversations");

            migrationBuilder.RenameTable(
                name: "CourseCollaborators",
                newName: "CourseCollaborator");

            migrationBuilder.RenameIndex(
                name: "IX_CourseCollaborators_CourseId",
                table: "CourseCollaborator",
                newName: "IX_CourseCollaborator_CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_CourseCollaborators_CollaboratorId",
                table: "CourseCollaborator",
                newName: "IX_CourseCollaborator_CollaboratorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CourseCollaborator",
                table: "CourseCollaborator",
                column: "Id");

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

            migrationBuilder.CreateIndex(
                name: "IX_Conversations_User1Id",
                table: "Conversations",
                column: "User1Id");

            migrationBuilder.CreateIndex(
                name: "IX_Conversations_User2Id",
                table: "Conversations",
                column: "User2Id");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMessages_SenderId",
                table: "ChatMessages",
                column: "SenderId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChatMessages_Users_SenderId",
                table: "ChatMessages",
                column: "SenderId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Conversations_Users_User1Id",
                table: "Conversations",
                column: "User1Id",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Conversations_Users_User2Id",
                table: "Conversations",
                column: "User2Id",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseCollaborator_Courses_CourseId",
                table: "CourseCollaborator",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseCollaborator_Users_CollaboratorId",
                table: "CourseCollaborator",
                column: "CollaboratorId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChatMessages_Users_SenderId",
                table: "ChatMessages");

            migrationBuilder.DropForeignKey(
                name: "FK_Conversations_Users_User1Id",
                table: "Conversations");

            migrationBuilder.DropForeignKey(
                name: "FK_Conversations_Users_User2Id",
                table: "Conversations");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseCollaborator_Courses_CourseId",
                table: "CourseCollaborator");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseCollaborator_Users_CollaboratorId",
                table: "CourseCollaborator");

            migrationBuilder.DropIndex(
                name: "IX_Conversations_User1Id",
                table: "Conversations");

            migrationBuilder.DropIndex(
                name: "IX_Conversations_User2Id",
                table: "Conversations");

            migrationBuilder.DropIndex(
                name: "IX_ChatMessages_SenderId",
                table: "ChatMessages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CourseCollaborator",
                table: "CourseCollaborator");

            migrationBuilder.RenameTable(
                name: "CourseCollaborator",
                newName: "CourseCollaborators");

            migrationBuilder.RenameIndex(
                name: "IX_CourseCollaborator_CourseId",
                table: "CourseCollaborators",
                newName: "IX_CourseCollaborators_CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_CourseCollaborator_CollaboratorId",
                table: "CourseCollaborators",
                newName: "IX_CourseCollaborators_CollaboratorId");

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

            migrationBuilder.AddPrimaryKey(
                name: "PK_CourseCollaborators",
                table: "CourseCollaborators",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "RecruitmentPosts",
                columns: table => new
                {
                    PostId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CourseId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    Requirement = table.Column<string>(type: "TEXT", nullable: false),
                    SalaryRange = table.Column<string>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false)
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
                name: "RecruitmentApplications",
                columns: table => new
                {
                    ApplicationId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ApplicantId = table.Column<int>(type: "INTEGER", nullable: false),
                    AppliedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Message = table.Column<string>(type: "TEXT", nullable: false),
                    PostId = table.Column<int>(type: "INTEGER", nullable: false),
                    RecruitmentPostPostId = table.Column<int>(type: "INTEGER", nullable: true),
                    Status = table.Column<string>(type: "TEXT", nullable: false)
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

            migrationBuilder.AddForeignKey(
                name: "FK_CourseCollaborators_Courses_CourseId",
                table: "CourseCollaborators",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseCollaborators_Users_CollaboratorId",
                table: "CourseCollaborators",
                column: "CollaboratorId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
