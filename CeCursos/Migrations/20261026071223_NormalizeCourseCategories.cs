using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CeCursos.Migrations
{
    /// <inheritdoc />
    public partial class NormalizeCourseCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    IconImage = table.Column<string>(type: "text", nullable: false),
                    ChipColor = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Courses",
                type: "integer",
                nullable: true);

            migrationBuilder.Sql(
                """
                INSERT INTO "Categories" ("Name", "IconImage", "ChipColor")
                SELECT
                    course."Category",
                    COALESCE(
                        MAX(NULLIF(course."IconImage", '')),
                        CASE course."Category"
                            WHEN 'Tecnologia' THEN '/assets/icon-tecnologia-36px.svg'
                            WHEN 'Gestão' THEN '/assets/icon-gestao-36.svg'
                            WHEN 'Design' THEN '/assets/icon-design-36px.svg'
                            WHEN 'Comunicação' THEN '/assets/icon-comunicacao-36px.svg'
                            WHEN 'Matemática' THEN '/assets/icon-matematica-20px.svg'
                            WHEN 'Ciências da Natureza' THEN '/assets/icon-ciencias-natureza-20px.svg'
                            ELSE ''
                        END),
                    COALESCE(
                        MAX(NULLIF(course."CategoryChipColor", '')),
                        CASE course."Category"
                            WHEN 'Tecnologia' THEN '#e9e2f5'
                            WHEN 'Gestão' THEN '#f6e7ce'
                            WHEN 'Design' THEN '#f5dfea'
                            WHEN 'Comunicação' THEN '#e2edf5'
                            WHEN 'Matemática' THEN '#ffc77f'
                            WHEN 'Ciências da Natureza' THEN '#e8d6f2'
                            ELSE '#fcd19c'
                        END)
                FROM "Courses" AS course
                GROUP BY course."Category";

                UPDATE "Courses" AS course
                SET "CategoryId" = category."Id"
                FROM "Categories" AS category
                WHERE category."Name" = course."Category";
                """);

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "Courses",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Courses_CategoryId",
                table: "Courses",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Name",
                table: "Categories",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Categories_CategoryId",
                table: "Courses",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.DropColumn(
                name: "Category",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "CategoryChipColor",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "IconImage",
                table: "Courses");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Courses",
                type: "text",
                nullable: false,
                defaultValue: "");

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

            migrationBuilder.Sql(
                """
                UPDATE "Courses" AS course
                SET
                    "Category" = category."Name",
                    "CategoryChipColor" = category."ChipColor",
                    "IconImage" = category."IconImage"
                FROM "Categories" AS category
                WHERE category."Id" = course."CategoryId";
                """);

            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Categories_CategoryId",
                table: "Courses");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Courses_CategoryId",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Courses");
        }
    }
}
