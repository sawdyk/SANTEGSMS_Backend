using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SANTEGSMS.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AttendancePeriod",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AttendancePeriodName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AttendancePeriod", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClassAlumni",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Category = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassAlumni", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DistrictAdministrators",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    Salt = table.Column<string>(nullable: true),
                    PasswordHash = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    LastLoginDate = table.Column<DateTime>(nullable: false),
                    LastPasswordChangedDate = table.Column<DateTime>(nullable: false),
                    LastUpdatedDate = table.Column<DateTime>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DistrictAdministrators", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmailConfirmationCodes",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<Guid>(nullable: false),
                    Code = table.Column<string>(nullable: true),
                    DateGenerated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailConfirmationCodes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ErrorLog",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ErrorMessage = table.Column<string>(nullable: true),
                    ErrorSource = table.Column<string>(nullable: true),
                    ErrorStackTrace = table.Column<string>(nullable: true),
                    ErrorDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ErrorLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Gender",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    GenderName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gender", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SchoolRoles",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    RoleName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SchoolType",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SchoolTypeName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ScoreCategory",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CategoryName = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScoreCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "States",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    StateName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_States", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Terms",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    TermName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Terms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LocalGovt",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    StateId = table.Column<long>(nullable: true),
                    LocalGovtName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocalGovt", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LocalGovt_States_StateId",
                        column: x => x.StateId,
                        principalTable: "States",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "District",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    StateId = table.Column<long>(nullable: true),
                    LocalGovtId = table.Column<long>(nullable: true),
                    DistrictAdminId = table.Column<Guid>(nullable: true),
                    DistrictName = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_District", x => x.Id);
                    table.ForeignKey(
                        name: "FK_District_DistrictAdministrators_DistrictAdminId",
                        column: x => x.DistrictAdminId,
                        principalTable: "DistrictAdministrators",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_District_LocalGovt_LocalGovtId",
                        column: x => x.LocalGovtId,
                        principalTable: "LocalGovt",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_District_States_StateId",
                        column: x => x.StateId,
                        principalTable: "States",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Schools",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SchoolName = table.Column<string>(nullable: true),
                    SchoolCode = table.Column<string>(nullable: true),
                    SchoolLogoUrl = table.Column<string>(nullable: true),
                    SchoolTypeId = table.Column<long>(nullable: true),
                    StateId = table.Column<long>(nullable: true),
                    LocalGovtId = table.Column<long>(nullable: true),
                    DistrictId = table.Column<long>(nullable: true),
                    IsVerified = table.Column<bool>(nullable: false),
                    IsApproved = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateUpdated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Schools", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Schools_District_DistrictId",
                        column: x => x.DistrictId,
                        principalTable: "District",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Schools_LocalGovt_LocalGovtId",
                        column: x => x.LocalGovtId,
                        principalTable: "LocalGovt",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Schools_SchoolType_SchoolTypeId",
                        column: x => x.SchoolTypeId,
                        principalTable: "SchoolType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Schools_States_StateId",
                        column: x => x.StateId,
                        principalTable: "States",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SchoolCampus",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SchoolId = table.Column<long>(nullable: true),
                    CampusName = table.Column<string>(nullable: true),
                    CampusAddress = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolCampus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SchoolCampus_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sessions",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SessionName = table.Column<string>(nullable: true),
                    SchoolId = table.Column<long>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sessions_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Classes",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ClassName = table.Column<string>(nullable: true),
                    SchoolId = table.Column<long>(nullable: true),
                    CampusId = table.Column<long>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Classes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Classes_SchoolCampus_CampusId",
                        column: x => x.CampusId,
                        principalTable: "SchoolCampus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Classes_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Parents",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: true),
                    GenderId = table.Column<long>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    Salt = table.Column<string>(nullable: true),
                    PasswordHash = table.Column<string>(nullable: true),
                    SchoolId = table.Column<long>(nullable: true),
                    CampusId = table.Column<long>(nullable: true),
                    Nationality = table.Column<string>(nullable: true),
                    State = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    HomeAddress = table.Column<string>(nullable: true),
                    Occupation = table.Column<string>(nullable: true),
                    StateOfOrigin = table.Column<string>(nullable: true),
                    LocalGovt = table.Column<string>(nullable: true),
                    Religion = table.Column<string>(nullable: true),
                    hasChild = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    LastLoginDate = table.Column<DateTime>(nullable: false),
                    LastPasswordChangedDate = table.Column<DateTime>(nullable: false),
                    LastUpdatedDate = table.Column<DateTime>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Parents_SchoolCampus_CampusId",
                        column: x => x.CampusId,
                        principalTable: "SchoolCampus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Parents_Gender_GenderId",
                        column: x => x.GenderId,
                        principalTable: "Gender",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Parents_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SchoolUsers",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    Salt = table.Column<string>(nullable: true),
                    PasswordHash = table.Column<string>(nullable: true),
                    SchoolId = table.Column<long>(nullable: true),
                    CampusId = table.Column<long>(nullable: true),
                    IsActive = table.Column<bool>(nullable: false),
                    LastLoginDate = table.Column<DateTime>(nullable: false),
                    LastPasswordChangedDate = table.Column<DateTime>(nullable: false),
                    LastUpdatedDate = table.Column<DateTime>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SchoolUsers_SchoolCampus_CampusId",
                        column: x => x.CampusId,
                        principalTable: "SchoolCampus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SchoolUsers_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    MiddleName = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: true),
                    AdmissionNumber = table.Column<string>(nullable: true),
                    GenderId = table.Column<long>(nullable: true),
                    StaffStatus = table.Column<long>(nullable: false),
                    DateOfBirth = table.Column<DateTime>(nullable: true),
                    YearOfAdmission = table.Column<string>(nullable: true),
                    StateOfOrigin = table.Column<string>(nullable: true),
                    LocalGovt = table.Column<string>(nullable: true),
                    Religion = table.Column<string>(nullable: true),
                    HomeAddress = table.Column<string>(nullable: true),
                    City = table.Column<string>(nullable: true),
                    State = table.Column<string>(nullable: true),
                    ProfilePictureUrl = table.Column<string>(nullable: true),
                    Status = table.Column<string>(nullable: true),
                    StudentStatus = table.Column<string>(nullable: true),
                    Salt = table.Column<string>(nullable: true),
                    PasswordHash = table.Column<string>(nullable: true),
                    SchoolId = table.Column<long>(nullable: true),
                    CampusId = table.Column<long>(nullable: true),
                    IsAssignedToClass = table.Column<bool>(nullable: false),
                    hasParent = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    LastLoginDate = table.Column<DateTime>(nullable: false),
                    LastPasswordChangedDate = table.Column<DateTime>(nullable: false),
                    LastUpdatedDate = table.Column<DateTime>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Students_SchoolCampus_CampusId",
                        column: x => x.CampusId,
                        principalTable: "SchoolCampus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Students_Gender_GenderId",
                        column: x => x.GenderId,
                        principalTable: "Gender",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Students_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ClassGrades",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ClassId = table.Column<long>(nullable: true),
                    SchoolId = table.Column<long>(nullable: true),
                    CampusId = table.Column<long>(nullable: true),
                    GradeName = table.Column<string>(nullable: true),
                    isAssignedToTeacher = table.Column<bool>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClassGrades", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClassGrades_SchoolCampus_CampusId",
                        column: x => x.CampusId,
                        principalTable: "SchoolCampus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClassGrades_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ClassGrades_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ScoreGrading",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SchoolId = table.Column<long>(nullable: false),
                    CampusId = table.Column<long>(nullable: false),
                    ClassId = table.Column<long>(nullable: false),
                    LowestRange = table.Column<long>(nullable: false),
                    HighestRange = table.Column<long>(nullable: false),
                    Grade = table.Column<string>(nullable: true),
                    Remark = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScoreGrading", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScoreGrading_SchoolCampus_CampusId",
                        column: x => x.CampusId,
                        principalTable: "SchoolCampus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScoreGrading_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScoreGrading_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ScoreSubCategoryConfig",
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
                    SubCategory = table.Column<string>(nullable: true),
                    ScoreObtainable = table.Column<long>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScoreSubCategoryConfig", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ScoreSubCategoryConfig_SchoolCampus_CampusId",
                        column: x => x.CampusId,
                        principalTable: "SchoolCampus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScoreSubCategoryConfig_ScoreCategory_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "ScoreCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScoreSubCategoryConfig_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScoreSubCategoryConfig_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScoreSubCategoryConfig_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ScoreSubCategoryConfig_Terms_TermId",
                        column: x => x.TermId,
                        principalTable: "Terms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubjectDepartment",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SchoolId = table.Column<long>(nullable: false),
                    CampusId = table.Column<long>(nullable: false),
                    ClassId = table.Column<long>(nullable: false),
                    DepartmentName = table.Column<string>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectDepartment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubjectDepartment_SchoolCampus_CampusId",
                        column: x => x.CampusId,
                        principalTable: "SchoolCampus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubjectDepartment_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubjectDepartment_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AcademicSessions",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SessionId = table.Column<long>(nullable: false),
                    TermId = table.Column<long>(nullable: false),
                    SchoolId = table.Column<long>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    DateStart = table.Column<DateTime>(nullable: false),
                    DateEnd = table.Column<DateTime>(nullable: false),
                    IsApproved = table.Column<bool>(nullable: false),
                    IsCurrent = table.Column<bool>(nullable: false),
                    IsClosed = table.Column<bool>(nullable: false),
                    IsOpened = table.Column<bool>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AcademicSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AcademicSessions_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AcademicSessions_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AcademicSessions_Terms_TermId",
                        column: x => x.TermId,
                        principalTable: "Terms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AcademicSessions_SchoolUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "SchoolUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SchoolUserRoles",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UserId = table.Column<Guid>(nullable: true),
                    RoleId = table.Column<long>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolUserRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SchoolUserRoles_SchoolRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "SchoolRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SchoolUserRoles_SchoolUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "SchoolUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Teachers",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SchoolUserId = table.Column<Guid>(nullable: false),
                    SchoolId = table.Column<long>(nullable: false),
                    CampusId = table.Column<long>(nullable: false),
                    IsAssignedToClass = table.Column<bool>(nullable: false),
                    IsAssignedSubjects = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    LastLoginDate = table.Column<DateTime>(nullable: false),
                    LastPasswordChangedDate = table.Column<DateTime>(nullable: false),
                    LastUpdatedDate = table.Column<DateTime>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teachers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Teachers_SchoolCampus_CampusId",
                        column: x => x.CampusId,
                        principalTable: "SchoolCampus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Teachers_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Teachers_SchoolUsers_SchoolUserId",
                        column: x => x.SchoolUserId,
                        principalTable: "SchoolUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ParentsStudentsMap",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ParentId = table.Column<Guid>(nullable: false),
                    StudentId = table.Column<Guid>(nullable: false),
                    SchoolId = table.Column<long>(nullable: false),
                    CampusId = table.Column<long>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ParentsStudentsMap", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ParentsStudentsMap_SchoolCampus_CampusId",
                        column: x => x.CampusId,
                        principalTable: "SchoolCampus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentsStudentsMap_Parents_ParentId",
                        column: x => x.ParentId,
                        principalTable: "Parents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentsStudentsMap_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ParentsStudentsMap_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentDuplicates",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    NewStudentFullName = table.Column<string>(nullable: true),
                    ExistingStudentId = table.Column<Guid>(nullable: false),
                    SchoolId = table.Column<long>(nullable: false),
                    CampusId = table.Column<long>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentDuplicates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentDuplicates_SchoolCampus_CampusId",
                        column: x => x.CampusId,
                        principalTable: "SchoolCampus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentDuplicates_Students_ExistingStudentId",
                        column: x => x.ExistingStudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentDuplicates_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Alumni",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SchoolId = table.Column<long>(nullable: false),
                    CampusId = table.Column<long>(nullable: false),
                    ClassId = table.Column<long>(nullable: false),
                    ClassGradeId = table.Column<long>(nullable: false),
                    SessionId = table.Column<long>(nullable: false),
                    StudentId = table.Column<Guid>(nullable: false),
                    GradeTeacherId = table.Column<Guid>(nullable: false),
                    StatusId = table.Column<long>(nullable: false),
                    DateGraduated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Alumni", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Alumni_SchoolCampus_CampusId",
                        column: x => x.CampusId,
                        principalTable: "SchoolCampus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Alumni_ClassGrades_ClassGradeId",
                        column: x => x.ClassGradeId,
                        principalTable: "ClassGrades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Alumni_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Alumni_SchoolUsers_GradeTeacherId",
                        column: x => x.GradeTeacherId,
                        principalTable: "SchoolUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Alumni_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Alumni_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Alumni_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GradeStudents",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    StudentId = table.Column<Guid>(nullable: false),
                    ClassId = table.Column<long>(nullable: false),
                    ClassGradeId = table.Column<long>(nullable: false),
                    SchoolId = table.Column<long>(nullable: false),
                    CampusId = table.Column<long>(nullable: false),
                    SessionId = table.Column<long>(nullable: false),
                    HasGraduated = table.Column<bool>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GradeStudents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GradeStudents_SchoolCampus_CampusId",
                        column: x => x.CampusId,
                        principalTable: "SchoolCampus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GradeStudents_ClassGrades_ClassGradeId",
                        column: x => x.ClassGradeId,
                        principalTable: "ClassGrades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GradeStudents_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GradeStudents_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GradeStudents_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GradeStudents_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GradeTeachers",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SchoolUserId = table.Column<Guid>(nullable: false),
                    ClassId = table.Column<long>(nullable: false),
                    ClassGradeId = table.Column<long>(nullable: false),
                    SchoolId = table.Column<long>(nullable: false),
                    CampusId = table.Column<long>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GradeTeachers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GradeTeachers_SchoolCampus_CampusId",
                        column: x => x.CampusId,
                        principalTable: "SchoolCampus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GradeTeachers_ClassGrades_ClassGradeId",
                        column: x => x.ClassGradeId,
                        principalTable: "ClassGrades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GradeTeachers_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GradeTeachers_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GradeTeachers_SchoolUsers_SchoolUserId",
                        column: x => x.SchoolUserId,
                        principalTable: "SchoolUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentAttendance",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SchoolId = table.Column<long>(nullable: false),
                    CampusId = table.Column<long>(nullable: false),
                    AdmissionNumber = table.Column<string>(nullable: true),
                    StudentId = table.Column<Guid>(nullable: false),
                    TermId = table.Column<long>(nullable: false),
                    SessionId = table.Column<long>(nullable: false),
                    ClassId = table.Column<long>(nullable: false),
                    ClassGradeId = table.Column<long>(nullable: false),
                    PresentAbsent = table.Column<long>(nullable: false),
                    AttendancePeriodId = table.Column<long>(nullable: false),
                    AttendancePeriodIdMorning = table.Column<long>(nullable: false),
                    AttendancePeriodIdAfternoon = table.Column<long>(nullable: false),
                    AttendanceDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentAttendance", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentAttendance_AttendancePeriod_AttendancePeriodId",
                        column: x => x.AttendancePeriodId,
                        principalTable: "AttendancePeriod",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentAttendance_SchoolCampus_CampusId",
                        column: x => x.CampusId,
                        principalTable: "SchoolCampus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentAttendance_ClassGrades_ClassGradeId",
                        column: x => x.ClassGradeId,
                        principalTable: "ClassGrades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentAttendance_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentAttendance_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentAttendance_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentAttendance_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentAttendance_Terms_TermId",
                        column: x => x.TermId,
                        principalTable: "Terms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SchoolSubjects",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    ClassId = table.Column<long>(nullable: false),
                    SchoolId = table.Column<long>(nullable: false),
                    CampusId = table.Column<long>(nullable: false),
                    SubjectName = table.Column<string>(nullable: true),
                    SubjectCode = table.Column<string>(nullable: true),
                    MaximumScore = table.Column<long>(nullable: false),
                    ReportCardOrder = table.Column<long>(nullable: false),
                    DepartmentId = table.Column<long>(nullable: true),
                    IsAssignedToClass = table.Column<bool>(nullable: false),
                    IsAssignedToTeacher = table.Column<bool>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchoolSubjects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SchoolSubjects_SchoolCampus_CampusId",
                        column: x => x.CampusId,
                        principalTable: "SchoolCampus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SchoolSubjects_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SchoolSubjects_SubjectDepartment_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "SubjectDepartment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SchoolSubjects_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubjectTeachers",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    SchoolUserId = table.Column<Guid>(nullable: false),
                    SubjectId = table.Column<long>(nullable: true),
                    ClassId = table.Column<long>(nullable: false),
                    ClassGradeId = table.Column<long>(nullable: false),
                    SchoolId = table.Column<long>(nullable: false),
                    CampusId = table.Column<long>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubjectTeachers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubjectTeachers_SchoolCampus_CampusId",
                        column: x => x.CampusId,
                        principalTable: "SchoolCampus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubjectTeachers_ClassGrades_ClassGradeId",
                        column: x => x.ClassGradeId,
                        principalTable: "ClassGrades",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubjectTeachers_Classes_ClassId",
                        column: x => x.ClassId,
                        principalTable: "Classes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubjectTeachers_Schools_SchoolId",
                        column: x => x.SchoolId,
                        principalTable: "Schools",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubjectTeachers_SchoolUsers_SchoolUserId",
                        column: x => x.SchoolUserId,
                        principalTable: "SchoolUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SubjectTeachers_SchoolSubjects_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "SchoolSubjects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "AttendancePeriod",
                columns: new[] { "Id", "AttendancePeriodName" },
                values: new object[,]
                {
                    { 1L, "Morning" },
                    { 2L, "Afternoon" },
                    { 3L, "Both" }
                });

            migrationBuilder.InsertData(
                table: "ClassAlumni",
                columns: new[] { "Id", "Category" },
                values: new object[,]
                {
                    { 1L, "Alumni" },
                    { 2L, "Class" }
                });

            migrationBuilder.InsertData(
                table: "Gender",
                columns: new[] { "Id", "GenderName" },
                values: new object[,]
                {
                    { 1L, "Male" },
                    { 2L, "Female" }
                });

            migrationBuilder.InsertData(
                table: "SchoolRoles",
                columns: new[] { "Id", "RoleName" },
                values: new object[,]
                {
                    { 4L, "Subject Teacher" },
                    { 3L, "Class Teacher" },
                    { 1L, "Super Administrator" },
                    { 2L, "Administrator" }
                });

            migrationBuilder.InsertData(
                table: "SchoolType",
                columns: new[] { "Id", "SchoolTypeName" },
                values: new object[,]
                {
                    { 1L, "Nursery and Primary" },
                    { 2L, "Secondary" }
                });

            migrationBuilder.InsertData(
                table: "ScoreCategory",
                columns: new[] { "Id", "CategoryName", "DateCreated" },
                values: new object[,]
                {
                    { 1L, "Exam", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2L, "CA", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3L, "Behavioural", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4L, "Extra Curricular", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Terms",
                columns: new[] { "Id", "TermName" },
                values: new object[,]
                {
                    { 2L, "2nd Term" },
                    { 1L, "1st Term" },
                    { 3L, "3rd Term" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AcademicSessions_SchoolId",
                table: "AcademicSessions",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_AcademicSessions_SessionId",
                table: "AcademicSessions",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_AcademicSessions_TermId",
                table: "AcademicSessions",
                column: "TermId");

            migrationBuilder.CreateIndex(
                name: "IX_AcademicSessions_UserId",
                table: "AcademicSessions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Alumni_CampusId",
                table: "Alumni",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_Alumni_ClassGradeId",
                table: "Alumni",
                column: "ClassGradeId");

            migrationBuilder.CreateIndex(
                name: "IX_Alumni_ClassId",
                table: "Alumni",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_Alumni_GradeTeacherId",
                table: "Alumni",
                column: "GradeTeacherId");

            migrationBuilder.CreateIndex(
                name: "IX_Alumni_SchoolId",
                table: "Alumni",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_Alumni_SessionId",
                table: "Alumni",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_Alumni_StudentId",
                table: "Alumni",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_CampusId",
                table: "Classes",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_Classes_SchoolId",
                table: "Classes",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassGrades_CampusId",
                table: "ClassGrades",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassGrades_ClassId",
                table: "ClassGrades",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_ClassGrades_SchoolId",
                table: "ClassGrades",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_District_DistrictAdminId",
                table: "District",
                column: "DistrictAdminId");

            migrationBuilder.CreateIndex(
                name: "IX_District_LocalGovtId",
                table: "District",
                column: "LocalGovtId");

            migrationBuilder.CreateIndex(
                name: "IX_District_StateId",
                table: "District",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_GradeStudents_CampusId",
                table: "GradeStudents",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_GradeStudents_ClassGradeId",
                table: "GradeStudents",
                column: "ClassGradeId");

            migrationBuilder.CreateIndex(
                name: "IX_GradeStudents_ClassId",
                table: "GradeStudents",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_GradeStudents_SchoolId",
                table: "GradeStudents",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_GradeStudents_SessionId",
                table: "GradeStudents",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_GradeStudents_StudentId",
                table: "GradeStudents",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_GradeTeachers_CampusId",
                table: "GradeTeachers",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_GradeTeachers_ClassGradeId",
                table: "GradeTeachers",
                column: "ClassGradeId");

            migrationBuilder.CreateIndex(
                name: "IX_GradeTeachers_ClassId",
                table: "GradeTeachers",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_GradeTeachers_SchoolId",
                table: "GradeTeachers",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_GradeTeachers_SchoolUserId",
                table: "GradeTeachers",
                column: "SchoolUserId");

            migrationBuilder.CreateIndex(
                name: "IX_LocalGovt_StateId",
                table: "LocalGovt",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_Parents_CampusId",
                table: "Parents",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_Parents_GenderId",
                table: "Parents",
                column: "GenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Parents_SchoolId",
                table: "Parents",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentsStudentsMap_CampusId",
                table: "ParentsStudentsMap",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentsStudentsMap_ParentId",
                table: "ParentsStudentsMap",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentsStudentsMap_SchoolId",
                table: "ParentsStudentsMap",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_ParentsStudentsMap_StudentId",
                table: "ParentsStudentsMap",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolCampus_SchoolId",
                table: "SchoolCampus",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_Schools_DistrictId",
                table: "Schools",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_Schools_LocalGovtId",
                table: "Schools",
                column: "LocalGovtId");

            migrationBuilder.CreateIndex(
                name: "IX_Schools_SchoolTypeId",
                table: "Schools",
                column: "SchoolTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Schools_StateId",
                table: "Schools",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolSubjects_CampusId",
                table: "SchoolSubjects",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolSubjects_ClassId",
                table: "SchoolSubjects",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolSubjects_DepartmentId",
                table: "SchoolSubjects",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolSubjects_SchoolId",
                table: "SchoolSubjects",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolUserRoles_RoleId",
                table: "SchoolUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolUserRoles_UserId",
                table: "SchoolUserRoles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolUsers_CampusId",
                table: "SchoolUsers",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_SchoolUsers_SchoolId",
                table: "SchoolUsers",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_ScoreGrading_CampusId",
                table: "ScoreGrading",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_ScoreGrading_ClassId",
                table: "ScoreGrading",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_ScoreGrading_SchoolId",
                table: "ScoreGrading",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_ScoreSubCategoryConfig_CampusId",
                table: "ScoreSubCategoryConfig",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_ScoreSubCategoryConfig_CategoryId",
                table: "ScoreSubCategoryConfig",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ScoreSubCategoryConfig_ClassId",
                table: "ScoreSubCategoryConfig",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_ScoreSubCategoryConfig_SchoolId",
                table: "ScoreSubCategoryConfig",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_ScoreSubCategoryConfig_SessionId",
                table: "ScoreSubCategoryConfig",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_ScoreSubCategoryConfig_TermId",
                table: "ScoreSubCategoryConfig",
                column: "TermId");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_SchoolId",
                table: "Sessions",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentAttendance_AttendancePeriodId",
                table: "StudentAttendance",
                column: "AttendancePeriodId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentAttendance_CampusId",
                table: "StudentAttendance",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentAttendance_ClassGradeId",
                table: "StudentAttendance",
                column: "ClassGradeId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentAttendance_ClassId",
                table: "StudentAttendance",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentAttendance_SchoolId",
                table: "StudentAttendance",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentAttendance_SessionId",
                table: "StudentAttendance",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentAttendance_StudentId",
                table: "StudentAttendance",
                column: "StudentId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentAttendance_TermId",
                table: "StudentAttendance",
                column: "TermId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentDuplicates_CampusId",
                table: "StudentDuplicates",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentDuplicates_ExistingStudentId",
                table: "StudentDuplicates",
                column: "ExistingStudentId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentDuplicates_SchoolId",
                table: "StudentDuplicates",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_CampusId",
                table: "Students",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_GenderId",
                table: "Students",
                column: "GenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Students_SchoolId",
                table: "Students",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectDepartment_CampusId",
                table: "SubjectDepartment",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectDepartment_ClassId",
                table: "SubjectDepartment",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectDepartment_SchoolId",
                table: "SubjectDepartment",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectTeachers_CampusId",
                table: "SubjectTeachers",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectTeachers_ClassGradeId",
                table: "SubjectTeachers",
                column: "ClassGradeId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectTeachers_ClassId",
                table: "SubjectTeachers",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectTeachers_SchoolId",
                table: "SubjectTeachers",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectTeachers_SchoolUserId",
                table: "SubjectTeachers",
                column: "SchoolUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SubjectTeachers_SubjectId",
                table: "SubjectTeachers",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_CampusId",
                table: "Teachers",
                column: "CampusId");

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_SchoolId",
                table: "Teachers",
                column: "SchoolId");

            migrationBuilder.CreateIndex(
                name: "IX_Teachers_SchoolUserId",
                table: "Teachers",
                column: "SchoolUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AcademicSessions");

            migrationBuilder.DropTable(
                name: "Alumni");

            migrationBuilder.DropTable(
                name: "ClassAlumni");

            migrationBuilder.DropTable(
                name: "EmailConfirmationCodes");

            migrationBuilder.DropTable(
                name: "ErrorLog");

            migrationBuilder.DropTable(
                name: "GradeStudents");

            migrationBuilder.DropTable(
                name: "GradeTeachers");

            migrationBuilder.DropTable(
                name: "ParentsStudentsMap");

            migrationBuilder.DropTable(
                name: "SchoolUserRoles");

            migrationBuilder.DropTable(
                name: "ScoreGrading");

            migrationBuilder.DropTable(
                name: "ScoreSubCategoryConfig");

            migrationBuilder.DropTable(
                name: "StudentAttendance");

            migrationBuilder.DropTable(
                name: "StudentDuplicates");

            migrationBuilder.DropTable(
                name: "SubjectTeachers");

            migrationBuilder.DropTable(
                name: "Teachers");

            migrationBuilder.DropTable(
                name: "Parents");

            migrationBuilder.DropTable(
                name: "SchoolRoles");

            migrationBuilder.DropTable(
                name: "ScoreCategory");

            migrationBuilder.DropTable(
                name: "AttendancePeriod");

            migrationBuilder.DropTable(
                name: "Sessions");

            migrationBuilder.DropTable(
                name: "Terms");

            migrationBuilder.DropTable(
                name: "Students");

            migrationBuilder.DropTable(
                name: "ClassGrades");

            migrationBuilder.DropTable(
                name: "SchoolSubjects");

            migrationBuilder.DropTable(
                name: "SchoolUsers");

            migrationBuilder.DropTable(
                name: "Gender");

            migrationBuilder.DropTable(
                name: "SubjectDepartment");

            migrationBuilder.DropTable(
                name: "Classes");

            migrationBuilder.DropTable(
                name: "SchoolCampus");

            migrationBuilder.DropTable(
                name: "Schools");

            migrationBuilder.DropTable(
                name: "District");

            migrationBuilder.DropTable(
                name: "SchoolType");

            migrationBuilder.DropTable(
                name: "DistrictAdministrators");

            migrationBuilder.DropTable(
                name: "LocalGovt");

            migrationBuilder.DropTable(
                name: "States");
        }
    }
}
