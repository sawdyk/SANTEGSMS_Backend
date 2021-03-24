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
        
    }
}
