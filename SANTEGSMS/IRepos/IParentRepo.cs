using SANTEGSMS.RequestModels;
using SANTEGSMS.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.IRepos
{
    public interface IParentRepo
    {
        Task<GenericRespModel> createParentInfoAndMapStudentAsync(CreateParentReqModel obj);
        Task<GenericRespModel> getParentDetailsByEmailAsync(string email, long schoolId, long campusId);
        Task<SchoolUsersLoginRespModel> parentLoginAsync(LoginReqModel obj);
        Task<GenericRespModel> getParentDetailsByIdAsync(Guid parentId, long schoolId, long campusId);
        Task<GenericRespModel> getAllParentAsync(long schoolId, long campusId);
        Task<ParentChildRespModel> getAllParentChildAsync(Guid parentId, long schoolId, long campusId);
        Task<GenericRespModel> updateParentDetailsAsync(Guid parentId, UpdateParentReqModel obj);
        Task<GenericRespModel> getAllParentInSchoolPerSessionAsync(long schoolId, long campusId);
        Task<GenericRespModel> getAllParentInSchoolPerSessionAsync(long schoolId);




        //-------------------------------------ChildrenProfile-----------------------------------------------
        Task<GenericRespModel> getChildrenProfileAsync(ChildrenProfileReqModel obj);
        //-------------------------------------ChildrenAttendance-----------------------------------------------
        Task<GenericRespModel> getChildrenAttendanceBySessionIdAsync(IList<Guid> childrenId, Guid parentId, long sessionId);
        Task<GenericRespModel> getChildrenAttendanceByTermIdAsync(IList<Guid> childrenId, Guid parentId, long termId);
        Task<GenericRespModel> getChildrenAttendanceByDateAsync(IList<Guid> childrenId, Guid parentId, DateTime startDate, DateTime endDate);
        //------------------------------------------ChildrenSubject--------------------------------------------------
        Task<GenericRespModel> getChildrenSubjectAsync(IList<Guid> childrenId, Guid parentId);
        //-------------------------------------ChildAttendance-----------------------------------------------
        Task<GenericRespModel> getChildAttendanceBySessionIdAsync(Guid childId, Guid parentId, long sessionId);
        Task<GenericRespModel> getChildAttendanceByTermIdAsync(Guid childId, Guid parentId, long termId);
        Task<GenericRespModel> getChildAttendanceByDateAsync(Guid childId, Guid parentId, DateTime startDate, DateTime endDate);
        //------------------------------------------ChildSubject--------------------------------------------------
        Task<GenericRespModel> getChildSubjectAsync(Guid childId, Guid parentId);

        Task<GenericRespModel> forgotPasswordAsync(string email);
        Task<GenericRespModel> changePasswordAsync(string email, string oldPassword, string newPassword);

    }
}
