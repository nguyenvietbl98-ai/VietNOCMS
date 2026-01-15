using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VietNOCMS.Migrations
{
    /// <inheritdoc />
    public partial class AddCourseCollaboratorsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseCollaborator_Courses_CourseId",
                table: "CourseCollaborator");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseCollaborator_Users_CollaboratorId",
                table: "CourseCollaborator");

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

            migrationBuilder.AddPrimaryKey(
                name: "PK_CourseCollaborators",
                table: "CourseCollaborators",
                column: "Id");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseCollaborators_Courses_CourseId",
                table: "CourseCollaborators");

            migrationBuilder.DropForeignKey(
                name: "FK_CourseCollaborators_Users_CollaboratorId",
                table: "CourseCollaborators");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CourseCollaborators",
                table: "CourseCollaborators");

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
    }
}
