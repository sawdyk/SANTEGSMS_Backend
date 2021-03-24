using SANTEGSMS.RequestModels;
using SANTEGSMS.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.IRepos
{
    public interface ILocalGovtRepo
    {
        //------------------------States-----------------------------------------------------------------------
        Task<GenericRespModel> createStatesAsync(StateReqModel obj);

        //------------------------States Local Govt-----------------------------------------------------------------------
        Task<GenericRespModel> createLocalGovtAsync(LocalGovtReqModel obj);
        Task<GenericRespModel> getLocalGovtByIdAsync(long localGovtId);
        Task<GenericRespModel> getAllLocalGovtInStatesAsync(long stateId);
        Task<GenericRespModel> updateLocalGovtAsync(long localGovtId, LocalGovtReqModel obj);
        Task<GenericRespModel> deleteLocalGovtAsync(long localGovtId);

    }
}
