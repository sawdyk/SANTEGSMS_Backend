using SANTEGSMS.RequestModels;
using SANTEGSMS.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.IRepos
{
    public interface IReportCardRepo
    {
        //---------------SAMPLE ALGORITHM TO CALCULATE STUDENTS POSITION BASED ON FINAL SCORE ACCUMULATED --------------------------------------
        Task<GenericRespModel> getScorePositionAsync();

        //---------------COMPUTE STUDENT SCORE (EXAM AND CA) TO GET STUDENT POSITION IN CLASS--------------------------------------
        Task<ComputeResultRespModel> computeResultAndSubjectPositionAsync(ComputeResultPositionReqModel obj);
        Task<ComputeResultRespModel> getAllComputedResultAsync(long classId, long classGradeId, long schoolId, long campusId, long termId, long sessionId);
        Task<ComputeResultRespModel> getComputedResultByStudentIdAsync(Guid studentId, long classId, long classGradeId, long schoolId, long campusId, long termId, long sessionId);
        Task<GenericRespModel> deleteComputedResultByStudentIdAsync(Guid studentId, long classId, long classGradeId, long schoolId, long campusId, long termId, long sessionId);
        Task<GenericRespModel> deleteAllComputedResultAsync(long classId, long classGradeId, long schoolId, long campusId, long termId, long sessionId);


        //----------------------------REPORT CARD PIN GENERATION----------------------------------------------------

        Task<GenericRespModel> generatePinsAsync(PinCreateReqModel obj);
        Task<GenericRespModel> getPinByIdAsync(long pinId);
        Task<GenericRespModel> getAllPinsAsync(long schoolId, long campusId, long termId, long sessionId);
        Task<GenericRespModel> getPinsByStatusAsync(long schoolId, long campusId, long termId, long sessionId, bool isUsed);


    }
}
