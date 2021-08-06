using SANTEGSMS.RequestModels;
using SANTEGSMS.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.IRepos
{
    public interface ITeacherRepo
    {
        Task<TeacherCreateRespModel> createTeacherAsync(TeacherCreateReqModel obj);
        Task<GenericRespModel> getTeacherByIdAsync(Guid teacherId, long schoolId, long campusId);
        Task<GenericRespModel> getTeacherRolesAsync();
        Task<GenericRespModel> getAllTeachersAsync(long schoolId, long campusId);
        Task<GenericRespModel> getAllTeachersByRoleIdAsync(long roleId, long schoolId, long campusId);
        Task<GenericRespModel> assignTeacherToClassGradeAsync(AssignTeacherToClassReqModel obj);
        Task<GenericRespModel> getAllClassGradeAssignedToTeacherAsync(long schoolId, long campusId, Guid teacherId);
        Task<GenericRespModel> getAssignedTeachersAsync(long schoolId, long campusId);
        Task<GenericRespModel> getAllRolesAssignedToTeacherAsync(Guid teacherId, long schoolId, long campusId);

        Task<GenericRespModel> deleteClassAndClassGradesAssignedToTeacherAsync(Guid teacherId, long schoolId, long campusId, long classId, long classGradeId);



        //-----------------------------------ATTENDANCE---------------------------------------------------------------
        Task<GenericRespModel> takeClassAttendanceAsync(TakeAttendanceReqModel obj);
        Task<GenericRespModel> getClassAttendanceAsync(long classId, DateTime attendanceDate, long schoolId, long campusId, long termId, long sessionId);
        Task<GenericRespModel> getClassGradeAttendanceAsync(long classId, long classGradeId, DateTime attendanceDate, long schoolId, long campusId, long termId, long sessionId);
        Task<GenericRespModel> getClassAttendanceByPeriodIdAsync(long classId, DateTime attendanceDate, long schoolId, long campusId, long periodId, long termId, long sessionId);
        Task<GenericRespModel> getClassGradeAttendanceByPeriodIdAsync(long classId, long classGradeId, DateTime attendanceDate, long schoolId, long campusId, long periodId, long termId, long sessionId);
        Task<GenericRespModel> getStudentAttendanceAsync(Guid studentId, long classId, long classGradeId, DateTime attendanceDate, long schoolId, long campusId, long termId, long sessionId);
        Task<GenericRespModel> getStudentAttendanceByPeriodIdAsync(Guid studentId, long classId, long classGradeId, DateTime attendanceDate, long schoolId, long campusId, long periodId, long termId, long sessionId);

    }
}
