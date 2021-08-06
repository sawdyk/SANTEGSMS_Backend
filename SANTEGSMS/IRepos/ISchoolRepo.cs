using SANTEGSMS.RequestModels;
using SANTEGSMS.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.IRepos
{
    public interface ISchoolRepo
    {
        //------------------------School-----------------------------------------------------------------------
        Task<SchoolSignUpRespModel> schoolSignUpAsync(SchoolSignUpReqModel obj);
        Task<GenericRespModel> activateAccountAsync(ActivateSchoolAccountReqModel obj);
        Task<GenericRespModel> resendActivationCodeAsync(string email);
        Task<GenericRespModel> updateSchoolDetailsAsync(long schoolId, UpdateSchoolDetailsReqModel obj);

        //------------------------School-----------------------------------------------------------------------
        Task<GenericRespModel> getSchoolResourcesAsync(long schoolId);

        Task<GenericRespModel> enableOrDisableStaffAsync(bool isEnabled, Guid schoolUserId, long schoolId, long campusId);
        Task<GenericRespModel> enableOrDisableStudentAsync(bool isEnabled, Guid studentId, long schoolId, long campusId);
        Task<GenericRespModel> enableOrDisableParentAsync(bool isEnabled, Guid parentId, long schoolId, long campusId);

    }
}
