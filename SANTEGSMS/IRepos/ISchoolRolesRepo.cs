using SANTEGSMS.RequestModels;
using SANTEGSMS.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.IRepos
{
    public interface ISchoolRolesRepo
    {
        //------------------------SchoolRoles------------------------------------------------------------------
        Task<GenericRespModel> getAllSchoolRolesAsync();
        Task<GenericRespModel> getSchoolRolesForSchoolUserCreationAsync();
        Task<GenericRespModel> getSchoolRolesByRoleIdAsync(long schoolRoleId);
        Task<GenericRespModel> getSchoolRolesAssignedToSchoolUsersAsync(long schoolId, long campusId, Guid schoolUserId);
        Task<GenericRespModel> assignRolesToSchoolUsersAsync(AssignRolesReqModel obj);
        Task<GenericRespModel> deleteRolesAssignedToSchoolUsersAsync(DeleteRolesAssignedReqModel obj);

    }
}
