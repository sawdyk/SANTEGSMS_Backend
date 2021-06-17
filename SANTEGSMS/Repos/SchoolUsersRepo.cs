using SANTEGSMS.DatabaseContext;
using SANTEGSMS.Entities;
using SANTEGSMS.Helpers;
using SANTEGSMS.IRepos;
using SANTEGSMS.RequestModels;
using SANTEGSMS.ResponseModels;
using SANTEGSMS.Reusables;
using SANTEGSMS.Services.Email;
using SANTEGSMS.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.Repos
{
    public class SchoolUsersRepo : ISchoolUsersRepo
    {
        private readonly AppDbContext _context;
        private readonly IEmailRepo _emailRepo;
        public SchoolUsersRepo(AppDbContext context, IEmailRepo emailRepo)
        {
            _context = context;
            _emailRepo = emailRepo;
        }

        //------------------------SchoolUsers------------------------------------------------------------------

        public async Task<SchoolUsersRespModel> createSchoolUsersAsync(SchoolUsersReqModel obj)
        {
            try
            {
                SchoolUsersInfoRespModel userRespData = new SchoolUsersInfoRespModel();
                SchoolBasicInfoLoginRespModel schData = new SchoolBasicInfoLoginRespModel();

                SchoolUsersRespModel respObj = new SchoolUsersRespModel();

                CheckerValidation checker = new CheckerValidation(_context);
                var emailCheckResult = checker.checkIfEmailExist(obj.Email, Convert.ToInt64(EnumUtility.UserCategoty.SchoolUsers));

                if (emailCheckResult == true)
                {
                    return new SchoolUsersRespModel { StatusCode = 409, StatusMessage = "This Email has been taken!" };
                }
                else
                {
                    var paswordHasher = new PasswordHasher();
                    //the salt
                    string salt = paswordHasher.getSalt();
                    //get deafault password
                    string password = DefaultPasswordReUsable.DefaultPassword();
                    //Hash the password and salt
                    string passwordHash = paswordHasher.hashedPassword(password, salt);

                    //save the SchoolAdmin details
                    var schUsr = new SchoolUsers
                    {
                        FirstName = obj.FirstName,
                        LastName = obj.LastName,
                        Email = obj.Email,
                        EmailConfirmed = true,
                        PhoneNumber = obj.PhoneNumber,
                        PhoneNumberConfirmed = false,
                        Salt = salt,
                        PasswordHash = passwordHash,
                        SchoolId = obj.SchoolId,
                        CampusId = obj.CampusId,
                        IsActive = true,
                        DateCreated = DateTime.Now
                    };

                    await _context.SchoolUsers.AddAsync(schUsr);
                    await _context.SaveChangesAsync();

                    //save the SchoolUser Roles
                    foreach (var roleId in obj.RoleIds)
                    {
                        var usrRol = new SchoolUserRoles
                        {
                            UserId = schUsr.Id,
                            RoleId = roleId.Id,
                            DateCreated = DateTime.Now
                        };

                        await _context.SchoolUserRoles.AddAsync(usrRol);
                        await _context.SaveChangesAsync();
                    }

                    //The data collected from the user 
                    userRespData.UserId = schUsr.Id.ToString();
                    userRespData.FirstName = schUsr.FirstName;
                    userRespData.LastName = schUsr.LastName;
                    userRespData.Email = schUsr.Email;

                    //Gets the School Information
                    var userSchool = _context.Schools.FirstOrDefault(u => u.Id == obj.SchoolId);
                    //Get the schoolType Name
                    var getSchType = _context.SchoolType.FirstOrDefault(u => u.Id == userSchool.SchoolTypeId);
                    //Get the Campus Name
                    var getCampus = _context.SchoolCampus.FirstOrDefault(u => u.Id == obj.CampusId);
                    //Get Roles Assigned to Teacher (TeacherRoles)
                    var getRoles = from rol in _context.SchoolUserRoles
                                   where rol.UserId == schUsr.Id
                                   select new
                                   {
                                       rol.Id,
                                       rol.UserId,
                                       rol.RoleId,
                                       rol.SchoolRoles.RoleName
                                   };

                    //school Details
                    schData.SchoolId = userSchool.Id;
                    schData.SchoolName = userSchool.SchoolName;
                    schData.SchoolCode = userSchool.SchoolCode;
                    schData.SchoolTypeName = getSchType.SchoolTypeName;
                    schData.CampusName = getCampus.CampusName;
                    schData.CampusAddress = getCampus.CampusAddress;

                    //The data to be sent as response
                    respObj.StatusCode = 200;
                    respObj.StatusMessage = "School User Created Successfully";
                    respObj.SchoolDetails = schData;
                    respObj.SchoolUserDetails = userRespData;
                    respObj.Roles = getRoles.ToList();

                }

                return respObj;
            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new SchoolUsersRespModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<SchoolUsersLoginRespModel> schoolUserLoginAsync(LoginReqModel obj)
        {
            try
            {
                //user data and schoolBasicInfo data objects
                SchoolBasicInfoLoginRespModel schData = new SchoolBasicInfoLoginRespModel();
                SchoolUsersInfoRespModel userData = new SchoolUsersInfoRespModel();

                //final data to be sent as response to the client
                SchoolUsersLoginRespModel respData = new SchoolUsersLoginRespModel();

                //Check if email exist
                CheckerValidation emailcheck = new CheckerValidation(_context);

                var getUser = _context.SchoolUsers.FirstOrDefault(u => u.Email == obj.Email);
                

                if (getUser != null)
                {
                    var accountCheckResult = emailcheck.checkIfAccountExistAndNotConfirmed(obj.Email, Convert.ToInt64(EnumUtility.UserCategoty.SchoolUsers));

                    //get the school user Role
                    var getSchUserRole = _context.SchoolUserRoles.Where(r => r.UserId == getUser.Id).FirstOrDefault();

                    long schUserRoleId = 0;

                    //get the school user RoleId
                    if (getSchUserRole != null)
                    {
                        schUserRoleId = (long)getSchUserRole.RoleId;
                    }

                    var paswordHasher = new PasswordHasher();
                    string salt = getUser.Salt; //gets the salt used to hash the user password
                    string decryptedPassword = paswordHasher.hashedPassword(obj.Password, salt); //decrypts the password

                    if (getUser != null && getUser.PasswordHash != decryptedPassword)
                    {
                        return new SchoolUsersLoginRespModel { StatusCode = 409, StatusMessage = "Invalid Username/Password!" };
                    }
                    else if (accountCheckResult == true && schUserRoleId == (int)EnumUtility.SchoolRoles.SuperAdministrator)
                    {
                        return new SchoolUsersLoginRespModel { StatusCode = 409, StatusMessage = "This Account Exist but has not been Activated!" };
                    }
                    else
                    {
                        //the userDetails
                        userData.UserId = getUser.Id.ToString();
                        userData.FirstName = getUser.FirstName;
                        userData.LastName = getUser.LastName;
                        userData.Email = getUser.Email;
                        userData.EmailConfirmed = getUser.EmailConfirmed;
                        userData.IsActive = getUser.IsActive;
                        userData.LastLoginDate = getUser.LastLoginDate;
                        userData.LastPasswordChangedDate = getUser.LastPasswordChangedDate;
                        userData.LastUpdatedDate = getUser.LastUpdatedDate;

                        //Gets the School Information
                        var userSchool = _context.Schools.FirstOrDefault(u => u.Id == getUser.SchoolId);
                        //Get the schoolType Name
                        var getSchType = _context.SchoolType.FirstOrDefault(u => u.Id == userSchool.SchoolTypeId);
                        //Get the Campus Name
                        var getCampus = _context.SchoolCampus.FirstOrDefault(u => u.Id == getUser.CampusId);

                        var getRole = from rol in _context.SchoolUserRoles
                                      where rol.UserId == getUser.Id
                                      select new
                                      {
                                          rol.UserId,
                                          rol.RoleId,
                                          rol.SchoolRoles.RoleName
                                      };

                        schData.SchoolId = userSchool.Id;
                        schData.SchoolName = userSchool.SchoolName;
                        schData.SchoolCode = userSchool.SchoolCode;
                        schData.SchoolTypeName = getSchType.SchoolTypeName;
                        schData.CampusId = getCampus.Id;
                        schData.CampusName = getCampus.CampusName;
                        schData.CampusAddress = getCampus.CampusAddress;

                        //The data to be sent as response
                        respData.StatusCode = 200;
                        respData.StatusMessage = "Login Successful";
                        respData.SchoolUserDetails = userData;
                        respData.schoolDetails = schData;
                        respData.Roles = getRole.ToList();
                    }
                }
                else
                {
                    return new SchoolUsersLoginRespModel { StatusCode = 409, StatusMessage = "Invalid Username/Password!" };
                }

                return respData;

            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new SchoolUsersLoginRespModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
                //}
            }
        }

        public async Task<GenericRespModel> getSchoolUsersByRoleIdAsync(long schoolId, long campusId, long roleId)
        {
            try
            {
                var result = from usRol in _context.SchoolUserRoles
                             join usr in _context.SchoolUsers on usRol.UserId equals usr.Id
                             where usr.SchoolId == schoolId
                             && usr.CampusId == campusId && usRol.RoleId == roleId
                             select usr;

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

        public async Task<GenericRespModel> updateSchoolUserDetailsAsync(Guid schoolUserId, UpdateSchoolUsersDetailsReqModel obj)
        {
            try
            {
                var getSchUser = _context.SchoolUsers.Where(s => s.Id == schoolUserId && s.SchoolId == obj.SchoolId && s.CampusId == obj.CampusId).FirstOrDefault();
                if (getSchUser != null)
                {
                    //get user by email address
                    var getUserByEmail = _context.SchoolUsers.Where(s => s.Email == obj.Email).FirstOrDefault();

                    if (getUserByEmail != null)
                    {
                        return new GenericRespModel { StatusCode = 409, StatusMessage = "A User with this Email Address Already Exists" };
                    }
                    else
                    {
                        getSchUser.FirstName = obj.FirstName;
                        getSchUser.LastName = obj.LastName;
                        getSchUser.Email = obj.Email;
                        getSchUser.PhoneNumber = obj.PhoneNumber;
                        getSchUser.SchoolId = obj.SchoolId;
                        getSchUser.CampusId = obj.CampusId;

                        await _context.SaveChangesAsync();

                        return new GenericRespModel { StatusCode = 200, StatusMessage = "User Details Updated Successfully", };
                    }
                }

                return new GenericRespModel { StatusCode = 200, StatusMessage = "No User With the Specified ID", };
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

        public async Task<GenericRespModel> deleteSchoolUsersAsync(Guid schoolUserId, long schoolId, long campusId)
        {
            try
            {
                var getSchUser = _context.SchoolUsers.Where(s => s.Id == schoolUserId && s.SchoolId == schoolId && s.CampusId == campusId).FirstOrDefault();
                if (getSchUser != null)
                {
                    _context.SchoolUsers.Remove(getSchUser);
                    await _context.SaveChangesAsync();

                    return new GenericRespModel { StatusCode = 200, StatusMessage = "User Details Deleted Successfully" };
                }

                return new GenericRespModel { StatusCode = 200, StatusMessage = "No User With the Specified ID" };
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
