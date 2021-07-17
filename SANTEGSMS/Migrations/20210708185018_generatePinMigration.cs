using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SANTEGSMS.Migrations
{
    public partial class generatePinMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "DateDeleted",
                table: "Teachers",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "Teachers",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Teachers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateDeleted",
                table: "SubjectTeachers",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "SubjectTeachers",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "SubjectTeachers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateDeleted",
                table: "SubjectNotes",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "SubjectNotes",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "SubjectNotes",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateDeleted",
                table: "SubjectDepartment",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "SubjectDepartment",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "SubjectDepartment",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateDeleted",
                table: "Students",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "Students",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Students",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateDeleted",
                table: "StudentAttendance",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "StudentAttendance",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "StudentAttendance",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateDeleted",
                table: "Sessions",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "Sessions",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Sessions",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateDeleted",
                table: "ScoreUploadSheetTemplates",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "ScoreUploadSheetTemplates",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ScoreUploadSheetTemplates",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateDeleted",
                table: "ScoreSubCategoryConfig",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "ScoreSubCategoryConfig",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ScoreSubCategoryConfig",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateDeleted",
                table: "ScoreGrading",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "ScoreGrading",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ScoreGrading",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateDeleted",
                table: "ScoreCategoryConfig",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "ScoreCategoryConfig",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ScoreCategoryConfig",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateDeleted",
                table: "ReportCardTemplates",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "ReportCardTemplates",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ReportCardTemplates",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateDeleted",
                table: "ReportCardSignature",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "ReportCardSignature",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ReportCardSignature",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateDeleted",
                table: "ReportCardPosition",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "ReportCardPosition",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ReportCardPosition",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateDeleted",
                table: "ReportCardNextTermBegins",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "ReportCardNextTermBegins",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ReportCardNextTermBegins",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateDeleted",
                table: "ReportCardData",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "ReportCardData",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ReportCardData",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateDeleted",
                table: "ReportCardConfigurationLegendList",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "ReportCardConfigurationLegendList",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ReportCardConfigurationLegendList",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateDeleted",
                table: "ReportCardConfigurationLegend",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "ReportCardConfigurationLegend",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ReportCardConfigurationLegend",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateDeleted",
                table: "ReportCardConfiguration",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "ReportCardConfiguration",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ReportCardConfiguration",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateDeleted",
                table: "ReportCardCommentsList",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "ReportCardCommentsList",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ReportCardCommentsList",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateDeleted",
                table: "ReportCardComments",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "ReportCardComments",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ReportCardComments",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateDeleted",
                table: "Parents",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "Parents",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Parents",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateDeleted",
                table: "LessonNotes",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "LessonNotes",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "LessonNotes",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateDeleted",
                table: "GradeTeachers",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "GradeTeachers",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "GradeTeachers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateDeleted",
                table: "GradeStudents",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "GradeStudents",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "GradeStudents",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateDeleted",
                table: "ExtraCurricularScores",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "ExtraCurricularScores",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ExtraCurricularScores",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateDeleted",
                table: "ExaminationScores",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "ExaminationScores",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ExaminationScores",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateDeleted",
                table: "ContinousAssessmentScores",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "ContinousAssessmentScores",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ContinousAssessmentScores",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateDeleted",
                table: "ClassGrades",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "ClassGrades",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ClassGrades",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateDeleted",
                table: "BehavioralScores",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "BehavioralScores",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "BehavioralScores",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateDeleted",
                table: "AssignmentsSubmitted",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "AssignmentsSubmitted",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AssignmentsSubmitted",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateDeleted",
                table: "Assignments",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "Assignments",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Assignments",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateDeleted",
                table: "Alumni",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "Alumni",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Alumni",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateDeleted",
                table: "AcademicSessions",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "AcademicSessions",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AcademicSessions",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "ReportCardPin",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Pin = table.Column<string>(nullable: true),
                    CreatedById = table.Column<Guid>(nullable: false),
                    ViewedClassId = table.Column<long>(nullable: false),
                    ViewedClassGradeId = table.Column<long>(nullable: false),
                    SchoolId = table.Column<long>(nullable: false),
                    CampusId = table.Column<long>(nullable: false),
                    SessionId = table.Column<long>(nullable: false),
                    TermId = table.Column<long>(nullable: false),
                    IsUsed = table.Column<bool>(nullable: false),
                    IsUsedById = table.Column<string>(nullable: true),
                    NoOfTimesValid = table.Column<long>(nullable: false),
                    NoOfTimesUsed = table.Column<long>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateLastUsed = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportCardPin", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReportCardPin_SchoolCampus_CampusId",
                        column: x => x.CampusId,
                        principalTable: "SchoolCampus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportCardPin_SchoolUsers_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "SchoolUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportCardPin_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportCardPin_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReportCardPin_Terms_TermId",
                        column: x => x.TermId,
                        principalTable: "Terms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReportCardPin_CampusId",
                table: "ReportCardPin",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportCardPin_CreatedById",
                table: "ReportCardPin",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ReportCardPin_SchoolId",
                table: "ReportCardPin",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportCardPin_SessionId",
                table: "ReportCardPin",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportCardPin_TermId",
                table: "ReportCardPin",
                column: "TermId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReportCardPin");

            migrationBuilder.DropColumn(
                name: "DateDeleted",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Teachers");

            migrationBuilder.DropColumn(
                name: "DateDeleted",
                table: "SubjectTeachers");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "SubjectTeachers");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "SubjectTeachers");

            migrationBuilder.DropColumn(
                name: "DateDeleted",
                table: "SubjectNotes");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "SubjectNotes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "SubjectNotes");

            migrationBuilder.DropColumn(
                name: "DateDeleted",
                table: "SubjectDepartment");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "SubjectDepartment");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "SubjectDepartment");

            migrationBuilder.DropColumn(
                name: "DateDeleted",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "DateDeleted",
                table: "StudentAttendance");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "StudentAttendance");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "StudentAttendance");

            migrationBuilder.DropColumn(
                name: "DateDeleted",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Sessions");

            migrationBuilder.DropColumn(
                name: "DateDeleted",
                table: "ScoreUploadSheetTemplates");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "ScoreUploadSheetTemplates");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ScoreUploadSheetTemplates");

            migrationBuilder.DropColumn(
                name: "DateDeleted",
                table: "ScoreSubCategoryConfig");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "ScoreSubCategoryConfig");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ScoreSubCategoryConfig");

            migrationBuilder.DropColumn(
                name: "DateDeleted",
                table: "ScoreGrading");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "ScoreGrading");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ScoreGrading");

            migrationBuilder.DropColumn(
                name: "DateDeleted",
                table: "ScoreCategoryConfig");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "ScoreCategoryConfig");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ScoreCategoryConfig");

            migrationBuilder.DropColumn(
                name: "DateDeleted",
                table: "ReportCardTemplates");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "ReportCardTemplates");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ReportCardTemplates");

            migrationBuilder.DropColumn(
                name: "DateDeleted",
                table: "ReportCardSignature");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "ReportCardSignature");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ReportCardSignature");

            migrationBuilder.DropColumn(
                name: "DateDeleted",
                table: "ReportCardPosition");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "ReportCardPosition");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ReportCardPosition");

            migrationBuilder.DropColumn(
                name: "DateDeleted",
                table: "ReportCardNextTermBegins");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "ReportCardNextTermBegins");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ReportCardNextTermBegins");

            migrationBuilder.DropColumn(
                name: "DateDeleted",
                table: "ReportCardData");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "ReportCardData");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ReportCardData");

            migrationBuilder.DropColumn(
                name: "DateDeleted",
                table: "ReportCardConfigurationLegendList");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "ReportCardConfigurationLegendList");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ReportCardConfigurationLegendList");

            migrationBuilder.DropColumn(
                name: "DateDeleted",
                table: "ReportCardConfigurationLegend");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "ReportCardConfigurationLegend");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ReportCardConfigurationLegend");

            migrationBuilder.DropColumn(
                name: "DateDeleted",
                table: "ReportCardConfiguration");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "ReportCardConfiguration");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ReportCardConfiguration");

            migrationBuilder.DropColumn(
                name: "DateDeleted",
                table: "ReportCardCommentsList");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "ReportCardCommentsList");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ReportCardCommentsList");

            migrationBuilder.DropColumn(
                name: "DateDeleted",
                table: "ReportCardComments");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "ReportCardComments");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ReportCardComments");

            migrationBuilder.DropColumn(
                name: "DateDeleted",
                table: "Parents");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Parents");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Parents");

            migrationBuilder.DropColumn(
                name: "DateDeleted",
                table: "LessonNotes");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "LessonNotes");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "LessonNotes");

            migrationBuilder.DropColumn(
                name: "DateDeleted",
                table: "GradeTeachers");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "GradeTeachers");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "GradeTeachers");

            migrationBuilder.DropColumn(
                name: "DateDeleted",
                table: "GradeStudents");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "GradeStudents");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "GradeStudents");

            migrationBuilder.DropColumn(
                name: "DateDeleted",
                table: "ExtraCurricularScores");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "ExtraCurricularScores");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ExtraCurricularScores");

            migrationBuilder.DropColumn(
                name: "DateDeleted",
                table: "ExaminationScores");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "ExaminationScores");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ExaminationScores");

            migrationBuilder.DropColumn(
                name: "DateDeleted",
                table: "ContinousAssessmentScores");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "ContinousAssessmentScores");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ContinousAssessmentScores");

            migrationBuilder.DropColumn(
                name: "DateDeleted",
                table: "ClassGrades");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "ClassGrades");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ClassGrades");

            migrationBuilder.DropColumn(
                name: "DateDeleted",
                table: "BehavioralScores");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "BehavioralScores");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "BehavioralScores");

            migrationBuilder.DropColumn(
                name: "DateDeleted",
                table: "AssignmentsSubmitted");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "AssignmentsSubmitted");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "AssignmentsSubmitted");

            migrationBuilder.DropColumn(
                name: "DateDeleted",
                table: "Assignments");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Assignments");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Assignments");

            migrationBuilder.DropColumn(
                name: "DateDeleted",
                table: "Alumni");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Alumni");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Alumni");

            migrationBuilder.DropColumn(
                name: "DateDeleted",
                table: "AcademicSessions");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "AcademicSessions");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "AcademicSessions");
        }
    }
}
