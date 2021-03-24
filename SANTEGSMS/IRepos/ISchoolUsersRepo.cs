using SANTEGSMS.RequestModels;
using SANTEGSMS.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.IRepos
{
    public interface ISchoolUsersRepo
    {
        //------------------------SchoolUsers------------------------------------------------------------------
        Task<SchoolUsersRespModel> createSchoolUsersAsync(SchoolUsersReqModel obj);
        Task<SchoolUsersLoginRespModel> schoolUserLoginAsync(LoginReqModel obj);
        Task<GenericRespModel> getSchoolUsersByRoleIdAsync(long schoolId, long campusId, long roleId);
        Task<GenericRespModel> updateSchoolUserDetailsAsync(Guid schoolUserId, UpdateSchoolUsersDetailsReqModel obj);
        Task<GenericRespModel> deleteSchoolUsersAsync(Guid schoolUserId, long schoolId, long campusId);
    }
}
