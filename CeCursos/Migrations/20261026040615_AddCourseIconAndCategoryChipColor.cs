using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CeCursos.Migrations
{
    /// <inheritdoc />
    public partial class AddCourseIconAndCategoryChipColor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CategoryChipColor",
                table: "Courses",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "IconImage",
                table: "Courses",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CategoryChipColor",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "IconImage",
                table: "Courses");
        }
    }
}
