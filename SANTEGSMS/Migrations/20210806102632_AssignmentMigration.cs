using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SANTEGSMS.Migrations
{
    public partial class AssignmentMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SuperAdmin",
                keyColumn: "Id",
                keyValue: new Guid("d51cc453-cb95-4ff9-a9aa-b30bacc18a86"));

            migrationBuilder.AddColumn<bool>(
                name: "IsGraded",
                table: "AssignmentsSubmitted",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "SuperAdmin",
                columns: new[] { "Id", "DateCreated", "Email", "FirstName", "LastName", "PasswordHash", "PhoneNumber", "Salt" },
                values: new object[] { new Guid("e0b79329-3a60-4780-978c-c53b63e496df"), new DateTime(2021, 8, 6, 11, 26, 31, 92, DateTimeKind.Local).AddTicks(7225), "AhmedSodiq7@gmail.com", "Super Admin", "Super Admin", "5d0f5f04dd1a074542a66e5c8a830a6940df68dc69f8749bc7e67ae915d4f00a", "09000990099", "e15ac795e450ef8dbb2c942f130ba264" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SuperAdmin",
                keyColumn: "Id",
                keyValue: new Guid("e0b79329-3a60-4780-978c-c53b63e496df"));

            migrationBuilder.DropColumn(
                name: "IsGraded",
                table: "AssignmentsSubmitted");

            migrationBuilder.InsertData(
                table: "SuperAdmin",
                columns: new[] { "Id", "DateCreated", "Email", "FirstName", "LastName", "PasswordHash", "PhoneNumber", "Salt" },
                values: new object[] { new Guid("d51cc453-cb95-4ff9-a9aa-b30bacc18a86"), new DateTime(2021, 8, 3, 10, 36, 8, 908, DateTimeKind.Local).AddTicks(698), "SuperAdmin@gmail.com", "Super Admin", "Super Admin", "b357ccc6166a97571f4a115ddadaf7a062c6fc4fc0a5d07eef500ec07a8cffe3", "09000990099", "0c7842fd6b7fda453f0742f23a74fd98" });
        }
    }
}
