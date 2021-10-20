using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SANTEGSMS.Migrations
{
    public partial class UpdateSchoolObjMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SuperAdmin",
                keyColumn: "Id",
                keyValue: new Guid("0c135cc1-928d-426e-bb64-1a8ef7c8deec"));

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Schools",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmailAddress",
                table: "Schools",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Motto",
                table: "Schools",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Schools",
                nullable: true);

            migrationBuilder.InsertData(
                table: "SuperAdmin",
                columns: new[] { "Id", "DateCreated", "Email", "FirstName", "LastName", "PasswordHash", "PhoneNumber", "Salt" },
                values: new object[] { new Guid("5cfd8208-343a-4ede-8ec1-477ce8027668"), new DateTime(2021, 10, 20, 0, 46, 56, 992, DateTimeKind.Local).AddTicks(8294), "Ahmedsodiq7@gmail.com", "Super Admin", "Super Admin", "e41a9bc38bebf38522422d08c79385b38aea99dfb1b37d0c543a0812fa91f3fc", "09000990099", "ad846ce0dc3f7587c623ecd78b7fc1c9" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SuperAdmin",
                keyColumn: "Id",
                keyValue: new Guid("5cfd8208-343a-4ede-8ec1-477ce8027668"));

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Schools");

            migrationBuilder.DropColumn(
                name: "EmailAddress",
                table: "Schools");

            migrationBuilder.DropColumn(
                name: "Motto",
                table: "Schools");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Schools");

            migrationBuilder.InsertData(
                table: "SuperAdmin",
                columns: new[] { "Id", "DateCreated", "Email", "FirstName", "LastName", "PasswordHash", "PhoneNumber", "Salt" },
                values: new object[] { new Guid("0c135cc1-928d-426e-bb64-1a8ef7c8deec"), new DateTime(2021, 8, 6, 17, 43, 37, 870, DateTimeKind.Local).AddTicks(25), "SuperAdmin@gmail.com", "Super Admin", "Super Admin", "9ecdd7907be1c6112e3a75c54d82ed84304cf59543ce7a2e0238455423b56d55", "09000990099", "fd3a55350d3cc7507390126965859246" });
        }
    }
}
