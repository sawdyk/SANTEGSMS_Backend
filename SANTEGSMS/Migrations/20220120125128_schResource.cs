using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SANTEGSMS.Migrations
{
    public partial class schResource : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SuperAdmin",
                keyColumn: "Id",
                keyValue: new Guid("b6d57aab-65bd-4bb9-bb25-e8f4024b7556"));

            migrationBuilder.AlterColumn<decimal>(
                name: "Percentage",
                table: "ScoreCategoryConfig",
                type: "decimal(60,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalScore",
                table: "ReportCardPosition",
                type: "decimal(60,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AlterColumn<decimal>(
                name: "PercentageScore",
                table: "ReportCardPosition",
                type: "decimal(60,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AlterColumn<decimal>(
                name: "SecondTermTotalScore",
                table: "ReportCardData",
                type: "decimal(60,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AlterColumn<decimal>(
                name: "FirstTermTotalScore",
                table: "ReportCardData",
                type: "decimal(60,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AlterColumn<decimal>(
                name: "CumulativeCA_Score",
                table: "ReportCardData",
                type: "decimal(60,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AlterColumn<decimal>(
                name: "AverageTotalScore",
                table: "ReportCardData",
                type: "decimal(60,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AlterColumn<decimal>(
                name: "MarkObtained",
                table: "ExtraCurricularScores",
                type: "decimal(60,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AlterColumn<decimal>(
                name: "MarkObtained",
                table: "ExaminationScores",
                type: "decimal(60,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AlterColumn<decimal>(
                name: "MarkObtained",
                table: "ContinousAssessmentScores",
                type: "decimal(60,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalScore",
                table: "BroadSheetData",
                type: "decimal(60,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AlterColumn<decimal>(
                name: "PercentageScore",
                table: "BroadSheetData",
                type: "decimal(60,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AlterColumn<decimal>(
                name: "MarkObtained",
                table: "BehavioralScores",
                type: "decimal(60,4)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AlterColumn<decimal>(
                name: "ScoreObtained",
                table: "AssignmentsSubmitted",
                type: "decimal(60,4)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ObtainableScore",
                table: "AssignmentsSubmitted",
                type: "decimal(60,4)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "SchoolResources",
                columns: new[] { "Id", "ResourceLink", "ResourceName" },
                values: new object[] { 1L, "http://161.97.77.250:8080/santegfilesrepository/schooldocuments/others/SubjectBulkUpload.xlsx", "Subject Bulk Upload Template" });

            migrationBuilder.InsertData(
                table: "SchoolResources",
                columns: new[] { "Id", "ResourceLink", "ResourceName" },
                values: new object[] { 2L, "http://161.97.77.250/santegfilesrepository/schooldocuments/others/StudentBulkUpload.xlsx", "Student Bulk Upload Template" });

            migrationBuilder.InsertData(
                table: "SuperAdmin",
                columns: new[] { "Id", "DateCreated", "Email", "FirstName", "LastName", "PasswordHash", "PhoneNumber", "Salt" },
                values: new object[] { new Guid("c350d7a4-79b7-43aa-b3ff-c1f5120b3c07"), new DateTime(2022, 1, 20, 13, 51, 27, 999, DateTimeKind.Local).AddTicks(9701), "Oluagbe1@gmail.com", "Super Admin", "Super Admin", "9977a1e88e7e4d7eb7fc19367eed073ea841ecaef35922d7b8072111c7abecd7", "08024174777", "09f07b2437aa3d0a0b897a395e49cc7c" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SchoolResources",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "SchoolResources",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "SuperAdmin",
                keyColumn: "Id",
                keyValue: new Guid("c350d7a4-79b7-43aa-b3ff-c1f5120b3c07"));

            migrationBuilder.AlterColumn<decimal>(
                name: "Percentage",
                table: "ScoreCategoryConfig",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(60,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalScore",
                table: "ReportCardPosition",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(60,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "PercentageScore",
                table: "ReportCardPosition",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(60,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "SecondTermTotalScore",
                table: "ReportCardData",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(60,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "FirstTermTotalScore",
                table: "ReportCardData",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(60,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "CumulativeCA_Score",
                table: "ReportCardData",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(60,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "AverageTotalScore",
                table: "ReportCardData",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(60,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "MarkObtained",
                table: "ExtraCurricularScores",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(60,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "MarkObtained",
                table: "ExaminationScores",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(60,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "MarkObtained",
                table: "ContinousAssessmentScores",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(60,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalScore",
                table: "BroadSheetData",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(60,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "PercentageScore",
                table: "BroadSheetData",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(60,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "MarkObtained",
                table: "BehavioralScores",
                type: "decimal(65,30)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(60,4)");

            migrationBuilder.AlterColumn<decimal>(
                name: "ScoreObtained",
                table: "AssignmentsSubmitted",
                type: "decimal(65,30)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(60,4)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ObtainableScore",
                table: "AssignmentsSubmitted",
                type: "decimal(65,30)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(60,4)",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "SuperAdmin",
                columns: new[] { "Id", "DateCreated", "Email", "FirstName", "LastName", "PasswordHash", "PhoneNumber", "Salt" },
                values: new object[] { new Guid("b6d57aab-65bd-4bb9-bb25-e8f4024b7556"), new DateTime(2022, 1, 9, 8, 28, 41, 889, DateTimeKind.Local).AddTicks(7653), "Ahmedsodiq7@gmail.com", "Super Admin", "Super Admin", "a01a8a38b94826cc63de0ad78f04e406b5cbbf4d344d683d488ee4e56c28199a", "09000990099", "8bfeb595f29aab2dc37d28b7c3124b6b" });
        }
    }
}
