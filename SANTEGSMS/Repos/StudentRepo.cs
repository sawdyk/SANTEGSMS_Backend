using OfficeOpenXml;
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
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SANTEGSMS.Services.Email;

namespace SANTEGSMS.Repos
{
    public class StudentRepo : IStudentRepo
    {
        private readonly AppDbContext _context;
        private readonly IEmailRepo _emailRepo;
        private readonly EmailTemplate _emailTemplate;
        public StudentRepo(AppDbContext context, IEmailRepo emailRepo, EmailTemplate emailTemplate)
        {
            _context = context;
            _emailRepo = emailRepo;
            _emailTemplate = emailTemplate;
        }
        public async Task<GenericRespModel> createStudentAsync(StudentCreationReqModel obj)
        {
            try
            {
                //check if the parent email exist
                CheckerValidation check = new CheckerValidation(_context);
                Parents parentExistsInSchool = _context.Parents.Where(u => u.Email == obj.ParentEmail && u.SchoolId == obj.SchoolId).FirstOrDefault();
                Parents parentEmailExists = _context.Parents.Where(u => u.Email == obj.ParentEmail).FirstOrDefault();

                //check for exsting students in the school
                var getStudentIfExist = _context.Students.Where(x => x.LastName == obj.StudentLastName
                                        && x.FirstName == obj.StudentFirstName
                                        && x.MiddleName == obj.MiddleName
                                        && x.SchoolId == obj.SchoolId
                                        && x.CampusId == obj.CampusId).FirstOrDefault();

                //check if the student admissionNumber exist
                //var studentAdmissionNumberCheck = check.checkStudentByAdmissionNumber(obj.AdmissionNumber);

                if (parentExistsInSchool != null)
                {
                    return new GenericRespModel { StatusCode = 409, StatusMessage = "This Parent Email Address already exist in the school, Kindly add the student to the exsting parent details!" };
                }
                else if (parentEmailExists != null)
                {
                    return new GenericRespModel { StatusCode = 409, StatusMessage = "This Parent Email Address already exist for Another School, Kinldy use a different Email Address!" };
                }
                else if (getStudentIfExist != null)
                {
                    return new GenericRespModel { StatusCode = 409, StatusMessage = $"Student With this FullName: {obj.StudentLastName + " " + obj.StudentFirstName + " " + obj.MiddleName} Already exists, Kindly Update the Student Class and ClassGrade" };
                }
                else
                {
                    //default password
                    string defaultPassword = DefaultPasswordReUsable.DefaultPassword();

                    var paswordHasher = new PasswordHasher();
                    //the salt
                    string salt = paswordHasher.getSalt();
                    //Hash the password and salt
                    string passwordHash = paswordHasher.hashedPassword(defaultPassword, salt);

                    //student AdmissionNumber/Username
                    string admissionNumber = new AdmissionNumberGenerator(_context).GenerateAdmissionNumber(obj.SchoolId);

                    //Save the Student details
                    var std = new Students
                    {
                        LastName = obj.StudentLastName,
                        FirstName = obj.StudentFirstName,
                        MiddleName = obj.MiddleName,
                        UserName = admissionNumber,
                        AdmissionNumber = admissionNumber,
                        Salt = salt,
                        PasswordHash = passwordHash,
                        SchoolId = obj.SchoolId,
                        CampusId = obj.CampusId,
                        GenderId = obj.GenderId,
                        StaffStatus = 0,
                        DateOfBirth = Convert.ToDateTime(obj.DateOfBirth),
                        YearOfAdmission = obj.YearOfAdmission,
                        StateOfOrigin = obj.StateOfOrigin,
                        LocalGovt = obj.LocalGovt,
                        Religion = obj.Religion,
                        HomeAddress = obj.HomeAddress,
                        City = obj.City,
                        State = obj.State,
                        StudentStatus = "",
                        ProfilePictureUrl = obj.ProfilePictureUrl,
                        Status = "",
                        IsAssignedToClass = false,
                        hasParent = true,
                        IsActive = true,
                        DateCreated = DateTime.Now

                    };

                    await _context.Students.AddAsync(std);
                    await _context.SaveChangesAsync();

                    //Save the Parent details
                    var prt = new Parents
                    {
                        FirstName = obj.ParentFirstName,
                        LastName = obj.ParentLastName,
                        Email = obj.ParentEmail,
                        EmailConfirmed = false,
                        PhoneNumber = obj.ParentPhoneNumber,
                        Salt = salt,
                        PasswordHash = passwordHash,
                        SchoolId = obj.SchoolId,
                        CampusId = obj.CampusId,
                        UserName = obj.ParentEmail,
                        GenderId = obj.ParentGenderId,
                        Nationality = obj.ParentNationality,
                        State = obj.ParentState,
                        City = obj.ParentCity,
                        HomeAddress = obj.ParentHomeAddress,
                        Occupation = obj.ParentOccupation,
                        StateOfOrigin = obj.ParentStateOfOrigin,
                        LocalGovt = obj.ParentLocalGovt,
                        Religion = obj.ParentReligion,
                        hasChild = true,
                        IsActive = true,
                        DateCreated = DateTime.Now
                    };

                    await _context.Parents.AddAsync(prt);
                    await _context.SaveChangesAsync();

                    //map student and parent
                    var mapp = new ParentsStudentsMap
                    {
                        ParentId = prt.Id,
                        StudentId = std.Id,
                        SchoolId = obj.SchoolId,
                        CampusId = obj.CampusId,
                        DateCreated = DateTime.Now
                    };
                    await _context.ParentsStudentsMap.AddAsync(mapp);
                    await _context.SaveChangesAsync();

                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Student Created Successfully!" };

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

        public async Task<GenericRespModel> addStudentToExistingParentAsync(StudentParentExistCreationReqModel obj)
        {
            try
            {
                CheckerValidation check = new CheckerValidation(_context);
                var checkSch = check.checkSchoolById(obj.SchoolId);
                var checkCamp = check.checkSchoolCampusById(obj.CampusId);
                var checkPrt = check.checkParentById(obj.ParentId);

                if (checkSch != true)
                {
                    return new GenericRespModel { StatusCode = 409, StatusMessage = "No School with the specified ID" };
                }
                if (checkCamp != true)
                {
                    return new GenericRespModel { StatusCode = 409, StatusMessage = "No School Campus with the specified ID" };
                }
                if (checkPrt != true)
                {
                    return new GenericRespModel { StatusCode = 409, StatusMessage = "No Parent with the specified ID" };
                }
                else
                {
                    //default password
                    string defaultPassword = DefaultPasswordReUsable.DefaultPassword();

                    var paswordHasher = new PasswordHasher();
                    //the salt
                    string salt = paswordHasher.getSalt();
                    //Hash the password and salt
                    string passwordHash = paswordHasher.hashedPassword(defaultPassword, salt);

                    //student AdmissionNumber/Username
                    string admissionNumber = new AdmissionNumberGenerator(_context).GenerateAdmissionNumber(obj.SchoolId);

                    //Save the Student details
                    var std = new Students
                    {
                        FirstName = obj.FirstName,
                        LastName = obj.LastName,
                        UserName = admissionNumber,
                        AdmissionNumber = admissionNumber,
                        Salt = salt,
                        PasswordHash = passwordHash,
                        SchoolId = obj.SchoolId,
                        CampusId = obj.CampusId,
                        GenderId = obj.GenderId,
                        StaffStatus = 0,
                        DateOfBirth = Convert.ToDateTime(obj.DateOfBirth),
                        YearOfAdmission = obj.YearOfAdmission,
                        StateOfOrigin = obj.StateOfOrigin,
                        LocalGovt = obj.LocalGovt,
                        Religion = obj.Religion,
                        HomeAddress = obj.HomeAddress,
                        City = obj.City,
                        State = obj.State,
                        StudentStatus = "",
                        ProfilePictureUrl = "",
                        Status = "",
                        IsAssignedToClass = false,
                        hasParent = true,
                        IsActive = true,
                        DateCreated = DateTime.Now
                    };

                    await _context.Students.AddAsync(std);
                    await _context.SaveChangesAsync();

                    //get the parent details
                    var parentDetails = _context.Parents.Where(x => x.Id == obj.ParentId).FirstOrDefault();

                    //map student and parent
                    var mapp = new ParentsStudentsMap
                    {
                        ParentId = parentDetails.Id,
                        StudentId = std.Id,
                        SchoolId = obj.SchoolId,
                        CampusId = obj.CampusId,
                        DateCreated = DateTime.Now
                    };
                    await _context.ParentsStudentsMap.AddAsync(mapp);
                    await _context.SaveChangesAsync();

                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Student Created Successfully!" };

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

        public async Task<SchoolUsersLoginRespModel> studentLoginAsync(StudentLoginReqModel obj)
        {
            try
            {
                //user data and schoolBasicInfo data objects
                SchoolBasicInfoLoginRespModel schData = new SchoolBasicInfoLoginRespModel();
                StudentInfoRespModel userData = new StudentInfoRespModel();

                //final data to be sent as response 
                SchoolUsersLoginRespModel respData = new SchoolUsersLoginRespModel();

                //Check if email exist
                CheckerValidation emailcheck = new CheckerValidation(_context);

                var getUser = _context.Students.FirstOrDefault(u => u.UserName == obj.Username);

                if (getUser != null)
                {
                    var paswordHasher = new PasswordHasher();
                    string salt = getUser.Salt; //gets the salt used to hash the user password
                    string decryptedPassword = paswordHasher.hashedPassword(obj.Password, salt); //decrypts the password

                    //get the student school
                    var getSch = _context.Schools.FirstOrDefault(u => u.Id == getUser.SchoolId);

                    if (getUser != null && getUser.PasswordHash != decryptedPassword)
                    {
                        return new SchoolUsersLoginRespModel { StatusCode = 409, StatusMessage = "Invalid Username/Password!" };
                    }
                    else if (getUser.IsActive != true)
                    {
                        return new SchoolUsersLoginRespModel { StatusCode = 409, StatusMessage = "Your Student Account has been deactivated, Kindly Contact your Admninistrator!" };
                    }
                    else if (getSch.IsApproved != true)
                    {
                        return new SchoolUsersLoginRespModel { StatusCode = 409, StatusMessage = "This School has not been Verified and Approved by the System Super Admninistrator!" };
                    }
                    else if (getSch.IsActive != true)
                    {
                        return new SchoolUsersLoginRespModel { StatusCode = 409, StatusMessage = "This School Accoun has been Disabled by the System Admninistrator, Kindly Contact the System Admninistrator!" };
                    }
                    else
                    {

                        //Gets the School Information
                        var userSchool = _context.Schools.FirstOrDefault(u => u.Id == getUser.SchoolId);
                        //Get the schoolType Name
                        var getSchType = _context.SchoolType.FirstOrDefault(u => u.Id == userSchool.SchoolTypeId);
                        //Get the Campus Name
                        var getCampus = _context.SchoolCampus.FirstOrDefault(u => u.Id == getUser.CampusId);

                        //get the current session and term of the school
                        SessionAndTerm sessionTerm = new SessionAndTerm(_context);
                        var currentSessionId = sessionTerm.getCurrentSessionId((long)getUser.SchoolId);

                        long classId = 0;
                        long classGradeId = 0;
                        string className = string.Empty;
                        string classGradeName = string.Empty;

                        //studentInfo
                        StudentClassInfo classInfo = new StudentClassInfo();

                        if (currentSessionId > 0)
                        {
                            //Get the Student Class and ClassGrade
                            GradeStudents getStudentClassAndGrade = _context.GradeStudents.FirstOrDefault(u => u.StudentId == getUser.Id && u.SessionId == currentSessionId);

                            if (getStudentClassAndGrade != null)
                            {
                                classId = getStudentClassAndGrade.ClassId;
                                classGradeId = getStudentClassAndGrade.ClassGradeId;

                                //get the class and ClassGrade
                                className = _context.Classes.FirstOrDefault(x => x.Id == classId && x.SchoolId == getUser.SchoolId && x.CampusId == getUser.CampusId).ClassName;
                                classGradeName = _context.ClassGrades.FirstOrDefault(x => x.Id == classGradeId && x.SchoolId == getUser.SchoolId && x.CampusId == getUser.CampusId).GradeName;

                                classInfo.Message = "Success";
                                classInfo.ClassId = classId;
                                classInfo.ClassName = className;
                                classInfo.ClassGradeId = classGradeId;
                                classInfo.ClassGradeName = classGradeName;
                            }
                            else
                            {
                                classInfo.Message = "Student has not been Assigned to a Class for the current Session";
                            }
                        }
                        else
                        {
                            classInfo.Message = "School Current Session has not been set and Student has not been Assigned to a Class";
                        }

                        //the userDetails
                        userData.UserId = getUser.Id.ToString();
                        userData.FirstName = getUser.FirstName;
                        userData.LastName = getUser.LastName;
                        userData.UserName = getUser.UserName;
                        userData.ProfilePictureUrl = getUser.ProfilePictureUrl;
                        userData.AdmissionNumber = getUser.AdmissionNumber;
                        userData.IsActive = getUser.IsActive;
                        userData.LastLoginDate = getUser.LastLoginDate;
                        userData.LastPasswordChangedDate = getUser.LastPasswordChangedDate;
                        userData.LastUpdatedDate = getUser.LastUpdatedDate;
                        userData.StudentClassInfo = classInfo;

                        //School and Campus details
                        schData.SchoolId = userSchool.Id;
                        schData.SchoolName = userSchool.SchoolName;
                        schData.SchoolCode = userSchool.SchoolCode;
                        schData.SchoolTypeName = getSchType.SchoolTypeName;
                        schData.SchoolLogoUrl = userSchool.SchoolLogoUrl;
                        schData.CampusId = getCampus.Id;
                        schData.CampusName = getCampus.CampusName;
                        schData.CampusAddress = getCampus.CampusAddress;

                        getUser.LastLoginDate = DateTime.Now;
                        await _context.SaveChangesAsync();

                        //The data to be sent as response
                        respData.StatusCode = 200;
                        respData.StatusMessage = "Login Successful!";
                        respData.SchoolUserDetails = userData;
                        respData.schoolDetails = schData;

                        //activityLog
                        var activitylog = new ActivityLogs()
                        {
                            UserId = getUser.Id.ToString(),
                            FirstName = getUser.FirstName,
                            LastName = getUser.LastName,
                            Action = "Student Login",
                            Message = "Successful Login",
                            Description = "Valid Username and Password",
                            ActionDate = DateTime.Now,
                        };

                        await _context.ActivityLogs.AddAsync(activitylog);
                        await _context.SaveChangesAsync();
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
            }
        }

        public async Task<GenericRespModel> getStudentByIdAsync(Guid studentId, long schoolId, long campusId)
        {
            try
            {
                CheckerValidation check = new CheckerValidation(_context);
                var checkStudent = check.checkStudentById(studentId);
                var checkSchool = check.checkSchoolById(schoolId);
                var checkCampus = check.checkSchoolCampusById(campusId);

                if (checkStudent != true)
                {
                    return new GenericRespModel { StatusCode = 409, StatusMessage = "No Student with the specified ID" };
                }
                if (checkSchool != true)
                {
                    return new GenericRespModel { StatusCode = 409, StatusMessage = "No School with the specified ID" };
                }
                if (checkCampus != true)
                {
                    return new GenericRespModel { StatusCode = 409, StatusMessage = "No SchoolCampus with the specified ID" };
                }
                else
                {
                    var result = from std in _context.Students
                                 where std.Id == studentId && std.SchoolId == schoolId && std.CampusId == campusId
                                 select new
                                 {
                                     std.Id,
                                     std.SchoolId,
                                     std.CampusId,
                                     std.FirstName,
                                     std.LastName,
                                     std.MiddleName,
                                     std.UserName,
                                     std.AdmissionNumber,
                                     std.YearOfAdmission,
                                     std.Status,
                                     std.StaffStatus,
                                     std.State,
                                     std.City,
                                     std.DateOfBirth,
                                     std.StateOfOrigin,
                                     std.LocalGovt,
                                     std.ProfilePictureUrl,
                                     std.HomeAddress,
                                     std.Gender.GenderName,
                                     std.hasParent,
                                     std.IsActive,
                                     std.LastPasswordChangedDate,
                                     std.LastLoginDate,
                                     std.LastUpdatedDate,
                                     std.DateCreated,
                                 };

                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = result.FirstOrDefault()};
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

        public async Task<GenericRespModel> assignStudentToClassAsync(AssignStudentToClassReqModel obj)
        {
            try
            {
                GenericRespModel respData = new GenericRespModel();
                //get the current sessionId
                var currentSessionId = new SessionAndTerm(_context).getCurrentSessionId(obj.SchoolId);

                if (currentSessionId == 0) //if the current session has not been set
                {
                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Current Academic Session has not been set!" };
                }
                else
                {
                    foreach (StudentId studId in obj.StudentIds)
                    {
                        //check unassigned students
                        var checkStudent = _context.Students.Where(x => x.Id == studId.Id && x.IsAssignedToClass == false).FirstOrDefault();

                        if (checkStudent != null)
                        {
                            var std = new GradeStudents
                            {
                                StudentId = studId.Id,
                                ClassId = obj.ClassId,
                                ClassGradeId = obj.ClassGradeId,
                                SchoolId = obj.SchoolId,
                                CampusId = obj.CampusId,
                                SessionId = currentSessionId,
                                HasGraduated = false,
                                DateCreated = DateTime.Now
                            };

                            //update the student "IsAssignedToClass" to true
                            checkStudent.IsAssignedToClass = true;

                            await _context.GradeStudents.AddAsync(std);
                            await _context.SaveChangesAsync();

                            //return all the students assigned to the Class
                            var clasStudents = from sub in _context.GradeStudents
                                               where sub.ClassId == obj.ClassId && sub.ClassGradeId == obj.ClassGradeId
                                               && sub.SchoolId == obj.SchoolId && sub.CampusId == obj.CampusId
                                               select new
                                               {
                                                   sub.StudentId,
                                                   sub.SchoolId,
                                                   sub.CampusId,
                                                   sub.Students.FirstName,
                                                   sub.Students.LastName,
                                                   sub.Students.AdmissionNumber,
                                                   sub.Students.UserName,
                                                   sub.ClassId,
                                                   sub.Classes.ClassName,
                                                   sub.ClassGradeId,
                                                   sub.ClassGrades.GradeName,
                                                   sub.Sessions.SessionName,
                                                   sub.DateCreated
                                               };

                            respData.StatusCode = 200;
                            respData.StatusMessage = "Students Assigned Successfully";
                            respData.Data = clasStudents.ToList();
                        }
                        else
                        {
                            var clasStudents = from sub in _context.GradeStudents
                                               where sub.ClassId == obj.ClassId && sub.ClassGradeId == obj.ClassGradeId
                                               && sub.SchoolId == obj.SchoolId && sub.CampusId == obj.CampusId
                                               select new
                                               {
                                                   sub.StudentId,
                                                   sub.SchoolId,
                                                   sub.CampusId,
                                                   sub.Students.FirstName,
                                                   sub.Students.LastName,
                                                   sub.Students.AdmissionNumber,
                                                   sub.Students.UserName,
                                                   sub.ClassId,
                                                   sub.Classes.ClassName,
                                                   sub.ClassGradeId,
                                                   sub.ClassGrades.GradeName,
                                                   sub.Sessions.SessionName,
                                                   sub.DateCreated
                                               };

                            respData.StatusCode = 200;
                            respData.StatusMessage = "One or more selected Students has been assigned!";
                            respData.Data = clasStudents.ToList();
                        }
                    }
                }

                return respData;
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

        public async Task<GenericRespModel> getStudentParentAsync(Guid studentId, long schoolId, long campusId)
        {
            try
            {
                CheckerValidation check = new CheckerValidation(_context);
                var checkStudent = check.checkStudentById(studentId);
                var checkSchool = check.checkSchoolById(schoolId);
                var checkCampus = check.checkSchoolCampusById(campusId);

                if (checkStudent != true)
                {
                    return new GenericRespModel { StatusCode = 409, StatusMessage = "No Student with the specified ID" };
                }
                if (checkSchool != true)
                {
                    return new GenericRespModel { StatusCode = 409, StatusMessage = "No School with the specified ID" };
                }
                if (checkCampus != true)
                {
                    return new GenericRespModel { StatusCode = 409, StatusMessage = "No SchoolCampus with the specified ID" };
                }
                else
                {
                    var result = from std in _context.ParentsStudentsMap
                                 where std.StudentId == studentId && std.SchoolId == schoolId && std.CampusId == campusId
                                 select new
                                 {
                                     std.Id,
                                     std.SchoolId,
                                     std.CampusId,
                                     std.StudentId,
                                     std.ParentId,
                                     std.Parents.FirstName,
                                     std.Parents.LastName,
                                     std.Parents.UserName,
                                     std.Parents.Email,
                                     std.Parents.PhoneNumber,
                                     std.Parents.HomeAddress,
                                     std.Parents.StateOfOrigin,
                                     std.Parents.LocalGovt,
                                     std.Parents.Nationality,
                                     std.Parents.Occupation,
                                     std.Parents.Religion,
                                     std.Parents.State,
                                     std.Parents.IsActive,
                                     std.Parents.LastLoginDate,
                                     std.Parents.LastPasswordChangedDate,
                                     std.Parents.LastUpdatedDate,
                                     std.DateCreated,
                                 };

                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = result.FirstOrDefault(), };
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

        public async Task<GenericRespModel> getAllAssignedStudentAsync(long schoolId, long campusId)
        {
            try
            {
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(schoolId);
                var checkCampus = check.checkSchoolCampusById(campusId);

                if (checkSchool != true && checkCampus != true)
                {
                    return new GenericRespModel { StatusCode = 409, StatusMessage = "No School or Campus with the specified ID" };
                }
                else
                {
                    var result = from std in _context.Students
                                 where std.SchoolId == schoolId && std.CampusId == campusId && std.IsAssignedToClass == true
                                 select new
                                 {
                                     std.Id,
                                     std.SchoolId,
                                     std.CampusId,
                                     std.FirstName,
                                     std.LastName,
                                     std.MiddleName,
                                     std.UserName,
                                     std.AdmissionNumber,
                                     std.YearOfAdmission,
                                     std.Status,
                                     std.StaffStatus,
                                     std.State,
                                     std.City,
                                     std.DateOfBirth,
                                     std.StateOfOrigin,
                                     std.LocalGovt,
                                     std.ProfilePictureUrl,
                                     std.HomeAddress,
                                     std.Gender.GenderName,
                                     std.hasParent,
                                     std.IsActive,
                                     std.LastPasswordChangedDate,
                                     std.LastLoginDate,
                                     std.LastUpdatedDate,
                                     std.DateCreated,
                                 };

                    if (result.Count() > 0)
                    {
                        return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList(), };
                    }

                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful, No Record Available", };
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

        public async Task<GenericRespModel> getAllUnAssignedStudentAsync(long schoolId, long campusId)
        {
            try
            {
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(schoolId);
                var checkCampus = check.checkSchoolCampusById(campusId);

                if (checkSchool != true && checkCampus != true)
                {
                    return new GenericRespModel { StatusCode = 409, StatusMessage = "No School or Campus with the specified ID" };
                }
                else
                {
                    var result = from std in _context.Students
                                 where std.SchoolId == schoolId && std.CampusId == campusId && std.IsAssignedToClass == false
                                 select new
                                 {
                                     std.Id,
                                     std.SchoolId,
                                     std.CampusId,
                                     std.FirstName,
                                     std.LastName,
                                     std.MiddleName,
                                     std.UserName,
                                     std.AdmissionNumber,
                                     std.YearOfAdmission,
                                     std.Status,
                                     std.StaffStatus,
                                     std.State,
                                     std.City,
                                     std.DateOfBirth,
                                     std.StateOfOrigin,
                                     std.LocalGovt,
                                     std.ProfilePictureUrl,
                                     std.HomeAddress,
                                     std.Gender.GenderName,
                                     std.hasParent,
                                     std.IsActive,
                                     std.LastPasswordChangedDate,
                                     std.LastLoginDate,
                                     std.LastUpdatedDate,
                                     std.DateCreated,
                                 };
                    if (result.Count() > 0)
                    {
                        return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList(), };
                    }

                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful, No Record Available", };
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

        public async Task<GenericRespModel> getAllStudentInSchoolAsync(long schoolId)
        {
            try
            {
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(schoolId);

                if (checkSchool != true)
                {
                    return new GenericRespModel { StatusCode = 409, StatusMessage = "No School with the specified ID" };
                }
                else
                {
                    var result = from std in _context.Students
                                 where std.SchoolId == schoolId
                                 select new
                                 {
                                     std.Id,
                                     std.SchoolId,
                                     std.CampusId,
                                     std.SchoolCampus.CampusName,
                                     std.FirstName,
                                     std.LastName,
                                     std.MiddleName,
                                     std.UserName,
                                     std.AdmissionNumber,
                                     std.YearOfAdmission,
                                     std.Status,
                                     std.StaffStatus,
                                     std.State,
                                     std.City,
                                     std.DateOfBirth,
                                     std.StateOfOrigin,
                                     std.LocalGovt,
                                     std.ProfilePictureUrl,
                                     std.HomeAddress,
                                     std.Gender.GenderName,
                                     std.hasParent,
                                     std.IsActive,
                                     std.LastPasswordChangedDate,
                                     std.LastLoginDate,
                                     std.LastUpdatedDate,
                                     std.DateCreated,
                                 };
                    if (result.Count() > 0)
                    {
                        return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList(), };
                    }

                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful, No Record Available", };
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

        public async Task<GenericRespModel> getAllStudentInCampusAsync(long schoolId, long campusId)
        {
            try
            {
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(schoolId);
                var checkCampus = check.checkSchoolCampusById(campusId);

                if (checkSchool != true && checkCampus != true)
                {
                    return new GenericRespModel { StatusCode = 409, StatusMessage = "No School or Campus with the specified ID" };
                }
                else
                {
                    var result = from std in _context.Students
                                 where std.SchoolId == schoolId && std.CampusId == campusId
                                 select new
                                 {
                                     std.Id,
                                     std.SchoolId,
                                     std.CampusId,
                                     std.FirstName,
                                     std.LastName,
                                     std.MiddleName,
                                     std.UserName,
                                     std.AdmissionNumber,
                                     std.YearOfAdmission,
                                     std.Status,
                                     std.StaffStatus,
                                     std.State,
                                     std.City,
                                     std.DateOfBirth,
                                     std.StateOfOrigin,
                                     std.LocalGovt,
                                     std.ProfilePictureUrl,
                                     std.HomeAddress,
                                     std.Gender.GenderName,
                                     std.hasParent,
                                     std.IsActive,
                                     std.LastPasswordChangedDate,
                                     std.LastLoginDate,
                                     std.LastUpdatedDate,
                                     std.DateCreated,
                                 };
                    if (result.Count() > 0)
                    {
                        return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList(), };
                    }

                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful, No Record Available", };
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

        public async Task<GenericRespModel> getStudentsBySessionIdAsync(long schoolId, long campusId, long sessionId)
        {
            try
            {
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(schoolId);
                var checkCampus = check.checkSchoolCampusById(campusId);

                if (checkSchool != true)
                {
                    return new GenericRespModel { StatusCode = 409, StatusMessage = "No School with the specified ID" };
                }
                if (checkCampus != true)
                {
                    return new GenericRespModel { StatusCode = 409, StatusMessage = "No SchoolCampus with the specified ID" };
                }
                else
                {
                    var result = from std in _context.GradeStudents
                                 where std.SchoolId == schoolId && std.CampusId == campusId && std.SessionId == sessionId
                                 select new
                                 {
                                     std.Id,
                                     std.SchoolId,
                                     std.CampusId,
                                     std.ClassId,
                                     std.Classes.ClassName,
                                     std.ClassGradeId,
                                     std.ClassGrades.GradeName,
                                     std.Students.FirstName,
                                     std.Students.LastName,
                                     std.Students.MiddleName,
                                     std.Students.UserName,
                                     std.Students.AdmissionNumber,
                                     std.Students.YearOfAdmission,
                                     std.Students.Status,
                                     std.Students.StaffStatus,
                                     std.Students.State,
                                     std.Students.City,
                                     std.Students.DateOfBirth,
                                     std.Students.StateOfOrigin,
                                     std.Students.LocalGovt,
                                     std.Students.ProfilePictureUrl,
                                     std.Students.HomeAddress,
                                     std.Students.Gender.GenderName,
                                     std.Students.hasParent,
                                     std.Students.IsActive,
                                     std.Students.LastPasswordChangedDate,
                                     std.Students.LastLoginDate,
                                     std.Students.LastUpdatedDate,
                                     std.DateCreated,
                                 };
                    if (result.Count() > 0)
                    {
                        return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList(), };
                    }

                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful, No Record Available", };
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

        public async Task<GenericRespModel> moveStudentToNewClassAndClassGradeAsync(MoveStudentReqModel obj)
        {
            try
            {
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(obj.SchoolId);
                var checkCampus = check.checkSchoolCampusById(obj.CampusId);
                var checkClass = check.checkClassById(obj.ClassId);
                var checkClassGrade = check.checkClassGradeById(obj.ClassGradeId);
                var checkSession = check.checkSessionById(obj.SessionId);

                //response data
                IList<GenericRespModel> responseList = new List<GenericRespModel>();

                //the teacherId
                var teacherId = _context.GradeTeachers.Where(g => g.ClassId == obj.ClassId && g.ClassGradeId == obj.ClassGradeId
                && g.SchoolId == obj.SchoolId && g.CampusId == obj.CampusId).FirstOrDefault();

                if (checkSchool != true && checkCampus != true && checkClass != true && checkClassGrade != true && checkSession != true)
                {
                    return new GenericRespModel { StatusCode = 409, StatusMessage = "One or More Paramters are Invalid" };
                }
                if (teacherId == null)
                {
                    return new GenericRespModel { StatusCode = 500, StatusMessage = "A Teacher has not been assigned to this Class and ClassGrade!" };
                }
                else
                {
                    //if category selected is Alumni; move students selected to the alumni table
                    //and update the students status (graduated to true)
                    if (obj.ClassOrAlumniId == (long)EnumUtility.ClassOrAlumni.Alumni)
                    {
                        foreach (var stdId in obj.StudentIds)
                        {
                            var getAlumni = _context.Alumni.FirstOrDefault(x => x.StudentId == stdId.Id);
                            var getStudents = _context.GradeStudents.Where(x => x.StudentId == stdId.Id).FirstOrDefault();

                            if (getAlumni == null)
                            {
                                var alumni = new Alumni
                                {
                                    SchoolId = obj.SchoolId,
                                    CampusId = obj.CampusId,
                                    ClassId = obj.ClassId,
                                    ClassGradeId = obj.ClassGradeId,
                                    SessionId = obj.SessionId,
                                    StudentId = stdId.Id,
                                    GradeTeacherId = teacherId.SchoolUserId,
                                    DateGraduated = DateTime.Now,
                                };

                                await _context.Alumni.AddAsync(alumni);

                                //update the student hasGraduated to true
                                getStudents.HasGraduated = true;
                                await _context.SaveChangesAsync();

                                //response data
                                GenericRespModel response = new GenericRespModel();

                                response.StatusCode = 200;
                                response.StatusMessage = "Student(s) Moved to Alumni Successfully!";

                                responseList.Add(response);

                            }
                            else
                            {
                                getAlumni.StudentId = stdId.Id;
                                getAlumni.SchoolId = obj.SchoolId;
                                getAlumni.CampusId = obj.CampusId;
                                getAlumni.SessionId = obj.SessionId;
                                getAlumni.ClassId = obj.ClassId;
                                getAlumni.ClassGradeId = obj.ClassGradeId;
                                getAlumni.GradeTeacherId = teacherId.SchoolUserId;
                                getAlumni.DateGraduated = DateTime.Now;

                                await _context.SaveChangesAsync();

                                GenericRespModel response = new GenericRespModel();

                                response.StatusCode = 200;
                                response.StatusMessage = "Student(s) Moved to Alumni Successfully!";

                                responseList.Add(response);
                            }

                        }
                    }
                    //if category selected is Class; add new students to the session
                    else if (obj.ClassOrAlumniId == (long)EnumUtility.ClassOrAlumni.Class)
                    {
                        foreach (var stdId in obj.StudentIds)
                        {
                            var studentExists = _context.GradeStudents.Where(x => x.StudentId == stdId.Id && x.SessionId == obj.SessionId && x.SchoolId == obj.SchoolId && x.CampusId == obj.CampusId).FirstOrDefault();
                            if (studentExists != null)
                            {
                                GenericRespModel response = new GenericRespModel();

                                response.StatusCode = 409;
                                response.StatusMessage = $"Student with ID: {studentExists.StudentId} Already exists in this Session";

                                responseList.Add(response);

                                //studentExists.StudentId = stdId.Id;
                                //studentExists.ClassId = obj.ClassId;
                                //studentExists.ClassGradeId = obj.ClassGradeId;
                                //studentExists.SchoolId = obj.SchoolId;
                                //studentExists.CampusId = obj.CampusId;
                                //studentExists.SessionId = obj.SessionId;
                                //studentExists.HasGraduated = false;
                                //studentExists.DateCreated = DateTime.Now;

                            }
                            else
                            {
                                var grdStd = new GradeStudents
                                {
                                    StudentId = stdId.Id,
                                    ClassId = obj.ClassId,
                                    ClassGradeId = obj.ClassGradeId,
                                    SchoolId = obj.SchoolId,
                                    CampusId = obj.CampusId,
                                    SessionId = obj.SessionId,
                                    HasGraduated = false,
                                    DateCreated = DateTime.Now,
                                };

                                await _context.GradeStudents.AddAsync(grdStd);
                                await _context.SaveChangesAsync();

                                GenericRespModel response = new GenericRespModel();
                                response.StatusCode = 200;
                                response.StatusMessage = $"Student with ID: {stdId.Id} Moved to a new Class and ClassGrade Successfully";

                                responseList.Add(response);
                            }
                        }
                    }
                }

                return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = responseList };

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

        public async Task<StudentBulkCreationRespModel> createStudentFromExcelAsync(BulkStudentReqModel obj)
        {
            IList<object> data = new List<object>();
            try
            {
                StudentBulkCreationRespModel response = new StudentBulkCreationRespModel();
                long numberOfStudentsCreated = 0;
                long numberOfExistingStudents = 0;
                IList<object> listOfParentThatExistsInSchool = new List<object>();
                IList<object> listOfStudentThatExists = new List<object>();
                IList<object> listOfStudentsCreated = new List<object>();
                IList<object> listOfParentEmailThatExists = new List<object>();
                //Validations
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(obj.SchoolId);
                var checkCampus = check.checkSchoolCampusById(obj.CampusId);

                //check if the School and CampusId is Valid
                if (checkSchool == false && checkCampus == false)
                {
                    return new StudentBulkCreationRespModel { StatusCode = 400, StatusMessage = "No School Or Campus With the specified ID" };
                }
                else if (obj.File == null || obj.File.Length <= 0)
                {
                    return new StudentBulkCreationRespModel { StatusCode = 400, StatusMessage = "No File Selected!, Please Select the Student Bulk Upload Template" };
                }
                else if (!Path.GetExtension(obj.File.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
                {
                    return new StudentBulkCreationRespModel { StatusCode = 400, StatusMessage = "Not a Supported File Format!" };
                }
                else
                {
                    //the file path
                    var FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", obj.File.FileName);
                    //copy the file to the stream and read from the file
                    using (var stream = new FileStream(FilePath, FileMode.Create))
                    {
                        await obj.File.CopyToAsync(stream);
                    }

                    // If you use EPPlus in a noncommercial context
                    // according to the Polyform Noncommercial license:
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                    FileInfo existingFile = new FileInfo(FilePath);
                    using (ExcelPackage package = new ExcelPackage(existingFile))
                    {
                        //get the first worksheet in the workbook
                        ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();
                        int colCount = worksheet.Dimension.Columns;  //get Column Count
                        int rowCount = worksheet.Dimension.Rows;     //get row count

                        //student AdmissionNumber/Username Instance
                        AdmissionNumberGenerator admNumber = new AdmissionNumberGenerator(_context);

                        //default password
                        string defaultPassword = DefaultPasswordReUsable.DefaultPassword();
                        //Password Encryption Instance
                        var paswordHasher = new PasswordHasher();

                        for (int row = 2; row <= rowCount; row++) // starts from the second row (Jumping the table headings)
                        {

                            //Check the Student FirstName, Gender, Date of birth, parent Email Address
                            //AdmissionNumber
                            var admissionNumber = admNumber.GenerateAdmissionNumber(obj.SchoolId);

                            //the salt
                            string salt = paswordHasher.getSalt();
                            //Hash the password and salt
                            string passwordHash = paswordHasher.hashedPassword(defaultPassword, salt);

                            long studentGenderId = 1; //set a default genderId (Male)

                            if (worksheet.Cells[row, 4].Value != null)
                            {
                                studentGenderId = (long)Converters.stringToGender(worksheet.Cells[row, 4].Value.ToString());
                            }

                            //check for exsting students in the school
                            var getExistingStudents = from x in _context.Students
                                                      where x.LastName == worksheet.Cells[row, 1].Value.ToString()
                                                        && x.FirstName == worksheet.Cells[row, 2].Value.ToString()
                                                        && x.MiddleName == worksheet.Cells[row, 3].Value.ToString()
                                                        && x.SchoolId == obj.SchoolId
                                                        && x.CampusId == obj.CampusId
                                                      select x;

                            //skip and add students to duplicate table if the students exists 
                            if (getExistingStudents.Count() > 0)
                            {
                                foreach (Students std in getExistingStudents)
                                {
                                    //check if the students exists in the duplicate table
                                    StudentDuplicates objDupStd = _context.StudentDuplicates.Where(x => x.NewStudentFullName == worksheet.Cells[row, 1].Value.ToString() + " " + worksheet.Cells[row, 2].Value.ToString() + " " + worksheet.Cells[row, 3].Value.ToString()
                                    && x.ExistingStudentId == std.Id
                                    && x.SchoolId == obj.SchoolId
                                    && x.CampusId == obj.CampusId).FirstOrDefault();

                                    //adds the students duplicate record
                                    if (objDupStd == null)
                                    {
                                        var objDup = new StudentDuplicates
                                        {
                                            NewStudentFullName = worksheet.Cells[row, 1].Value.ToString() + " " + worksheet.Cells[row, 2].Value.ToString() + " " + worksheet.Cells[row, 3].Value.ToString(),
                                            ExistingStudentId = std.Id,
                                            SchoolId = obj.SchoolId,
                                            CampusId = obj.CampusId,
                                            DateCreated = DateTime.Now,
                                        };
                                        await _context.StudentDuplicates.AddAsync(objDup);
                                    }
                                    else //updates the students duplicate record
                                    {
                                        objDupStd.NewStudentFullName = worksheet.Cells[row, 1].Value.ToString() + " " + worksheet.Cells[row, 2].Value.ToString() + " " + worksheet.Cells[row, 3].Value.ToString();
                                        objDupStd.ExistingStudentId = std.Id;
                                        objDupStd.SchoolId = obj.SchoolId;
                                        objDupStd.CampusId = obj.CampusId;
                                        objDupStd.DateCreated = DateTime.Now;
                                    }
                                    await _context.SaveChangesAsync();

                                    //the student data existing
                                    var existingStudent = (from sd in _context.Students
                                                           where sd.Id == std.Id
                                                           select new
                                                           {
                                                               sd.Id,
                                                               sd.SchoolId,
                                                               sd.CampusId,
                                                               sd.FirstName,
                                                               sd.LastName,
                                                               sd.UserName,
                                                               sd.AdmissionNumber,
                                                               sd.hasParent,
                                                               sd.IsActive,
                                                               sd.LastPasswordChangedDate,
                                                               sd.LastLoginDate,
                                                               sd.LastUpdatedDate,
                                                               sd.DateCreated,
                                                           }).FirstOrDefault();

                                    //add the existing students to a list as response
                                    listOfStudentThatExists.Add(existingStudent);
                                    numberOfExistingStudents++;
                                }
                            }
                            else
                            {
                                var std = new Students
                                {
                                    LastName = worksheet.Cells[row, 1].Value.ToString(),
                                    FirstName = worksheet.Cells[row, 2].Value.ToString(),
                                    MiddleName = worksheet.Cells[row, 3].Value.ToString(),
                                    GenderId = (long)Converters.stringToGender(worksheet.Cells[row, 4].Value.ToString()),
                                    StaffStatus = 0,
                                    DateOfBirth = Convert.ToDateTime(worksheet.Cells[row, 6].Value.ToString()),
                                    YearOfAdmission = worksheet.Cells[row, 7].Value.ToString(),
                                    StateOfOrigin = worksheet.Cells[row, 8].Value.ToString(),
                                    LocalGovt = worksheet.Cells[row, 9].Value.ToString(),
                                    Religion = worksheet.Cells[row, 10].Value.ToString(),
                                    HomeAddress = worksheet.Cells[row, 11].Value.ToString(),
                                    City = worksheet.Cells[row, 12].Value.ToString(),
                                    State = worksheet.Cells[row, 13].Value.ToString(),
                                    UserName = admissionNumber,
                                    AdmissionNumber = admissionNumber,
                                    Salt = salt,
                                    PasswordHash = passwordHash,
                                    StudentStatus = "",
                                    SchoolId = obj.SchoolId,
                                    CampusId = obj.CampusId,
                                    IsAssignedToClass = false,
                                    hasParent = false,
                                    IsActive = true,
                                    ProfilePictureUrl = "",
                                    Status = "",
                                    DateCreated = DateTime.Now,
                                };

                                await _context.Students.AddAsync(std);
                                await _context.SaveChangesAsync();

                                //the students created
                                var stud = (from sd in _context.Students
                                            where sd.Id == std.Id
                                            select new
                                            {
                                                sd.Id,
                                                sd.SchoolId,
                                                sd.CampusId,
                                                sd.FirstName,
                                                sd.LastName,
                                                sd.UserName,
                                                sd.AdmissionNumber,
                                                sd.hasParent,
                                                sd.IsActive,
                                                sd.LastPasswordChangedDate,
                                                sd.LastLoginDate,
                                                sd.LastUpdatedDate,
                                                sd.DateCreated,
                                            }).FirstOrDefault();

                                //add all students created to a list
                                listOfStudentsCreated.Add(stud);

                                //the parents email Address
                                string parentsEmailAddress = worksheet.Cells[row, 17].Value.ToString(); //gets the parent email Address

                                //Check if the parent exists in the school and map student to the parent if true
                                var parentExists = _context.Parents.Where(P => P.Email == parentsEmailAddress && P.SchoolId == obj.SchoolId).FirstOrDefault();

                                if (parentExists != null)
                                {
                                    //map student to existing parent
                                    var mapp = new ParentsStudentsMap
                                    {
                                        ParentId = parentExists.Id,
                                        StudentId = std.Id,
                                        SchoolId = obj.SchoolId,
                                        CampusId = obj.CampusId,
                                        DateCreated = DateTime.Now
                                    };

                                    //student was mapped to a parent
                                    var getStudent = _context.Students.Where(s => s.Id == std.Id).FirstOrDefault();
                                    getStudent.hasParent = true;

                                    await _context.ParentsStudentsMap.AddAsync(mapp);
                                    await _context.SaveChangesAsync();

                                    //the list of parent that exits
                                    var prts = (from prt in _context.Parents
                                                where prt.Email == parentExists.Email
                                                select new
                                                {
                                                    prt.Id,
                                                    prt.SchoolId,
                                                    prt.CampusId,
                                                    prt.FirstName,
                                                    prt.LastName,
                                                    prt.UserName,
                                                    prt.Email,
                                                    prt.PhoneNumber,
                                                    prt.hasChild,
                                                    prt.IsActive,
                                                    prt.DateCreated,
                                                }).FirstOrDefault();
                                    //add all existing parent to a list and send as part of API respponse
                                    listOfParentThatExistsInSchool.Add(prts);
                                }
                                else
                                {
                                    //Check if the parent exists in the Database
                                    var parentExistsInDb = _context.Parents.Where(P => P.Email == parentsEmailAddress).FirstOrDefault();
                                    if (parentExistsInDb == null)
                                    {
                                        //Save new parents details
                                        var parent = new Parents
                                        {
                                            FirstName = worksheet.Cells[row, 14].Value.ToString(),
                                            LastName = worksheet.Cells[row, 15].Value.ToString(),
                                            UserName = parentsEmailAddress,
                                            GenderId = (long)Converters.stringToGender(worksheet.Cells[row, 16].Value.ToString()),
                                            Email = parentsEmailAddress,
                                            PhoneNumber = worksheet.Cells[row, 18].Value.ToString(),
                                            Salt = salt,
                                            PasswordHash = passwordHash,
                                            SchoolId = obj.SchoolId,
                                            CampusId = obj.CampusId,
                                            Nationality = worksheet.Cells[row, 19].Value.ToString(),
                                            State = worksheet.Cells[row, 20].Value.ToString(),
                                            City = worksheet.Cells[row, 21].Value.ToString(),
                                            HomeAddress = worksheet.Cells[row, 22].Value.ToString(),
                                            Occupation = worksheet.Cells[row, 23].Value.ToString(),
                                            StateOfOrigin = worksheet.Cells[row, 24].Value.ToString(),
                                            LocalGovt = worksheet.Cells[row, 25].Value.ToString(),
                                            Religion = worksheet.Cells[row, 26].Value.ToString(),
                                            IsActive = true,
                                            hasChild = true,
                                            DateCreated = DateTime.Now,
                                        };

                                        await _context.Parents.AddAsync(parent);
                                        await _context.SaveChangesAsync();

                                        //map student and parent
                                        var mapp = new ParentsStudentsMap
                                        {
                                            ParentId = parent.Id,
                                            StudentId = std.Id,
                                            SchoolId = obj.SchoolId,
                                            CampusId = obj.CampusId,
                                            DateCreated = DateTime.Now
                                        };

                                        //student was mapped to a parent
                                        var getStudent = _context.Students.Where(s => s.Id == std.Id).FirstOrDefault();
                                        getStudent.hasParent = true;

                                        await _context.ParentsStudentsMap.AddAsync(mapp);
                                        await _context.SaveChangesAsync();
                                    }
                                    else
                                    {
                                        //This parent info will not be created, but student info will be created.
                                        //Parent profile should be created with a different email that doesn't exists in the DB and 
                                        //student created should be mapped to the parent
                                        listOfParentEmailThatExists.Add(parentsEmailAddress);
                                    }
                                }

                                //increments the numbers of students created from the excel file
                                numberOfStudentsCreated++;
                            }

                            response.StatusCode = 200;
                            response.StatusMessage = "Uploaded Successfully!, Student(s) with existing Parent Details was updated Successfully!";
                            response.NumberOfStudentsCreated = numberOfStudentsCreated;
                            response.StudentsData = listOfStudentsCreated.ToList();
                            response.NumberOfExistingParents = listOfParentThatExistsInSchool.Count();
                            response.ExistingParentsInfoInSchool = listOfParentThatExistsInSchool.ToList();
                            response.ExistingStudentsInfo = listOfStudentThatExists.ToList();
                            response.ExistingParentsEmail = listOfParentEmailThatExists.ToList();
                        }
                    }
                }

                return response;

            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new StudentBulkCreationRespModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<GenericRespModel> updateStudentDetailsAsync(Guid studentId, UpdateStudentReqModel obj)
        {
            try
            {
                //Check if the student exists
                var getStudent = _context.Students.Where(s => s.Id == studentId && s.SchoolId == obj.SchoolId && s.CampusId == obj.CampusId).FirstOrDefault();

                if (getStudent != null)
                {
                    getStudent.LastName = obj.StudentLastName;
                    getStudent.FirstName = obj.StudentFirstName;
                    getStudent.MiddleName = obj.MiddleName;
                    getStudent.SchoolId = obj.SchoolId;
                    getStudent.CampusId = obj.CampusId;
                    getStudent.GenderId = obj.GenderId;
                    getStudent.StaffStatus = 0;
                    getStudent.DateOfBirth = Convert.ToDateTime(obj.DateOfBirth);
                    getStudent.YearOfAdmission = obj.YearOfAdmission;
                    getStudent.StateOfOrigin = obj.StateOfOrigin;
                    getStudent.LocalGovt = obj.LocalGovt;
                    getStudent.Religion = obj.Religion;
                    getStudent.HomeAddress = obj.HomeAddress;
                    getStudent.City = obj.City;
                    getStudent.State = obj.State;
                    getStudent.StudentStatus = "";
                    getStudent.ProfilePictureUrl = obj.ProfilePictureUrl;
                    getStudent.Status = "";
                    getStudent.LastUpdatedDate = DateTime.Now;

                    await _context.SaveChangesAsync();

                    //activityLog
                    var activitylog = new ActivityLogs()
                    {
                        UserId = getStudent.Id.ToString(),
                        FirstName = getStudent.FirstName,
                        LastName = getStudent.LastName,
                        Action = "Update Student Details",
                        Message = "Student Details Updated Successfully",
                        Description = "Successfully Updated the Student Details",
                        ActionDate = DateTime.Now,
                    };

                    await _context.ActivityLogs.AddAsync(activitylog);
                    await _context.SaveChangesAsync();


                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Student Details Updated Successfully!" };
                }

                return new GenericRespModel { StatusCode = 409, StatusMessage = "No Student With the Specified ID" };

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

        public async Task<GenericRespModel> deleteStudentsAssignedToClassAsync(DeleteStudentAssignedReqModel obj)
        {
            try
            {
                var getStudent = _context.Students.Where(s => s.Id == obj.StudentId && s.SchoolId == obj.SchoolId && s.CampusId == obj.CampusId).FirstOrDefault();
                if (getStudent != null)
                {
                    var countExists = from st in _context.GradeStudents where st.StudentId == obj.StudentId && st.SchoolId == obj.SchoolId && st.CampusId == obj.CampusId select st;

                    //if student with the specified ID exists more than once in the GradeStudents table, the student is deleted from the class, classgrade 
                    //and the session specified and the status (IsAssigned) remains true 
                    if (countExists.Count() > 1)
                    {
                        var studentInGrade = _context.GradeStudents.Where(s => s.StudentId == obj.StudentId && s.ClassId == obj.ClassId
                        && s.ClassGradeId == obj.ClassGradeId && s.SessionId == obj.SessionId && s.SchoolId == obj.SchoolId && s.CampusId == obj.CampusId).FirstOrDefault();

                        if (studentInGrade != null)
                        {
                            _context.GradeStudents.Remove(studentInGrade);
                            await _context.SaveChangesAsync();

                            return new GenericRespModel { StatusCode = 500, StatusMessage = "Student Deleted from the Class and ClassGrade Assigned!" };
                        }

                    }
                    else //if student with the specified ID exists in the GradeStudents table only once, the student is deleted and the status (IsAssigned) is updated to false
                    {
                        var studentInGrade = _context.GradeStudents.Where(s => s.StudentId == obj.StudentId && s.ClassId == obj.ClassId
                            && s.ClassGradeId == obj.ClassGradeId && s.SessionId == obj.SessionId && s.SchoolId == obj.SchoolId && s.CampusId == obj.CampusId).FirstOrDefault();

                        if (studentInGrade != null)
                        {
                            //updates isAssignedToClass to false
                            getStudent.IsAssignedToClass = false;

                            _context.GradeStudents.Remove(studentInGrade);
                            await _context.SaveChangesAsync();

                            return new GenericRespModel { StatusCode = 500, StatusMessage = "Student Deleted from the Class and ClassGrade Assigned!" };
                        }
                    }
                }

                return new GenericRespModel { StatusCode = 500, StatusMessage = "No Student with the specified ID!" };

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

        public async Task<GenericRespModel> deleteStudentAsync(Guid studentId, long schoolId, long campusId)
        {
            try
            {
                var getStudent = _context.Students.Where(s => s.Id == studentId && s.SchoolId == schoolId && s.CampusId == campusId).FirstOrDefault();
                if (getStudent != null)
                {
                    _context.Students.Remove(getStudent);
                    await _context.SaveChangesAsync();

                    return new GenericRespModel { StatusCode = 500, StatusMessage = "Student and other Information related to student deleted Successfully!" };
                }

                return new GenericRespModel { StatusCode = 500, StatusMessage = "No Student with the specified Id!" };
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

        public async Task<GenericRespModel> getAllStudentDuplicatesAsync(long schoolId, long campusId)
        {
            try
            {
                //Validations
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(schoolId);
                var checkCampus = check.checkSchoolCampusById(campusId);

                if (checkSchool == true && checkCampus == true)
                {
                    //the list of student duplicates
                    var result = from std in _context.StudentDuplicates
                                 where std.SchoolId == schoolId && std.CampusId == campusId
                                 select new
                                 {
                                     std.Id,
                                     std.ExistingStudentId,
                                     std.NewStudentFullName, //concantenation of firstname, surname and middlename
                                     std.SchoolId,
                                     std.CampusId,
                                     std.DateCreated
                                 };

                    if (result.Count() > 0)
                    {
                        return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList() };
                    }

                    return new GenericRespModel { StatusCode = 200, StatusMessage = "No Available Record!" };
                }

                return new GenericRespModel { StatusCode = 409, StatusMessage = "Invalid School/CampusId!" };

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

        public async Task<GenericRespModel> getStudentDuplicateByStudentIdAsync(Guid studentId, long schoolId, long campusId)
        {
            try
            {
                //Validations
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(schoolId);
                var checkCampus = check.checkSchoolCampusById(campusId);

                if (checkSchool == true && checkCampus == true)
                {
                    //the list of student duplicates
                    var result = from std in _context.StudentDuplicates
                                 where std.SchoolId == schoolId && std.CampusId == campusId && std.ExistingStudentId == studentId
                                 select new
                                 {
                                     std.Id,
                                     std.ExistingStudentId,
                                     std.NewStudentFullName, //concantenation of firstname, surname and middlename
                                     std.SchoolId,
                                     std.CampusId,
                                     std.DateCreated
                                 };

                    if (result.Count() > 0)
                    {
                        return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = result.FirstOrDefault() };
                    }

                    return new GenericRespModel { StatusCode = 200, StatusMessage = "No Available Record!" };
                }

                return new GenericRespModel { StatusCode = 409, StatusMessage = "Invalid School/CampusId!" };

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


        public async Task<GenericRespModel> updateStudentDuplicateAsync(StudentDuplicateReqModel obj)
        {
            try
            {
                //response lists
                IList<GenericRespModel> responseList = new List<GenericRespModel>();

                //Validations
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(obj.SchoolId);
                var checkCampus = check.checkSchoolCampusById(obj.CampusId);
                var checkClass = check.checkClassById(obj.ClassId);
                var checkClassGrade = check.checkClassGradeById(obj.ClassGradeId);
                var checkSession = check.checkSessionById(obj.SessionId);

                if (checkSchool != true && checkCampus != true && checkClass != true && checkClassGrade != true && checkSession != true)
                {
                    return new GenericRespModel { StatusCode = 409, StatusMessage = "One or More Paramters are Invalid" };
                }
                else
                {
                    foreach (StudentId std in obj.StudentIds)
                    {
                        //check if student exists in duplicate table
                        StudentDuplicates studentDuplicate = _context.StudentDuplicates.Where(s => s.ExistingStudentId == std.Id && s.SchoolId == obj.SchoolId && s.CampusId == obj.CampusId).FirstOrDefault();

                        if (studentDuplicate != null)
                        {
                            //check if student exists in the session
                            var studentExists = _context.GradeStudents.Where(x => x.StudentId == std.Id && x.SessionId == obj.SessionId && x.SchoolId == obj.SchoolId && x.CampusId == obj.CampusId).FirstOrDefault();
                            if (studentExists == null)
                            {
                                var grdStd = new GradeStudents
                                {
                                    StudentId = std.Id,
                                    ClassId = obj.ClassId,
                                    ClassGradeId = obj.ClassGradeId,
                                    SchoolId = obj.SchoolId,
                                    CampusId = obj.CampusId,
                                    SessionId = obj.SessionId,
                                    HasGraduated = false,
                                    DateCreated = DateTime.Now,
                                };

                                await _context.GradeStudents.AddAsync(grdStd);
                                await _context.SaveChangesAsync();

                                //update students isAssigned to true
                                var gettSudent = _context.Students.Where(x => x.Id == std.Id && x.SchoolId == obj.SchoolId && x.CampusId == obj.CampusId).FirstOrDefault();
                                if (gettSudent != null)
                                {
                                    gettSudent.IsAssignedToClass = true;
                                    await _context.SaveChangesAsync();
                                }

                                //delete the student record from the studentDuplicate Table
                                var getStudentDuplicate = _context.StudentDuplicates.Where(s => s.ExistingStudentId == std.Id && s.SchoolId == obj.SchoolId && s.CampusId == obj.CampusId).FirstOrDefault();
                                if (getStudentDuplicate != null)
                                {
                                    _context.StudentDuplicates.Remove(getStudentDuplicate);
                                    await _context.SaveChangesAsync();
                                }

                                GenericRespModel response = new GenericRespModel
                                {
                                    StatusCode = 200,
                                    StatusMessage = $"Student with ID: {std.Id} Data has been Updated Successfully",
                                };
                                responseList.Add(response);
                            }
                            else
                            {
                                GenericRespModel response = new GenericRespModel
                                {
                                    StatusCode = 409,
                                    StatusMessage = $"Student with ID: {studentExists.StudentId} Already exists in this Session",
                                };
                                responseList.Add(response);
                            }
                        }
                        else
                        {
                            GenericRespModel response = new GenericRespModel
                            {
                                StatusCode = 409,
                                StatusMessage = $"Student with ID: {std.Id} does not have a duplicate Record",
                            };
                            responseList.Add(response);
                        }
                    }

                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Success", Data = responseList };
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

        public async Task<GenericRespModel> deleteStudentDuplicateAsync(Guid studentId, long schoolId, long campusId)
        {
            try
            {
                var getStudentDuplicate = _context.StudentDuplicates.Where(s => s.ExistingStudentId == studentId && s.SchoolId == schoolId && s.CampusId == campusId).FirstOrDefault();
                if (getStudentDuplicate != null)
                {
                    _context.StudentDuplicates.Remove(getStudentDuplicate);
                    await _context.SaveChangesAsync();

                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Student Duplicate Record Deleted Successfully!" };
                }

                return new GenericRespModel { StatusCode = 409, StatusMessage = "Student with the specified ID does not have a duplicate record!" };

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

        public async Task<GenericRespModel> forgotPasswordAsync(string admissionNumber)
        {
            try
            {
                var response = new GenericRespModel();
                //Check if email exist
                CheckerValidation emailcheck = new CheckerValidation(_context);

                var getStudent = _context.Students.FirstOrDefault(u => u.AdmissionNumber == admissionNumber);

                if (getStudent != null)
                {
                    var getParent = _context.ParentsStudentsMap.FirstOrDefault(u => u.StudentId == getStudent.Id);

                    if (getParent != null)
                    {
                        var parentDetails = _context.Parents.FirstOrDefault(u => u.Id == getParent.ParentId);

                        if (parentDetails != null)
                        {
                            var paswordHasher = new PasswordHasher();
                            //the salt
                            string salt = paswordHasher.getSalt();
                            //get deafault password
                            string password = RandomNumberGenerator.RandomString();
                            //Hash the password and salt
                            string passwordHash = paswordHasher.hashedPassword(password, salt);

                            getStudent.Salt = salt;
                            getStudent.PasswordHash = passwordHash;
                            await _context.SaveChangesAsync();

                            //code to send Mail to user for account activation
                            string MailContent = _emailTemplate.EmailForgotPassword(password);
                            EmailMessage message = new EmailMessage(parentDetails.Email, MailContent);
                            _emailRepo.SendEmail(message);

                            //response
                            response.StatusCode = 200;
                            response.StatusMessage = "Default Password has been Generated for you and sent to your Parent's mail Successfully, Kindly Change your Password after Login!";

                            //activityLog
                            var activitylog = new ActivityLogs()
                            {
                                UserId = getStudent.Id.ToString(),
                                FirstName = getStudent.FirstName,
                                LastName = getStudent.LastName,
                                Action = "Student Forgot Password",
                                Message = "Password Generated Successfully",
                                Description = "Default Password Generated and sent to Parent mail Successfully, Kindly Change Password after Login!",
                                ActionDate = DateTime.Now,
                            };

                            await _context.ActivityLogs.AddAsync(activitylog);
                            await _context.SaveChangesAsync();
                        }
                        else
                        {
                            return new GenericRespModel { StatusCode = 409, StatusMessage = "Parent Details does not exist!" };
                        }
                    }
                    else
                    {
                        return new GenericRespModel { StatusCode = 409, StatusMessage = "Student Parent Details does not exist!" };
                    }
                }
                else
                {
                    return new GenericRespModel { StatusCode = 409, StatusMessage = "Invalid Student!" };
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

        public async Task<GenericRespModel> changePasswordAsync(string admissionNumber, string oldPassword, string newPassword)
        {
            try
            {
                var response = new GenericRespModel();
                //Check if email exist
                CheckerValidation emailcheck = new CheckerValidation(_context);

                var getUser = _context.Students.FirstOrDefault(u => u.AdmissionNumber == admissionNumber);

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

                        //activityLog
                        var activitylog = new ActivityLogs()
                        {
                            UserId = getUser.Id.ToString(),
                            FirstName = getUser.FirstName,
                            LastName = getUser.LastName,
                            Action = "Student Change Password",
                            Message = "Password Changed Successfully",
                            Description = "Password Chnaged Successfully!",
                            ActionDate = DateTime.Now,
                        };

                        await _context.ActivityLogs.AddAsync(activitylog);
                        await _context.SaveChangesAsync();
                    }
                }
                else
                {
                    return new GenericRespModel { StatusCode = 409, StatusMessage = "Invalid Student!" };
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

        public async Task<GenericRespModel> getAllStudentWithoutParentsInfoAsync(long schoolId, long campusId)
        {
            try
            {
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(schoolId);
                var checkCampus = check.checkSchoolCampusById(campusId);

                if (checkSchool != true && checkCampus != true)
                {
                    return new GenericRespModel { StatusCode = 400, StatusMessage = "No School or Campus with the specified ID" };
                }
                else
                {
                    var result = from std in _context.Students
                                 where std.SchoolId == schoolId && std.CampusId == campusId && std.hasParent == false
                                 select new
                                 {
                                     std.Id,
                                     std.SchoolId,
                                     std.CampusId,
                                     std.FirstName,
                                     std.LastName,
                                     std.MiddleName,
                                     std.UserName,
                                     std.AdmissionNumber,
                                     std.YearOfAdmission,
                                     std.Status,
                                     std.StaffStatus,
                                     std.State,
                                     std.City,
                                     std.DateOfBirth,
                                     std.StateOfOrigin,
                                     std.LocalGovt,
                                     std.ProfilePictureUrl,
                                     std.HomeAddress,
                                     std.Gender.GenderName,
                                     std.hasParent,
                                     std.IsActive,
                                     std.LastPasswordChangedDate,
                                     std.LastLoginDate,
                                     std.LastUpdatedDate,
                                     std.DateCreated,
                                 };
                    if (result.Count() > 0)
                    {
                        return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList()};
                    }

                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful, No Record Available", };
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
