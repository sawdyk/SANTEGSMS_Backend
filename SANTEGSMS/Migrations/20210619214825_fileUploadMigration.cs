using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SANTEGSMS.Migrations
{
    public partial class fileUploadMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppTypes",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AppName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FolderTypes",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AppId = table.Column<long>(nullable: false),
                    FolderName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FolderTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FolderTypes_AppTypes_AppId",
                        column: x => x.AppId,
                        principalTable: "AppTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AppTypes",
                columns: new[] { "Id", "AppName" },
                values: new object[] { 1L, "SchoolApp" });

            migrationBuilder.InsertData(
                table: "FolderTypes",
                columns: new[] { "Id", "AppId", "FolderName" },
                values: new object[,]
                {
                    { 1L, 1L, "Assignments" },
                    { 2L, 1L, "LessonNotes" },
                    { 3L, 1L, "SchoolLogos" },
                    { 4L, 1L, "Signatures" },
                    { 5L, 1L, "StudentPassports" },
                    { 6L, 1L, "SubjectNotes" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_FolderTypes_AppId",
                table: "FolderTypes",
                column: "AppId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FolderTypes");

            migrationBuilder.DropTable(
                name: "AppTypes");
        }
    }
}
