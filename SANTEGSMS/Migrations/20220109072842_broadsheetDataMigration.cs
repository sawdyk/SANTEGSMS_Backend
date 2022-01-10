using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SANTEGSMS.Migrations
{
    public partial class broadsheetDataMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SuperAdmin",
                keyColumn: "Id",
                keyValue: new Guid("cd35562f-d70b-46e9-86a1-6268cead3343"));

            migrationBuilder.CreateTable(
                name: "BroadSheetData",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    StudentId = table.Column<Guid>(nullable: false),
                    GenderId = table.Column<long>(nullable: true),
                    SchoolId = table.Column<long>(nullable: false),
                    CampusId = table.Column<long>(nullable: false),
                    ClassId = table.Column<long>(nullable: false),
                    ClassGradeId = table.Column<long>(nullable: false),
                    SessionId = table.Column<long>(nullable: false),
                    TermId = table.Column<long>(nullable: false),
                    NoOfSubjectsComputed = table.Column<long>(nullable: false),
                    TotalScore = table.Column<decimal>(nullable: false),
                    PercentageScore = table.Column<decimal>(nullable: false),
                    Grade = table.Column<string>(nullable: true),
                    Remark = table.Column<string>(nullable: true),
                    DateComputed = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DateDeleted = table.Column<DateTime>(nullable: false),
                    DeletedBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BroadSheetData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BroadSheetData_SchoolCampus_CampusId",
                        column: x => x.CampusId,
                        principalTable: "SchoolCampus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BroadSheetData_ClassGrades_ClassGradeId",
                        column: x => x.ClassGradeId,
                        principalTable: "ClassGrades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BroadSheetData_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BroadSheetData_Gender_GenderId",
                        column: x => x.GenderId,
                        principalTable: "Gender",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BroadSheetData_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BroadSheetData_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BroadSheetData_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BroadSheetData_Terms_TermId",
                        column: x => x.TermId,
                        principalTable: "Terms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "SuperAdmin",
                columns: new[] { "Id", "DateCreated", "Email", "FirstName", "LastName", "PasswordHash", "PhoneNumber", "Salt" },
                values: new object[] { new Guid("b6d57aab-65bd-4bb9-bb25-e8f4024b7556"), new DateTime(2022, 1, 9, 8, 28, 41, 889, DateTimeKind.Local).AddTicks(7653), "Ahmedsodiq7@gmail.com", "Super Admin", "Super Admin", "a01a8a38b94826cc63de0ad78f04e406b5cbbf4d344d683d488ee4e56c28199a", "09000990099", "8bfeb595f29aab2dc37d28b7c3124b6b" });

            migrationBuilder.CreateIndex(
                name: "IX_BroadSheetData_CampusId",
                table: "BroadSheetData",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_BroadSheetData_ClassGradeId",
                table: "BroadSheetData",
                column: "ClassGradeId");

            migrationBuilder.CreateIndex(
                name: "IX_BroadSheetData_ClassId",
                table: "BroadSheetData",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_BroadSheetData_GenderId",
                table: "BroadSheetData",
                column: "GenderId");

            migrationBuilder.CreateIndex(
                name: "IX_BroadSheetData_SchoolId",
                table: "BroadSheetData",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_BroadSheetData_SessionId",
                table: "BroadSheetData",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_BroadSheetData_StudentId",
                table: "BroadSheetData",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_BroadSheetData_TermId",
                table: "BroadSheetData",
                column: "TermId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BroadSheetData");

            migrationBuilder.DeleteData(
                table: "SuperAdmin",
                keyColumn: "Id",
                keyValue: new Guid("b6d57aab-65bd-4bb9-bb25-e8f4024b7556"));

            migrationBuilder.InsertData(
                table: "SuperAdmin",
                columns: new[] { "Id", "DateCreated", "Email", "FirstName", "LastName", "PasswordHash", "PhoneNumber", "Salt" },
                values: new object[] { new Guid("cd35562f-d70b-46e9-86a1-6268cead3343"), new DateTime(2021, 12, 31, 14, 32, 35, 849, DateTimeKind.Local).AddTicks(509), "Ahmedsodiq7@gmail.com", "Super Admin", "Super Admin", "e1958df31e7fa1d1b9f2d8ae47dad2140217ee351ac49439a2620597e9a19c60", "09000990099", "7b8ef6c6d54c68170de8eeb447dd3b74" });
        }
    }
}
