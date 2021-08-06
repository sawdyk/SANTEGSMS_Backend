using SANTEGSMS.RequestModels;
using SANTEGSMS.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.IRepos
{
    public interface IReportCardDataGenerateRepo
    {
        Task<ReportCardDataRespModel> getReportCardDataByStudentIdAsync(ReportCardDataReqModel obj);
        Task<ReportCardDataRespModel> getReportCardDataByStudentIdAndPinAsync(ReportCardDataWithPinReqModel obj);
    }
}
