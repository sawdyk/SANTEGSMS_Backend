using SANTEGSMS.RequestModels;
using SANTEGSMS.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.IRepos
{
    public interface IAssignmentRepo
    {
        //------------------------------------------------ASSIGNMENTS---------------------------------------------------------------
        Task<GenericRespModel> createAssignmentAsync(AssignmentCreationReqModel obj);
        Task<GenericRespModel> getAssignmentByIdAsync(long assignmentId, long schoolId, long campusId);
        Task<GenericRespModel> getAssignmentBySubjectIdAsync(long subjectId, long schoolId, long campusId, long termId, long sessionId);
        Task<GenericRespModel> getAssignmentByTeacherIdAsync(Guid teacherId, long schoolId, long campusId, long termId, long sessionId);
        Task<GenericRespModel> updateAssignmentAsync(long assignmentId, AssignmentCreationReqModel obj);
        Task<GenericRespModel> deleteAssignmentAsync(long assignmentId, long schoolId, long campusId);



        //-----------------------------------------------SUBMIT AND GRADE ASSIGNMENTS-------------------------------------------------
        Task<GenericRespModel> submitAssignmentAsync(SubmitAssignmentReqModel obj);
        Task<GenericRespModel> getSubmittedAssignmentByIdAsync(long assignmentSubmittedId, long schoolId, long campusId);
        Task<GenericRespModel> getAllSubmittedAssignmentsByAssignmentIdAsync(long classId, long classGradeId, long assignmentId, long schoolId, long campusId, long termId, long sessionId);
        Task<GenericRespModel> getAllSubmittedAssignmentsByStudentIdAndAssignmentIdAsync(Guid studentId, long classId, long classGradeId, long assignmentId, long schoolId, long campusId, long termId, long sessionId);
        Task<GenericRespModel> getAllUnSubmittedAssignmentsByStudentIdAsync(Guid studentId, long classId, long classGradeId, long schoolId, long campusId, long termId, long sessionId);
        Task<GenericRespModel> getAllUnSubmittedAssignmentsByIndividualStudentIdAsync(Guid studentId, long schoolId, long campusId);
        Task<GenericRespModel> getSubmittedAssignmentsByIndividualStudentIdAsync(Guid studentId, long assignmentId, long schoolId, long campusId);
        Task<GenericRespModel> updateSubmittedAssignmentsAsync(long assignmentSubmittedId, SubmitAssignmentReqModel obj);
        Task<GenericRespModel> deleteSubmittedAssignmentsAsync(long assignmentSubmittedId, long schoolId, long campusId);
        Task<GenericRespModel> gradeSubmittedAssignmentsAsync(GradeAssignmentsReqModel obj);
        Task<GenericRespModel> getSubmittedAssignmentsBySubjectIdAsync(long subjectId, long schoolId, long campusId, long termId, long sessionId);
        Task<GenericRespModel> getUnSubmittedAssignmentsByClassIdAndClassGradeIdAsync(long classId, long classGradeId, long schoolId, long campusId, long termId, long sessionId);
        Task<GenericRespModel> getSubmittedAssignmentsByClassIdAndClassGradeIdAsync(long classId, long classGradeId, long schoolId, long campusId, long termId, long sessionId);
        Task<GenericRespModel> getAllSubmittedAssignmentsByStudentIdAsync(Guid studentId, long classId, long classGradeId, long schoolId, long campusId, long termId, long sessionId);
        Task<GenericRespModel> getAllSubmittedAssignmentsByTeacherIdAsync(Guid teacherId, long classId, long classGradeId, long schoolId, long campusId, long termId, long sessionId);
        Task<GenericRespModel> getAllSubmittedAssignmentsByTeacherIdAndSubjectIdAsync(Guid teacherId, long subjectId, long classId, long classGradeId, long schoolId, long campusId, long termId, long sessionId);


    }
}
