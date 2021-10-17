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
    public class DistrictRepo : IDistrictRepo
    {
        private readonly AppDbContext _context;
        public DistrictRepo(AppDbContext context)
        {
            _context = context;
        }

        //-------------------------------Districts---------------------------------------------------------------
        public async Task<GenericRespModel> createDistrictAsync(DistrictReqModel obj)
        {
            try
            {
                var check = _context.District.Where(x => x.DistrictName == obj.DistrictName && x.StateId == obj.StateId && x.LocalGovtId == obj.LocalGovtId).FirstOrDefault();

                if (check == null)
                {
                    //Save the District
                    var district = new District
                    {
                        DistrictName = obj.DistrictName,
                        StateId = obj.StateId,
                        LocalGovtId = obj.LocalGovtId,
                        DistrictAdminId = obj.DistrictAdminId,
                        DateCreated = DateTime.Now,
                    };

                    await _context.District.AddAsync(district);
                    await _context.SaveChangesAsync();

                    //return the District Created
                    var result = from st in _context.District
                                 where st.Id == district.Id
                                 select new
                                 {
                                     st.Id,
                                     st.DistrictName,
                                     st.StateId,
                                     st.States.StateName,
                                     st.LocalGovtId,
                                     st.LocalGovt.LocalGovtName,
                                     st.DistrictAdminId,
                                     st.DistrictAdministrators.FirstName,
                                     st.DistrictAdministrators.LastName
                                 };

                    return new GenericRespModel { StatusCode = 200, StatusMessage = "District Created Successfully", Data = result.FirstOrDefault() };
                }

                return new GenericRespModel { StatusCode = 409, StatusMessage = "District Already Exists" };

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

        public async Task<GenericRespModel> getAllDistrictsAsync(long localGovtId, long stateId)
        {
            try
            {
                //return the District Created
                var result = from st in _context.District
                             where st.StateId == stateId && st.LocalGovtId == localGovtId
                             select new
                             {
                                 st.Id,
                                 st.DistrictName,
                                 st.StateId,
                                 st.States.StateName,
                                 st.LocalGovtId,
                                 st.LocalGovt.LocalGovtName,
                                 st.DistrictAdminId,
                                 st.DistrictAdministrators.FirstName,
                                 st.DistrictAdministrators.LastName
                             };

                if (result.Count() > 0)
                {
                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList() };
                }

                return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful, No Record Available", };

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

        public async Task<GenericRespModel> getAllDistrictInLocalGovtAsync(long localGovtId)
        {
            try
            {
                //return the District Created
                var result = from st in _context.District
                             where st.LocalGovtId == localGovtId
                             select new
                             {
                                 st.Id,
                                 st.DistrictName,
                                 st.StateId,
                                 st.States.StateName,
                                 st.LocalGovtId,
                                 st.LocalGovt.LocalGovtName,
                                 st.DistrictAdminId,
                                 st.DistrictAdministrators.FirstName,
                                 st.DistrictAdministrators.LastName
                             };

                if (result.Count() > 0)
                {
                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList() };
                }

                return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful, No Record Available", };

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

        public async Task<GenericRespModel> getAllDistrictInStateAsync(long stateId)
        {
            try
            {
                //return the District Created
                var result = from st in _context.District
                             where st.StateId == stateId
                             select new
                             {
                                 st.Id,
                                 st.DistrictName,
                                 st.StateId,
                                 st.States.StateName,
                                 st.LocalGovtId,
                                 st.LocalGovt.LocalGovtName,
                                 st.DistrictAdminId,
                                 st.DistrictAdministrators.FirstName,
                                 st.DistrictAdministrators.LastName
                             };

                if (result.Count() > 0)
                {
                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList() };
                }

                return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful, No Record Available", };

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

        public async Task<GenericRespModel> getDistrictByIdAsync(long districtId)
        {
            try
            {
                //return the District Created
                var result = from st in _context.District
                             where st.Id == districtId
                             select new
                             {
                                 st.Id,
                                 st.DistrictName,
                                 st.StateId,
                                 st.States.StateName,
                                 st.LocalGovtId,
                                 st.LocalGovt.LocalGovtName,
                                 st.DistrictAdminId,
                                 st.DistrictAdministrators.FirstName,
                                 st.DistrictAdministrators.LastName
                             };

                if (result.Count() > 0)
                {
                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = result.FirstOrDefault() };
                }

                return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful, No Record Available", };

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

        public async Task<GenericRespModel> updateDistrictAsync(long districtId, DistrictReqModel obj)
        {
            try
            {
                //check if the district exists
                var getDistrict = _context.District.Where(x => x.Id == districtId).FirstOrDefault();

                if (getDistrict != null)
                {
                    //check if the district to be created already exists
                    var check = _context.District.Where(x => x.DistrictName == obj.DistrictName && x.StateId == obj.StateId && x.LocalGovtId == obj.LocalGovtId).FirstOrDefault();

                    if (check == null)
                    {
                        //Update the District
                        getDistrict.DistrictName = obj.DistrictName;
                        getDistrict.StateId = obj.StateId;
                        getDistrict.LocalGovtId = obj.LocalGovtId;
                        getDistrict.DistrictAdminId = obj.DistrictAdminId;

                        await _context.SaveChangesAsync();

                        //return the District Created
                        var result = from st in _context.District
                                     where st.Id == getDistrict.Id
                                     select new
                                     {
                                         st.Id,
                                         st.DistrictName,
                                         st.StateId,
                                         st.States.StateName,
                                         st.LocalGovtId,
                                         st.LocalGovt.LocalGovtName,
                                         st.DistrictAdminId,
                                         st.DistrictAdministrators.FirstName,
                                         st.DistrictAdministrators.LastName
                                     };

                        return new GenericRespModel { StatusCode = 200, StatusMessage = "District Created Successfully", Data = result.FirstOrDefault() };
                    }

                    return new GenericRespModel { StatusCode = 409, StatusMessage = "District Already Exists" };
                }

                return new GenericRespModel { StatusCode = 409, StatusMessage = "No District With the Specified ID" };

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

        public async Task<GenericRespModel> deleteDistrictAsync(long districtId)
        {
            try
            {
                //check if the District Exists
                var district = _context.District.Where(x => x.Id == districtId).FirstOrDefault();

                if (district != null)
                {
                    _context.District.Remove(district);
                    await _context.SaveChangesAsync();

                    return new GenericRespModel { StatusCode = 409, StatusMessage = "District Deleted Successfully" };
                }

                return new GenericRespModel { StatusCode = 409, StatusMessage = "No District With the Specified ID" };

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

        //-------------------------------Districts Administrator---------------------------------------------------------------
        public async Task<GenericRespModel> createDistrictAdministratorAsync(DistrictAdminReqModel obj)
        {
            try
            {
                //check if the district Email already exists
                var check = _context.DistrictAdministrators.Where(x => x.Email == obj.Email).FirstOrDefault();

                if (check == null)
                {
                    var paswordHasher = new PasswordHasher();
                    //the salt
                    string salt = paswordHasher.getSalt();
                    //get the default password
                    string password = DefaultPasswordReUsable.DefaultPassword();
                    //Hash the password and salt
                    string passwordHash = paswordHasher.hashedPassword(password, salt);

                    //Save the DistrictAdmin
                    var districtAdmin = new DistrictAdministrators
                    {
                        FirstName = obj.FirstName,
                        LastName = obj.LastName,
                        UserName = obj.Email,
                        Email = obj.Email,
                        EmailConfirmed = true,
                        PhoneNumber = obj.PhoneNumber,
                        PhoneNumberConfirmed = true,
                        Salt = salt,
                        PasswordHash = passwordHash,
                        IsActive = true,
                        DateCreated = DateTime.Now
                    };

                    await _context.DistrictAdministrators.AddAsync(districtAdmin);
                    await _context.SaveChangesAsync();

                    //return the districtAdmin Created
                    var result = from st in _context.DistrictAdministrators
                                 where st.Id == districtAdmin.Id
                                 select new
                                 {
                                     st.Id,
                                     st.FirstName,
                                     st.LastName,
                                     st.UserName,
                                     st.Email,
                                     st.EmailConfirmed,
                                     st.PhoneNumber,
                                     st.LastPasswordChangedDate,
                                     st.LastLoginDate,
                                     st.LastUpdatedDate,
                                     st.DateCreated,
                                 };

                    return new GenericRespModel { StatusCode = 200, StatusMessage = "District Administrator Created Successfully", Data = result.FirstOrDefault() };
                }

                return new GenericRespModel { StatusCode = 409, StatusMessage = "A District Administrator With Email Already Exists" };

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

        public async Task<GenericRespModel> getAllDistrictAssignedToDistrictAdministratorAsync(Guid districtAdminId)
        {
            try
            {
                //return the districtAdmin Created
                var result = from st in _context.District
                             where st.DistrictAdminId == districtAdminId
                             select new
                             {
                                 st.Id,
                                 st.DistrictName,
                                 st.StateId,
                                 st.States.StateName,
                                 st.LocalGovtId,
                                 st.LocalGovt.LocalGovtName,
                                 st.DistrictAdminId,
                                 st.DistrictAdministrators.FirstName,
                                 st.DistrictAdministrators.LastName,
                             };

                if (result.Count() > 0)
                {
                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList() };
                }

                return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful, No Record Available", };

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

        public async Task<GenericRespModel> getDistrictAdministratorByIdAsync(Guid districtAdminId)
        {
            try
            {
                //return the districtAdmin Created
                var result = from st in _context.DistrictAdministrators
                             where st.Id == districtAdminId
                             select new
                             {
                                 st.Id,
                                 st.FirstName,
                                 st.LastName,
                                 st.UserName,
                                 st.Email,
                                 st.EmailConfirmed,
                                 st.PhoneNumber,
                                 st.LastPasswordChangedDate,
                                 st.LastLoginDate,
                                 st.LastUpdatedDate,
                                 st.DateCreated,
                             };

                if (result.Count() > 0)
                {
                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList() };
                }

                return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful, No Record Available", };

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

        public async Task<GenericRespModel> getAllDistrictAdministratorAsync()
        {
            try
            {
                //return the districtAdmin Created
                var result = from st in _context.DistrictAdministrators
                             select new
                             {
                                 st.Id,
                                 st.FirstName,
                                 st.LastName,
                                 st.UserName,
                                 st.Email,
                                 st.EmailConfirmed,
                                 st.PhoneNumber,
                                 st.LastPasswordChangedDate,
                                 st.LastLoginDate,
                                 st.LastUpdatedDate,
                                 st.DateCreated,
                             };

                if (result.Count() > 0)
                {
                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList()};
                }

                return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful, No Record Available", };

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
