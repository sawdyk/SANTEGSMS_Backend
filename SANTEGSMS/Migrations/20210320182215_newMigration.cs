using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SANTEGSMS.Migrations
{
    public partial class newMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubCategory",
                table: "ScoreSubCategoryConfig");

            migrationBuilder.AddColumn<string>(
                name: "SubCategoryName",
                table: "ScoreSubCategoryConfig",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Assignments",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true),
                    ObtainableScore = table.Column<long>(nullable: false),
                    FileUrl = table.Column<string>(nullable: true),
                    SubjectId = table.Column<long>(nullable: false),
                    TeacherId = table.Column<Guid>(nullable: false),
                    ClassId = table.Column<long>(nullable: false),
                    ClassGradeId = table.Column<long>(nullable: false),
                    SchoolId = table.Column<long>(nullable: false),
                    CampusId = table.Column<long>(nullable: false),
                    SessionId = table.Column<long>(nullable: false),
                    TermId = table.Column<long>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    DueDate = table.Column<DateTime>(nullable: false),
                    DateUploaded = table.Column<DateTime>(nullable: false),
                    LastDateUpdated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Assignments_SchoolCampus_CampusId",
                        column: x => x.CampusId,
                        principalTable: "SchoolCampus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Assignments_ClassGrades_ClassGradeId",
                        column: x => x.ClassGradeId,
                        principalTable: "ClassGrades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Assignments_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Assignments_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Assignments_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Assignments_SchoolSubjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "SchoolSubjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Assignments_SchoolUsers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "SchoolUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Assignments_Terms_TermId",
                        column: x => x.TermId,
                        principalTable: "Terms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ScoreCategoryConfig",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CategoryId = table.Column<long>(nullable: false),
                    SchoolId = table.Column<long>(nullable: false),
                    CampusId = table.Column<long>(nullable: false),
                    ClassId = table.Column<long>(nullable: false),
                    SessionId = table.Column<long>(nullable: false),
                    TermId = table.Column<long>(nullable: false),
                    Percentage = table.Column<decimal>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScoreCategoryConfig", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScoreCategoryConfig_SchoolCampus_CampusId",
                        column: x => x.CampusId,
                        principalTable: "SchoolCampus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScoreCategoryConfig_ScoreCategory_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "ScoreCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScoreCategoryConfig_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScoreCategoryConfig_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScoreCategoryConfig_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScoreCategoryConfig_Terms_TermId",
                        column: x => x.TermId,
                        principalTable: "Terms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ScoreStatus",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ScoreStatusName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScoreStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Status",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    StatusName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Status", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SubjectNotes",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true),
                    FileUrl = table.Column<string>(nullable: true),
                    SubjectId = table.Column<long>(nullable: false),
                    TeacherId = table.Column<Guid>(nullable: false),
                    ClassId = table.Column<long>(nullable: false),
                    ClassGradeId = table.Column<long>(nullable: false),
                    SchoolId = table.Column<long>(nullable: false),
                    CampusId = table.Column<long>(nullable: false),
                    SessionId = table.Column<long>(nullable: false),
                    TermId = table.Column<long>(nullable: false),
                    DateUploaded = table.Column<DateTime>(nullable: false),
                    LastDateUpdated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectNotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubjectNotes_SchoolCampus_CampusId",
                        column: x => x.CampusId,
                        principalTable: "SchoolCampus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubjectNotes_ClassGrades_ClassGradeId",
                        column: x => x.ClassGradeId,
                        principalTable: "ClassGrades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubjectNotes_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubjectNotes_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubjectNotes_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubjectNotes_SchoolSubjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "SchoolSubjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubjectNotes_SchoolUsers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "SchoolUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubjectNotes_Terms_TermId",
                        column: x => x.TermId,
                        principalTable: "Terms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AssignmentsSubmitted",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AssignmentId = table.Column<long>(nullable: false),
                    StudentId = table.Column<Guid>(nullable: false),
                    ObtainableScore = table.Column<decimal>(nullable: true),
                    ScoreObtained = table.Column<decimal>(nullable: true),
                    FileUrl = table.Column<string>(nullable: true),
                    ClassId = table.Column<long>(nullable: false),
                    ClassGradeId = table.Column<long>(nullable: false),
                    SchoolId = table.Column<long>(nullable: false),
                    CampusId = table.Column<long>(nullable: false),
                    SessionId = table.Column<long>(nullable: false),
                    TermId = table.Column<long>(nullable: false),
                    ScoreStatusId = table.Column<long>(nullable: false),
                    DateSubmitted = table.Column<DateTime>(nullable: true),
                    DateGraded = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssignmentsSubmitted", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AssignmentsSubmitted_Assignments_AssignmentId",
                        column: x => x.AssignmentId,
                        principalTable: "Assignments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssignmentsSubmitted_SchoolCampus_CampusId",
                        column: x => x.CampusId,
                        principalTable: "SchoolCampus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssignmentsSubmitted_ClassGrades_ClassGradeId",
                        column: x => x.ClassGradeId,
                        principalTable: "ClassGrades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssignmentsSubmitted_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssignmentsSubmitted_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssignmentsSubmitted_ScoreStatus_ScoreStatusId",
                        column: x => x.ScoreStatusId,
                        principalTable: "ScoreStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssignmentsSubmitted_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssignmentsSubmitted_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssignmentsSubmitted_Terms_TermId",
                        column: x => x.TermId,
                        principalTable: "Terms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LessonNotes",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Description = table.Column<string>(nullable: true),
                    FileUrl = table.Column<string>(nullable: true),
                    SubjectId = table.Column<long>(nullable: false),
                    TeacherId = table.Column<Guid>(nullable: false),
                    ClassId = table.Column<long>(nullable: false),
                    ClassGradeId = table.Column<long>(nullable: false),
                    SchoolId = table.Column<long>(nullable: false),
                    CampusId = table.Column<long>(nullable: false),
                    SessionId = table.Column<long>(nullable: false),
                    TermId = table.Column<long>(nullable: false),
                    StatusId = table.Column<long>(nullable: false),
                    DateUploaded = table.Column<DateTime>(nullable: false),
                    LastDateUpdated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LessonNotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LessonNotes_SchoolCampus_CampusId",
                        column: x => x.CampusId,
                        principalTable: "SchoolCampus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LessonNotes_ClassGrades_ClassGradeId",
                        column: x => x.ClassGradeId,
                        principalTable: "ClassGrades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LessonNotes_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LessonNotes_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LessonNotes_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LessonNotes_Status_StatusId",
                        column: x => x.StatusId,
                        principalTable: "Status",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LessonNotes_SchoolSubjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "SchoolSubjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LessonNotes_SchoolUsers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "SchoolUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LessonNotes_Terms_TermId",
                        column: x => x.TermId,
                        principalTable: "Terms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ScoreStatus",
                columns: new[] { "Id", "ScoreStatusName" },
                values: new object[,]
                {
                    { 1L, "Passed" },
                    { 2L, "Failed" },
                    { 3L, "Pending" }
                });

            migrationBuilder.InsertData(
                table: "Status",
                columns: new[] { "Id", "StatusName" },
                values: new object[,]
                {
                    { 1L, "Approved" },
                    { 2L, "Pending" },
                    { 3L, "Declined" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_CampusId",
                table: "Assignments",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_ClassGradeId",
                table: "Assignments",
                column: "ClassGradeId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_ClassId",
                table: "Assignments",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_SchoolId",
                table: "Assignments",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_SessionId",
                table: "Assignments",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_SubjectId",
                table: "Assignments",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_TeacherId",
                table: "Assignments",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_Assignments_TermId",
                table: "Assignments",
                column: "TermId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentsSubmitted_AssignmentId",
                table: "AssignmentsSubmitted",
                column: "AssignmentId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentsSubmitted_CampusId",
                table: "AssignmentsSubmitted",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentsSubmitted_ClassGradeId",
                table: "AssignmentsSubmitted",
                column: "ClassGradeId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentsSubmitted_ClassId",
                table: "AssignmentsSubmitted",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentsSubmitted_SchoolId",
                table: "AssignmentsSubmitted",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentsSubmitted_ScoreStatusId",
                table: "AssignmentsSubmitted",
                column: "ScoreStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentsSubmitted_SessionId",
                table: "AssignmentsSubmitted",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentsSubmitted_StudentId",
                table: "AssignmentsSubmitted",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_AssignmentsSubmitted_TermId",
                table: "AssignmentsSubmitted",
                column: "TermId");

            migrationBuilder.CreateIndex(
                name: "IX_LessonNotes_CampusId",
                table: "LessonNotes",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_LessonNotes_ClassGradeId",
                table: "LessonNotes",
                column: "ClassGradeId");

            migrationBuilder.CreateIndex(
                name: "IX_LessonNotes_ClassId",
                table: "LessonNotes",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_LessonNotes_SchoolId",
                table: "LessonNotes",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_LessonNotes_SessionId",
                table: "LessonNotes",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_LessonNotes_StatusId",
                table: "LessonNotes",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_LessonNotes_SubjectId",
                table: "LessonNotes",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_LessonNotes_TeacherId",
                table: "LessonNotes",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_LessonNotes_TermId",
                table: "LessonNotes",
                column: "TermId");

            migrationBuilder.CreateIndex(
                name: "IX_ScoreCategoryConfig_CampusId",
                table: "ScoreCategoryConfig",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_ScoreCategoryConfig_CategoryId",
                table: "ScoreCategoryConfig",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ScoreCategoryConfig_ClassId",
                table: "ScoreCategoryConfig",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_ScoreCategoryConfig_SchoolId",
                table: "ScoreCategoryConfig",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_ScoreCategoryConfig_SessionId",
                table: "ScoreCategoryConfig",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_ScoreCategoryConfig_TermId",
                table: "ScoreCategoryConfig",
                column: "TermId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectNotes_CampusId",
                table: "SubjectNotes",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectNotes_ClassGradeId",
                table: "SubjectNotes",
                column: "ClassGradeId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectNotes_ClassId",
                table: "SubjectNotes",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectNotes_SchoolId",
                table: "SubjectNotes",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectNotes_SessionId",
                table: "SubjectNotes",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectNotes_SubjectId",
                table: "SubjectNotes",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectNotes_TeacherId",
                table: "SubjectNotes",
                column: "TeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectNotes_TermId",
                table: "SubjectNotes",
                column: "TermId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssignmentsSubmitted");

            migrationBuilder.DropTable(
                name: "LessonNotes");

            migrationBuilder.DropTable(
                name: "ScoreCategoryConfig");

            migrationBuilder.DropTable(
                name: "SubjectNotes");

            migrationBuilder.DropTable(
                name: "Assignments");

            migrationBuilder.DropTable(
                name: "ScoreStatus");

            migrationBuilder.DropTable(
                name: "Status");

            migrationBuilder.DropColumn(
                name: "SubCategoryName",
                table: "ScoreSubCategoryConfig");

            migrationBuilder.AddColumn<string>(
                name: "SubCategory",
                table: "ScoreSubCategoryConfig",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);
        }
    }
}
