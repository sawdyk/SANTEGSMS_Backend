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
    public class ParentRepo :IParentRepo
    {
        private readonly AppDbContext _context;
        private readonly IEmailRepo _emailRepo;
        private readonly EmailTemplate _emailTemplate;
        public ParentRepo(AppDbContext context, IEmailRepo emailRepo, EmailTemplate emailTemplate)
        {
            _context = context;
            _emailRepo = emailRepo;
            _emailTemplate = emailTemplate;
        }

        public async Task<SchoolUsersLoginRespModel> parentLoginAsync(LoginReqModel obj)
        {
            try
            {
                //user data and schoolBasicInfo data objects
                SchoolBasicInfoLoginRespModel schData = new SchoolBasicInfoLoginRespModel();
                SchoolUsersInfoRespModel userData = new SchoolUsersInfoRespModel();

                //final data to be sent as response 
                SchoolUsersLoginRespModel respData = new SchoolUsersLoginRespModel();

                //Check if email exist
                CheckerValidation emailcheck = new CheckerValidation(_context);

                //var accountCheckResult = emailcheck.checkIfAccountExistAndNotConfirmed(obj.Email, Convert.ToInt64(EnumUtility.UserCategoty.Parents));
                var getUser = _context.Parents.FirstOrDefault(u => u.Email == obj.Email);

                if (getUser != null)
                {
                    var paswordHasher = new PasswordHasher();
                    string salt = getUser.Salt; //gets the salt used to hash the user password
                    string decryptedPassword = paswordHasher.hashedPassword(obj.Password, salt); //decrypts the password

                    //get the parent school
                    var getSch = _context.Schools.FirstOrDefault(u => u.Id == getUser.SchoolId);

                    if (getUser != null && getUser.PasswordHash != decryptedPassword)
                    {
                        return new SchoolUsersLoginRespModel { StatusCode = 409, StatusMessage = "Invalid Username/Password!" };
                    }
                    else if (getUser.IsActive != true)
                    {
                        return new SchoolUsersLoginRespModel { StatusCode = 409, StatusMessage = "Your Account has been deactivated, Kindly Contact your Admninistrator!" };
                    }
                    else if (getSch.IsApproved != true)
                    {
                        return new SchoolUsersLoginRespModel { StatusCode = 409, StatusMessage = "This School has not been Verified and Approved by the System Super Admninistrator!" };
                    }
                    else if (getSch.IsActive != true)
                    {
                        return new SchoolUsersLoginRespModel { StatusCode = 409, StatusMessage = "This School Account has been Disabled by the System Admninistrator, Kindly Contact the System Admninistrator!" };
                    }
                    //else if (getUser != null && getUser.PasswordHash == decryptedPassword && accountCheckResult == true)
                    //{
                    //    return new SchoolUsersLoginResponseModel { StatusCode = 409, StatusMessage = "This Account Exist but has not been Activated!" };
                    //}
                    else
                    {
                        //the userDetails
                        userData.UserId = getUser.Id.ToString();
                        userData.FirstName = getUser.FirstName;
                        userData.LastName = getUser.LastName;
                        userData.UserName = getUser.UserName;
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

                        schData.SchoolId = userSchool.Id;
                        schData.SchoolName = userSchool.SchoolName;
                        schData.SchoolCode = userSchool.SchoolCode;
                        schData.SchoolTypeName = getSchType.SchoolTypeName;
                        schData.CampusId = getCampus.Id;
                        schData.CampusName = getCampus.CampusName;
                        schData.CampusAddress = getCampus.CampusAddress;

                        getUser.LastLoginDate = DateTime.Now;
                        await _context.SaveChangesAsync();

                        //The data to be sent as response
                        respData.StatusCode = 200;
                        respData.StatusMessage = "Login Successful";
                        respData.SchoolUserDetails = userData;
                        respData.schoolDetails = schData;
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

        //Parents details by Email
        public async Task<GenericRespModel> getParentDetailsByEmailAsync(string email, long schoolId, long campusId)
        {
            try
            {
                CheckerValidation emailcheck = new CheckerValidation(_context);
                var emailCheckResult = emailcheck.checkIfEmailExist(email, Convert.ToInt64(EnumUtility.UserCategoty.Parents));
                if (emailCheckResult == true)
                {
                    var result = from prt in _context.Parents
                                 where prt.Email == email && prt.SchoolId == schoolId && prt.CampusId == campusId
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
                                     prt.LastPasswordChangedDate,
                                     prt.LastLoginDate,
                                     prt.LastUpdatedDate,
                                     prt.DateCreated,
                                 };

                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = result.FirstOrDefault(), };
                }

                return new GenericRespModel { StatusCode = 409, StatusMessage = "No Parent with the specified Email Address" };


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

        //Parents details by ID
        public async Task<GenericRespModel> getParentDetailsByIdAsync(Guid parentId, long schoolId, long campusId)
        {
            try
            {
                CheckerValidation check = new CheckerValidation(_context);
                var checkResult = check.checkParentById(parentId);
                if (checkResult == true)
                {
                    var result = from prt in _context.Parents
                                 where prt.Id == parentId && prt.SchoolId == schoolId && prt.CampusId == campusId
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
                                     prt.LastPasswordChangedDate,
                                     prt.LastLoginDate,
                                     prt.LastUpdatedDate,
                                     prt.DateCreated,
                                 };

                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = result.FirstOrDefault(), };
                }

                return new GenericRespModel { StatusCode = 409, StatusMessage = "No Parent with the specified ID" };


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

        //All parents in the school
        public async Task<GenericRespModel> getAllParentAsync(long schoolId, long campusId)
        {
            try
            {
                CheckerValidation check = new CheckerValidation(_context);
                var checkResult = check.checkSchoolById(schoolId);

                if (checkResult == true)
                {
                    var result = from prt in _context.Parents
                                 where prt.SchoolId == schoolId && prt.CampusId == campusId
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
                                     prt.LastPasswordChangedDate,
                                     prt.LastLoginDate,
                                     prt.LastUpdatedDate,
                                     prt.DateCreated,
                                 };

                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList(), };
                }

                return new GenericRespModel { StatusCode = 409, StatusMessage = "No School with the specified ID" };


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

        //All parents children in school
        public async Task<ParentChildRespModel> getAllParentChildAsync(Guid parentId, long schoolId, long campusId)
        {
            try
            {
                CheckerValidation check = new CheckerValidation(_context);
                var checkSch = check.checkSchoolById(schoolId);
                var checkPrt = check.checkParentById(parentId);

                if (checkSch != true)
                {
                    return new ParentChildRespModel { StatusCode = 409, StatusMessage = "No School with the specified ID" };
                }
                if (checkPrt != true)
                {
                    return new ParentChildRespModel { StatusCode = 409, StatusMessage = "No Parent with the specified ID" };
                }
                else
                {
                    //Parent details
                    var parentDetails = from prt in _context.Parents
                                        where prt.SchoolId == schoolId && prt.CampusId == campusId
                                        select new
                                        {
                                            prt.Id,
                                            prt.SchoolId,
                                            prt.CampusId,
                                            prt.Email,
                                            prt.PhoneNumber,
                                            prt.hasChild,
                                            prt.IsActive,
                                            prt.LastPasswordChangedDate,
                                            prt.LastLoginDate,
                                            prt.LastUpdatedDate,
                                            prt.DateCreated,
                                        };

                    //Child List details
                    var childListDetails = from prt in _context.ParentsStudentsMap
                                           where prt.ParentId == parentId && prt.SchoolId == schoolId && prt.CampusId == campusId
                                           select new
                                           {
                                               prt.Id,
                                               prt.SchoolId,
                                               prt.CampusId,
                                               prt.ParentId,
                                               prt.Students.FirstName,
                                               prt.Students.LastName,
                                               prt.Students.UserName,
                                               prt.Students.AdmissionNumber,
                                               prt.Students.IsAssignedToClass,
                                               prt.Students.hasParent,
                                               studentId = prt.Students.Id,
                                               prt.DateCreated,
                                           };

                    return new ParentChildRespModel { StatusCode = 200, StatusMessage = "Successful", ParentDetails = parentDetails.FirstOrDefault(), childDetails = childListDetails.ToList() };
                }

            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new ParentChildRespModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }
        //-------------------------------------ChildrenProfile-----------------------------------------------
        public async Task<GenericRespModel> getChildrenProfileAsync(ChildrenProfileReqModel obj)
        {
            IList<object> data = new List<object>();
            try
            {
                CheckerValidation check = new CheckerValidation(_context);
                var checkPrt = check.checkParentById(obj.ParentId);
                if (checkPrt != true)
                {
                    return new GenericRespModel { StatusCode = 409, StatusMessage = "No Parent with the specified ID" };
                }
                else
                {
                    if (obj.ChildrenId.Count > 0)
                    {
                        foreach (Guid childId in obj.ChildrenId)
                        {

                            var checkchild = check.checkStudentById(childId);
                            if (checkchild != true)
                            {
                                data.Add(new GenericRespModel { StatusCode = 407, StatusMessage = "No Child with the specified ID" });
                                continue;
                            }
                            var parentStudentMap = _context.ParentsStudentsMap.Where(x => x.ParentId == obj.ParentId && x.StudentId == childId).FirstOrDefault();
                            if (parentStudentMap == null)
                            {
                                data.Add(new GenericRespModel { StatusCode = 406, StatusMessage = "No Relationship between the parent and the child" });
                                continue;
                            }

                            //Child List details
                            var childListDetails = from prt in _context.ParentsStudentsMap
                                                   where prt.ParentId == obj.ParentId && prt.StudentId == childId
                                                   select new
                                                   {
                                                       prt.Id,
                                                       prt.SchoolId,
                                                       prt.CampusId,
                                                       prt.ParentId,
                                                       prt.StudentId,
                                                       prt.Students.FirstName,
                                                       prt.Students.LastName,
                                                       prt.Students.MiddleName,
                                                       prt.Students.UserName,
                                                       prt.Students.AdmissionNumber,
                                                       prt.Students.IsAssignedToClass,
                                                       prt.Students.hasParent,
                                                       prt.Students.City,
                                                       prt.Students.DateOfBirth,
                                                       prt.Students.Gender.GenderName,
                                                       prt.Students.GenderId,
                                                       prt.Students.HomeAddress,
                                                       studentId = prt.Students.Id,
                                                       prt.Students.IsActive,
                                                       prt.Students.LastLoginDate,
                                                       prt.Students.LastPasswordChangedDate,
                                                       prt.Students.LastUpdatedDate,
                                                       prt.Students.LocalGovt,
                                                       prt.Students.ProfilePictureUrl,
                                                       prt.Students.Religion,
                                                       prt.Students.SchoolCampus.CampusName,
                                                       prt.Students.Schools.SchoolName,
                                                       prt.Students.StaffStatus,
                                                       prt.Students.State,
                                                       prt.Students.StateOfOrigin,
                                                       prt.Students.Status,
                                                       prt.Students.StudentStatus,
                                                       prt.Students.YearOfAdmission,
                                                       prt.DateCreated,
                                                   };

                            data.Add(new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = childListDetails.FirstOrDefault() });
                        }
                    }
                    else
                    {
                        return new GenericRespModel { StatusCode = 408, StatusMessage = "No Child Selected" };
                    }
                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = data };
                }
            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                data.Add(new GenericRespModel { StatusCode = 500, StatusMessage = "An Error Occured!" });
                return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = data };
            }
        }
        //-------------------------------------ChildrenAttendance-----------------------------------------------
        public async Task<GenericRespModel> getChildrenAttendanceBySessionIdAsync(IList<Guid> childrenId, Guid parentId, long sessionId)
        {
            IList<object> data = new List<object>();
            try
            {
                CheckerValidation check = new CheckerValidation(_context);
                var checkSession = check.checkSessionById(sessionId);
                if (checkSession != true)
                {
                    return new GenericRespModel { StatusCode = 409, StatusMessage = "No Session with the specified ID" };
                }
                var checkPrt = check.checkParentById(parentId);
                if (checkPrt != true)
                {
                    return new GenericRespModel { StatusCode = 409, StatusMessage = "No Parent with the specified ID" };
                }
                else
                {
                    if (childrenId.Count > 0)
                    {
                        foreach (Guid childId in childrenId)
                        {

                            var checkchild = check.checkStudentById(childId);
                            if (checkchild != true)
                            {
                                data.Add(new GenericRespModel { StatusCode = 407, StatusMessage = "No Child with the specified ID" });
                                continue;
                            }
                            var parentStudentMap = _context.ParentsStudentsMap.Where(x => x.ParentId == parentId && x.StudentId == childId).FirstOrDefault();
                            if (parentStudentMap == null)
                            {
                                data.Add(new GenericRespModel { StatusCode = 406, StatusMessage = "No Relationship between the parent and the child" });
                                continue;
                            }

                            //Attendance details
                            var attendanceDetails = from atd in _context.StudentAttendance
                                                    where atd.SessionId == sessionId && atd.StudentId == childId
                                                    select new
                                                    {
                                                        atd.Id,
                                                        atd.SchoolId,
                                                        atd.CampusId,
                                                        atd.StudentId,
                                                        atd.AdmissionNumber,
                                                        atd.Students.FirstName,
                                                        atd.Students.LastName,
                                                        atd.Terms.TermName,
                                                        atd.Sessions.SessionName,
                                                        atd.Classes.ClassName,
                                                        atd.ClassGrades.GradeName,
                                                        atd.AttendancePeriodIdMorning,
                                                        atd.AttendancePeriodIdAfternoon,
                                                        atd.AttendanceDate
                                                    };

                            data.Add(new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = attendanceDetails.ToList() });
                        }
                    }
                    else
                    {
                        return new GenericRespModel { StatusCode = 408, StatusMessage = "No Child Selected" };
                    }
                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = data };
                }
            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                data.Add(new GenericRespModel { StatusCode = 500, StatusMessage = "An Error Occured!" });
                return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = data };
            }
        }

        public async Task<GenericRespModel> getChildrenAttendanceByTermIdAsync(IList<Guid> childrenId, Guid parentId, long termId)
        {
            IList<object> data = new List<object>();
            try
            {
                CheckerValidation check = new CheckerValidation(_context);
                var checkTerm = check.checkTermById(termId);
                if (checkTerm != true)
                {
                    return new GenericRespModel { StatusCode = 409, StatusMessage = "No Term with the specified ID" };
                }
                var checkPrt = check.checkParentById(parentId);
                if (checkPrt != true)
                {
                    return new GenericRespModel { StatusCode = 409, StatusMessage = "No Parent with the specified ID" };
                }
                else
                {
                    if (childrenId.Count > 0)
                    {
                        foreach (Guid childId in childrenId)
                        {

                            var checkchild = check.checkStudentById(childId);
                            if (checkchild != true)
                            {
                                data.Add(new GenericRespModel { StatusCode = 407, StatusMessage = "No Child with the specified ID" });
                                continue;
                            }
                            var parentStudentMap = _context.ParentsStudentsMap.Where(x => x.ParentId == parentId && x.StudentId == childId).FirstOrDefault();
                            if (parentStudentMap == null)
                            {
                                data.Add(new GenericRespModel { StatusCode = 406, StatusMessage = "No Relationship between the parent and the child" });
                                continue;
                            }

                            //Attendance details
                            var attendanceDetails = from atd in _context.StudentAttendance
                                                    where atd.TermId == termId && atd.StudentId == childId
                                                    select new
                                                    {
                                                        atd.Id,
                                                        atd.SchoolId,
                                                        atd.CampusId,
                                                        atd.StudentId,
                                                        atd.AdmissionNumber,
                                                        atd.Students.FirstName,
                                                        atd.Students.LastName,
                                                        atd.Terms.TermName,
                                                        atd.Sessions.SessionName,
                                                        atd.Classes.ClassName,
                                                        atd.ClassGrades.GradeName,
                                                        atd.AttendancePeriodIdMorning,
                                                        atd.AttendancePeriodIdAfternoon,
                                                        atd.AttendanceDate
                                                    };

                            data.Add(new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = attendanceDetails.ToList() });
                        }
                    }
                    else
                    {
                        return new GenericRespModel { StatusCode = 408, StatusMessage = "No Child Selected" };
                    }
                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = data };
                }
            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                data.Add(new GenericRespModel { StatusCode = 500, StatusMessage = "An Error Occured!" });
                return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = data };
            }
        }

        public async Task<GenericRespModel> getChildrenAttendanceByDateAsync(IList<Guid> childrenId, Guid parentId, DateTime startDate, DateTime endDate)
        {
            IList<object> data = new List<object>();
            try
            {
                CheckerValidation check = new CheckerValidation(_context);
                var checkPrt = check.checkParentById(parentId);
                if (checkPrt != true)
                {
                    return new GenericRespModel { StatusCode = 409, StatusMessage = "No Parent with the specified ID" };
                }
                else
                {
                    if (childrenId.Count > 0)
                    {
                        foreach (Guid childId in childrenId)
                        {

                            var checkchild = check.checkStudentById(childId);
                            if (checkchild != true)
                            {
                                data.Add(new GenericRespModel { StatusCode = 407, StatusMessage = "No Child with the specified ID" });
                                continue;
                            }
                            var parentStudentMap = _context.ParentsStudentsMap.Where(x => x.ParentId == parentId && x.StudentId == childId).FirstOrDefault();
                            if (parentStudentMap == null)
                            {
                                data.Add(new GenericRespModel { StatusCode = 406, StatusMessage = "No Relationship between the parent and the child" });
                                continue;
                            }

                            //Attendance details
                            var attendanceDetails = from atd in _context.StudentAttendance
                                                    where atd.StudentId == childId && atd.AttendanceDate >= startDate && atd.AttendanceDate < endDate.AddDays(1)
                                                    select new
                                                    {
                                                        atd.Id,
                                                        atd.SchoolId,
                                                        atd.CampusId,
                                                        atd.StudentId,
                                                        atd.AdmissionNumber,
                                                        atd.Students.FirstName,
                                                        atd.Students.LastName,
                                                        atd.Terms.TermName,
                                                        atd.Sessions.SessionName,
                                                        atd.Classes.ClassName,
                                                        atd.ClassGrades.GradeName,
                                                        atd.AttendancePeriodIdMorning,
                                                        atd.AttendancePeriodIdAfternoon,
                                                        atd.AttendanceDate
                                                    };

                            data.Add(new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = attendanceDetails.ToList() });
                        }
                    }
                    else
                    {
                        return new GenericRespModel { StatusCode = 408, StatusMessage = "No Child Selected" };
                    }
                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = data };
                }
            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                data.Add(new GenericRespModel { StatusCode = 500, StatusMessage = "An Error Occured!" });
                return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = data };
            }
        }
        //------------------------------------------ChildrenSubject--------------------------------------------------
        public async Task<GenericRespModel> getChildrenSubjectAsync(IList<Guid> childrenId, Guid parentId)
        {
            IList<object> data = new List<object>();
            try
            {
                CheckerValidation check = new CheckerValidation(_context);
                var checkPrt = check.checkParentById(parentId);
                if (checkPrt != true)
                {
                    return new GenericRespModel { StatusCode = 409, StatusMessage = "No Parent with the specified ID" };
                }
                else
                {
                    if (childrenId.Count > 0)
                    {
                        foreach (Guid childId in childrenId)
                        {

                            var checkchild = check.checkStudentById(childId);
                            if (checkchild != true)
                            {
                                data.Add(new GenericRespModel { StatusCode = 407, StatusMessage = "No Child with the specified ID" });
                                continue;
                            }
                            var parentStudentMap = _context.ParentsStudentsMap.Where(x => x.ParentId == parentId && x.StudentId == childId).FirstOrDefault();
                            if (parentStudentMap == null)
                            {
                                data.Add(new GenericRespModel { StatusCode = 406, StatusMessage = "No Relationship between the parent and the child" });
                                continue;
                            }
                            var childGrade = _context.GradeStudents.Where(x => x.StudentId == childId).FirstOrDefault();
                            if (childGrade != null)
                            {

                                //Subject
                                var subjects = from sub in _context.SchoolSubjects
                                               where sub.ClassId == childGrade.ClassId
                                               select new
                                               {
                                                   sub.Classes.CampusId,
                                                   sub.ClassId,
                                                   sub.DepartmentId,
                                                   sub.Id,
                                                   sub.IsActive,
                                                   sub.SchoolCampus.CampusName,
                                                   sub.SchoolId,
                                                   sub.Schools.SchoolName,
                                                   sub.SubjectCode,
                                                   sub.SubjectDepartment.DepartmentName,
                                                   sub.SubjectName
                                               };


                                data.Add(new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = subjects.ToList() });
                            }
                            else
                            {
                                data.Add(new GenericRespModel { StatusCode = 405, StatusMessage = "Child has not been assigned to any Grade Class" });
                            }
                        }
                    }
                    else
                    {
                        return new GenericRespModel { StatusCode = 408, StatusMessage = "No Child Selected" };
                    }
                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = data };
                }
            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                data.Add(new GenericRespModel { StatusCode = 500, StatusMessage = "An Error Occured!" });
                return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = data };
            }
        }
        //-------------------------------------ChildAttendance-----------------------------------------------
        public async Task<GenericRespModel> getChildAttendanceBySessionIdAsync(Guid childId, Guid parentId, long sessionId)
        {
            try
            {
                CheckerValidation check = new CheckerValidation(_context);
                var checkSession = check.checkSessionById(sessionId);
                if (checkSession != true)
                {
                    return new GenericRespModel { StatusCode = 409, StatusMessage = "No Session with the specified ID" };
                }
                var checkPrt = check.checkParentById(parentId);
                if (checkPrt != true)
                {
                    return new GenericRespModel { StatusCode = 405, StatusMessage = "No Parent with the specified ID" };
                }
                var checkChild = check.checkStudentById(childId);
                if (checkChild != true)
                {
                    return new GenericRespModel { StatusCode = 407, StatusMessage = "No Child with the specified ID" };
                }
                var parentStudentMap = _context.ParentsStudentsMap.Where(x => x.ParentId == parentId && x.StudentId == childId).FirstOrDefault();
                if (parentStudentMap == null)
                {
                    return new GenericRespModel { StatusCode = 406, StatusMessage = "No Relationship between the parent and the child" };
                }

                //Attendance details
                var attendanceDetails = from atd in _context.StudentAttendance
                                        where atd.SessionId == sessionId && atd.StudentId == childId
                                        select new
                                        {
                                            atd.Id,
                                            atd.SchoolId,
                                            atd.CampusId,
                                            atd.StudentId,
                                            atd.AdmissionNumber,
                                            atd.Students.FirstName,
                                            atd.Students.LastName,
                                            atd.Terms.TermName,
                                            atd.Sessions.SessionName,
                                            atd.Classes.ClassName,
                                            atd.ClassGrades.GradeName,
                                            atd.AttendancePeriodIdMorning,
                                            atd.AttendancePeriodIdAfternoon,
                                            atd.AttendanceDate
                                        };

                return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = attendanceDetails.ToList() };

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

        public async Task<GenericRespModel> getChildAttendanceByTermIdAsync(Guid childId, Guid parentId, long termId)
        {
            try
            {
                CheckerValidation check = new CheckerValidation(_context);
                var checkTerm = check.checkTermById(termId);
                if (checkTerm != true)
                {
                    return new GenericRespModel { StatusCode = 409, StatusMessage = "No Term with the specified ID" };
                }
                var checkPrt = check.checkParentById(parentId);
                if (checkPrt != true)
                {
                    return new GenericRespModel { StatusCode = 405, StatusMessage = "No Parent with the specified ID" };
                }
                var checkChild = check.checkStudentById(childId);
                if (checkChild != true)
                {
                    return new GenericRespModel { StatusCode = 407, StatusMessage = "No Child with the specified ID" };
                }
                var parentStudentMap = _context.ParentsStudentsMap.Where(x => x.ParentId == parentId && x.StudentId == childId).FirstOrDefault();
                if (parentStudentMap == null)
                {
                    return new GenericRespModel { StatusCode = 406, StatusMessage = "No Relationship between the parent and the child" };
                }

                //Attendance details
                var attendanceDetails = from atd in _context.StudentAttendance
                                        where atd.TermId == termId && atd.StudentId == childId
                                        select new
                                        {
                                            atd.Id,
                                            atd.SchoolId,
                                            atd.CampusId,
                                            atd.StudentId,
                                            atd.AdmissionNumber,
                                            atd.Students.FirstName,
                                            atd.Students.LastName,
                                            atd.Terms.TermName,
                                            atd.Sessions.SessionName,
                                            atd.Classes.ClassName,
                                            atd.ClassGrades.GradeName,
                                            atd.AttendancePeriodIdMorning,
                                            atd.AttendancePeriodIdAfternoon,
                                            atd.AttendanceDate
                                        };

                return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = attendanceDetails.ToList() };

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

        public async Task<GenericRespModel> getChildAttendanceByDateAsync(Guid childId, Guid parentId, DateTime startDate, DateTime endDate)
        {
            try
            {
                CheckerValidation check = new CheckerValidation(_context);
                var checkPrt = check.checkParentById(parentId);
                if (checkPrt != true)
                {
                    return new GenericRespModel { StatusCode = 405, StatusMessage = "No Parent with the specified ID" };
                }
                var checkChild = check.checkStudentById(childId);
                if (checkChild != true)
                {
                    return new GenericRespModel { StatusCode = 407, StatusMessage = "No Child with the specified ID" };
                }
                var parentStudentMap = _context.ParentsStudentsMap.Where(x => x.ParentId == parentId && x.StudentId == childId).FirstOrDefault();
                if (parentStudentMap == null)
                {
                    return new GenericRespModel { StatusCode = 406, StatusMessage = "No Relationship between the parent and the child" };
                }

                //Attendance details
                var attendanceDetails = from atd in _context.StudentAttendance
                                        where atd.StudentId == childId && atd.AttendanceDate >= startDate && atd.AttendanceDate < endDate.AddDays(1)
                                        select new
                                        {
                                            atd.Id,
                                            atd.SchoolId,
                                            atd.CampusId,
                                            atd.StudentId,
                                            atd.AdmissionNumber,
                                            atd.Students.FirstName,
                                            atd.Students.LastName,
                                            atd.Terms.TermName,
                                            atd.Sessions.SessionName,
                                            atd.Classes.ClassName,
                                            atd.ClassGrades.GradeName,
                                            atd.AttendancePeriodIdMorning,
                                            atd.AttendancePeriodIdAfternoon,
                                            atd.AttendanceDate
                                        };

                return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = attendanceDetails.ToList() };

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
        //------------------------------------------ChildSubject--------------------------------------------------
        public async Task<GenericRespModel> getChildSubjectAsync(Guid childId, Guid parentId)
        {
            try
            {
                CheckerValidation check = new CheckerValidation(_context);
                var checkPrt = check.checkParentById(parentId);
                if (checkPrt != true)
                {
                    return new GenericRespModel { StatusCode = 409, StatusMessage = "No Parent with the specified ID" };
                }
                var checkChild = check.checkStudentById(childId);
                if (checkChild != true)
                {
                    return new GenericRespModel { StatusCode = 407, StatusMessage = "No Child with the specified ID" };
                }
                var parentStudentMap = _context.ParentsStudentsMap.Where(x => x.ParentId == parentId && x.StudentId == childId).FirstOrDefault();
                if (parentStudentMap == null)
                {
                    return new GenericRespModel { StatusCode = 406, StatusMessage = "No Relationship between the parent and the child" };
                }
                var childGrade = _context.GradeStudents.Where(x => x.StudentId == childId).FirstOrDefault();
                if (childGrade != null)
                {

                    //Subject
                    var subjects = from sub in _context.SchoolSubjects
                                   where sub.ClassId == childGrade.ClassId
                                   select new
                                   {
                                       sub.Classes.CampusId,
                                       sub.ClassId,
                                       sub.DepartmentId,
                                       sub.Id,
                                       sub.IsActive,
                                       sub.SchoolCampus.CampusName,
                                       sub.SchoolId,
                                       sub.Schools.SchoolName,
                                       sub.SubjectCode,
                                       sub.SubjectDepartment.DepartmentName,
                                       sub.SubjectName
                                   };


                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = subjects.ToList() };
                }
                else
                {
                    return new GenericRespModel { StatusCode = 405, StatusMessage = "Child has not been assigned to any Grade Class" };
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

        public async Task<GenericRespModel> forgotPasswordAsync(string email)
        {
            try
            {
                var response = new GenericRespModel();
                //Check if email exist
                CheckerValidation emailcheck = new CheckerValidation(_context);

                var getUser = _context.Parents.FirstOrDefault(u => u.Email == email);

                if (getUser != null)
                {
                    var paswordHasher = new PasswordHasher();
                    //the salt
                    string salt = paswordHasher.getSalt();
                    //get deafault password
                    string password = RandomNumberGenerator.RandomString();
                    //Hash the password and salt
                    string passwordHash = paswordHasher.hashedPassword(password, salt);

                    getUser.Salt = salt;
                    getUser.PasswordHash = passwordHash;
                    await _context.SaveChangesAsync();

                    //code to send Mail to user for account activation
                    string MailContent = _emailTemplate.EmailForgotPassword(password);
                    EmailMessage message = new EmailMessage(getUser.Email, MailContent);
                    _emailRepo.SendEmail(message);

                    //response
                    response.StatusCode = 200;
                    response.StatusMessage = "Default Password Generated and sent to mail Successfully, Kindly Change Password after Login!";

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
                var response = new GenericRespModel();
                //Check if email exist
                CheckerValidation emailcheck = new CheckerValidation(_context);

                var getUser = _context.Parents.FirstOrDefault(u => u.Email == email);

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

        public async Task<GenericRespModel> updateParentDetailsAsync(Guid parentId, UpdateParentReqModel obj)
        {
            try
            {
                CheckerValidation check = new CheckerValidation(_context);
                var checkPrt = check.checkParentById(parentId);
                if (checkPrt == true)
                {
                    var getParent = _context.Parents.Where(p => p.Id == parentId).FirstOrDefault();

                    getParent.FirstName = obj.ParentFirstName;
                    getParent.LastName = obj.ParentLastName;
                    getParent.Email = obj.ParentEmail;
                    getParent.PhoneNumber = obj.ParentPhoneNumber;
                    getParent.GenderId = obj.ParentGenderId;
                    getParent.Nationality = obj.ParentNationality;
                    getParent.State = obj.ParentState;
                    getParent.City = obj.ParentCity;
                    getParent.HomeAddress = obj.ParentHomeAddress;
                    getParent.Occupation = obj.ParentOccupation;
                    getParent.StateOfOrigin = obj.ParentStateOfOrigin;
                    getParent.LocalGovt = obj.ParentLocalGovt;
                    getParent.Religion = obj.ParentReligion;
                    getParent.LastUpdatedDate = DateTime.Now;

                    await _context.SaveChangesAsync();

                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Parent Details Updated Successfully" };

                }

                return new GenericRespModel { StatusCode = 409, StatusMessage = "No Parent with the specified ID" };

            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericRespModel { StatusCode = 500, StatusMessage = "An Error Occurred!",};
            }
        }
    }
}
