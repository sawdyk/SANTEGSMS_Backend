using SANTEGSMS.DatabaseContext;
using SANTEGSMS.Entities;
using SANTEGSMS.Helpers;
using SANTEGSMS.IRepos;
using SANTEGSMS.RequestModels;
using SANTEGSMS.ResponseModels;
using SANTEGSMS.Reusables;
using SANTEGSMS.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.Repos
{
    public class SchoolCampusRepo : ISchoolCampusRepo
    {
        private readonly AppDbContext _context;
        public SchoolCampusRepo(AppDbContext context)
        {
            _context = context;
        }

        //------------------------SchoolCampus------------------------------------------------------------------

        public async Task<GenericRespModel> createSchoolCampusAsync(SchoolCampusReqModel obj)
        {
            try
            {
                CheckerValidation checker = new CheckerValidation(_context);
                var campusNameCheckResult = checker.checkIfSchoolCampusNameExist(obj.CampusName);

                if (campusNameCheckResult == true)
                {
                    return new GenericRespModel { StatusCode = 409, StatusMessage = "This Campus Name Already Exists" };
                }
                else
                {
                    //Campus info
                    var camp = new SchoolCampus
                    {
                        SchoolId = obj.SchoolId,
                        CampusName = obj.CampusName,
                        CampusAddress = obj.CampusAddress,
                        IsActive = true,
                        DateCreated = DateTime.Now
                    };
                    await _context.SchoolCampus.AddAsync(camp);
                    await _context.SaveChangesAsync();

                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Campus Created Successfully" };

                }
            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericRespModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<GenericRespModel> deleteSchoolCampusAsync(long campusId)
        {
            try
            {
                var schCamp = _context.SchoolCampus.Where(x => x.Id == campusId).FirstOrDefault();
                if (schCamp != null)
                {
                    _context.SchoolCampus.Remove(schCamp);
                    await _context.SaveChangesAsync();

                    return new GenericRespModel { StatusCode = 200, StatusMessage = "School Campus Deleted Successfully", };
                }

                return new GenericRespModel { StatusCode = 200, StatusMessage = "No School Campus with the specified ID" };

            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericRespModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<GenericRespModel> getAllSchoolCampusAsync(long schoolId)
        {
            try
            {
                var result = from camp in _context.SchoolCampus
                             where camp.SchoolId == schoolId
                             select new
                             {
                                 camp.Id,
                                 camp.SchoolId,
                                 camp.CampusName,
                                 camp.CampusAddress,
                                 camp.IsActive,
                                 camp.DateCreated
                             };
                if (result.Count() > 0)
                {
                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList() };
                }

                return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful, No Record Available" };

            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericRespModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<GenericRespModel> getSchoolCampusByIdAsync(long campusId)
        {
            try
            {
                var result = from camp in _context.SchoolCampus
                             where camp.Id == campusId
                             select new
                             {
                                 camp.Id,
                                 camp.SchoolId,
                                 camp.CampusName,
                                 camp.CampusAddress,
                                 camp.IsActive,
                                 camp.DateCreated
                             };
                if (result.Count() > 0)
                {
                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = result.FirstOrDefault() };
                }

                return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful, No Record Available" };

            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericRespModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<GenericRespModel> updateCampusDetailsAsync(long campusId, SchoolCampusReqModel obj)
        {
            try
            {
                var getCamp = _context.SchoolCampus.Where(s => s.Id == campusId).FirstOrDefault();
                if (getCamp != null)
                {
                    CheckerValidation chk = new CheckerValidation(_context);
                    var campNameExist = chk.checkIfSchoolCampusNameExist(obj.CampusName);

                    if (campNameExist == true)
                    {
                        return new GenericRespModel { StatusCode = 409, StatusMessage = "A SchoolCampus with this Name Already Exists" };
                    }
                    else
                    {
                        getCamp.SchoolId = obj.SchoolId;
                        getCamp.CampusName = obj.CampusName;
                        getCamp.CampusAddress = obj.CampusAddress;

                        await _context.SaveChangesAsync();

                        return new GenericRespModel { StatusCode = 200, StatusMessage = "Campus Details Updated Successfully" };
                    }
                }

                return new GenericRespModel { StatusCode = 200, StatusMessage = "No Campus With the Specified ID" };
            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericRespModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }

        }

    }
}
