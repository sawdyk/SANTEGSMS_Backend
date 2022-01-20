using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SANTEGSMS.Migrations
{
    public partial class schResource2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.InsertData(
                table: "SuperAdmin",
                columns: new[] { "Id", "DateCreated", "Email", "FirstName", "LastName", "PasswordHash", "PhoneNumber", "Salt" },
                values: new object[] { new Guid("626f2482-9e48-4aa8-949e-6c22b240855b"), new DateTime(2022, 1, 20, 13, 52, 52, 26, DateTimeKind.Local).AddTicks(1611), "Oluagbe1@gmail.com", "Super Admin", "Super Admin", "5565b9a7f1c679be17b3d741eb326525f58e4f3fd520ff6809bbaded262adacd", "08024174777", "19a9bcf9fe22f6cce5c0949ffc32c302" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SuperAdmin",
                keyColumn: "Id",
                keyValue: new Guid("626f2482-9e48-4aa8-949e-6c22b240855b"));

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
    }
}
