using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SANTEGSMS.Migrations
{
    public partial class broadsheetMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SuperAdmin",
                keyColumn: "Id",
                keyValue: new Guid("448552a4-7c55-4150-b0ae-c41d1c97c631"));

            migrationBuilder.CreateTable(
                name: "BroadSheetGrading",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SchoolId = table.Column<long>(nullable: false),
                    CampusId = table.Column<long>(nullable: false),
                    SessionId = table.Column<long>(nullable: false),
                    LowestRange = table.Column<long>(nullable: false),
                    HighestRange = table.Column<long>(nullable: false),
                    Grade = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    DateDeleted = table.Column<DateTime>(nullable: false),
                    DeletedBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BroadSheetGrading", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BroadSheetGrading_SchoolCampus_CampusId",
                        column: x => x.CampusId,
                        principalTable: "SchoolCampus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BroadSheetGrading_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BroadSheetGrading_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BroadSheetRemark",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SchoolId = table.Column<long>(nullable: false),
                    CampusId = table.Column<long>(nullable: false),
                    ClassId = table.Column<long>(nullable: false),
                    Mandatory = table.Column<bool>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BroadSheetRemark", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BroadSheetRemark_SchoolCampus_CampusId",
                        column: x => x.CampusId,
                        principalTable: "SchoolCampus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BroadSheetRemark_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BroadSheetRemark_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "SuperAdmin",
                columns: new[] { "Id", "DateCreated", "Email", "FirstName", "LastName", "PasswordHash", "PhoneNumber", "Salt" },
                values: new object[] { new Guid("531ea07f-9d03-4170-81a5-b524718a97ca"), new DateTime(2021, 12, 30, 17, 30, 44, 629, DateTimeKind.Local).AddTicks(5205), "Ahmedsodiq7@gmail.com", "Super Admin", "Super Admin", "6511ecef5b67428cc91d66125ba555d8192280d679aa2ab336406a6d1ff0c7c1", "09000990099", "10d95cef0a2674b0f8a53e66cef279fa" });

            migrationBuilder.CreateIndex(
                name: "IX_BroadSheetGrading_CampusId",
                table: "BroadSheetGrading",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_BroadSheetGrading_SchoolId",
                table: "BroadSheetGrading",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_BroadSheetGrading_SessionId",
                table: "BroadSheetGrading",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_BroadSheetRemark_CampusId",
                table: "BroadSheetRemark",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_BroadSheetRemark_ClassId",
                table: "BroadSheetRemark",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_BroadSheetRemark_SchoolId",
                table: "BroadSheetRemark",
                column: "SchoolId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BroadSheetGrading");

            migrationBuilder.DropTable(
                name: "BroadSheetRemark");

            migrationBuilder.DeleteData(
                table: "SuperAdmin",
                keyColumn: "Id",
                keyValue: new Guid("531ea07f-9d03-4170-81a5-b524718a97ca"));

            migrationBuilder.InsertData(
                table: "SuperAdmin",
                columns: new[] { "Id", "DateCreated", "Email", "FirstName", "LastName", "PasswordHash", "PhoneNumber", "Salt" },
                values: new object[] { new Guid("448552a4-7c55-4150-b0ae-c41d1c97c631"), new DateTime(2021, 11, 12, 23, 14, 17, 215, DateTimeKind.Local).AddTicks(1284), "Ahmedsodiq7@gmail.com", "Super Admin", "Super Admin", "f372598eb90dda1c52b78cd831ec942a3953a756ac0d8c0a67d5a70ae82d4f14", "09000990099", "d4d82e52cef840f28b4de3cd21caa1f8" });
        }
    }
}
