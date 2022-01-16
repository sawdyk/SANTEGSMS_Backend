using SANTEGSMS.RequestModels;
using SANTEGSMS.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.IRepos
{
    public interface IBroadSheetRepo
    {
        //------------------------------------------------------BROADSHEET GRADING CONFIG-------------------------------------------------------------------
        Task<GenericRespModel> createBroadsheetGradingConfigAsync(BroadsheetGradeReqModel obj);
        Task<GenericRespModel> updateBroadsheetGradingConfigAsync(BroadsheetGradeReqModel obj, long broadSheetConfigId);
        Task<GenericRespModel> deleteBroadsheetGradingConfigAsync(long broadSheetConfigId);
        Task<GenericRespModel> getBroadsheetGradingConfigByIdAsync(long broadSheetConfigId);
        Task<GenericRespModel> getBroadsheetGradingConfigAsync(long schoolId, long campusId, long sessionId);


        //------------------------------------------------------BROADSHEET REMARK CONFIG--------------------------------------------------------------------
        Task<GenericRespModel> createBroadsheetRemarkConfigAsync(BroadSheetRemarkReqModel obj);
        Task<GenericRespModel> getBroadsheetRemarkConfigAsync(long schoolId, long campusId, long classId);


        //------------------------------------------------------BROADSHEET GENERATION------------------------------------------------------------------------
        Task<BroadSheetRespModel> generateClassBroadsheetAsync(long schoolId, long campusId, long classId, long classGradeId, long termId, long sessionId);

        //------------------------------------------------------PERFORMANCE ANALYSIS------------------------------------------------------------------------
        Task<PerformanceRespModel> generatePerformanceAnalysisSheetAsync(PerformanceAnalysisReqModel obj);

    }
}
