using Microsoft.EntityFrameworkCore;
using SANTEGSMS.DataSeed;
using SANTEGSMS.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.DatabaseContext
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Schools> Schools { get; set; }
        public DbSet<SchoolType> SchoolType { get; set; }
        public DbSet<SchoolRoles> SchoolRoles { get; set; }
        public DbSet<SchoolUsers> SchoolUsers { get; set; }
        public DbSet<SchoolUserRoles> SchoolUserRoles { get; set; }
        public DbSet<SchoolCampus> SchoolCampus { get; set; }
        public DbSet<LocalGovt> LocalGovt { get; set; }
        public DbSet<States> States { get; set; }
        public DbSet<District> District { get; set; }
        public DbSet<DistrictAdministrators> DistrictAdministrators { get; set; }
        public DbSet<ErrorLog> ErrorLog { get; set; }
        public DbSet<Gender> Gender { get; set; }
        public DbSet<Parents> Parents { get; set; }
        public DbSet<Students> Students { get; set; }
        public DbSet<EmailConfirmationCodes> EmailConfirmationCodes { get; set; }
        public DbSet<Classes> Classes { get; set; }
        public DbSet<ClassGrades> ClassGrades { get; set; }
        public DbSet<GradeStudents> GradeStudents { get; set; }
        public DbSet<Sessions> Sessions { get; set; }
        public DbSet<Terms> Terms { get; set; }
        public DbSet<AcademicSessions> AcademicSessions { get; set; }
        public DbSet<Teachers> Teachers { get; set; }
        public DbSet<GradeTeachers> GradeTeachers { get; set; }
        public DbSet<ParentsStudentsMap> ParentsStudentsMap { get; set; }
        public DbSet<Alumni> Alumni { get; set; }
        public DbSet<ClassAlumni> ClassAlumni { get; set; }
        public DbSet<AttendancePeriod> AttendancePeriod { get; set; }
        public DbSet<StudentAttendance> StudentAttendance { get; set; }
        public DbSet<SchoolSubjects> SchoolSubjects { get; set; }
        public DbSet<SubjectDepartment> SubjectDepartment { get; set; }
        public DbSet<SubjectTeachers> SubjectTeachers { get; set; }
        public DbSet<ScoreCategory> ScoreCategory { get; set; }
        public DbSet<ScoreSubCategoryConfig> ScoreSubCategoryConfig { get; set; }
        public DbSet<ScoreGrading> ScoreGrading { get; set; }
        public DbSet<StudentDuplicates> StudentDuplicates { get; set; }
        public DbSet<ScoreCategoryConfig> ScoreCategoryConfig { get; set; }
        public DbSet<Assignments> Assignments { get; set; }
        public DbSet<ScoreStatus> ScoreStatus { get; set; }
        public DbSet<AssignmentsSubmitted> AssignmentsSubmitted { get; set; }
        public DbSet<LessonNotes> LessonNotes { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<SubjectNotes> SubjectNotes { get; set; }
        public DbSet<ExtraCurricularScores> ExtraCurricularScores { get; set; }
        public DbSet<BehavioralScores> BehavioralScores { get; set; }
        public DbSet<ReportCardCommentsList> ReportCardCommentsList { get; set; }
        public DbSet<ReportCardSignature> ReportCardSignature { get; set; }
        public DbSet<ReportCardNextTermBegins> ReportCardNextTermBegins { get; set; }
        public DbSet<ReportCardCommentConfig> ReportCardCommentConfig { get; set; }
        public DbSet<ReportCardComments> ReportCardComments { get; set; }
        public DbSet<SchoolSubTypes> SchoolSubTypes { get; set; }
        public DbSet<ReportCardTemplates> ReportCardTemplates { get; set; }
        public DbSet<ReportCardConfiguration> ReportCardConfiguration { get; set; }
        public DbSet<ActiveInActiveStatus> ActiveInActiveStatus { get; set; }
        public DbSet<ReportCardConfigurationLegendList> ReportCardConfigurationLegendList { get; set; }
        public DbSet<ReportCardConfigurationLegend> ReportCardConfigurationLegend { get; set; }
        public DbSet<ExaminationScores> ExaminationScores { get; set; }
        public DbSet<ContinousAssessmentScores> ContinousAssessmentScores { get; set; }
        public DbSet<ReportCardData> ReportCardData { get; set; }
        public DbSet<ReportCardPosition> ReportCardPosition { get; set; }
        public DbSet<ScoreUploadSheetTemplates> ScoreUploadSheetTemplates { get; set; }
        public DbSet<AppTypes> AppTypes { get; set; }
        public DbSet<FolderTypes> FolderTypes { get; set; }
        public DbSet<ReportCardPin> ReportCardPin { get; set; }
        public DbSet<SchoolResources> SchoolResources { get; set; }
        public DbSet<SuperAdmin> SuperAdmin { get; set; }
        public DbSet<ActivityLogs> ActivityLogs { get; set; }
        public DbSet<PrincipalReportCardSignature> PrincipalReportCardSignature { get; set; }
        public DbSet<BroadSheetGrading> BroadSheetGrading { get; set; }
        public DbSet<BroadSheetRemark> BroadSheetRemark { get; set; }
        public DbSet<BroadSheetData> BroadSheetData { get; set; }



        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.SeedSchoolRoles();
            builder.SeedSchoolTypes();
            builder.SeedGenderTypes();
            builder.SeedTerms();
            builder.SeedClassOrAlumni();
            builder.SeedAttendancePeriod();
            builder.seedScoreCategory();
            builder.SeedScoreStatus();
            builder.SeedStatus();
            builder.SeedSchoolSubTypes();
            builder.SeedActiveInActiveStatus();
            builder.SeedReportCardConfig();
            builder.SeedAppTypes();
            builder.SeedFolderTypes();
            builder.SeedSystemSuperAdmin();
        }
    }
}
