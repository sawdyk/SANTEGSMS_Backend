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
    public class SchoolRepo : ISchoolRepo
    {
        private readonly AppDbContext _context;
        private readonly IEmailRepo _emailRepo;
        private readonly EmailTemplate _emailTemplate;

        public SchoolRepo(AppDbContext context, IEmailRepo emailRepo, EmailTemplate emailTemplate)
        {
            _context = context;
            _emailRepo = emailRepo;
            _emailTemplate = emailTemplate;
        }

        //------------------------School-----------------------------------------------------------------------

        public async Task<SchoolSignUpRespModel> schoolSignUpAsync(SchoolSignUpReqModel obj)
        {
            try
            {
                SchoolUsersInfoRespModel schUserRespData = new SchoolUsersInfoRespModel();
                SchoolBasicInfoRespModel schBasicRespData = new SchoolBasicInfoRespModel();

                SchoolSignUpRespModel respObj = new SchoolSignUpRespModel();

                CheckerValidation checker = new CheckerValidation(_context);
                var emailCheckResult = checker.checkIfEmailExist(obj.Email, Convert.ToInt64(EnumUtility.UserCategoty.SchoolUsers));

                var accountCheckResult = checker.checkIfAccountExistAndNotConfirmed(obj.Email, Convert.ToInt64(EnumUtility.UserCategoty.SchoolUsers));

                //the school name
                var schoolNameCheckResult = checker.checkIfSchoolNameExist(obj.SchoolName);
                //the school Code
                var schoolCodeCheckResult = checker.checkIfSchoolCodeExist(obj.SchoolCode);


                if (schoolNameCheckResult == true)
                {
                    return new SchoolSignUpRespModel { StatusCode = 409, StatusMessage = "This School Name Already Exists!" };
                }
                else if (schoolCodeCheckResult == true)
                {
                    return new SchoolSignUpRespModel { StatusCode = 409, StatusMessage = "A School Already Uses This Code!" };
                }
                else if (emailCheckResult == true)
                {
                    return new SchoolSignUpRespModel { StatusCode = 409, StatusMessage = "This Email has been taken!" };
                }
                //else if (accountCheckResult == true)
                //{
                //    return new SchoolSignUpRespModel { StatusCode = 409, StatusMessage = "This Account Exist but has not been Activated!" };
                //}
                else
                {
                    //school info
                    var newSch = new Schools
                    {
                        SchoolName = obj.SchoolName,
                        SchoolCode = obj.SchoolCode,
                        SchoolTypeId = obj.SchoolTypeId,
                        IsActive = false,
                        IsApproved = false,
                        IsVerified = false,
                        DateCreated = DateTime.Now
                    };
                    await _context.Schools.AddAsync(newSch);
                    await _context.SaveChangesAsync();


                    //Campus info
                    var camp = new SchoolCampus
                    {
                        SchoolId = newSch.Id,
                        CampusName = obj.CampusName,
                        CampusAddress = obj.CampusAddress,
                        IsActive = true,
                        DateCreated = DateTime.Now
                    };
                    await _context.SchoolCampus.AddAsync(camp);
                    await _context.SaveChangesAsync();

                    //Password Security
                    var paswordHasher = new PasswordHasher();
                    //the salt
                    string salt = paswordHasher.getSalt();
                    //Hash the password and salt
                    string passwordHash = paswordHasher.hashedPassword(obj.Password, salt);

                    //save the School User details
                    var schUsr = new SchoolUsers
                    {
                        FirstName = obj.FirstName,
                        LastName = obj.LastName,
                        Email = obj.Email,
                        EmailConfirmed = false, 
                        PhoneNumber = obj.PhoneNumber,
                        PhoneNumberConfirmed = false,
                        Salt = salt,
                        PasswordHash = passwordHash,
                        SchoolId = newSch.Id,
                        CampusId = camp.Id,
                        IsActive = true,
                        DateCreated = DateTime.Now
                    };

                    await _context.SchoolUsers.AddAsync(schUsr);
                    await _context.SaveChangesAsync();

                    //schoolUser Role
                    var usrRol = new SchoolUserRoles
                    {
                        UserId = schUsr.Id,
                        RoleId = Convert.ToInt64(EnumUtility.SchoolRoles.SuperAdministrator),
                        DateCreated = DateTime.Now
                    };

                    await _context.SchoolUserRoles.AddAsync(usrRol);
                    await _context.SaveChangesAsync();

                    //generate the code for email confirmation
                    var confirmationCode = new RandomNumberGenerator();
                    string codeGenerated = confirmationCode.randomCodesGen();

                    //save the code generated
                    var emailConfirmation = new EmailConfirmationCodes
                    {
                        UserId = schUsr.Id,
                        Code = codeGenerated,
                        DateGenerated = DateTime.Now
                    };
                    await _context.AddAsync(emailConfirmation);
                    await _context.SaveChangesAsync();

                    //The data collected from the user 
                    schUserRespData.UserId = schUsr.Id.ToString();
                    schUserRespData.FirstName = schUsr.FirstName;
                    schUserRespData.LastName = schUsr.LastName;
                    schUserRespData.Email = schUsr.Email;
                    schUserRespData.PhoneNumber = schUsr.PhoneNumber;

                    //Get the schoolType Name
                    var getSchType = _context.SchoolType.FirstOrDefault(u => u.Id == newSch.SchoolTypeId);

                    //The data collected from the School Information
                    schBasicRespData.SchoolSuperAdministratorId = schUsr.Id;
                    schBasicRespData.SchoolId = newSch.Id;
                    schBasicRespData.SchoolName = newSch.SchoolName;
                    schBasicRespData.SchoolCode = newSch.SchoolCode;
                    schBasicRespData.SchoolTypeId = (long)newSch.SchoolTypeId;
                    schBasicRespData.SchoolTypeName = getSchType.SchoolTypeName;

                    //The data to be sent as response
                    respObj.StatusCode = 200;
                    respObj.StatusMessage = "School created successfully, use the code sent to your mail to activate and verify your School account!";
                    respObj.SchoolUserDetails = schUserRespData;
                    respObj.SchoolDetails = schBasicRespData;


                    //code to send Mail to user for account activation
                    string mailSubject = "SANTEG SCHOOL MANAGEMENT SYSTEM - REGISTRATION";
                    string MailContent = _emailTemplate.EmailAccountCreation(codeGenerated);
                    EmailMessage message = new EmailMessage(schUsr.Email, MailContent);
                    _emailRepo.SendEmail(message);

                    //code to send Mail to system super admin for new school Created
                    var getSysAdmin = _context.SuperAdmin.FirstOrDefault();
                    if (getSysAdmin != null)
                    {
                        string mailSubjectSysAdmin = "SANTEG SCHOOL MANAGEMENT SYSTEM - REGISTRATION";
                        string systemAdminMail = _emailTemplate.EmailSchoolCreationNotify(schUsr.FirstName, schUsr.LastName, schUsr.Email, newSch.SchoolName, newSch.SchoolCode, getSchType.SchoolTypeName, DateTime.Now.ToString("MM/dd/yyy"));
                        EmailMessage messageSysAdmin = new EmailMessage(getSysAdmin.Email, systemAdminMail);
                        _emailRepo.SendEmail(messageSysAdmin);
                    }
                }

                return respObj;
            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new SchoolSignUpRespModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }

        }

        public async Task<GenericRespModel> activateAccountAsync(ActivateSchoolAccountReqModel obj)
        {
            try
            {
                CheckerValidation emailcheck = new CheckerValidation(_context);

                var emailCheckResult = emailcheck.checkIfEmailExist(obj.Email, Convert.ToInt64(EnumUtility.UserCategoty.SchoolUsers));
                var accountCheckResult = emailcheck.checkIfAccountExistAndNotConfirmed(obj.Email, Convert.ToInt64(EnumUtility.UserCategoty.SchoolUsers));

                if (emailCheckResult == true && accountCheckResult == false) //check if email exist and account has been verified and activated
                {
                    return new GenericRespModel { StatusCode = 409, StatusMessage = "This Account has been activated!" };
                }
                else if (emailCheckResult == true) //email exist
                {
                    SchoolUsers getUser = _context.SchoolUsers.FirstOrDefault(u => u.Email == obj.Email);

                    if (getUser != null)
                    {
                        EmailConfirmationCodes getUserCode = _context.EmailConfirmationCodes.FirstOrDefault(u => u.UserId == getUser.Id);
                        Schools getUserSch = _context.Schools.FirstOrDefault(u => u.Id == getUser.SchoolId);

                        if (getUserCode != null && getUserCode.Code == obj.Code.Trim())
                        {
                            getUser.EmailConfirmed = true; //Update the user account as confirmed (EmailConfirmed set to true)
                            getUserSch.IsActive = true; //Update the school IsActive to true

                            _context.EmailConfirmationCodes.Remove(getUserCode);
                            await _context.SaveChangesAsync();
                        }
                        else
                        {
                            return new GenericRespModel { StatusCode = 409, StatusMessage = "Invalid Code Entered!" };
                        }
                    }

                    //send mail for account activation
                    string mailSubject = "SANTEG SCHOOL MANAGEMENT SYSTEM - ACTIVATION";
                    string MailContent = _emailTemplate.EmailAccountActivation();
                    EmailMessage message = new EmailMessage(getUser.Email, MailContent);
                    _emailRepo.SendEmail(message);

                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Account Verification Successful!" };
                }

                else
                {
                    return new GenericRespModel { StatusCode = 409, StatusMessage = "This User doesnt exist!" };
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

        public async Task<GenericRespModel> resendActivationCodeAsync(string email)
        {
            try
            {
                CheckerValidation emailcheck = new CheckerValidation(_context);
                var emailCheckResult = emailcheck.checkIfEmailExist(email, Convert.ToInt64(EnumUtility.UserCategoty.SchoolUsers));
                var accountCheckResult = emailcheck.checkIfAccountExistAndNotConfirmed(email, Convert.ToInt64(EnumUtility.UserCategoty.SchoolUsers));

                if (emailCheckResult == true && accountCheckResult == false) //email exist and account is activated/Confirmed
                {
                    return new GenericRespModel { StatusCode = 409, StatusMessage = "This Account has been activated!" };
                }
                else if (emailCheckResult == true && accountCheckResult == true) //email exist and account is not activated/Confirmed
                {
                    SchoolUsers getUser = _context.SchoolUsers.FirstOrDefault(u => u.Email == email);
                    EmailConfirmationCodes getUserCode = _context.EmailConfirmationCodes.FirstOrDefault(u => u.UserId == getUser.Id);
                    string codeGenerated = string.Empty;

                    if (getUserCode != null)
                    {
                        //get the code previously generated if userId exist in the emailConfirmationcode table
                        codeGenerated = getUserCode.Code;

                        //send Mail to user for account activation
                        //send mail for account activation
                        string mailSubject = "SANTEG SCHOOL MANAGEMENT SYSTEM - ACTIVATION";
                        string MailContent = _emailTemplate.EmailAccountCreation(codeGenerated);
                        EmailMessage message = new EmailMessage(getUser.Email, MailContent);
                        _emailRepo.SendEmail(message);
                        
                        return new GenericRespModel { StatusCode = 200, StatusMessage = "Activation Code Sent Successfully"};
                    }
                    else
                    {
                        //generate a new code for email confirmation and account activation
                        var confirmationCode = new RandomNumberGenerator();
                        codeGenerated = confirmationCode.randomCodesGen();

                        //save the code generated
                        var emailConfirmation = new EmailConfirmationCodes
                        {
                            UserId = getUser.Id,
                            Code = codeGenerated,
                            DateGenerated = DateTime.Now
                        };
                        await _context.AddAsync(emailConfirmation);
                        await _context.SaveChangesAsync();

                        //send Mail to user for account activation
                        //send mail for account activation
                        string mailSubject = "SANTEG SCHOOL MANAGEMENT SYSTEM - ACTIVATION";
                        string MailContent = _emailTemplate.EmailAccountCreation(codeGenerated);
                        EmailMessage message = new EmailMessage(getUser.Email, MailContent);
                        _emailRepo.SendEmail(message);

                        return new GenericRespModel { StatusCode = 200, StatusMessage = "Activation Code Sent Successfully!" };

                    }
                }

                else
                {
                    return new GenericRespModel { StatusCode = 409, StatusMessage = "This User doesnt exist!" };
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

        public async Task<GenericRespModel> updateSchoolDetailsAsync(long schoolId, UpdateSchoolDetailsReqModel obj)
        {
            try
            {
                var getSch = _context.Schools.Where(s => s.Id == schoolId).FirstOrDefault();
                if (getSch != null)
                {
                    CheckerValidation chk = new CheckerValidation(_context);
                    var schNameExist = chk.checkIfSchoolNameExist(obj.SchoolName);

                    if (schNameExist == true)
                    {
                        return new GenericRespModel { StatusCode = 409, StatusMessage = "A School with this Name Already Exists" };
                    }
                    else
                    {
                        getSch.SchoolName = obj.SchoolName;
                        getSch.SchoolLogoUrl = obj.SchoolLogouRL;

                        await _context.SaveChangesAsync();

                        return new GenericRespModel { StatusCode = 200, StatusMessage = "School Details Updated Successfully" };
                    }
                }

                return new GenericRespModel { StatusCode = 200, StatusMessage = "No School With the Specified ID" };
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

        public async Task<GenericRespModel> getSchoolResourcesAsync(long schoolId)
        {
            try
            {
                var getSch = _context.Schools.Where(s => s.Id == schoolId).FirstOrDefault();
                if (getSch != null)
                {
                    var result = from rsc in _context.SchoolResources
                                 select new
                                 {
                                     rsc.Id,
                                     rsc.ResourceName,
                                     rsc.ResourceLink
                                 };

                    if (result.Count() > 0)
                    {
                        return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList(), };
                    }

                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful, No Record Available", };
                }

                return new GenericRespModel { StatusCode = 200, StatusMessage = "No School With the Specified ID" };
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


        public async Task<GenericRespModel> enableOrDisableStaffAsync(bool isEnabled, Guid schoolUserId, long schoolId, long campusId)
        {
            try
            {
                SuperAdminInfoRespModel userData = new SuperAdminInfoRespModel();
                var response = new GenericRespModel();
                //Check if email exist
                CheckerValidation schoolCheck = new CheckerValidation(_context);
                var checkSchool = schoolCheck.checkSchoolById(schoolId);
                var checkCampus = schoolCheck.checkSchoolCampusById(campusId);

                if (checkSchool == true && checkCampus == true)
                {
                    var getSchUser = _context.SchoolUsers.Where(u => u.Id == schoolUserId && u.SchoolId == schoolId && u.CampusId == campusId).FirstOrDefault();

                    if (getSchUser != null)
                    {
                        string statusMessage = string.Empty;
                        string notification = string.Empty;

                        if (isEnabled == true)
                        {
                            getSchUser.IsActive = true;
                            await _context.SaveChangesAsync();

                            statusMessage = "Enabled";
                            notification = "Your School User Account has been Enabled Successfully!";

                            //code to send Mail to user for account activation
                            string mailSubject = "SANTEG SCHOOL MANAGEMENT SYSTEM";
                            string MailContent = _emailTemplate.EmailSchoolUsersApproved(statusMessage, notification, getSchUser.FirstName, getSchUser.LastName);
                            EmailMessage message = new EmailMessage(getSchUser.Email, MailContent);
                            _emailRepo.SendEmail(message);

                            //add email logs here!

                            //response
                            response.StatusCode = 200;
                            response.StatusMessage = $"{getSchUser.FirstName + " " + getSchUser.LastName} School User Account Enabled Successfully!";
                            response.Data = getSchUser.Email;
                        }
                        else
                        {
                            getSchUser.IsActive = false;
                            await _context.SaveChangesAsync();

                            statusMessage = "Disabled";
                            notification = "Your School User Account has been Disabled, Kindly Contact the School Administrator for Further Details!";
                            string mailSubject = "SANTEG SCHOOL MANAGEMENT SYSTEM";
                            //code to send Mail to user for account activation
                            string MailContent = _emailTemplate.EmailSchoolUsersApproved(statusMessage, notification, getSchUser.FirstName, getSchUser.LastName);
                            EmailMessage message = new EmailMessage(getSchUser.Email, MailContent);
                            _emailRepo.SendEmail(message);

                            //response
                            response.StatusCode = 200;
                            response.StatusMessage = $"{getSchUser.FirstName + " " + getSchUser.LastName} School User Account Disabled Successfully!";
                            response.Data = getSchUser.Email;
                        }
                    }
                    else
                    {
                        return new GenericRespModel { StatusCode = 409, StatusMessage = "No School User with the specified ID!" };
                    }
                }
                else
                {
                    return new GenericRespModel { StatusCode = 409, StatusMessage = "No School/Campus with the specified ID!" };
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

        public async Task<GenericRespModel> enableOrDisableStudentAsync(bool isEnabled, Guid studentId, long schoolId, long campusId)
        {
            try
            {
                SuperAdminInfoRespModel userData = new SuperAdminInfoRespModel();
                var response = new GenericRespModel();
                //Check if email exist
                CheckerValidation schoolCheck = new CheckerValidation(_context);
                var checkSchool = schoolCheck.checkSchoolById(schoolId);
                var checkCampus = schoolCheck.checkSchoolCampusById(campusId);

                if (checkSchool == true && checkCampus == true)
                {
                    var getStudent = _context.Students.Where(u => u.Id == studentId && u.SchoolId == schoolId && u.CampusId == campusId).FirstOrDefault();

                    if (getStudent != null)
                    {
                        var getParent = _context.ParentsStudentsMap.FirstOrDefault(u => u.StudentId == getStudent.Id);

                        if (getParent != null)
                        {
                            var parentDetails = _context.Parents.FirstOrDefault(u => u.Id == getParent.ParentId);

                            if (parentDetails != null)
                            {
                                string statusMessage = string.Empty;
                                string notification = string.Empty;

                                if (isEnabled == true)
                                {
                                    getStudent.IsActive = true;
                                    await _context.SaveChangesAsync();

                                    statusMessage = "Enabled";
                                    notification = $"Your Child/Ward {getStudent.FirstName + " " + getStudent.LastName} School User Account has been Enabled Successfully!";

                                    //code to send Mail to user for account activation
                                    string mailSubject = "SANTEG SCHOOL MANAGEMENT SYSTEM";
                                    string MailContent = _emailTemplate.EmailSchoolUsersApproved(statusMessage, notification, parentDetails.FirstName, parentDetails.LastName);
                                    EmailMessage message = new EmailMessage(parentDetails.Email, MailContent);
                                    _emailRepo.SendEmail(message);

                                    //add email logs here!

                                    //response
                                    response.StatusCode = 200;
                                    response.StatusMessage = $"Your Child/Ward {getStudent.FirstName + " " + getStudent.LastName} School User Account has been Enabled Successfully!";
                                    response.Data = parentDetails.Email;
                                }
                                else
                                {
                                    getStudent.IsActive = false;
                                    await _context.SaveChangesAsync();

                                    statusMessage = "Disabled";
                                    notification = $"Your Child/Ward {getStudent.FirstName + " " + getStudent.LastName} School User Account has been Disabled, Kindly Contact the School Administrator for Further Details!";
                                    string mailSubject = "SANTEG SCHOOL MANAGEMENT SYSTEM";
                                    //code to send Mail to user for account activation
                                    string MailContent = _emailTemplate.EmailSchoolUsersApproved(statusMessage, notification, parentDetails.FirstName, parentDetails.LastName);
                                    EmailMessage message = new EmailMessage(parentDetails.Email, MailContent);
                                    _emailRepo.SendEmail(message);

                                    //response
                                    response.StatusCode = 200;
                                    response.StatusMessage = $"Your Child/Ward {getStudent.FirstName + " " + getStudent.LastName} School User Account has been Disabled Successfully!";
                                    response.Data = parentDetails.Email;
                                }
                            }
                        }
                    }
                    else
                    {
                        return new GenericRespModel { StatusCode = 409, StatusMessage = "No Student with the specified ID!" };
                    }
                }
                else
                {
                    return new GenericRespModel { StatusCode = 409, StatusMessage = "No School/Campus with the specified ID!" };
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

        public async Task<GenericRespModel> enableOrDisableParentAsync(bool isEnabled, Guid parentId, long schoolId, long campusId)
        {
            try
            {
                SuperAdminInfoRespModel userData = new SuperAdminInfoRespModel();
                var response = new GenericRespModel();
                //Check if email exist
                CheckerValidation schoolCheck = new CheckerValidation(_context);
                var checkSchool = schoolCheck.checkSchoolById(schoolId);
                var checkCampus = schoolCheck.checkSchoolCampusById(campusId);

                if (checkSchool == true && checkCampus == true)
                {
                    var getParent = _context.Parents.Where(u => u.Id == parentId && u.SchoolId == schoolId && u.CampusId == campusId).FirstOrDefault();

                    if (getParent != null)
                    {
                        string statusMessage = string.Empty;
                        string notification = string.Empty;

                        if (isEnabled == true)
                        {
                            getParent.IsActive = true;
                            await _context.SaveChangesAsync();

                            statusMessage = "Enabled";
                            notification = "Your School User Account has been Enabled Successfully!";

                            //code to send Mail to user for account activation
                            string mailSubject = "SANTEG SCHOOL MANAGEMENT SYSTEM";
                            string MailContent = _emailTemplate.EmailSchoolUsersApproved(statusMessage, notification, getParent.FirstName, getParent.LastName);
                            EmailMessage message = new EmailMessage(getParent.Email, MailContent);
                            _emailRepo.SendEmail(message);

                            //add email logs here!

                            //response
                            response.StatusCode = 200;
                            response.StatusMessage = $"{getParent.FirstName + " " + getParent.LastName} School User Account Enabled Successfully!";
                            response.Data = getParent.Email;
                        }
                        else
                        {
                            getParent.IsActive = false;
                            await _context.SaveChangesAsync();

                            statusMessage = "Disabled";
                            notification = "Your School User Account has been Disabled, Kindly Contact the School Administrator for Further Details!";
                            string mailSubject = "SANTEG SCHOOL MANAGEMENT SYSTEM";
                            //code to send Mail to user for account activation
                            string MailContent = _emailTemplate.EmailSchoolUsersApproved(statusMessage, notification, getParent.FirstName, getParent.LastName);
                            EmailMessage message = new EmailMessage(getParent.Email, MailContent);
                            _emailRepo.SendEmail(message);

                            //response
                            response.StatusCode = 200;
                            response.StatusMessage = $"{getParent.FirstName + " " + getParent.LastName} School User Account Disabled Successfully!";
                            response.Data = getParent.Email;
                        }
                    }
                    else
                    {
                        return new GenericRespModel { StatusCode = 409, StatusMessage = "No Parent with the specified ID!" };
                    }
                }
                else
                {
                    return new GenericRespModel { StatusCode = 409, StatusMessage = "No School/Campus with the specified ID!" };
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
    }
}
