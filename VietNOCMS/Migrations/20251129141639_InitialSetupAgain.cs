using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace VietNOCMS.Migrations
{
    /// <inheritdoc />
    public partial class InitialSetupAgain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    CategoryId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CategoryName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: false),
                    Icon = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: true),
                    CreateAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FullName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    PhoneNumber = table.Column<string>(type: "TEXT", maxLength: 15, nullable: true),
                    PasswordHash = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastLoginAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: true),
                    Role = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false, defaultValue: "Student"),
                    Avatar = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Courses",
                columns: table => new
                {
                    CourseId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CourseName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    ShortDescription = table.Column<string>(type: "TEXT", nullable: false),
                    Thumbnail = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Requirements = table.Column<string>(type: "TEXT", nullable: true),
                    WhatYouWillLearn = table.Column<string>(type: "TEXT", nullable: true),
                    DiscountPercent = table.Column<int>(type: "INTEGER", nullable: true),
                    Level = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Language = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    DurationInHours = table.Column<int>(type: "INTEGER", nullable: false),
                    IsPublished = table.Column<bool>(type: "INTEGER", nullable: false),
                    ViewCount = table.Column<int>(type: "INTEGER", nullable: false),
                    EnrollmentCount = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    InstructorId = table.Column<int>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    CategoryId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Courses", x => x.CourseId);
                    table.ForeignKey(
                        name: "FK_Courses_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Courses_Users_InstructorId",
                        column: x => x.InstructorId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InstructorRequests",
                columns: table => new
                {
                    RequestId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Bio = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    CvUrl = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    LinkedInProfile = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    FacebookProfile = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    Status = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    RejectionReason = table.Column<string>(type: "TEXT", nullable: true),
                    RequestedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ReviewedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ReviewedByAdminId = table.Column<int>(type: "INTEGER", nullable: true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InstructorRequests", x => x.RequestId);
                    table.ForeignKey(
                        name: "FK_InstructorRequests_Users_ReviewedByAdminId",
                        column: x => x.ReviewedByAdminId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InstructorRequests_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    NotificationId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 300, nullable: false),
                    Message = table.Column<string>(type: "TEXT", nullable: false),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    IsRead = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    RedirectUrl = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.NotificationId);
                    table.ForeignKey(
                        name: "FK_Notifications_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Chapters",
                columns: table => new
                {
                    ChapterId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ChapterName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    OrderIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CourseId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chapters", x => x.ChapterId);
                    table.ForeignKey(
                        name: "FK_Chapters_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "CourseId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Enrollments",
                columns: table => new
                {
                    EnrollmentId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EnrollmentAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    PaidAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentStatus = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    PaymentMethod = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    CompleteAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ProgressPersent = table.Column<int>(type: "INTEGER", nullable: false),
                    StudentId = table.Column<int>(type: "INTEGER", nullable: false),
                    CourseId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enrollments", x => x.EnrollmentId);
                    table.ForeignKey(
                        name: "FK_Enrollments_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "CourseId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Enrollments_Users_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    ReviewId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Rating = table.Column<int>(type: "INTEGER", nullable: false),
                    Comment = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    CreateAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    StudentId = table.Column<int>(type: "INTEGER", nullable: false),
                    CourseId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.ReviewId);
                    table.ForeignKey(
                        name: "FK_Reviews_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "CourseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reviews_Users_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Lessons",
                columns: table => new
                {
                    LessonId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LessonName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    LessonType = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    VideoUrl = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    Content = table.Column<string>(type: "TEXT", nullable: true),
                    DocumentUrl = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    DurationInMinutes = table.Column<int>(type: "INTEGER", nullable: false),
                    OrderIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    IsFree = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    StartTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    MeetingLink = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    DueDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    MaxScore = table.Column<int>(type: "INTEGER", nullable: true),
                    ChapterId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lessons", x => x.LessonId);
                    table.ForeignKey(
                        name: "FK_Lessons_Chapters_ChapterId",
                        column: x => x.ChapterId,
                        principalTable: "Chapters",
                        principalColumn: "ChapterId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LessonProgresses",
                columns: table => new
                {
                    ProgressId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    IsCompleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    CompleteAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    WatchedDuration = table.Column<int>(type: "INTEGER", nullable: false),
                    LastAccessedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EnrollmentId = table.Column<int>(type: "INTEGER", nullable: false),
                    SubmissionUrl = table.Column<string>(type: "TEXT", nullable: true),
                    SubmittedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Score = table.Column<int>(type: "INTEGER", nullable: true),
                    InstructorFeedback = table.Column<string>(type: "TEXT", nullable: true),
                    LessonId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LessonProgresses", x => x.ProgressId);
                    table.ForeignKey(
                        name: "FK_LessonProgresses_Enrollments_EnrollmentId",
                        column: x => x.EnrollmentId,
                        principalTable: "Enrollments",
                        principalColumn: "EnrollmentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LessonProgresses_Lessons_LessonId",
                        column: x => x.LessonId,
                        principalTable: "Lessons",
                        principalColumn: "LessonId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "CategoryId", "CategoryName", "CreateAt", "Description", "Icon", "IsActive" },
                values: new object[,]
                {
                    { 1, "Lập trình Web", new DateTime(2025, 11, 29, 21, 16, 39, 143, DateTimeKind.Local).AddTicks(1157), "Các khóa học về phát triển web", "bi-code-slash", true },
                    { 2, "Thiết kế UI/UX", new DateTime(2025, 11, 29, 21, 16, 39, 143, DateTimeKind.Local).AddTicks(1171), "Các khóa học về thiết kế giao diện", "bi-palette", true },
                    { 3, "Marketing", new DateTime(2025, 11, 29, 21, 16, 39, 143, DateTimeKind.Local).AddTicks(1173), "Các khóa học về Digital Marketing", "bi-megaphone", true },
                    { 4, "Kinh doanh", new DateTime(2025, 11, 29, 21, 16, 39, 143, DateTimeKind.Local).AddTicks(1174), "Các khóa học về quản trị kinh doanh", "bi-briefcase", true },
                    { 5, "Ngoại ngữ", new DateTime(2025, 11, 29, 21, 16, 39, 143, DateTimeKind.Local).AddTicks(1175), "Các khóa học ngoại ngữ", "bi-translate", true }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "Avatar", "CreatedAt", "Email", "FullName", "IsActive", "LastLoginAt", "PasswordHash", "PhoneNumber", "Role" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2025, 11, 29, 21, 16, 38, 881, DateTimeKind.Local).AddTicks(3833), "admin@vietnocms.com", "Administrator", true, null, "$2a$11$4dCVB6O6RpNEl7jaCaXTgeW755fLAATfH.Nj76McbK..qq2Tm/X4u", "0123456789", "Admin" },
                    { 2, null, new DateTime(2025, 11, 29, 21, 16, 39, 11, DateTimeKind.Local).AddTicks(3151), "instructor@vietnocms.com", "Nguyễn Văn A", true, null, "$2a$11$a6tmdV2bTRjn8aKvbXnCm.PAXcgx5HA6ZYk7z1yIJY4Y0cc2nkE9W", "0987654321", "Instructor" },
                    { 3, null, new DateTime(2025, 11, 29, 21, 16, 39, 143, DateTimeKind.Local).AddTicks(468), "student@vietnocms.com", "Trần Thị B", true, null, "$2a$11$fAv0U4y6DVz/uZxjCf.BuOsmfLwycgsX0UK3WdhulfMsX2oEMDJG6", "0369852147", "Student" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Chapters_CourseId",
                table: "Chapters",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_CategoryId",
                table: "Courses",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_InstructorId",
                table: "Courses",
                column: "InstructorId");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_CourseId",
                table: "Enrollments",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Enrollments_StudentId_CourseId",
                table: "Enrollments",
                columns: new[] { "StudentId", "CourseId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InstructorRequests_ReviewedByAdminId",
                table: "InstructorRequests",
                column: "ReviewedByAdminId");

            migrationBuilder.CreateIndex(
                name: "IX_InstructorRequests_UserId",
                table: "InstructorRequests",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_LessonProgresses_EnrollmentId",
                table: "LessonProgresses",
                column: "EnrollmentId");

            migrationBuilder.CreateIndex(
                name: "IX_LessonProgresses_LessonId",
                table: "LessonProgresses",
                column: "LessonId");

            migrationBuilder.CreateIndex(
                name: "IX_Lessons_ChapterId",
                table: "Lessons",
                column: "ChapterId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId",
                table: "Notifications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_CourseId",
                table: "Reviews",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_StudentId",
                table: "Reviews",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InstructorRequests");

            migrationBuilder.DropTable(
                name: "LessonProgresses");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "Enrollments");

            migrationBuilder.DropTable(
                name: "Lessons");

            migrationBuilder.DropTable(
                name: "Chapters");

            migrationBuilder.DropTable(
                name: "Courses");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
