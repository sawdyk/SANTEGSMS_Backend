using SANTEGSMS.RequestModels;
using SANTEGSMS.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.IRepos
{
    public interface IScoresConfigRepo
    {
        //------------------------Score Grading---------------------------------------------------------//
        Task<GenericRespModel> createScoreGradesAsync(ScoreGradeCreateReqModel obj);
        Task<GenericRespModel> getAllScoreGradesAsync(long schoolId, long campusId);
        Task<GenericRespModel> getScoreGradeByIdAsync(long scoreGradeId);
        Task<GenericRespModel> getScoreGradeByClassIdAsync(long classId, long schoolId, long campusId);
        Task<GenericRespModel> updateScoreGradeAsync(long scoreGradeId, ScoreGradeCreateReqModel obj);
        Task<GenericRespModel> deleteScoreGradeAsync(long scoreGradeId);

        //------------------------Score Category---------------------------------------------------------//

        Task<GenericRespModel> getAllScoreCategoryAsync();
        Task<GenericRespModel> getScoreCategoryByIdAsync(long scoreCategoryId);


        //------------------------Score Category Configuration---------------------------------------------------------//

        Task<GenericRespModel> createScoreCategoryConfigAsync(ScoreCategoryConfigReqModel obj);
        Task<GenericRespModel> getAllScoreCategoryConfigAsync(long schoolId, long campusId);
        Task<GenericRespModel> getScoreCategoryConfigByIdAsync(long scoreCategoryConfigId);
        Task<GenericRespModel> updateScoreCategoryConfigAsync(long scoreCategoryConfigId, ScoreCategoryConfigReqModel obj);
        Task<GenericRespModel> deleteScoreCategoryConfigAsync(long scoreCategoryConfigId);

        //------------------------Score SubCategory Configuration---------------------------------------------------------//

        Task<GenericRespModel> createScoreSubCategoryConfigAsync(ScoreSubCategoryConfigReqModel obj);
        Task<GenericRespModel> getAllScoreSubCategoryConfigAsync(long schoolId, long campusId);
        Task<GenericRespModel> getScoreSubCategoryConfigByIdAsync(long scoreSubCategoryConfigId);
        Task<GenericRespModel> updateScoreSubCategoryConfigAsync(long scoreSubCategoryConfigId, ScoreSubCategoryConfigReqModel obj);
        Task<GenericRespModel> deleteScoreSubCategoryConfigAsync(long scoreSubCategoryConfigId);
        Task<GenericRespModel> getScoreSubCategoryConfigByCategoryIdAsync(long scoreCategoryConfigId, long schoolId, long campusId, long classId, long termId, long sessionId);
        Task<GenericRespModel> getScoreCategoryConfigAsync(long schoolId, long campusId, long classId, long termId, long sessionId);


    }
}
