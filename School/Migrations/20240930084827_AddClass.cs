using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace School.Migrations
{
    /// <inheritdoc />
    public partial class AddClass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Subjects",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateTable(
                name: "ClassSchool",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassSchool", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SubjectsClassDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SubjectId = table.Column<int>(type: "int", nullable: false),
                    TeacherId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ClassSchoolId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectsClassDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubjectsClassDetails_AspNetUsers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SubjectsClassDetails_ClassSchool_ClassSchoolId",
                        column: x => x.ClassSchoolId,
                        principalTable: "ClassSchool",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SubjectsClassDetails_Subjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentsClassDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    SubjectsClassDetailId = table.Column<int>(type: "int", nullable: false),
                    Grade = table.Column<decimal>(type: "decimal(4,2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentsClassDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentsClassDetails_AspNetUsers_StudentId",
                        column: x => x.StudentId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StudentsClassDetails_SubjectsClassDetails_SubjectsClassDetailId",
                        column: x => x.SubjectsClassDetailId,
                        principalTable: "SubjectsClassDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentsClassDetails_StudentId",
                table: "StudentsClassDetails",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentsClassDetails_SubjectsClassDetailId",
                table: "StudentsClassDetails",
                column: "SubjectsClassDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectsClassDetails_ClassSchoolId",
                table: "SubjectsClassDetails",
                column: "ClassSchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectsClassDetails_SubjectId",
                table: "SubjectsClassDetails",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectsClassDetails_TeacherId",
                table: "SubjectsClassDetails",
                column: "TeacherId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentsClassDetails");

            migrationBuilder.DropTable(
                name: "SubjectsClassDetails");

            migrationBuilder.DropTable(
                name: "ClassSchool");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Subjects",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
