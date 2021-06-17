using SANTEGSMS.RequestModels;
using SANTEGSMS.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.IRepos
{
    public interface IScoreUploadRepo
    {
        Task<UploadScoreRespModel> uploadScoresAsync(UploadSubjectScoreReqModel obj);
        Task<GenericRespModel> getScoresBySubjectIdAsync(long schoolId, long campusId, long classId, long classGradeId, long subjectId, long categoryId, long subCategoryId, long termId, long sessionId);
        Task<GenericRespModel> getScoresByStudentIdAndSubjectIdAsync(Guid studentId, long schoolId, long campusId, long classId, long classGradeId, long subjectId, long categoryId, long subCategoryId, long termId, long sessionId);
        Task<GenericRespModel> getAllScoresByStudentIdAsync(Guid studentId, long schoolId, long campusId, long classId, long classGradeId, long categoryId, long subCategoryId, long termId, long sessionId);
        Task<UploadScoreRespModel> uploadSingleStudentScoreAsync(UploadScorePerSubjectAndStudentReqModel obj);
        Task<UploadScoreRespModel> updateSingleStudentScoresAsync(UploadScorePerSubjectAndStudentReqModel obj);
        Task<UploadScoreRespModel> updateScoresAsync(UploadSubjectScoreReqModel obj);
        Task<GenericRespModel> deleteScoresPerSubjectForSingleStudentAsync(Guid studentId, long schoolId, long campusId, long classId, long classGradeId, long subjectId, long categoryId, long subCategoryId, long termId, long sessionId);
        Task<GenericRespModel> deleteScoresPerSubjectForAllStudentAsync(long schoolId, long campusId, long classId, long classGradeId, long subjectId, long categoryId, long subCategoryId, long termId, long sessionId);
        Task<GenericRespModel> deleteScoresPerCategoryForSingleStudentAsync(Guid studentId, long schoolId, long campusId, long classId, long classGradeId, long categoryId, long subCategoryId, long termId, long sessionId);
        Task<GenericRespModel> deleteScoresPerCategoryForAllStudentAsync(long schoolId, long campusId, long classId, long classGradeId, long categoryId, long subCategoryId, long termId, long sessionId);

        //-----------------------------------------------------EXTENDED SCORES-----------------------------------------------------------------------------------------------
        Task<ExtendedScoresRespModel> getAllStudentAndSubjectScoresExtendedAsync(long schoolId, long campusId, long classId, long classGradeId, long categoryId, long subCategoryId, long termId, long sessionId, IList<SubjectId> subjectId);

        //-----------------------------------------------------SCORE UPLOAD SHEET-----------------------------------------------------------------------------------------------
        Task<ScoreUploadSheetRespModel> createScoreUploadSheetTemplateAsync(ScoreUploadSheetTemplateReqModel obj);
        Task<GenericRespModel> getScoreSheetTemplateByIdAsync(long scoreSheetTemplateId);
        Task<GenericRespModel> getAllUsedScoreSheetTemplateAsync(long schoolId, long campusId, long classId, long classGradeId, Guid teacherId);
        Task<GenericRespModel> getAllUnUsedScoreSheetTemplateAsync(long schoolId, long campusId, long classId, long classGradeId, Guid teacherId);


        //-----------------------------------------------------BULK UPLOAD OF SCORES(EXCEL)-----------------------------------------------------------------------------------------------
        Task<UploadScoreRespModel> bulkScoresUploadAsync(BulkScoresUploadReqModel obj);

        //-----------------------------------------------------STUDENT GRADE BOOK (Student Ability to View their scores per subject, category and subcategory)-----------------------------------------------------------------------------------------------
        Task<GenericRespModel> studentGradeBookScoresPerSubjectAndCategoryAsync(Guid studentId, long schoolId, long campusId, long categoryId, long subCategoryId);

    }
}
