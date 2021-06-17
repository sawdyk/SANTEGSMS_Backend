using Microsoft.EntityFrameworkCore.Migrations;

namespace SANTEGSMS.Migrations
{
    public partial class secondMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ReportCardCommentConfig",
                columns: new[] { "Id", "CommentBy" },
                values: new object[,]
                {
                    { 1L, "Examiner" },
                    { 2L, "Class Teacher" },
                    { 3L, "Head Teacher" },
                    { 4L, "Principal" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ReportCardCommentConfig",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "ReportCardCommentConfig",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "ReportCardCommentConfig",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "ReportCardCommentConfig",
                keyColumn: "Id",
                keyValue: 4L);
        }
    }
}
