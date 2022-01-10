using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SANTEGSMS.Migrations
{
    public partial class broadsheetMigration2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SuperAdmin",
                keyColumn: "Id",
                keyValue: new Guid("531ea07f-9d03-4170-81a5-b524718a97ca"));

            migrationBuilder.AddColumn<long>(
                name: "SubjectId",
                table: "BroadSheetRemark",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.InsertData(
                table: "SuperAdmin",
                columns: new[] { "Id", "DateCreated", "Email", "FirstName", "LastName", "PasswordHash", "PhoneNumber", "Salt" },
                values: new object[] { new Guid("cd35562f-d70b-46e9-86a1-6268cead3343"), new DateTime(2021, 12, 31, 14, 32, 35, 849, DateTimeKind.Local).AddTicks(509), "Ahmedsodiq7@gmail.com", "Super Admin", "Super Admin", "e1958df31e7fa1d1b9f2d8ae47dad2140217ee351ac49439a2620597e9a19c60", "09000990099", "7b8ef6c6d54c68170de8eeb447dd3b74" });

            migrationBuilder.CreateIndex(
                name: "IX_BroadSheetRemark_SubjectId",
                table: "BroadSheetRemark",
                column: "SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_BroadSheetRemark_SchoolSubjects_SubjectId",
                table: "BroadSheetRemark",
                column: "SubjectId",
                principalTable: "SchoolSubjects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BroadSheetRemark_SchoolSubjects_SubjectId",
                table: "BroadSheetRemark");

            migrationBuilder.DropIndex(
                name: "IX_BroadSheetRemark_SubjectId",
                table: "BroadSheetRemark");

            migrationBuilder.DeleteData(
                table: "SuperAdmin",
                keyColumn: "Id",
                keyValue: new Guid("cd35562f-d70b-46e9-86a1-6268cead3343"));

            migrationBuilder.DropColumn(
                name: "SubjectId",
                table: "BroadSheetRemark");

            migrationBuilder.InsertData(
                table: "SuperAdmin",
                columns: new[] { "Id", "DateCreated", "Email", "FirstName", "LastName", "PasswordHash", "PhoneNumber", "Salt" },
                values: new object[] { new Guid("531ea07f-9d03-4170-81a5-b524718a97ca"), new DateTime(2021, 12, 30, 17, 30, 44, 629, DateTimeKind.Local).AddTicks(5205), "Ahmedsodiq7@gmail.com", "Super Admin", "Super Admin", "6511ecef5b67428cc91d66125ba555d8192280d679aa2ab336406a6d1ff0c7c1", "09000990099", "10d95cef0a2674b0f8a53e66cef279fa" });
        }
    }
}
