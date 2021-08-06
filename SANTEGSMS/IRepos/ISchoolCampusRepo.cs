using SANTEGSMS.RequestModels;
using SANTEGSMS.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.IRepos
{
    public interface ISchoolCampusRepo
    {
        //------------------------SchoolCampus------------------------------------------------------------------
        Task<GenericRespModel> createSchoolCampusAsync(SchoolCampusReqModel obj);
        Task<GenericRespModel> getAllSchoolCampusAsync(long schoolId);
        Task<GenericRespModel> getSchoolCampusByIdAsync(long campusId);
        Task<GenericRespModel> updateCampusDetailsAsync(long campusId, SchoolCampusReqModel obj);
        Task<GenericRespModel> deleteSchoolCampusAsync(long campusId);

    }
}
