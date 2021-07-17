using SANTEGSMS.RequestModels;
using SANTEGSMS.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.IRepos
{
    public interface ISubjectRepo
    {
        Task<GenericRespModel> createSubjectAsync(SubjectCreationReqModel obj);
        Task<GenericRespModel> getSubjectByIdAsync(long subjectId);
        Task<GenericRespModel> getAllSchoolSubjectsAsync(long schoolId, long campusId);
        Task<GenericRespModel> getAllClassSubjectsAsync(long classId, long schoolId, long campusId);
        Task<GenericRespModel> assignSubjectToTeacherAsync(AssignSubjectToTeacherReqModel obj);
        Task<GenericRespModel> getAllAssignedSubjectsAsync(long schoolId, long campusId);
        Task<GenericRespModel> getAllUnAssignedSubjectsAsync(long schoolId, long campusId);
        Task<GenericRespModel> getAllSubjectsAssignedToTeacherAsync(Guid teacherId, long schoolId, long campusId);
        Task<GenericRespModel> createBulkSubjectAsync(BulkSubjectReqModel obj);


        //--------------------------------------------Subject Department--------------------------------------------------

        Task<GenericRespModel> createSubjectDepartmentAsync(SubjectDepartmentCreateReqModel obj);
        Task<GenericRespModel> getAllSubjectDepartmentAsync(long schoolId, long campusId);
        Task<GenericRespModel> getSubjectDepartmentByIdAsync(long subjectDepartmentId);
        Task<GenericRespModel> assignSubjectToDepartmentAsync(AssignSubjectToDepartmentReqModel obj);
        Task<GenericRespModel> getAllSubjectsAssignedToDepartmentAsync(long subjectDepartmentId);

        //--------------------------------------------Order Of Subject--------------------------------------------------
        Task<GenericRespModel> orderOfSubjectsAsync(long classId, IEnumerable<OrderOfSubjectsReqModel> obj);
        Task<GenericRespModel> deleteSubjectAsync(long subjectId);
        Task<GenericRespModel> updateSubjectAsync(long subjectId, SubjectCreationReqModel obj);
        Task<GenericRespModel> deleteAssignedSubjectsAsync(long subjectAssignedId);
        Task<GenericRespModel> deleteSubjectDepartmentAsync(long subjectDepartmentId);
    }
}
