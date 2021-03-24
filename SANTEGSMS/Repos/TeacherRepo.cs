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
    public class TeacherRepo : ITeacherRepo
    {
        private readonly AppDbContext _context;

        public TeacherRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<TeacherCreateRespModel> createTeacherAsync(TeacherCreateReqModel obj)
        {
            try
            {
                SchoolUsersInfoRespModel teacherRespData = new SchoolUsersInfoRespModel();
                SchoolBasicInfoLoginRespModel schData = new SchoolBasicInfoLoginRespModel();
                TeacherCreateRespModel respObj = new TeacherCreateRespModel();
                CheckerValidation emailcheck = new CheckerValidation(_context);

                var emailCheckResult = emailcheck.checkIfEmailExist(obj.Email, Convert.ToInt64(EnumUtility.UserCategoty.SchoolUsers));

                if (emailCheckResult == true)
                {
                    return new TeacherCreateRespModel { StatusCode = 409, StatusMessage = "This Email has been taken" };
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

                    //save the SchoolUser details
                    var schUsrs = new SchoolUsers
                    {
                        FirstName = obj.FirstName,
                        LastName = obj.LastName,
                        Email = obj.Email,
                        EmailConfirmed = false,
                        PhoneNumber = obj.PhoneNumber,
                        PhoneNumberConfirmed = false,
                        Salt = salt,
                        PasswordHash = passwordHash,
                        SchoolId = obj.SchoolId,
                        CampusId = obj.CampusId,
                        IsActive = true,
                        DateCreated = DateTime.Now
                    };

                    await _context.SchoolUsers.AddAsync(schUsrs);
                    await _context.SaveChangesAsync();

                    //save the teacher details
                    var tch = new Teachers
                    {
                        SchoolUserId = schUsrs.Id,
                        SchoolId = obj.SchoolId,
                        CampusId = obj.CampusId,
                        IsAssignedToClass = false,
                        IsAssignedSubjects = false,
                        IsActive = true,
                        DateCreated = DateTime.Now
                    };

                    await _context.Teachers.AddAsync(tch);
                    await _context.SaveChangesAsync();

                    //Saves the Roles of the Teacher Created i.e A teacher can be assigned two roles (Subject and Class Teacher)
                    foreach (var roleId in obj.RoleIds)
                    {
                        var tchRole = new SchoolUserRoles
                        {
                            UserId = schUsrs.Id,
                            RoleId = roleId.Id,
                            DateCreated = DateTime.Now
                        };

                        await _context.SchoolUserRoles.AddAsync(tchRole);
                        await _context.SaveChangesAsync();

                    }

                    //The data collected from the user on successful creation
                    teacherRespData.UserId = schUsrs.Id.ToString();
                    teacherRespData.FirstName = schUsrs.FirstName;
                    teacherRespData.LastName = schUsrs.LastName;
                    teacherRespData.Email = schUsrs.Email;

                    //Gets the School Information
                    var userSchool = _context.Schools.FirstOrDefault(u => u.Id == obj.SchoolId);
                    //Get the schoolType Name
                    var getSchType = _context.SchoolType.FirstOrDefault(u => u.Id == userSchool.SchoolTypeId);
                    //Get the Campus Name
                    var getCampus = _context.SchoolCampus.FirstOrDefault(u => u.Id == obj.CampusId);
                    //Get Roles Assigned to Teacher (TeacherRoles)
                    var getTeacherRole = from tr in _context.SchoolUserRoles
                                         where tr.UserId == schUsrs.Id
                                         select new
                                         {
                                             tr.UserId,
                                             tr.RoleId,
                                             tr.SchoolRoles.RoleName
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
                    respObj.StatusMessage = "Teacher Created Successfully";
                    respObj.TeacherDetails = teacherRespData;
                    respObj.SchoolDetails = schData;
                    respObj.TeacherRoles = getTeacherRole.ToList(); //List of teacher Roles

                }

                return respObj;
            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new TeacherCreateRespModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<GenericRespModel> getTeacherByIdAsync(Guid teacherId, long schoolId, long campusId)
        {
            try
            {
                var result = from tch in _context.Teachers
                             where tch.SchoolUserId == teacherId && tch.SchoolId == schoolId && tch.CampusId == campusId
                             select new
                             {
                                 tch.Id,
                                 tch.SchoolUserId,
                                 tch.SchoolId,
                                 tch.CampusId,
                                 tch.SchoolUsers.FirstName,
                                 tch.SchoolUsers.LastName,
                                 tch.SchoolUsers.UserName,
                                 tch.SchoolUsers.Email,
                                 tch.SchoolUsers.PhoneNumber,
                                 tch.IsAssignedToClass,
                                 tch.DateCreated,

                             };
                if (result.Count() > 0)
                {
                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = result.FirstOrDefault(), };
                }

                return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful, No Record Available", };

            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericRespModel { StatusCode = 500, StatusMessage = "An Error Occured" };
            }
        }

        public async Task<GenericRespModel> getAllTeachersAsync(long schoolId, long campusId)
        {
            try
            {
                var result = from tch in _context.Teachers
                             where tch.SchoolId == schoolId && tch.CampusId == campusId
                             select new
                             {
                                 tch.Id,
                                 tch.SchoolUserId,
                                 tch.SchoolId,
                                 tch.CampusId,
                                 tch.SchoolUsers.FirstName,
                                 tch.SchoolUsers.LastName,
                                 tch.SchoolUsers.UserName,
                                 tch.SchoolUsers.Email,
                                 tch.SchoolUsers.PhoneNumber,
                                 tch.IsAssignedToClass,
                                 tch.DateCreated,
                             };
                if (result.Count() > 0)
                {
                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList()};
                }

                return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful, No Record Available"};

            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericRespModel { StatusCode = 500, StatusMessage = "An Error Occured" };
            }
        }

        //Teacher By their roles (Subject or Class Teacher)
        public async Task<GenericRespModel> getAllTeachersByRoleIdAsync(long roleId, long schoolId, long campusId)
        {
            try
            {
                var result = from usRol in _context.SchoolUserRoles
                             join usr in _context.SchoolUsers on usRol.UserId equals usr.Id
                             join tch in _context.Teachers on usRol.UserId equals tch.SchoolUserId
                             where usr.SchoolId == schoolId && usr.CampusId == campusId && usRol.RoleId == roleId
                             select new
                             {
                                 usRol.Id,
                                 tch.SchoolUserId,
                                 usr.SchoolId,
                                 usr.CampusId,
                                 SchoolRoleId = usRol.SchoolRoles.Id,
                                 usRol.SchoolRoles.RoleName,
                                 usr.FirstName,
                                 usr.LastName,
                                 usr.UserName,
                                 usr.Email,
                                 usr.PhoneNumber,
                                 tch.IsAssignedToClass,
                                 usRol.SchoolUsers.DateCreated,
                             };

                if (result.Count() > 0)
                {
                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList(), };
                }

                return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful, No Record Available", };

            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericRespModel { StatusCode = 500, StatusMessage = "An Error Occured" };
            }
        }

        // All Types of Teacher Roles  (Class Teacher or Subject Teacher)
        public async Task<GenericRespModel> getTeacherRolesAsync()
        {
            try
            {//get the Class Teacher and Subject Teacher Roles
                var result = from rol in _context.SchoolRoles
                             where rol.Id > Convert.ToInt64(EnumUtility.SchoolRoles.Administrator)
                             && rol.Id <= Convert.ToInt64(EnumUtility.SchoolRoles.SubjectTeacher)
                             select new
                             {
                                 rol.Id,
                                 rol.RoleName,
                             };
                if (result.Count() > 0)
                {
                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList(), };
                }

                return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful, No Record Available", };

            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericRespModel { StatusCode = 500, StatusMessage = "An Error Occured" };
            }
        }

        //Assign a Teacher to a Class
        public async Task<GenericRespModel> assignTeacherToClassGradeAsync(AssignTeacherToClassReqModel obj)
        {
            //NB: A Teacher can also be assigned to many classes
            try
            {
                //checks if a teacher exists
                var checkTeacher = _context.SchoolUsers.Where(x => x.Id == obj.TeacherId).FirstOrDefault();

                if (checkTeacher != null)
                {
                    //checks if the Classarm/Grade has been assigned to a teacher
                    var gradeIsAssigned = _context.ClassGrades.Where(x => x.Id == obj.ClassGradeId).FirstOrDefault();

                    if (gradeIsAssigned != null && gradeIsAssigned.isAssignedToTeacher == true)
                    {
                        return new GenericRespModel { StatusCode = 200, StatusMessage = "A Teacher has been Assigned to this Class Arm!" };
                    }
                    var grd = new GradeTeachers
                    {
                        SchoolUserId = obj.TeacherId,
                        ClassId = obj.ClassId,
                        ClassGradeId = obj.ClassGradeId,
                        SchoolId = obj.SchoolId,
                        CampusId = obj.CampusId,
                        DateCreated = DateTime.Now
                    };

                    //update the Class Grade (isAssignedToTeacher to True)
                    var classGrade = _context.ClassGrades.Where(x => x.Id == obj.ClassGradeId).FirstOrDefault();
                    classGrade.isAssignedToTeacher = true;

                    var tch = _context.Teachers.Where(x => x.SchoolUserId == obj.TeacherId).FirstOrDefault();
                    //update the teacher "IsAssignedToClass" to true
                    tch.IsAssignedToClass = true;

                    await _context.GradeTeachers.AddAsync(grd);
                    await _context.SaveChangesAsync();

                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Teacher Assigned to Class Arm Successful!" };

                }

                return new GenericRespModel { StatusCode = 200, StatusMessage = "Teacher with the specified ID does not exist!" };

            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericRespModel { StatusCode = 500, StatusMessage = "An Error Occured" };
            }
        }


        public async Task<GenericRespModel> getAllClassGradeAssignedToTeacherAsync(long schoolId, long campusId, Guid teacherId)
        {
            try
            {
                var result = from tch in _context.GradeTeachers
                             where tch.SchoolUserId == teacherId && tch.SchoolId == schoolId && tch.CampusId == campusId
                             select new
                             {
                                 tch.Id,
                                 tch.SchoolUserId,
                                 tch.SchoolId,
                                 tch.CampusId,
                                 tch.ClassId,
                                 tch.ClassGradeId,
                                 tch.Classes.ClassName,
                                 tch.ClassGrades.GradeName,
                                 tch.DateCreated,
                             };
                if (result.Count() > 0)
                {
                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList(), };
                }

                return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful, No Record Available", };

            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericRespModel { StatusCode = 500, StatusMessage = "An Error Occured" };
            }
        }

        //Assigned Teachers
        public async Task<GenericRespModel> getAssignedTeachersAsync(long schoolId, long campusId)
        {
            try
            {
                var result = from tch in _context.Teachers
                             where tch.IsAssignedToClass == true && tch.SchoolId == schoolId && tch.CampusId == campusId
                             select new
                             {
                                 tch.Id,
                                 tch.SchoolUserId,
                                 tch.SchoolId,
                                 tch.CampusId,
                                 tch.SchoolUsers.FirstName,
                                 tch.SchoolUsers.LastName,
                                 tch.SchoolUsers.UserName,
                                 tch.SchoolUsers.Email,
                                 tch.SchoolUsers.PhoneNumber,
                                 tch.IsAssignedToClass,
                                 tch.DateCreated,
                             };
                if (result.Count() > 0)
                {
                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList(), };
                }

                return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful, No Record Available", };

            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericRespModel { StatusCode = 500, StatusMessage = "An Error Occured" };
            }
        }

        public async Task<GenericRespModel> getAllRolesAssignedToTeacherAsync(Guid teacherId, long schoolId, long campusId)
        {
            try
            {
                var result = from usRol in _context.SchoolUserRoles
                             join tch in _context.Teachers on usRol.UserId equals tch.SchoolUserId
                             where tch.SchoolUserId == teacherId && tch.SchoolId == schoolId && tch.CampusId == campusId
                             select new
                             {
                                 tch.SchoolId,
                                 tch.CampusId,
                                 tch.SchoolUserId,
                                 usRol.RoleId,
                                 usRol.SchoolRoles.RoleName,
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
                return new GenericRespModel { StatusCode = 500, StatusMessage = "An Error Occured" };
            }
        }

        //--------------------------------------------------ATTENDANCE--------------------------------------------------------------------------------------------

        public async Task<GenericRespModel> takeClassAttendanceAsync(TakeAttendanceReqModel obj)
        {
            try
            {
                //Checkers
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(obj.SchoolId);
                var checkCampus = check.checkSchoolCampusById(obj.CampusId);

                //response data
                GenericRespModel response = new GenericRespModel();

                //Session and Term
                SessionAndTerm sessionTerm = new SessionAndTerm(_context);
                var currentSessionId = sessionTerm.getCurrentSessionId(obj.SchoolId);
                var currentTermId = sessionTerm.getCurrentTermId(obj.SchoolId);


                if (checkSchool != true)
                {
                    return new GenericRespModel { StatusCode = 409, StatusMessage = "No School with the specified ID" };
                }
                if (checkCampus != true)
                {
                    return new GenericRespModel { StatusCode = 409, StatusMessage = "No Campus with the specified ID" };
                }
                if (currentSessionId == 0 || currentTermId == 0)
                {
                    return new GenericRespModel { StatusCode = 409, StatusMessage = "Academic current Term and Session has not been set" };
                }
                else
                {
                    const int maximumAttendanceForaDay = 2;
                    //Counts the number of students in the class
                    var allStudentInClass = (from gd in _context.GradeStudents where gd.SchoolId == obj.SchoolId && gd.CampusId == obj.CampusId && gd.SessionId == currentSessionId select gd).ToList().Count();
                    int maxAttendanceCount = allStudentInClass * maximumAttendanceForaDay;

                    var attendanceList = (from att in _context.StudentAttendance
                                          where att.SchoolId == obj.SchoolId && att.CampusId == obj.CampusId && att.ClassId == obj.ClassId && att.ClassGradeId == obj.ClassGradeId && att.AttendanceDate == obj.AttendanceDate
                                          select att).ToList();

                    if (attendanceList.ToList().Count > maxAttendanceCount)
                    {
                        return new GenericRespModel { StatusCode = 200, StatusMessage = "Maximum attendance has been taken for today" };
                    }
                    else if (allStudentInClass != obj.StudentIds.Count())
                    {
                        return new GenericRespModel { StatusCode = 200, StatusMessage = "Number of Student IDs must be equal to the total number of students in class" };
                    }
                    else
                    {
                        //if the Attendance period to be taken is Morning
                        if (obj.AttendancePeriodId == (long)EnumUtility.AttendancePeriod.Morning)
                        {
                            foreach (var stdId in obj.StudentIds)
                            {
                                //get the student Admission Number using the student ID
                                var student =  _context.Students.Where(x => x.Id == stdId.StudentId).FirstOrDefault();

                                var periods = _context.StudentAttendance.FirstOrDefault(x => x.StudentId == stdId.StudentId && x.AttendanceDate == obj.AttendanceDate && x.AttendancePeriodId == (long)EnumUtility.AttendancePeriod.Both);
                                if (periods != null)
                                {
                                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Attendance has been taken for both periods today!" };
                                }
                                else
                                {
                                    //Check if the student Attendance for that day and period has been taken
                                    var studentAttendance =  _context.StudentAttendance.Where(x => x.StudentId == stdId.StudentId && x.AttendanceDate == obj.AttendanceDate).FirstOrDefault();

                                    //if the attendance has not been taken, take new attendance for that day
                                    if (studentAttendance == null)
                                    {
                                        //if the student AdmissionNumber is checked
                                        if (stdId.isChecked == true)
                                        {
                                            //take the attendance and set PresentAbsent property to 1 (i.e. student is present)
                                            var newAttendance = new StudentAttendance
                                            {
                                                SchoolId = obj.SchoolId,
                                                CampusId = obj.CampusId,
                                                ClassId = obj.ClassId,
                                                ClassGradeId = obj.ClassGradeId,
                                                SessionId = currentSessionId,
                                                TermId = currentTermId,
                                                AdmissionNumber = student.AdmissionNumber,
                                                StudentId = stdId.StudentId,
                                                AttendancePeriodId = obj.AttendancePeriodId,
                                                PresentAbsent = 1, //student is present
                                                AttendancePeriodIdMorning = (long)EnumUtility.AttendancePeriod.Morning,
                                                AttendanceDate = obj.AttendanceDate,
                                            };
                                            await _context.StudentAttendance.AddAsync(newAttendance);
                                            await _context.SaveChangesAsync();
                                        }
                                        else
                                        {
                                            //take the attendance and set PresentAbsent property to 0 (i.e. student is Absent)
                                            var newAttendance = new StudentAttendance
                                            {
                                                SchoolId = obj.SchoolId,
                                                CampusId = obj.CampusId,
                                                ClassId = obj.ClassId,
                                                ClassGradeId = obj.ClassGradeId,
                                                SessionId = currentSessionId,
                                                TermId = currentTermId,
                                                AdmissionNumber = student.AdmissionNumber,
                                                StudentId = stdId.StudentId,
                                                AttendancePeriodId = obj.AttendancePeriodId,
                                                PresentAbsent = 0, //student is Absent
                                                AttendancePeriodIdMorning = 0, //attendance not taken for morning
                                                AttendanceDate = obj.AttendanceDate,
                                            };
                                            await _context.StudentAttendance.AddAsync(newAttendance);
                                            await _context.SaveChangesAsync();
                                        }

                                        response.StatusCode = 200;
                                        response.StatusMessage = "Attendance Taken Successfully";
                                        response.Data = null;
                                    }
                                    else //if the attendance has been taken for that day Update the student attendance for that day and period selected
                                    {
                                        //if (studentAttendance.AttendancePeriodIdMorning > 0 && studentAttendance.AttendancePeriodIdAfternoon > 0)
                                        //{
                                        //    return new GenericRespModel { StatusCode = 409, StatusMessage = "Maximum attendance has been taken for today" };
                                        //}
                                        //else
                                        //{
                                        //if the student AdmissionNumber is checked
                                        if (stdId.isChecked == true)
                                        {
                                            //Update the attendance and set PresentAbsent property to 1 (i.e. student is present)
                                            studentAttendance.SchoolId = obj.SchoolId;
                                            studentAttendance.CampusId = obj.CampusId;
                                            studentAttendance.ClassId = obj.ClassId;
                                            studentAttendance.ClassGradeId = obj.ClassGradeId;
                                            studentAttendance.SessionId = currentSessionId;
                                            studentAttendance.TermId = currentTermId;
                                            studentAttendance.AdmissionNumber = student.AdmissionNumber;
                                            studentAttendance.StudentId = stdId.StudentId;
                                            studentAttendance.AttendancePeriodId = obj.AttendancePeriodId;
                                            studentAttendance.PresentAbsent = 1; //student is present
                                            studentAttendance.AttendancePeriodIdMorning = (long)EnumUtility.AttendancePeriod.Morning;
                                            studentAttendance.AttendanceDate = obj.AttendanceDate;

                                            await _context.SaveChangesAsync();
                                        }
                                        else
                                        {
                                            //Update the attendance and set PresentAbsent property to 0 (i.e. student is Absent)
                                            studentAttendance.SchoolId = obj.SchoolId;
                                            studentAttendance.CampusId = obj.CampusId;
                                            studentAttendance.ClassId = obj.ClassId;
                                            studentAttendance.ClassGradeId = obj.ClassGradeId;
                                            studentAttendance.SessionId = currentSessionId;
                                            studentAttendance.TermId = currentTermId;
                                            studentAttendance.AdmissionNumber = student.AdmissionNumber;
                                            studentAttendance.StudentId = stdId.StudentId;
                                            studentAttendance.AttendancePeriodId = obj.AttendancePeriodId;
                                            studentAttendance.PresentAbsent = 0; //student is Absent
                                            studentAttendance.AttendancePeriodIdMorning = 0;  //attendance not taken for morning
                                            studentAttendance.AttendanceDate = obj.AttendanceDate;

                                            await _context.SaveChangesAsync();
                                        }

                                        response.StatusCode = 200;
                                        response.StatusMessage = "Attendance Taken Successfully";
                                        response.Data = null;
                                    }
                                }
                            }
                        }
                        else if (obj.AttendancePeriodId == (long)EnumUtility.AttendancePeriod.Afternoon)
                        {
                            foreach (var stdId in obj.StudentIds)
                            {
                                //get the student Admission Number using the student ID
                                var student =  _context.Students.Where(x => x.Id == stdId.StudentId).FirstOrDefault();

                                var periods = _context.StudentAttendance.FirstOrDefault(x => x.StudentId == stdId.StudentId && x.AttendanceDate == obj.AttendanceDate && x.AttendancePeriodId == (long)EnumUtility.AttendancePeriod.Both);
                                if (periods != null)
                                {
                                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Attendance has been taken for both periods today!" };
                                }
                                else
                                {
                                    //Check if the student Attendance for that day has been taken
                                    var studentAttendance =  _context.StudentAttendance.Where(x => x.StudentId == stdId.StudentId && x.AttendanceDate == obj.AttendanceDate).FirstOrDefault();

                                    //if the attendance has not been taken, take new attendance for that day
                                    if (studentAttendance == null)
                                    {
                                        //if the student AdmissionNumber is checked
                                        if (stdId.isChecked == true)
                                        {
                                            //take the attendance and set PresentAbsent property to 1 (i.e. student is present)
                                            var newAttendance = new StudentAttendance
                                            {
                                                SchoolId = obj.SchoolId,
                                                CampusId = obj.CampusId,
                                                ClassId = obj.ClassId,
                                                ClassGradeId = obj.ClassGradeId,
                                                SessionId = currentSessionId,
                                                TermId = currentTermId,
                                                AdmissionNumber = student.AdmissionNumber,
                                                StudentId = stdId.StudentId,
                                                AttendancePeriodId = obj.AttendancePeriodId,
                                                PresentAbsent = 1, //student is present
                                                AttendancePeriodIdAfternoon = (long)EnumUtility.AttendancePeriod.Afternoon,
                                                AttendanceDate = obj.AttendanceDate,
                                            };
                                            await _context.StudentAttendance.AddAsync(newAttendance);
                                            await _context.SaveChangesAsync();
                                        }
                                        else
                                        {
                                            //take the attendance and set PresentAbsent property to 0 (i.e. student is Absent)
                                            var newAttendance = new StudentAttendance
                                            {
                                                SchoolId = obj.SchoolId,
                                                CampusId = obj.CampusId,
                                                ClassId = obj.ClassId,
                                                ClassGradeId = obj.ClassGradeId,
                                                SessionId = currentSessionId,
                                                TermId = currentTermId,
                                                AdmissionNumber = student.AdmissionNumber,
                                                StudentId = stdId.StudentId,
                                                AttendancePeriodId = obj.AttendancePeriodId,
                                                PresentAbsent = 0, //student is Absent
                                                AttendancePeriodIdAfternoon = 0, //attendance not taken for morning
                                                AttendanceDate = obj.AttendanceDate,
                                            };
                                            await _context.StudentAttendance.AddAsync(newAttendance);
                                            await _context.SaveChangesAsync();
                                        }
                                        response.StatusCode = 200;
                                        response.StatusMessage = "Attendance Taken Successfully";
                                        response.Data = null;

                                    }
                                    else //if the attendance has been taken for that day Update the student attendance for that day and period selected
                                    {
                                        //if the student AdmissionNumber is checked
                                        if (stdId.isChecked == true)
                                        {
                                            //Update the attendance and set PresentAbsent property to 1 (i.e. student is present)

                                            studentAttendance.SchoolId = obj.SchoolId;
                                            studentAttendance.CampusId = obj.CampusId;
                                            studentAttendance.ClassId = obj.ClassId;
                                            studentAttendance.ClassGradeId = obj.ClassGradeId;
                                            studentAttendance.SessionId = currentSessionId;
                                            studentAttendance.TermId = currentTermId;
                                            studentAttendance.AdmissionNumber = student.AdmissionNumber;
                                            studentAttendance.StudentId = stdId.StudentId;
                                            studentAttendance.AttendancePeriodId = obj.AttendancePeriodId;
                                            studentAttendance.PresentAbsent = 1; //student is present
                                            studentAttendance.AttendancePeriodIdAfternoon = (long)EnumUtility.AttendancePeriod.Afternoon;
                                            studentAttendance.AttendanceDate = obj.AttendanceDate;

                                            await _context.SaveChangesAsync();
                                        }
                                        else
                                        {
                                            //Update the attendance and set PresentAbsent property to 0 (i.e. student is Absent)
                                            studentAttendance.SchoolId = obj.SchoolId;
                                            studentAttendance.CampusId = obj.CampusId;
                                            studentAttendance.ClassId = obj.ClassId;
                                            studentAttendance.ClassGradeId = obj.ClassGradeId;
                                            studentAttendance.SessionId = currentSessionId;
                                            studentAttendance.TermId = currentTermId;
                                            studentAttendance.AdmissionNumber = student.AdmissionNumber;
                                            studentAttendance.StudentId = stdId.StudentId;
                                            studentAttendance.AttendancePeriodId = obj.AttendancePeriodId;
                                            studentAttendance.PresentAbsent = 0; //student is Absent
                                            studentAttendance.AttendancePeriodIdAfternoon = 0;  //attendance not taken for morning
                                            studentAttendance.AttendanceDate = obj.AttendanceDate;

                                            await _context.SaveChangesAsync();
                                        }

                                        response.StatusCode = 200;
                                        response.StatusMessage = "Attendance Taken Successfully";
                                        response.Data = null;
                                    }
                                }
                            }
                        }
                        else if (obj.AttendancePeriodId == (long)EnumUtility.AttendancePeriod.Both)
                        {
                            foreach (var stdId in obj.StudentIds)
                            {
                                //get the student Admission Number using the student ID
                                var student =  _context.Students.Where(x => x.Id == stdId.StudentId).FirstOrDefault();

                                var periods = _context.StudentAttendance.FirstOrDefault(x => x.StudentId == stdId.StudentId && x.AttendanceDate == obj.AttendanceDate && (x.AttendancePeriodId == (long)EnumUtility.AttendancePeriod.Morning || x.AttendancePeriodId == (long)EnumUtility.AttendancePeriod.Afternoon));
                                if (periods != null)
                                {
                                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Attendance has been taken for one or both periods today" };
                                }
                                else
                                {
                                    //Check if the student Attendance for that day has been taken
                                    var studentAttendance =  _context.StudentAttendance.Where(x => x.StudentId == stdId.StudentId && x.AttendanceDate == obj.AttendanceDate).FirstOrDefault();

                                    //if the attendance has not been taken, take new attendance for that day
                                    if (studentAttendance == null)
                                    {
                                        //if the student AdmissionNumber is checked
                                        if (stdId.isChecked == true)
                                        {
                                            //take the attendance and set PresentAbsent property to 1 (i.e. student is present)
                                            var newAttendance = new StudentAttendance
                                            {
                                                SchoolId = obj.SchoolId,
                                                CampusId = obj.CampusId,
                                                ClassId = obj.ClassId,
                                                ClassGradeId = obj.ClassGradeId,
                                                SessionId = currentSessionId,
                                                TermId = currentTermId,
                                                AdmissionNumber = student.AdmissionNumber,
                                                StudentId = stdId.StudentId,
                                                AttendancePeriodId = obj.AttendancePeriodId,
                                                PresentAbsent = 1, //student is present
                                                AttendancePeriodIdMorning = (long)EnumUtility.AttendancePeriod.Morning,
                                                AttendancePeriodIdAfternoon = (long)EnumUtility.AttendancePeriod.Afternoon,
                                                AttendanceDate = obj.AttendanceDate,
                                            };
                                            await _context.StudentAttendance.AddAsync(newAttendance);
                                            await _context.SaveChangesAsync();
                                        }
                                        else
                                        {
                                            //take the attendance and set PresentAbsent property to 0 (i.e. student is Absent)
                                            var newAttendance = new StudentAttendance
                                            {
                                                SchoolId = obj.SchoolId,
                                                CampusId = obj.CampusId,
                                                ClassId = obj.ClassId,
                                                ClassGradeId = obj.ClassGradeId,
                                                SessionId = currentSessionId,
                                                TermId = currentTermId,
                                                AdmissionNumber = student.AdmissionNumber,
                                                StudentId = stdId.StudentId,
                                                AttendancePeriodId = obj.AttendancePeriodId,
                                                PresentAbsent = 0, //student is Absent
                                                AttendancePeriodIdMorning = 0, //attendance not taken for morning
                                                AttendancePeriodIdAfternoon = 0, //attendance not taken for Afternoon
                                                AttendanceDate = obj.AttendanceDate,
                                            };
                                            await _context.StudentAttendance.AddAsync(newAttendance);
                                            await _context.SaveChangesAsync();
                                        }
                                        response.StatusCode = 200;
                                        response.StatusMessage = "Attendance Taken Successfully";
                                        response.Data = null;
                                    }
                                    else //if the attendance has been taken for that day Update the student attendance for that day and period selected
                                    {
                                        //if the student AdmissionNumber is checked
                                        if (stdId.isChecked == true)
                                        {
                                            //Update the attendance and set PresentAbsent property to 1 (i.e. student is present)
                                            studentAttendance.SchoolId = obj.SchoolId;
                                            studentAttendance.CampusId = obj.CampusId;
                                            studentAttendance.ClassId = obj.ClassId;
                                            studentAttendance.ClassGradeId = obj.ClassGradeId;
                                            studentAttendance.SessionId = currentSessionId;
                                            studentAttendance.TermId = currentTermId;
                                            studentAttendance.AdmissionNumber = student.AdmissionNumber;
                                            studentAttendance.StudentId = stdId.StudentId;
                                            studentAttendance.AttendancePeriodId = obj.AttendancePeriodId;
                                            studentAttendance.PresentAbsent = 1; //student is present
                                            studentAttendance.AttendancePeriodIdMorning = (long)EnumUtility.AttendancePeriod.Morning;
                                            studentAttendance.AttendancePeriodIdAfternoon = (long)EnumUtility.AttendancePeriod.Afternoon;
                                            studentAttendance.AttendanceDate = obj.AttendanceDate;

                                            await _context.SaveChangesAsync();
                                        }
                                        else
                                        {
                                            //Update the attendance and set PresentAbsent property to 0 (i.e. student is Absent)
                                            studentAttendance.SchoolId = obj.SchoolId;
                                            studentAttendance.CampusId = obj.CampusId;
                                            studentAttendance.ClassId = obj.ClassId;
                                            studentAttendance.ClassGradeId = obj.ClassGradeId;
                                            studentAttendance.SessionId = currentSessionId;
                                            studentAttendance.TermId = currentTermId;
                                            studentAttendance.AdmissionNumber = student.AdmissionNumber;
                                            studentAttendance.StudentId = stdId.StudentId;
                                            studentAttendance.AttendancePeriodId = obj.AttendancePeriodId;
                                            studentAttendance.PresentAbsent = 0; //student is Absent
                                            studentAttendance.AttendancePeriodIdMorning = 0;  //attendance not taken for morning
                                            studentAttendance.AttendancePeriodIdAfternoon = 0;  //attendance not taken for Afternoon
                                            studentAttendance.AttendanceDate = obj.AttendanceDate;

                                            await _context.SaveChangesAsync();
                                        }

                                        response.StatusCode = 200;
                                        response.StatusMessage = "Attendance Taken Successfully";
                                        response.Data = null;
                                    }
                                }
                            }
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
                return new GenericRespModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<GenericRespModel> getClassAttendanceAsync(long classId, DateTime attendanceDate, long schoolId, long campusId, long termId, long sessionId)
        {
            try
            {
                var result = from atd in _context.StudentAttendance
                             where atd.SchoolId == schoolId && atd.CampusId == campusId && atd.ClassId == classId && atd.AttendanceDate == attendanceDate
                             && atd.TermId == termId && atd.SessionId == sessionId
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
                if (result.Count() > 0)
                {
                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList(), };
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

        public async Task<GenericRespModel> getClassGradeAttendanceAsync(long classId, long classGradeId, DateTime attendanceDate, long schoolId, long campusId, long termId, long sessionId)
        {
            try
            {
                var result = from atd in _context.StudentAttendance
                             where atd.SchoolId == schoolId && atd.CampusId == campusId && atd.ClassId == classId && atd.ClassGradeId == classGradeId
                             && atd.AttendanceDate == attendanceDate && atd.TermId == termId && atd.SessionId == sessionId
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
                if (result.Count() > 0)
                {
                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList(), };
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

        public async Task<GenericRespModel> getClassAttendanceByPeriodIdAsync(long classId, DateTime attendanceDate, long schoolId, long campusId, long periodId, long termId, long sessionId)
        {
            try
            {
                if (periodId == (long)EnumUtility.AttendancePeriod.Morning)
                {
                    var result = from atd in _context.StudentAttendance
                                 where atd.SchoolId == schoolId && atd.CampusId == campusId && atd.ClassId == classId
                                 && atd.AttendanceDate == attendanceDate && atd.TermId == termId && atd.SessionId == sessionId
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
                                     atd.AttendanceDate
                                 };

                    if (result.Count() > 0)
                    {
                        return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList(), };
                    }

                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful, No Record Available", };
                }
                if (periodId == (long)EnumUtility.AttendancePeriod.Afternoon)
                {
                    var result = from atd in _context.StudentAttendance
                                 where atd.SchoolId == schoolId && atd.CampusId == campusId && atd.ClassId == classId
                                 && atd.AttendanceDate == attendanceDate && atd.TermId == termId && atd.SessionId == sessionId
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
                                     atd.AttendancePeriodIdAfternoon,
                                     atd.AttendanceDate
                                 };
                    if (result.Count() > 0)
                    {
                        return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList(), };
                    }

                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful, No Record Available", };
                }
                if (periodId == (long)EnumUtility.AttendancePeriod.Both)
                {
                    var result = from atd in _context.StudentAttendance
                                 where atd.SchoolId == schoolId && atd.CampusId == campusId && atd.ClassId == classId
                                 && atd.AttendanceDate == attendanceDate && atd.TermId == termId && atd.SessionId == sessionId
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
                    if (result.Count() > 0)
                    {
                        return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList(), };
                    }

                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful, No Record Available", };
                }

                return new GenericRespModel { StatusCode = 409, StatusMessage = "Specify a Period", };

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

        public async Task<GenericRespModel> getClassGradeAttendanceByPeriodIdAsync(long classId, long classGradeId, DateTime attendanceDate, long schoolId, long campusId, long periodId, long termId, long sessionId)
        {
            try
            {
                if (periodId == (long)EnumUtility.AttendancePeriod.Morning)
                {
                    var result = from atd in _context.StudentAttendance
                                 where atd.SchoolId == schoolId && atd.CampusId == campusId && atd.ClassId == classId && atd.ClassGradeId == classGradeId
                                 && atd.AttendanceDate == attendanceDate && atd.TermId == termId && atd.SessionId == sessionId
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
                                     atd.AttendanceDate
                                 };

                    if (result.Count() > 0)
                    {
                        return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList(), };
                    }

                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful, No Record Available", };
                }
                if (periodId == (long)EnumUtility.AttendancePeriod.Afternoon)
                {
                    var result = from atd in _context.StudentAttendance
                                 where atd.SchoolId == schoolId && atd.CampusId == campusId && atd.ClassId == classId && atd.ClassGradeId == classGradeId
                                 && atd.AttendanceDate == attendanceDate && atd.TermId == termId && atd.SessionId == sessionId
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
                                     atd.AttendancePeriodIdAfternoon,
                                     atd.AttendanceDate
                                 };
                    if (result.Count() > 0)
                    {
                        return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList(), };
                    }

                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful, No Record Available", };
                }
                if (periodId == (long)EnumUtility.AttendancePeriod.Both)
                {
                    var result = from atd in _context.StudentAttendance
                                 where atd.SchoolId == schoolId && atd.CampusId == campusId && atd.ClassId == classId && atd.ClassGradeId == classGradeId
                                 && atd.AttendanceDate == attendanceDate && atd.TermId == termId && atd.SessionId == sessionId
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
                    if (result.Count() > 0)
                    {
                        return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList(), };
                    }

                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful, No Record Available", };
                }

                return new GenericRespModel { StatusCode = 409, StatusMessage = "Specify a Period", };

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

        public async Task<GenericRespModel> getStudentAttendanceAsync(Guid studentId, long classId, long classGradeId, DateTime attendanceDate, long schoolId, long campusId, long termId, long sessionId)
        {
            try
            {
                var result = from atd in _context.StudentAttendance
                             where atd.StudentId == studentId && atd.SchoolId == schoolId && atd.CampusId == campusId && atd.ClassId == classId && atd.ClassGradeId == classGradeId
                             && atd.AttendanceDate == attendanceDate && atd.TermId == termId && atd.SessionId == sessionId
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

                if (result.Count() > 0)
                {
                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList(), };
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

        public async Task<GenericRespModel> getStudentAttendanceByPeriodIdAsync(Guid studentId, long classId, long classGradeId, DateTime attendanceDate, long schoolId, long campusId, long periodId, long termId, long sessionId)
        {
            try
            {
                if (periodId == (long)EnumUtility.AttendancePeriod.Morning)
                {
                    var result = from atd in _context.StudentAttendance
                                 where atd.StudentId == studentId && atd.SchoolId == schoolId && atd.CampusId == campusId && atd.ClassId == classId && atd.ClassGradeId == classGradeId
                                 && atd.AttendanceDate == attendanceDate && atd.TermId == termId && atd.SessionId == sessionId
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
                                     atd.AttendanceDate
                                 };

                    if (result.Count() > 0)
                    {
                        return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList(), };
                    }

                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful, No Record Available", };
                }
                if (periodId == (long)EnumUtility.AttendancePeriod.Afternoon)
                {
                    var result = from atd in _context.StudentAttendance
                                 where atd.StudentId == studentId && atd.SchoolId == schoolId && atd.CampusId == campusId && atd.ClassId == classId && atd.ClassGradeId == classGradeId
                                 && atd.AttendanceDate == attendanceDate && atd.TermId == termId && atd.SessionId == sessionId
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
                                     atd.AttendancePeriodIdAfternoon,
                                     atd.AttendanceDate
                                 };
                    if (result.Count() > 0)
                    {
                        return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList(), };
                    }

                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful, No Record Available", };
                }
                if (periodId == (long)EnumUtility.AttendancePeriod.Both)
                {
                    var result = from atd in _context.StudentAttendance
                                 where atd.StudentId == studentId && atd.SchoolId == schoolId && atd.CampusId == campusId && atd.ClassId == classId && atd.ClassGradeId == classGradeId
                                 && atd.AttendanceDate == attendanceDate && atd.TermId == termId && atd.SessionId == sessionId
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
                    if (result.Count() > 0)
                    {
                        return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList(), };
                    }

                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful, No Record Available", };
                }

                return new GenericRespModel { StatusCode = 409, StatusMessage = "Specify a Period", };

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
