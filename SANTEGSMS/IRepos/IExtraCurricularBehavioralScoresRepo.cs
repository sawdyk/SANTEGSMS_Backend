using SANTEGSMS.RequestModels;
using SANTEGSMS.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.IRepos
{
    public interface IExtraCurricularBehavioralScoresRepo
    {
        Task<UploadScoreRespModel> uploadExtraCurricularBehavioralScoresAsync(UploadScoreReqModel obj);
        Task<GenericRespModel> getExtraCurricularBehavioralScoresAsync(long schoolId, long campusId, long classId, long classGradeId, long categoryId, long subCategoryId, long termId, long sessionId);
        Task<GenericRespModel> getExtraCurricularBehavioralScoresByStudentIdAsync(Guid studentId, long schoolId, long campusId, long classId, long classGradeId, long categoryId, long subCategoryId, long termId, long sessionId);
        Task<GenericRespModel> getExtraCurricularBehavioralScoresByStudentIdAndCategoryIdAsync(Guid studentId, long schoolId, long campusId, long classId, long classGradeId, long categoryId, long termId, long sessionId);
        Task<UploadScoreRespModel> uploadSingleStudentExtraCurricularBehavioralScoreAsync(UploadScorePerStudentReqModel obj);
        Task<UploadScoreRespModel> updateExtraCurricularBehavioralScoresAsync(UploadScoreReqModel obj);
        Task<GenericRespModel> deleteExtraCurricularBehavioralScoresForSingleStudentAsync(Guid studentId, long schoolId, long campusId, long classId, long classGradeId, long categoryId, long subCategoryId, long termId, long sessionId);
        Task<GenericRespModel> deleteExtraCurricularBehavioralScoresForAllStudentAsync(long schoolId, long campusId, long classId, long classGradeId, long categoryId, long subCategoryId, long termId, long sessionId);
        Task<GenericRespModel> deleteExtraCurricularBehavioralScoresPerCategoryForSingleStudentAsync(Guid studentId, long schoolId, long campusId, long classId, long classGradeId, long categoryId, long termId, long sessionId);
        Task<GenericRespModel> deleteExtraCurricularBehavioralScoresPerCategoryForAllStudentAsync(long schoolId, long campusId, long classId, long classGradeId, long categoryId, long termId, long sessionId);

    }
}
