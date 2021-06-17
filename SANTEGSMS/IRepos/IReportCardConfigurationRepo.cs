using SANTEGSMS.RequestModels;
using SANTEGSMS.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.IRepos
{
    public interface IReportCardConfigurationRepo
    {
        Task<GenericRespModel> createCommentsListAsync(CommentListReqModel obj);
        Task<GenericRespModel> getCommentByIdAsync(long schoolId, long campusId, long commentId);
        Task<GenericRespModel> getAllCommentsAsync(long schoolId, long campusId);
        Task<GenericRespModel> updateCommentsAsync(long commentId, UpdateCommentReqModel obj);
        Task<GenericRespModel> deleteCommentsAsync(long commentId);
        Task<GenericRespModel> uploadReportCardSignatureAsync(ReportCardSignatureReqModel obj);
        Task<GenericRespModel> getReportCardSignatureAsync(long schoolId, long campusId);
        Task<GenericRespModel> nextTermBeginsAsync(NextTermBeginsReqModel obj);
        Task<GenericRespModel> getNextTermBeginsAsync(long schoolId, long campusId);
        Task<GenericRespModel> addCommentsOnReportCardForAllStudentsAsync(CommentsOnReportsCardForAllStudent obj);
        Task<GenericRespModel> getAllCommentOnReportCardAsync(long schoolId, long campusId, long CommentConfigId, long classId, long classGradeId, long termId, long sessionId);
        Task<GenericRespModel> getCommentOnReportCardByIdAsync(long commentOnReportCardId);
        Task<GenericRespModel> getCommentOnReportCardByStudentIdAsync(Guid studentId, long schoolId, long campusId, long commentConfigId, long classId, long classGradeId, long termId, long sessionId);
        Task<GenericRespModel> addCommentsOnReportCardForSingleStudentAsync(CommentsOnReportsCardForSingleStudent obj);
        Task<GenericRespModel> updateCommentsOnReportCardForAllStudentsAsync(CommentsOnReportsCardForAllStudent obj);
        Task<GenericRespModel> updateCommentsOnReportCardForSingleStudentAsync(CommentsOnReportsCardForSingleStudent obj);
        Task<GenericRespModel> deleteCommentsOnReportCardForSingleStudentAsync(long commentConfigId, Guid studentId, long schoolId, long campusId, long classId, long classGradeId, long termId, long sessionId);
        Task<GenericRespModel> deleteCommentsOnReportCardForAllStudentAsync(long commentConfigId, long schoolId, long campusId, long classId, long classGradeId, long termId, long sessionId);

        //----------------------------------------------------SYSTEM DEFINED/DEFAULT------------------------------------------------------------------------------------------------------
        Task<GenericRespModel> getAllReportCardCommenConfigAsync();
        Task<GenericRespModel> getAllReportCardCommenConfigByIdAsync(long commentConfigId);

        //----------------------------------------------------REPORT CARD TEMPLATE------------------------------------------------------------------------------------------------------
        Task<GenericRespModel> createReportCardTemplateAsync(ReportCardTemplateReqModel obj);
        Task<GenericRespModel> getReportCardTemplateByIdAsync(long reportCardTemplateId);
        Task<GenericRespModel> getReportCardTemplateAsync(long schoolId, long campusId);

        //----------------------------------------------------REPORT CARD CONFIGURATION------------------------------------------------------------------------------------------------------
        Task<GenericRespModel> createReportCardConfigurationAsync(ReportCardConfigReqModel obj);
        Task<GenericRespModel> getAllReportCardConfigurationAsync(long schoolId, long campusId);
        Task<GenericRespModel> getReportCardConfigurationByIdAsync(long schoolId, long campusId, long reportCardConfigId);
        Task<GenericRespModel> getReportCardConfigurationByTermIdAsync(long schoolId, long campusId, long termId);
        Task<GenericRespModel> updateReportCardConfigurationAsync(ReportCardConfigReqModel obj, long reportCardConfigId);
        Task<GenericRespModel> deleteReportCardConfigurationAsync(long reportCardConfigId);

        //----------------------------------------------------REPORT CARD CONFIGURATION (LEGEND)---------------------------------------------------------------------------------------------
        Task<GenericRespModel> createReportCardConfigurationLegendAsync(ReportCardConfigurationLegendReqModel obj);
        Task<GenericRespModel> getAllReportCardConfigurationLegendAsync(long schoolId, long campusId);
        Task<GenericRespModel> getReportCardConfigurationLegendByIdAsync(long schoolId, long campusId, long reportCardConfigLegendId);
        Task<GenericRespModel> getReportCardConfigurationLegendByTermIdAsync(long schoolId, long campusId, long termId);
        Task<GenericRespModel> updateReportCardConfigurationLegendAsync(long reportCardConfigLegendId, UpdateLegendReqModel obj);

        Task<GenericRespModel> updateReportCardConfigurationLegendListAsync(long reportCardConfigLegendId, long legendListId, long schoolId, long campusId, LegendList obj);
        Task<GenericRespModel> deleteReportCardConfigurationLegendListAsync(long reportCardConfigLegendId, long legendListId);
        Task<GenericRespModel> addReportCardConfigurationLegendListAsync(long reportCardConfigLegendId, long schoolId, long campusId, IList<LegendList> legendList);

        Task<GenericRespModel> deleteReportCardConfigurationLegendAsync(long reportCardConfigLegendId);
    }
}
