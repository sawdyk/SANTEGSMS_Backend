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
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.Repos
{
    public class SubjectRepo : ISubjectRepo
    {
        private readonly AppDbContext _context;

        public SubjectRepo(AppDbContext context)
        {
            _context = context;
        }

        //Create new Subject
        public async Task<GenericRespModel> createSubjectAsync(SubjectCreationReqModel obj)
        {
            try
            {
                //checks if a subject has been created for a Class
                var checkResult = _context.SchoolSubjects.Where(x => x.SubjectName == obj.SubjectName && x.ClassId == obj.ClassId && x.SchoolId == obj.SchoolId && x.CampusId == obj.CampusId).FirstOrDefault();
                if (checkResult != null)
                {
                    return new GenericRespModel { StatusCode = 409, StatusMessage = "This Subject Already exist for this Class!" };
                }
                var subj = new SchoolSubjects
                {
                    ClassId = obj.ClassId,
                    SchoolId = obj.SchoolId,
                    CampusId = obj.CampusId,
                    SubjectName = obj.SubjectName,
                    SubjectCode = obj.SubjectCode,
                    MaximumScore = obj.MaximumScore,
                    IsAssignedToTeacher = false,
                    IsActive = true,
                    DateCreated = DateTime.Now
                };

                await _context.SchoolSubjects.AddAsync(subj);
                await _context.SaveChangesAsync();

                return new GenericRespModel { StatusCode = 200, StatusMessage = "Subject Created Successfully" };

            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful" };
            }
        }

        //Subject By ID
        public async Task<GenericRespModel> getSubjectByIdAsync(long subjectId)
        {
            try
            {
                var result = from sub in _context.SchoolSubjects
                             where sub.Id == subjectId
                             select new
                             {
                                 sub.Id,
                                 sub.ClassId,
                                 sub.Classes.ClassName,
                                 sub.SchoolId,
                                 sub.CampusId,
                                 sub.SubjectName,
                                 sub.SubjectCode,
                                 sub.MaximumScore,
                                 sub.SubjectDepartment.DepartmentName,
                                 sub.IsAssignedToTeacher,
                                 sub.IsActive,
                                 sub.DateCreated
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
                return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful" };
            }
        }

        //All Subjects In a Class
        public async Task<GenericRespModel> getAllClassSubjectsAsync(long classId, long schoolId, long campusId)
        {
            try
            {
                var result = from sub in _context.SchoolSubjects
                             where sub.ClassId == classId && sub.SchoolId == schoolId && sub.CampusId == campusId
                             select new
                             {
                                 sub.Id,
                                 sub.ClassId,
                                 sub.Classes.ClassName,
                                 sub.SchoolId,
                                 sub.CampusId,
                                 sub.SubjectName,
                                 sub.SubjectCode,
                                 sub.MaximumScore,
                                 sub.SubjectDepartment.DepartmentName,
                                 sub.IsAssignedToTeacher,
                                 sub.IsActive,
                                 sub.DateCreated
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
                return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful" };
            }
        }

        //All Subjects in School
        public async Task<GenericRespModel> getAllSchoolSubjectsAsync(long schoolId, long campusId)
        {
            try
            {
                var result = from sub in _context.SchoolSubjects
                             where sub.SchoolId == schoolId && sub.CampusId == campusId
                             select new
                             {
                                 sub.Id,
                                 sub.ClassId,
                                 sub.Classes.ClassName,
                                 sub.SchoolId,
                                 sub.CampusId,
                                 sub.SubjectName,
                                 sub.SubjectCode,
                                 sub.MaximumScore,
                                 sub.SubjectDepartment.DepartmentName,
                                 sub.IsAssignedToTeacher,
                                 sub.IsActive,
                                 sub.DateCreated
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
                return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful" };
            }
        }

        //All Assigned Subjects
        public async Task<GenericRespModel> getAllAssignedSubjectsAsync(long schoolId, long campusId)
        {
            try
            {
                var result = from sub in _context.SchoolSubjects
                             where sub.IsAssignedToTeacher == true && sub.SchoolId == schoolId && sub.CampusId == campusId
                             select new
                             {
                                 sub.Id,
                                 sub.ClassId,
                                 sub.Classes.ClassName,
                                 sub.SchoolId,
                                 sub.CampusId,
                                 sub.SubjectName,
                                 sub.SubjectCode,
                                 sub.MaximumScore,
                                 sub.SubjectDepartment.DepartmentName,
                                 sub.IsAssignedToTeacher,
                                 sub.IsActive,
                                 sub.DateCreated
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
                return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful" };
            }
        }

        //All UnAssigned Subjects
        public async Task<GenericRespModel> getAllUnAssignedSubjectsAsync(long schoolId, long campusId)
        {
            try
            {
                var result = from sub in _context.SchoolSubjects
                             where sub.IsAssignedToTeacher == false && sub.SchoolId == schoolId && sub.CampusId == campusId
                             select new
                             {
                                 sub.Id,
                                 sub.ClassId,
                                 sub.Classes.ClassName,
                                 sub.SchoolId,
                                 sub.CampusId,
                                 sub.SubjectName,
                                 sub.SubjectCode,
                                 sub.MaximumScore,
                                 sub.SubjectDepartment.DepartmentName,
                                 sub.IsAssignedToTeacher,
                                 sub.IsActive,
                                 sub.DateCreated
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
                return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful" };
            }
        }

        //Assign subjects to Teachers
        public async Task<GenericRespModel> assignSubjectToTeacherAsync(AssignSubjectToTeacherReqModel obj)
        {
            try
            {
                GenericRespModel respData = new GenericRespModel();

                foreach (SubjectId subjId in obj.SubjectIds)
                {
                    //check if a subject exist and if it hasnt been assigned
                    var checkSubject = _context.SchoolSubjects.Where(x => x.Id == subjId.Id && x.IsAssignedToTeacher == false).FirstOrDefault();

                    if (checkSubject != null)
                    {
                        var getSubj = _context.SchoolSubjects.Where(x => x.Id == subjId.Id).FirstOrDefault();
                        var subj = new SubjectTeachers
                        {
                            SchoolUserId = obj.TeacherId,
                            SubjectId = subjId.Id,
                            ClassId = obj.ClassId, //the classId for the subject
                            ClassGradeId = obj.ClassGradeId, //the classGradeId for the subject
                            SchoolId = obj.SchoolId,
                            CampusId = obj.CampusId,
                            DateCreated = DateTime.Now
                        };

                        //update the teacher "IsAssignedToTeacher" to true
                        checkSubject.IsAssignedToTeacher = true;

                        await _context.SubjectTeachers.AddAsync(subj);
                        await _context.SaveChangesAsync();

                        //return the subjects assigned to the teacher
                        var teachersSubjects = from sub in _context.SubjectTeachers
                                               where sub.SchoolUserId == obj.TeacherId && sub.SchoolId == obj.SchoolId && sub.CampusId == obj.CampusId
                                               select new
                                               {
                                                   sub.SchoolUserId,
                                                   sub.SchoolId,
                                                   sub.CampusId,
                                                   sub.Classes.ClassName,
                                                   sub.SchoolSubjects.SubjectName,
                                                   sub.DateCreated
                                               };

                        respData.StatusCode = 200;
                        respData.StatusMessage = "Subject(s) Assigned Successfully";
                        respData.Data = teachersSubjects.ToList();
                    }
                    else
                    {
                        var teachersSubjects = from sub in _context.SubjectTeachers
                                               where sub.SchoolUserId == obj.TeacherId && sub.SchoolId == obj.SchoolId && sub.CampusId == obj.CampusId
                                               select new
                                               {
                                                   sub.SchoolUserId,
                                                   sub.SchoolId,
                                                   sub.CampusId,
                                                   sub.Classes.ClassName,
                                                   sub.SchoolSubjects.SubjectName,
                                                   sub.DateCreated
                                               };

                        respData.StatusCode = 200;
                        respData.StatusMessage = "One or more selected Subject(s) has been assigned!";
                        respData.Data = teachersSubjects.ToList();
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
                return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful" };
            }

        }

        //All Subjects Assigned to Teacher
        public async Task<GenericRespModel> getAllSubjectsAssignedToTeacherAsync(Guid teacherId, long schoolId, long campusId)
        {
            try
            {
                //check the teacherId
                var checkTeacher = _context.Teachers.Where(x => x.SchoolUserId == teacherId).FirstOrDefault();
                if (checkTeacher != null)
                {
                    var result = from sub in _context.SubjectTeachers
                                 where sub.SchoolUserId == teacherId && sub.SchoolId == schoolId && sub.CampusId == campusId
                                 select new
                                 {
                                     sub.Id,
                                     sub.SchoolUserId,
                                     sub.ClassId,
                                     sub.Classes.ClassName,
                                     sub.SchoolId,
                                     sub.CampusId,
                                     sub.SchoolSubjects.SubjectName,
                                     sub.DateCreated
                                 };
                    if (result.Count() > 0)
                    {
                        return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList(), };
                    }

                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful, No Record Available", };
                }

                return new GenericRespModel { StatusCode = 200, StatusMessage = "No Teacher With the Specified ID", };

            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful" };
            }
        }


        public async Task<GenericRespModel> createSubjectDepartmentAsync(SubjectDepartmentCreateReqModel obj)
        {
            try
            {
                //checks if a subject Department has been created for a Class
                var checkResult = _context.SubjectDepartment.Where(x => x.DepartmentName == obj.DepartmentName && x.ClassId == obj.ClassId && x.SchoolId == obj.SchoolId && x.CampusId == obj.CampusId).FirstOrDefault();
                if (checkResult != null)
                {
                    return new GenericRespModel { StatusCode = 409, StatusMessage = "This Subject Department Already exist!" };
                }
                var subj = new SubjectDepartment
                {
                    DepartmentName = obj.DepartmentName,
                    SchoolId = obj.SchoolId,
                    CampusId = obj.CampusId,
                    ClassId = obj.ClassId,
                    DateCreated = DateTime.Now
                };

                await _context.SubjectDepartment.AddAsync(subj);
                await _context.SaveChangesAsync();

                return new GenericRespModel { StatusCode = 200, StatusMessage = "Subject Department Created Successful!" };

            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful" };
            }
        }

        public async Task<GenericRespModel> getAllSubjectDepartmentAsync(long schoolId, long campusId)
        {
            try
            {
                //returns all the subject Departments
                var result = from sub in _context.SubjectDepartment
                             where sub.SchoolId == schoolId && sub.CampusId == campusId
                             select new
                             {
                                 sub.Id,
                                 sub.CampusId,
                                 sub.SchoolId,
                                 sub.ClassId,
                                 sub.Classes.ClassName,
                                 sub.DepartmentName,
                                 sub.DateCreated
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
                return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful" };
            }
        }

        public async Task<GenericRespModel> getSubjectDepartmentByIdAsync(long subjectDepartmentId)
        {
            try
            {
                //returns all the subject Departments
                var result = from sub in _context.SubjectDepartment
                             where sub.Id == subjectDepartmentId
                             select new
                             {
                                 sub.Id,
                                 sub.CampusId,
                                 sub.SchoolId,
                                 sub.ClassId,
                                 sub.Classes.ClassName,
                                 sub.DepartmentName,
                                 sub.DateCreated
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
                return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful" };
            }
        }


        //assigning subjects to department for report card config
        public async Task<GenericRespModel> assignSubjectToDepartmentAsync(AssignSubjectToDepartmentReqModel obj)
        {
            try
            {
                foreach (var subject in obj.SubjectId)
                {
                    var subj = _context.SchoolSubjects.Where(x => x.Id == subject.Id).FirstOrDefault();
                    //updates the subject departmentId
                    subj.DepartmentId = obj.DepartmentId;
                    await _context.SaveChangesAsync();
                }

                return new GenericRespModel { StatusCode = 200, StatusMessage = "Subject(s) Assigned to Department Successfully" };
            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful" };
            }
        }

        public async Task<GenericRespModel> getAllSubjectsAssignedToDepartmentAsync(long subjectDepartmentId)
        {
            try
            {
                var result = from sub in _context.SchoolSubjects
                             where sub.DepartmentId == subjectDepartmentId
                             select new
                             {
                                 sub.Id,
                                 sub.ClassId,
                                 sub.Classes.ClassName,
                                 sub.SchoolId,
                                 sub.CampusId,
                                 sub.SubjectName,
                                 sub.SubjectCode,
                                 sub.MaximumScore,
                                 sub.SubjectDepartment.DepartmentName,
                                 sub.IsAssignedToTeacher,
                                 sub.IsActive,
                                 sub.DateCreated
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
                return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful" };
            }
        }

        public async Task<GenericRespModel> orderOfSubjectsAsync(long classId, IEnumerable<OrderOfSubjectsReqModel> obj)
        {
            try
            {
                GenericRespModel resp = new GenericRespModel();
                foreach (var sub in obj)
                {
                    var subj = _context.SchoolSubjects.Where(x => x.Id == sub.SubjectId).FirstOrDefault();
                    if (subj != null)
                    {
                        //check if the order exists for the class
                        var chkOrder = _context.SchoolSubjects.Where(x => x.ClassId == classId && x.ReportCardOrder == sub.OrderNumber).FirstOrDefault();

                        if (chkOrder != null)
                        {
                            resp.StatusCode = 409;
                            resp.StatusMessage = "One or more Subject has been assigned a selected Order";
                        }
                        else
                        {
                            //updates the subject Reportcard Order
                            subj.ReportCardOrder = sub.OrderNumber;
                            await _context.SaveChangesAsync();

                            resp.StatusCode = 200;
                            resp.StatusMessage = "Subjects Order assigned Successfully";
                        }
                    }
                }

                return resp;
            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful" };
            }
        }

        public async Task<GenericRespModel> deleteSubjectAsync(long subjectId)
        {
            try
            {
                var subj = _context.SchoolSubjects.Where(x => x.Id == subjectId).FirstOrDefault();
                if (subj != null)
                {
                    _context.SchoolSubjects.Remove(subj);
                    await _context.SaveChangesAsync();

                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Subject(s) Deleted Successfully" };
                }

                return new GenericRespModel { StatusCode = 200, StatusMessage = "No Subject with the specified Id" };
            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful" };
            }
        }

        public async Task<GenericRespModel> updateSubjectAsync(long subjectId, SubjectCreationReqModel obj)
        {
            try
            {

                var subj = _context.SchoolSubjects.Where(x => x.Id == subjectId).FirstOrDefault();
                if (subj != null)
                {
                    var checkResult = _context.SchoolSubjects.Where(x => x.SubjectName == obj.SubjectName && x.ClassId == obj.ClassId && x.SchoolId == obj.SchoolId && x.CampusId == obj.CampusId).FirstOrDefault();

                    if (checkResult != null)
                    {
                        return new GenericRespModel { StatusCode = 409, StatusMessage = "This Subject Already exist for this Class!" };
                    }
                    else
                    {
                        subj.ClassId = obj.ClassId;
                        subj.SchoolId = obj.SchoolId;
                        subj.CampusId = obj.CampusId;
                        subj.SubjectName = obj.SubjectName;
                        subj.SubjectCode = obj.SubjectCode;
                        subj.MaximumScore = obj.MaximumScore;

                        await _context.SaveChangesAsync();
                        return new GenericRespModel { StatusCode = 200, StatusMessage = "Subject Updated Successfully" };

                    }
                }

                return new GenericRespModel { StatusCode = 200, StatusMessage = "No Subject with the specified Id" };

            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful" };
            }
        }

        public async Task<GenericRespModel> deleteAssignedSubjectsAsync(long subjectAssignedId)
        {
            try
            {
                //check if subject Assigned exists
                var subj = _context.SubjectTeachers.Where(x => x.Id == subjectAssignedId).FirstOrDefault();
                if (subj != null)
                {
                    //check if subject exists
                    var schSubj = _context.SchoolSubjects.Where(x => x.Id == subj.SubjectId).FirstOrDefault();
                    if (schSubj != null)
                    {
                        //updates the isAssignedToTeachers column for the subject to false
                        schSubj.IsAssignedToTeacher = false;

                        //delete the subject assigned from the subject teachers table
                        _context.SubjectTeachers.Remove(subj);
                        await _context.SaveChangesAsync();

                        return new GenericRespModel { StatusCode = 200, StatusMessage = "Subject(s) Assigned Deleted Successfully" };
                    }
                }

                return new GenericRespModel { StatusCode = 200, StatusMessage = "No Subject Assigned with the specified Id" };

            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful" };
            }
        }

        public async Task<GenericRespModel> deleteSubjectDepartmentAsync(long subjectDepartmentId)
        {
            try
            {
                //check if subject Department exists
                var subj = _context.SubjectDepartment.Where(x => x.Id == subjectDepartmentId).FirstOrDefault();
                if (subj != null)
                {
                    //check if subject exists
                    var schSubj = _context.SchoolSubjects.Where(x => x.DepartmentId == subjectDepartmentId).FirstOrDefault();
                    if (schSubj != null)
                    {
                        //updates the departmentId column to null
                        schSubj.DepartmentId = null;

                        //delete the subject department
                        _context.SubjectDepartment.Remove(subj);
                        await _context.SaveChangesAsync();

                        return new GenericRespModel { StatusCode = 200, StatusMessage = "Subject Department Deleted Successfully" };
                    }
                }

                return new GenericRespModel { StatusCode = 200, StatusMessage = "No Subject Department with the specified Id" };

            }
            catch (Exception exMessage)
            {
                ErrorLogger err = new ErrorLogger();
                var logError = err.logError(exMessage);
                await _context.ErrorLog.AddAsync(logError);
                await _context.SaveChangesAsync();
                return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful" };
            }
        }

        //Bulk Creation of Subjects
        public async Task<GenericRespModel> createBulkSubjectAsync(BulkSubjectReqModel obj)
        {
            IList<object> data = new List<object>();
            try
            {
                GenericRespModel response = new GenericRespModel();
                List<GenericRespModel> responseList = new List<GenericRespModel>();

                //Validations
                CheckerValidation check = new CheckerValidation(_context);
                var checkSchool = check.checkSchoolById(obj.SchoolId);
                var checkCampus = check.checkSchoolCampusById(obj.CampusId);
                var checkClass = check.checkClassById(obj.ClassId);

                //check if the School and CampusId is Valid
                if (checkSchool == false && checkCampus == false && checkClass == false)
                {
                    return new GenericRespModel { StatusCode = 400, StatusMessage = "No School, Campus or Class With the specified ID" };
                }
                else if (obj.File == null || obj.File.Length <= 0)
                {
                    return new GenericRespModel { StatusCode = 400, StatusMessage = "No File Selected!, Please Select the Subject Bulk Upload Template" };
                }
                else if (!Path.GetExtension(obj.File.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
                {
                    return new GenericRespModel { StatusCode = 400, StatusMessage = "Not a Supported File Format!" };
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
                        
                        for (int row = 2; row <= rowCount; row++) // starts from the second row (Jumping the table headings)
                        {
                            //subjectname
                            string subjectName = worksheet.Cells[row, 1].Value.ToString();
                            string subjectCode = worksheet.Cells[row, 2].Value.ToString();
                            int subjectMaxScore = Convert.ToInt32(worksheet.Cells[row, 3].Value);

                            var checkResult = _context.SchoolSubjects.Where(x => x.SubjectName == subjectName && x.ClassId == obj.ClassId && x.SchoolId == obj.SchoolId && x.CampusId == obj.CampusId).FirstOrDefault();
                            if (checkResult == null)
                            {
                                var subject = new SchoolSubjects
                                {
                                    ClassId = obj.ClassId,
                                    SchoolId = obj.SchoolId,
                                    CampusId = obj.CampusId,
                                    SubjectName = subjectName,
                                    SubjectCode = subjectCode,
                                    MaximumScore = subjectMaxScore,
                                    IsAssignedToTeacher = false,
                                    IsActive = true,
                                    DateCreated = DateTime.Now,
                                };

                                await _context.SchoolSubjects.AddAsync(subject);
                                int result = await _context.SaveChangesAsync();

                                if (result > 0)
                                    responseList.Add(new GenericRespModel { StatusCode = 200, StatusMessage = $"Subject: {subjectName} Created Successfully!" });
                                else
                                    responseList.Add(new GenericRespModel { StatusCode = 500, StatusMessage = $"Internal Server/Database Error!"});
                            }
                            else
                            {
                                responseList.Add(new GenericRespModel { StatusCode = 409, StatusMessage = $"This Subject {subjectName} Already exist for this Class!" });
                            }

                            response.StatusCode = 200;
                            response.StatusMessage = "Successfully!";
                            response.Data = responseList;
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
    }
}
