using SANTEGSMS.RequestModels;
using SANTEGSMS.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.IRepos
{
    public interface ISuperAdminRepo
    {
        Task<GenericRespModel> superAdminLoginAsync(LoginReqModel obj);
        Task<GenericRespModel> getAllSchoolsAsync();
        Task<GenericRespModel> updateSuperAdminDetailsAsync(Guid superAdminId, UpdateSuperAdminReqModel obj);
        Task<GenericRespModel> forgotPasswordAsync(string email);
        Task<GenericRespModel> changePasswordAsync(string email, string oldPassword, string newPassword);
        Task<GenericRespModel> approveOrDeclineSchoolCreationAsync(bool isApproved, long schoolId);
        Task<GenericRespModel> enableOrDisableSchoolAccountAsync(bool isEnabled, long schoolId);

    }
}
