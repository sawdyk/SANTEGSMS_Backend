using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SANTEGSMS.Migrations
{
    public partial class ActivityLogMigrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SuperAdmin",
                keyColumn: "Id",
                keyValue: new Guid("e0b79329-3a60-4780-978c-c53b63e496df"));

            migrationBuilder.CreateTable(
                name: "ActivityLogs",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    Action = table.Column<string>(nullable: true),
                    Message = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ActionDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActivityLogs", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "SuperAdmin",
                columns: new[] { "Id", "DateCreated", "Email", "FirstName", "LastName", "PasswordHash", "PhoneNumber", "Salt" },
                values: new object[] { new Guid("0c135cc1-928d-426e-bb64-1a8ef7c8deec"), new DateTime(2021, 8, 6, 17, 43, 37, 870, DateTimeKind.Local).AddTicks(25), "SuperAdmin@gmail.com", "Super Admin", "Super Admin", "9ecdd7907be1c6112e3a75c54d82ed84304cf59543ce7a2e0238455423b56d55", "09000990099", "fd3a55350d3cc7507390126965859246" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActivityLogs");

            migrationBuilder.DeleteData(
                table: "SuperAdmin",
                keyColumn: "Id",
                keyValue: new Guid("0c135cc1-928d-426e-bb64-1a8ef7c8deec"));

            migrationBuilder.InsertData(
                table: "SuperAdmin",
                columns: new[] { "Id", "DateCreated", "Email", "FirstName", "LastName", "PasswordHash", "PhoneNumber", "Salt" },
                values: new object[] { new Guid("e0b79329-3a60-4780-978c-c53b63e496df"), new DateTime(2021, 8, 6, 11, 26, 31, 92, DateTimeKind.Local).AddTicks(7225), "SuperAdmin@gmail.com", "Super Admin", "Super Admin", "5d0f5f04dd1a074542a66e5c8a830a6940df68dc69f8749bc7e67ae915d4f00a", "09000990099", "e15ac795e450ef8dbb2c942f130ba264" });
        }
    }
}
