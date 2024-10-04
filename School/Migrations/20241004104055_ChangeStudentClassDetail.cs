using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace School.Migrations
{
    /// <inheritdoc />
    public partial class ChangeStudentClassDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentsClassDetails_AspNetUsers_StudentId",
                table: "StudentsClassDetails");

            migrationBuilder.AlterColumn<string>(
                name: "StudentId",
                table: "StudentsClassDetails",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentsClassDetails_AspNetUsers_StudentId",
                table: "StudentsClassDetails",
                column: "StudentId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentsClassDetails_AspNetUsers_StudentId",
                table: "StudentsClassDetails");

            migrationBuilder.AlterColumn<string>(
                name: "StudentId",
                table: "StudentsClassDetails",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentsClassDetails_AspNetUsers_StudentId",
                table: "StudentsClassDetails",
                column: "StudentId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
