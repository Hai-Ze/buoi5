using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace buoi5.Migrations
{
    /// <inheritdoc />
    public partial class Themdulieumau : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
           table: "Grades",
           columns: ["GradeId", "GradeName"],
           values: [1, "21DTHA"]
            );

            migrationBuilder.InsertData(
                table: "Grades",
                columns: ["GradeId", "GradeName"],
                values: [2, "21DTHB"]
            );

            migrationBuilder.InsertData(
                table: "Grades",
                columns: ["GradeId", "GradeName"],
                values: [3, "21DTHC"]
            );

            // Thêm dữ liệu vào bảng Students
            migrationBuilder.InsertData(
                table: "Students",
                columns: ["StudentId", "FirstName", "LastName", "GradeId"],
                values: [1, "Nguyen", "A", 1]
            );

            migrationBuilder.InsertData(
                table: "Students",
                columns: ["StudentId", "FirstName", "LastName", "GradeId"],
                values: [2, "Tran", "B", 2]
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
