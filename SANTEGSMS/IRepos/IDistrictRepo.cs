using SANTEGSMS.RequestModels;
using SANTEGSMS.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.IRepos
{
    public interface IDistrictRepo
    {
        //------------------------District-----------------------------------------------------------------------
        Task<GenericRespModel> createDistrictAsync(DistrictReqModel obj);
        Task<GenericRespModel> getDistrictByIdAsync(long districtId);
        Task<GenericRespModel> getAllDistrictInStateAsync(long stateId);
        Task<GenericRespModel> getAllDistrictInLocalGovtAsync(long localGovtId);
        Task<GenericRespModel> getAllDistrictsAsync(long localGovtId, long stateId);
        Task<GenericRespModel> updateDistrictAsync(long districtId, DistrictReqModel obj);
        Task<GenericRespModel> deleteDistrictAsync(long districtId);

        //------------------------DistrictAdministrators------------------------------------------------------------------
        Task<GenericRespModel> createDistrictAdministratorAsync(DistrictAdminReqModel obj);
        Task<GenericRespModel> getAllDistrictAssignedToDistrictAdministratorAsync(Guid districtAdminId);
        Task<GenericRespModel> getDistrictAdministratorByIdAsync(Guid districtAdminId);

    }
}
