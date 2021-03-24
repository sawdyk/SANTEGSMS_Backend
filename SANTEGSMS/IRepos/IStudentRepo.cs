using SANTEGSMS.RequestModels;
using SANTEGSMS.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.IRepos
{
    public interface IStudentRepo
    {
        Task<GenericRespModel> createStudentAsync(StudentCreationReqModel obj);
        Task<GenericRespModel> addStudentToExistingParentAsync(StudentParentExistCreationReqModel obj);
        Task<SchoolUsersLoginRespModel> studentLoginAsync(StudentLoginReqModel obj);
        Task<GenericRespModel> getStudentByIdAsync(Guid studentId, long schoolId, long campusId);
        Task<GenericRespModel> assignStudentToClassAsync(AssignStudentToClassReqModel obj);
        Task<GenericRespModel> getStudentParentAsync(Guid studentId, long schoolId, long campusId);
        Task<GenericRespModel> getAllAssignedStudentAsync(long schoolId, long campusId);
        Task<GenericRespModel> getAllUnAssignedStudentAsync(long schoolId, long campusId);
        Task<GenericRespModel> getAllStudentInSchoolAsync(long schoolId);
        Task<GenericRespModel> getAllStudentInCampusAsync(long schoolId, long campusId);
        Task<GenericRespModel> getStudentsBySessionIdAsync(long schoolId, long campusId, long sessionId);
        Task<GenericRespModel> moveStudentToNewClassAndClassGradeAsync(MoveStudentReqModel obj);
        Task<GenericRespModel> updateStudentDetailsAsync(Guid studentId, UpdateStudentReqModel obj);
        Task<GenericRespModel> deleteStudentsAssignedToClassAsync(DeleteStudentAssignedReqModel obj);
        Task<GenericRespModel> deleteStudentAsync(Guid studentId, long schoolId, long campusId);


        //--------------------------BULK CREATION OF STUDENTS-----------------------------------------------
        Task<StudentBulkCreationRespModel> createStudentFromExcelAsync(BulkStudentReqModel obj);

        //------------------------------DUPLICATE STUDENTS-----------------------------------------------
        Task<GenericRespModel> getAllStudentDuplicatesAsync(long schoolId, long campusId);
        Task<GenericRespModel> getStudentDuplicateByStudentIdAsync(Guid studentId, long schoolId, long campusId);
        Task<GenericRespModel> updateStudentDuplicateAsync(StudentDuplicateReqModel obj);
        Task<GenericRespModel> deleteStudentDuplicateAsync(Guid studentId, long schoolId, long campusId);
    }
}
