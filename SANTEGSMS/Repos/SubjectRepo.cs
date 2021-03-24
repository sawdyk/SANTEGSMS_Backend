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
    }
}
