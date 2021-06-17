using SANTEGSMS.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.IRepos
{
    public interface ISystemDefaultRepo
    {
        //------------------------States-----------------------------------------------------------------------
        Task<GenericRespModel> getStatesByIdAsync(long stateId);
        Task<GenericRespModel> getAllStatesAsync();

        //-----------------------SchoolType---------------------------------------------------------------------
        Task<GenericRespModel> getAllSchoolTypesAsync();
        Task<GenericRespModel> getSchoolTypeByIdAsync(long schoolTypeId);

        //-----------------------Gender---------------------------------------------------------------------
        Task<GenericRespModel> getAllGenderAsync();
        Task<GenericRespModel> getGenderByIdAsync(long genderId);

        //-----------------------Class or Alumni---------------------------------------------------------------------
        Task<GenericRespModel> getClassOrAlumniAsync();
        Task<GenericRespModel> getClassOrAlumniByIdAsync(long classOrAlumniId);

        //-----------------------Attendance Period---------------------------------------------------------------------
        Task<GenericRespModel> getAllAttendancePeriodAsync();
        Task<GenericRespModel> getAttendancePeriodByIdAsync(long periodId);

        Task<GenericRespModel> getActiveInActiveStatusAsync();
        Task<GenericRespModel> getActiveInActiveStatusByIdAsync(long statusId);

        Task<GenericRespModel> getAllSchoolSubTypesBySchoolTypeIdAsync(long schoolTypeId);

    }
}
