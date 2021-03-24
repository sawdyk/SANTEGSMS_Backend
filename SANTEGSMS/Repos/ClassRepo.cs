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
    public class ClassRepo : IClassRepo
    {
        private readonly AppDbContext _context;

        public ClassRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<GenericRespModel> createClassAsync(ClassReqModel obj)
        {
            try
            {
                //check if the class already exists
                var checkClassExist = _context.Classes.Where(x => x.ClassName == obj.ClassName && x.SchoolId == obj.SchoolId && x.CampusId == obj.CampusId).FirstOrDefault();

                if (checkClassExist == null)
                {
                    //Save the Class
                    var newClass = new Classes
                    {
                        ClassName = obj.ClassName,
                        SchoolId = obj.SchoolId,
                        CampusId = obj.CampusId,
                        DateCreated = DateTime.Now
                    };
                    await _context.Classes.AddAsync(newClass);
                    await _context.SaveChangesAsync();

                    //return the class Created
                    var result = from cr in _context.Classes.Where(c => c.Id == newClass.Id)
                                 select new
                                 {
                                     cr.Id,
                                     cr.SchoolId,
                                     cr.CampusId,
                                     cr.ClassName,
                                     cr.DateCreated
                                 };

                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Class Created Successfully", Data = result.FirstOrDefault() };

                }

                return new GenericRespModel { StatusCode = 409, StatusMessage = "This Class Already Exists!" };

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

        public async Task<GenericRespModel> createClassGradesAsync(ClassGradeReqModel obj)
        {
            try
            {
                var checkClassGradeExist = _context.ClassGrades.Where(x => x.ClassId == obj.ClassId && x.SchoolId == obj.SchoolId && x.CampusId == obj.CampusId && x.GradeName == obj.GradeName).FirstOrDefault();
                if (checkClassGradeExist == null)
                {
                    //Save the ClassGrade
                    var newClassGrade = new ClassGrades
                    {
                        ClassId = obj.ClassId,
                        SchoolId = obj.SchoolId,
                        CampusId = obj.CampusId,
                        GradeName = obj.GradeName,
                        isAssignedToTeacher = false,
                        DateCreated = DateTime.Now
                    };
                    await _context.ClassGrades.AddAsync(newClassGrade);
                    await _context.SaveChangesAsync();


                    //return the class Created
                    var result = from cr in _context.ClassGrades.Where(c => c.Id == newClassGrade.Id)
                                 select new
                                 {
                                     cr.Id,
                                     cr.SchoolId,
                                     cr.CampusId,
                                     cr.Classes.ClassName,
                                     cr.GradeName,
                                     cr.isAssignedToTeacher,
                                     cr.DateCreated
                                 };

                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Class Grade Created Successfully", Data = result.FirstOrDefault() };

                }

                return new GenericRespModel { StatusCode = 409, StatusMessage = "Class Grade Already Exists" };
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

        public async Task<GenericRespModel> getAllClassesAsync(long schoolId, long campusId)
        {
            try
            {
                var result = from cl in _context.Classes where cl.SchoolId == schoolId && cl.CampusId == campusId select cl;

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

        public async Task<GenericRespModel> getAllClassGradesAsync(long schoolId, long campusId)
        {
            try
            {
                var result = from cl in _context.ClassGrades
                             where cl.SchoolId == schoolId && cl.CampusId == campusId
                             select new
                             {
                                 cl.Id,
                                 cl.SchoolId,
                                 cl.CampusId,
                                 cl.SchoolCampus.CampusName,
                                 cl.Schools.SchoolName,
                                 cl.Classes.ClassName,
                                 cl.GradeName,
                                 cl.DateCreated
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

        public async Task<GenericRespModel> getClassByClassIdAsync(long classId, long schoolId, long campusId)
        {
            try
            {
                var result = from cl in _context.Classes
                             where cl.Id == classId && cl.SchoolId == schoolId && cl.CampusId == campusId
                             select cl;

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

        public async Task<GenericRespModel> getClassGradesByClassGradeIdAsync(long classGradeId, long schoolId, long campusId)
        {
            try
            {
                var result = from cl in _context.ClassGrades
                             where cl.Id == classGradeId && cl.SchoolId == schoolId && cl.CampusId == campusId
                             select new
                             {
                                 cl.Id,
                                 cl.SchoolId,
                                 cl.CampusId,
                                 cl.SchoolCampus.CampusName,
                                 cl.Schools.SchoolName,
                                 cl.Classes.ClassName,
                                 cl.GradeName,
                                 cl.DateCreated
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

        public async Task<GenericRespModel> getClassGradesByClassIdAsync(long classId, long schoolId, long campusId)
        {
            try
            {
                var result = from cl in _context.ClassGrades
                             where cl.ClassId == classId && cl.SchoolId == schoolId && cl.CampusId == campusId
                             select new
                             {
                                 cl.Id,
                                 cl.SchoolId,
                                 cl.CampusId,
                                 cl.SchoolCampus.CampusName,
                                 cl.Schools.SchoolName,
                                 cl.Classes.ClassName,
                                 cl.GradeName,
                                 cl.DateCreated
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

        public async Task<GenericRespModel> updateClassAsync(long classId, ClassReqModel obj)
        {
            try
            {
                var clsRst = _context.Classes.Where(x => x.Id == classId).FirstOrDefault();
                if (clsRst != null)
                {
                    var checkResult = _context.Classes.Where(x => x.ClassName == obj.ClassName && x.SchoolId == obj.SchoolId && x.CampusId == obj.CampusId).FirstOrDefault();

                    if (checkResult != null)
                    {
                        return new GenericRespModel { StatusCode = 409, StatusMessage = "This Class Already Exist, Class Updated Successfully!" };
                    }
                    else
                    {
                        clsRst.ClassName = obj.ClassName;
                        clsRst.SchoolId = obj.SchoolId;
                        clsRst.CampusId = obj.CampusId;

                        await _context.SaveChangesAsync();
                        return new GenericRespModel { StatusCode = 200, StatusMessage = "Class Updated Successfully" };

                    }
                }

                return new GenericRespModel { StatusCode = 200, StatusMessage = "No Class with the specified Id" };

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

        public async Task<GenericRespModel> updateClassGradeAsync(long classGradeId, ClassGradeReqModel obj)
        {
            try
            {
                var clsRst = _context.ClassGrades.Where(x => x.Id == classGradeId).FirstOrDefault();
                if (clsRst != null)
                {
                    var checkResult = _context.ClassGrades.Where(x => x.GradeName == obj.GradeName && x.ClassId == obj.ClassId && x.SchoolId == obj.SchoolId && x.CampusId == obj.CampusId).FirstOrDefault();

                    if (checkResult != null)
                    {
                        return new GenericRespModel { StatusCode = 409, StatusMessage = "This ClassGrade Already Exist, ClassGrade Updated Successfully!" };
                    }
                    else
                    {
                        clsRst.ClassId = obj.ClassId;
                        clsRst.SchoolId = obj.SchoolId;
                        clsRst.CampusId = obj.CampusId;
                        clsRst.GradeName = obj.GradeName;

                        await _context.SaveChangesAsync();
                        return new GenericRespModel { StatusCode = 200, StatusMessage = "ClassGrade Updated Successfully" };

                    }
                }

                return new GenericRespModel { StatusCode = 200, StatusMessage = "No Class Grade with the specified Id" };

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

        public async Task<GenericRespModel> getAllStudentInClassAsync(long classId, long schoolId, long campusId, long sessionId)
        {
            try
            {
                var result = from cl in _context.GradeStudents
                             where cl.SchoolId == schoolId && cl.ClassId == classId
                             && cl.CampusId == campusId && cl.SessionId == sessionId
                             select new
                             {
                                 cl.Id,
                                 cl.SchoolId,
                                 cl.CampusId,
                                 cl.SchoolCampus.CampusName,
                                 StudentId = cl.Students.Id,
                                 cl.Students.FirstName,
                                 cl.Students.LastName,
                                 cl.Students.UserName,
                                 cl.Students.AdmissionNumber,
                                 cl.Students.IsAssignedToClass,
                                 cl.ClassGradeId,
                                 cl.ClassGrades.GradeName,
                                 cl.ClassId,
                                 cl.Classes.ClassName,
                                 cl.DateCreated
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

        public async Task<GenericRespModel> getAllStudentInClassGradeAsync(long classId, long classGradeId, long schoolId, long campusId, long sessionId)
        {
            try
            {
                var result = from cl in _context.GradeStudents
                             where cl.SchoolId == schoolId && cl.CampusId == campusId && cl.ClassId == classId
                             && cl.ClassGradeId == classGradeId && cl.SessionId == sessionId
                             select new
                             {
                                 cl.Id,
                                 cl.SchoolId,
                                 cl.CampusId,
                                 cl.SchoolCampus.CampusName,
                                 StudentId = cl.Students.Id,
                                 cl.Students.FirstName,
                                 cl.Students.LastName,
                                 cl.Students.UserName,
                                 cl.Students.AdmissionNumber,
                                 cl.Students.IsAssignedToClass,
                                 cl.ClassGradeId,
                                 cl.ClassGrades.GradeName,
                                 cl.ClassId,
                                 cl.Classes.ClassName,
                                 cl.DateCreated
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

        public async Task<GenericRespModel> getAllStudentInClassForCurrentSessionAsync(long classId, long schoolId, long campusId)
        {
            try
            {
                //get the current sessionId
                var currentSessionId = new SessionAndTerm(_context).getCurrentSessionId(schoolId);

                if (currentSessionId == 0) //if the current session has not been set
                {
                    return new GenericRespModel { StatusCode = 409, StatusMessage = "Current Academic Session has not been set!" };
                }
                else
                {
                    var result = from cl in _context.GradeStudents
                                 where cl.SchoolId == schoolId && cl.ClassId == classId
                                 && cl.CampusId == campusId && cl.SessionId == currentSessionId
                                 select new
                                 {
                                     cl.Id,
                                     cl.SchoolId,
                                     cl.CampusId,
                                     cl.SchoolCampus.CampusName,
                                     StudentId = cl.Students.Id,
                                     cl.Students.FirstName,
                                     cl.Students.LastName,
                                     cl.Students.UserName,
                                     cl.Students.AdmissionNumber,
                                     cl.Students.IsAssignedToClass,
                                     cl.ClassGradeId,
                                     cl.ClassGrades.GradeName,
                                     cl.ClassId,
                                     cl.Classes.ClassName,
                                     cl.DateCreated
                                 };

                    if (result.Count() > 0)
                    {
                        return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList() };
                    }

                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful, No Record Available" };
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

        public async Task<GenericRespModel> getAllStudentInClassGradeForCurrentSessionAsync(long classId, long classGradeId, long schoolId, long campusId)
        {
            try
            {
                //get the current sessionId
                var currentSessionId = new SessionAndTerm(_context).getCurrentSessionId(schoolId);

                if (currentSessionId == 0) //if the current session has not been set
                {
                    return new GenericRespModel { StatusCode = 409, StatusMessage = "Current Academic Session has not been set!" };
                }
                else
                {
                    var result = from cl in _context.GradeStudents
                                 where cl.SchoolId == schoolId && cl.CampusId == campusId && cl.ClassId == classId
                                 && cl.ClassGradeId == classGradeId && cl.SessionId == currentSessionId
                                 select new
                                 {
                                     cl.Id,
                                     cl.SchoolId,
                                     cl.CampusId,
                                     cl.SchoolCampus.CampusName,
                                     StudentId = cl.Students.Id,
                                     cl.Students.FirstName,
                                     cl.Students.LastName,
                                     cl.Students.UserName,
                                     cl.Students.AdmissionNumber,
                                     cl.Students.IsAssignedToClass,
                                     cl.ClassGradeId,
                                     cl.ClassGrades.GradeName,
                                     cl.ClassId,
                                     cl.Classes.ClassName,
                                     cl.DateCreated
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

        public async Task<GenericRespModel> deleteClassAsync(long classId, long schoolId, long campusId)
        {
            try
            {
                var clsRst = _context.Classes.Where(x => x.Id == classId && x.SchoolId == schoolId && x.CampusId == campusId).FirstOrDefault();
                if (clsRst != null)
                {
                    IList<ClassGrades> clsGrd = (_context.ClassGrades.Where(x => x.ClassId == classId && x.SchoolId == schoolId && x.CampusId == campusId).ToList());

                    if (clsGrd.Count() > 0)
                    {
                        _context.ClassGrades.RemoveRange(clsGrd);
                        await _context.SaveChangesAsync();
                    }

                    _context.Classes.Remove(clsRst);
                    await _context.SaveChangesAsync();

                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Class Deleted Successfully", };
                }

                return new GenericRespModel { StatusCode = 200, StatusMessage = "No Class with the specified Id" };

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

        public async Task<GenericRespModel> deleteClassGradeAsync(long classGradeId, long schoolId, long campusId)
        {
            try
            {
                var clsGrd = _context.ClassGrades.Where(x => x.Id == classGradeId && x.SchoolId == schoolId && x.CampusId == campusId).FirstOrDefault();
                if (clsGrd != null)
                {
                    _context.ClassGrades.Remove(clsGrd);
                    await _context.SaveChangesAsync();

                    return new GenericRespModel { StatusCode = 200, StatusMessage = "ClassGrade Deleted Successfully", };
                }

                return new GenericRespModel { StatusCode = 200, StatusMessage = "No ClassGrade with the specified Id" };

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
