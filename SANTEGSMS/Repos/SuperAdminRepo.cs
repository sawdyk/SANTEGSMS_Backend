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
    public class SuperAdminRepo : ISuperAdminRepo
    {
        private readonly AppDbContext _context;
        private readonly IEmailRepo _emailRepo;
        private readonly EmailTemplate _emailTemplate;

        public SuperAdminRepo(AppDbContext context, IEmailRepo emailRepo, EmailTemplate emailTemplate)
        {
            _context = context;
            _emailRepo = emailRepo;
            _emailTemplate = emailTemplate;
        }
        public async Task<GenericRespModel> superAdminLoginAsync(LoginReqModel obj)
        {
            try
            {
                SuperAdminInfoRespModel userData = new SuperAdminInfoRespModel();
                var response = new GenericRespModel();
                //Check if email exist
                CheckerValidation emailcheck = new CheckerValidation(_context);

                var getUser = _context.SuperAdmin.FirstOrDefault(u => u.Email == obj.Email);

                if (getUser != null)
                {
                    var paswordHasher = new PasswordHasher();
                    string salt = getUser.Salt; //gets the salt used to hash the user password
                    string decryptedPassword = paswordHasher.hashedPassword(obj.Password, salt); //decrypts the password

                    if (getUser != null && getUser.PasswordHash != decryptedPassword)
                    {
                        return new GenericRespModel { StatusCode = 409, StatusMessage = "Invalid Username/Password!" };
                    }
                    else
                    {
                        //the userDetails
                        userData.UserId = getUser.Id.ToString();
                        userData.FirstName = getUser.FirstName;
                        userData.LastName = getUser.LastName;
                        userData.Email = getUser.Email;

                        //response
                        response.StatusCode = 200;
                        response.StatusMessage = "Login Successful";
                        response.Data = userData;
                    }
                }
                else
                {
                    return new GenericRespModel { StatusCode = 409, StatusMessage = "Invalid Username/Password!" };
                }

                return response;

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

        public async Task<GenericRespModel> getAllSchoolsAsync()
        {
            try
            {
                var result = from cl in _context.Schools
                             select new
                             {
                                 cl.Id,
                                 cl.SchoolName,
                                 cl.SchoolCode,
                                 cl.SchoolLogoUrl,
                                 cl.SchoolType.SchoolTypeName,
                                 cl.States.StateName,
                                 cl.LocalGovt.LocalGovtName,
                                 cl.District.DistrictName,
                                 DistricAdministratorFullName = cl.District.DistrictAdministrators.FirstName +" "+ cl.District.DistrictAdministrators.LastName,
                                 cl.IsActive,
                                 cl.IsApproved,
                                 cl.IsVerified,
                                 cl.DateCreated,
                                 cl.DateUpdated,
                             };

                if (result.Count() > 0)
                {
                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList()};
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

        public async Task<GenericRespModel> forgotPasswordAsync(string email)
        {
            try
            {
                SuperAdminForgotPasswordInfoRespModel userData = new SuperAdminForgotPasswordInfoRespModel();
                var response = new GenericRespModel();
                //Check if email exist
                CheckerValidation emailcheck = new CheckerValidation(_context);

                var getUser = _context.SuperAdmin.FirstOrDefault(u => u.Email == email);

                if (getUser != null)
                {
                    var paswordHasher = new PasswordHasher();
                    //the salt
                    string salt = paswordHasher.getSalt();
                    //get deafault password
                    string password = RandomNumberGenerator.RandomString();
                    //Hash the password and salt
                    string passwordHash = paswordHasher.hashedPassword(password, salt);

                    //the userDetails
                    userData.UserId = getUser.Id.ToString();
                    userData.FirstName = getUser.FirstName;
                    userData.LastName = getUser.LastName;
                    userData.Email = getUser.Email;
                    userData.Password = password;

                    getUser.Salt = salt;
                    getUser.PasswordHash = passwordHash;
                    await _context.SaveChangesAsync();

                    //response
                    response.StatusCode = 200;
                    response.StatusMessage = "Default Password Generated Successfully, Kindly Change Password after Login!";
                    response.Data = userData;

                }
                else
                {
                    return new GenericRespModel { StatusCode = 409, StatusMessage = "Invalid User!" };
                }

                return response;

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

        public async Task<GenericRespModel> changePasswordAsync(string email, string oldPassword, string newPassword)
        {
            try
            {
                SuperAdminInfoRespModel userData = new SuperAdminInfoRespModel();
                var response = new GenericRespModel();
                //Check if email exist
                CheckerValidation emailcheck = new CheckerValidation(_context);

                var getUser = _context.SuperAdmin.FirstOrDefault(u => u.Email == email);

                if (getUser != null)
                {
                    var paswordHasher = new PasswordHasher();
                    string salt = getUser.Salt; //gets the salt used to hash the user password
                    string decryptedPassword = paswordHasher.hashedPassword(oldPassword, salt); //decrypts the password

                    if (getUser.PasswordHash != decryptedPassword)
                    {
                        return new GenericRespModel { StatusCode = 409, StatusMessage = "Old Password MisMatch!" };
                    }
                    else
                    {
                        var paswordHasher2 = new PasswordHasher();
                        //the salt
                        string salt2 = paswordHasher2.getSalt();
                        //Hash the password and salt
                        string passwordHash = paswordHasher2.hashedPassword(newPassword, salt2);

                        getUser.Salt = salt2;
                        getUser.PasswordHash = passwordHash;
                        await _context.SaveChangesAsync();

                        //response
                        response.StatusCode = 200;
                        response.StatusMessage = "Password Chnaged Successfully!";
                    }
                }
                else
                {
                    return new GenericRespModel { StatusCode = 409, StatusMessage = "Invalid User!" };
                }

                return response;

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

        public async Task<GenericRespModel> approveOrDeclineSchoolCreationAsync(bool isApproved, long schoolId)
        {
            try
            {
                SuperAdminInfoRespModel userData = new SuperAdminInfoRespModel();
                var response = new GenericRespModel();
                //Check if email exist
                CheckerValidation schoolCheck = new CheckerValidation(_context);
                var checkSchool = schoolCheck.checkSchoolById(schoolId);

                if (checkSchool == true)
                {
                    var getSch = _context.Schools.FirstOrDefault(u => u.Id == schoolId);

                    var superAdminsList = _context.SchoolUserRoles.Where(u => u.RoleId == (int)EnumUtility.SchoolRoles.SuperAdministrator).ToList();

                    var superAdminEmail = string.Empty;

                    foreach (SchoolUserRoles rol in superAdminsList)
                    {
                        var getEmail = _context.SchoolUsers.Where(u => u.Id == rol.UserId && u.SchoolId == schoolId).FirstOrDefault();
                        if (getEmail != null)
                        {
                            superAdminEmail = getEmail.Email;
                            break;
                        }
                    }

                    var getSchUserRole = _context.SchoolUserRoles.FirstOrDefault(u => u.Id == schoolId);
                    string statusMessage = string.Empty;
                    string notification = string.Empty;

                    if (isApproved == true)
                    {
                        getSch.IsActive = true;
                        getSch.IsVerified = true;
                        getSch.IsApproved = true;

                        await _context.SaveChangesAsync();

                        statusMessage = "Verified and Approved";
                        notification = "Kindly login to your Dashboard and explore the SANTEG School Management System Features!";

                        //code to send Mail to user for account activation
                        string mailSubject = "SANTEG SCHOOL MANAGEMENT SYSTEM - APPROVED/DECLINED";
                        string MailContent = _emailTemplate.EmailSchoolCreationApproved(statusMessage, notification);
                        EmailMessage message = new EmailMessage(superAdminEmail, MailContent);
                        _emailRepo.SendEmail(message);

                        //add email logs here!

                        //response
                        response.StatusCode = 200;
                        response.StatusMessage = $"{getSch.SchoolName} Approved and Activated Successfully!";
                        response.Data = superAdminEmail;
                    }
                    else
                    {
                        getSch.IsActive = false;
                        getSch.IsVerified = false;
                        getSch.IsApproved = false;

                        await _context.SaveChangesAsync();

                        statusMessage = "Declined";
                        notification = "Kindly Contact SANTEG Administrator for Declination Reasons!";
                        string mailSubject = "SANTEG SCHOOL MANAGEMENT SYSTEM - APPROVED/DECLINED";
                        //code to send Mail to user for account activation
                        string MailContent = _emailTemplate.EmailSchoolCreationApproved(statusMessage, notification);
                        EmailMessage message = new EmailMessage(superAdminEmail, MailContent);
                        _emailRepo.SendEmail(message);

                        //response
                        response.StatusCode = 200;
                        response.StatusMessage = $"{getSch.SchoolName} Declined Successfully!";
                        response.Data = superAdminEmail;
                    }
                }
                else
                {
                    return new GenericRespModel { StatusCode = 409, StatusMessage = "No School with the specified ID!" };
                }

                return response;

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

        public async Task<GenericRespModel> enableOrDisableSchoolAccountAsync(bool isEnabled, long schoolId)
        {
            try
            {
                SuperAdminInfoRespModel userData = new SuperAdminInfoRespModel();
                var response = new GenericRespModel();
                //Check if email exist
                CheckerValidation schoolCheck = new CheckerValidation(_context);
                var checkSchool = schoolCheck.checkSchoolById(schoolId);

                if (checkSchool == true)
                {
                    var getSch = _context.Schools.FirstOrDefault(u => u.Id == schoolId);

                    var superAdminsList = _context.SchoolUserRoles.Where(u => u.RoleId == (int)EnumUtility.SchoolRoles.SuperAdministrator).ToList();

                    var superAdminEmail = string.Empty;

                    foreach (SchoolUserRoles rol in superAdminsList)
                    {
                        var getEmail = _context.SchoolUsers.Where(u => u.Id == rol.UserId && u.SchoolId == schoolId).FirstOrDefault();
                        if (getEmail != null)
                        {
                            superAdminEmail = getEmail.Email;
                            break;
                        }
                    }

                    var getSchUserRole = _context.SchoolUserRoles.FirstOrDefault(u => u.Id == schoolId);
                    string statusMessage = string.Empty;
                    string notification = string.Empty;

                    if (isEnabled == true)
                    {
                        getSch.IsActive = true;
                        await _context.SaveChangesAsync();

                        statusMessage = "Enabled";
                        notification = "Your School Account has been Enabled Successfully!";

                        //code to send Mail to user for account activation
                        string mailSubject = "SANTEG SCHOOL MANAGEMENT SYSTEM - SCHOOL ACCOUNT";
                        string MailContent = _emailTemplate.EmailSchoolCreationApproved(statusMessage, notification);
                        EmailMessage message = new EmailMessage(superAdminEmail, MailContent);
                        _emailRepo.SendEmail(message);

                        //add email logs here!

                        //response
                        response.StatusCode = 200;
                        response.StatusMessage = $"{getSch.SchoolName} Account Enabled Successfully!";
                        response.Data = superAdminEmail;
                    }
                    else
                    {
                        getSch.IsActive = false;
                        await _context.SaveChangesAsync();

                        statusMessage = "Disabled";
                        notification = "Your School Account has been Disabled, Kindly Contact the Sytem Administrator for Further Details!";
                        string mailSubject = "SANTEG SCHOOL MANAGEMENT SYSTEM - SCHOOL ACCOUNT";
                        //code to send Mail to user for account activation
                        string MailContent = _emailTemplate.EmailSchoolCreationApproved(statusMessage, notification);
                        EmailMessage message = new EmailMessage(superAdminEmail, MailContent);
                        _emailRepo.SendEmail(message);

                        //response
                        response.StatusCode = 200;
                        response.StatusMessage = $"{getSch.SchoolName} Account Disabled Successfully!";
                        response.Data = superAdminEmail;
                    }
                }
                else
                {
                    return new GenericRespModel { StatusCode = 409, StatusMessage = "No School with the specified ID!" };
                }

                return response;

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

        public async Task<GenericRespModel> updateSuperAdminDetailsAsync(Guid superAdminId, UpdateSuperAdminReqModel obj)
        {
            try
            {
                SuperAdminInfoRespModel userData = new SuperAdminInfoRespModel();
                var response = new GenericRespModel();
                var getAdmin = _context.SuperAdmin.Where(s => s.Id == superAdminId).FirstOrDefault();

                if (getAdmin != null)
                {
                    getAdmin.FirstName = obj.FirstName;
                    getAdmin.LastName = obj.LastName;
                    getAdmin.Email = obj.Email;
                    getAdmin.PhoneNumber = obj.PhoneNumber;

                    await _context.SaveChangesAsync();

                    var superAdm = from s in _context.SuperAdmin
                                   where s.Id == getAdmin.Id
                                   select new
                                   {
                                       s.Id,
                                       s.FirstName,
                                       s.LastName,
                                       s.Email,
                                       s.PhoneNumber,
                                       s.DateCreated
                                   };

                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Super Admin Details Updated Successfully!", Data = superAdm };
                }
                else
                {
                    return new GenericRespModel { StatusCode = 409, StatusMessage = "No Super Admin with the specified ID!" };
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
    }
}
