using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace School.Migrations
{
    /// <inheritdoc />
    public partial class ChangeSubjectClassDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubjectsClassDetails_AspNetUsers_TeacherId",
                table: "SubjectsClassDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_SubjectsClassDetails_ClassSchool_ClassSchoolId",
                table: "SubjectsClassDetails");

            migrationBuilder.AlterColumn<string>(
                name: "TeacherId",
                table: "SubjectsClassDetails",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ClassSchoolId",
                table: "SubjectsClassDetails",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectsClassDetails_AspNetUsers_TeacherId",
                table: "SubjectsClassDetails",
                column: "TeacherId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectsClassDetails_ClassSchool_ClassSchoolId",
                table: "SubjectsClassDetails",
                column: "ClassSchoolId",
                principalTable: "ClassSchool",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubjectsClassDetails_AspNetUsers_TeacherId",
                table: "SubjectsClassDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_SubjectsClassDetails_ClassSchool_ClassSchoolId",
                table: "SubjectsClassDetails");

            migrationBuilder.AlterColumn<string>(
                name: "TeacherId",
                table: "SubjectsClassDetails",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<int>(
                name: "ClassSchoolId",
                table: "SubjectsClassDetails",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectsClassDetails_AspNetUsers_TeacherId",
                table: "SubjectsClassDetails",
                column: "TeacherId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SubjectsClassDetails_ClassSchool_ClassSchoolId",
                table: "SubjectsClassDetails",
                column: "ClassSchoolId",
                principalTable: "ClassSchool",
                principalColumn: "Id");
        }
    }
}
