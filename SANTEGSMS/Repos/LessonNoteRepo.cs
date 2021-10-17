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
    public class LessonNoteRepo : ILessonNoteRepo
    {
        private readonly AppDbContext _context;

        public LessonNoteRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<GenericRespModel> createLessonNotesAsync(LessonNoteCreateReqModel obj)
        {
            try
            {
                //checks if a LessonNote exists
                var checkLessonNote =  _context.LessonNotes.Where(x => x.Description == obj.Description && x.TeacherId == obj.TeacherId && x.ClassId == obj.ClassId && x.SubjectId == obj.SubjectId
                && x.ClassGradeId == obj.ClassGradeId && x.TermId == obj.TermId && x.SessionId == obj.SessionId && x.SchoolId == obj.SchoolId && x.CampusId == obj.CampusId).FirstOrDefault();

                if (checkLessonNote == null)
                {
                    var les = new LessonNotes
                    {
                        Description = obj.Description,
                        FileUrl = obj.FileUrl,
                        SubjectId = obj.SubjectId,
                        TeacherId = obj.TeacherId,
                        ClassId = obj.ClassId,
                        ClassGradeId = obj.ClassGradeId,
                        SchoolId = obj.SchoolId,
                        CampusId = obj.CampusId,
                        TermId = obj.TermId,
                        SessionId = obj.SessionId,
                        StatusId = (long)EnumUtility.Status.Pending,
                        DateUploaded = DateTime.Now,
                        LastDateUpdated = DateTime.Now,
                    };

                    await _context.LessonNotes.AddAsync(les);
                    await _context.SaveChangesAsync();

                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Lesson Note Created Successfully" };

                }

                return new GenericRespModel { StatusCode = 200, StatusMessage = "A Lesson Note with this description Already Exist" };

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

        public async Task<GenericRespModel> getLessonNotesByIdAsync(long lessonNoteId, long schoolId, long campusId)
        {
            try
            {
                var result = from ass in _context.LessonNotes
                             where ass.Id == lessonNoteId && ass.SchoolId == schoolId && ass.CampusId == campusId
                             select new
                             {
                                 ass.Id,
                                 ass.Description,
                                 ass.FileUrl,
                                 ass.SchoolSubjects.SubjectName,
                                 ass.TeacherId,
                                 ass.Classes.ClassName,
                                 ass.ClassGrades.GradeName,
                                 ass.SchoolId,
                                 ass.CampusId,
                                 ass.Sessions.SessionName,
                                 ass.Terms.TermName,
                                 ass.Status.StatusName,
                                 ass.DateUploaded,
                                 ass.LastDateUpdated,
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
                return new GenericRespModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }


        public async Task<GenericRespModel> getLessonNotesByClassGradeIdAsync(long classId, long classGradeId, long schoolId, long campusId, long termId, long sessionId)
        {
            try
            {
                var result = from ass in _context.LessonNotes
                             where ass.ClassId == classId && ass.ClassGradeId == classGradeId && ass.TermId == termId && ass.SessionId == sessionId
                             && ass.SchoolId == schoolId && ass.CampusId == campusId
                             select new
                             {
                                 ass.Id,
                                 ass.Description,
                                 ass.FileUrl,
                                 ass.SchoolSubjects.SubjectName,
                                 ass.TeacherId,
                                 ass.Classes.ClassName,
                                 ass.ClassGrades.GradeName,
                                 ass.SchoolId,
                                 ass.CampusId,
                                 ass.Sessions.SessionName,
                                 ass.Terms.TermName,
                                 ass.Status.StatusName,
                                 ass.DateUploaded,
                                 ass.LastDateUpdated,
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

        public async Task<GenericRespModel> getLessonNotesBySubjectIdAsync(long subjectId, long schoolId, long campusId, long termId, long sessionId)
        {
            try
            {
                var result = from ass in _context.LessonNotes
                             where ass.SubjectId == subjectId && ass.TermId == termId && ass.SessionId == sessionId
                             && ass.SchoolId == schoolId && ass.CampusId == campusId
                             select new
                             {
                                 ass.Id,
                                 ass.Description,
                                 ass.FileUrl,
                                 ass.SchoolSubjects.SubjectName,
                                 ass.TeacherId,
                                 ass.Classes.ClassName,
                                 ass.ClassGrades.GradeName,
                                 ass.SchoolId,
                                 ass.CampusId,
                                 ass.Sessions.SessionName,
                                 ass.Terms.TermName,
                                 ass.Status.StatusName,
                                 ass.DateUploaded,
                                 ass.LastDateUpdated,
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

        public async Task<GenericRespModel> getLessonNotesByTeacherIdAsync(Guid teacherId, long schoolId, long campusId, long termId, long sessionId)
        {
            try
            {
                var result = from ass in _context.LessonNotes
                             where ass.TeacherId == teacherId && ass.TermId == termId && ass.SessionId == sessionId
                             && ass.SchoolId == schoolId && ass.CampusId == campusId
                             select new
                             {
                                 ass.Id,
                                 ass.Description,
                                 ass.FileUrl,
                                 ass.SchoolSubjects.SubjectName,
                                 ass.TeacherId,
                                 ass.Classes.ClassName,
                                 ass.ClassGrades.GradeName,
                                 ass.SchoolId,
                                 ass.CampusId,
                                 ass.Sessions.SessionName,
                                 ass.Terms.TermName,
                                 ass.Status.StatusName,
                                 ass.DateUploaded,
                                 ass.LastDateUpdated,
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

        public async Task<GenericRespModel> updateLessonNotesAsync(long lessonNoteId, LessonNoteCreateReqModel obj)
        {
            try
            {
                //checks if a lessonNote Exists
                var lessonNote =  _context.LessonNotes.Where(x => x.Id == lessonNoteId && x.SchoolId == obj.SchoolId && x.CampusId == obj.CampusId).FirstOrDefault();

                if (lessonNote != null)
                {
                    //checks if a lessonNote with description exists
                    var less =  _context.LessonNotes.Where(x => x.Description == obj.Description && x.TeacherId == obj.TeacherId && x.ClassId == obj.ClassId && x.SubjectId == obj.SubjectId
                    && x.ClassGradeId == obj.ClassGradeId && x.TermId == obj.TermId && x.SessionId == obj.SessionId && x.SchoolId == obj.SchoolId && x.CampusId == obj.CampusId).FirstOrDefault();

                    if (less == null)
                    {
                        less.Description = obj.Description;
                        less.FileUrl = obj.FileUrl;
                        less.SubjectId = obj.SubjectId;
                        less.TeacherId = obj.TeacherId;
                        less.ClassId = obj.ClassId;
                        less.ClassGradeId = obj.ClassGradeId;
                        less.SchoolId = obj.SchoolId;
                        less.CampusId = obj.CampusId;
                        less.TermId = obj.TermId;
                        less.SessionId = obj.SessionId;
                        less.LastDateUpdated = DateTime.Now;
                        await _context.SaveChangesAsync();

                        return new GenericRespModel { StatusCode = 200, StatusMessage = "Lesson Note Updated Successfully!" };
                    }

                    return new GenericRespModel { StatusCode = 200, StatusMessage = "A Lesson Note With this Description Already Exists!" };
                }

                return new GenericRespModel { StatusCode = 200, StatusMessage = "No Lesson Note With the Specified ID!" };

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

        public async Task<GenericRespModel> approveLessonNotesAsync(long lessonNoteId, long statusId, long schoolId, long campusId)
        {
            try
            {
                //checks if a lessonNote Exists
                var less =  _context.LessonNotes.Where(x => x.Id == lessonNoteId && x.SchoolId == schoolId && x.CampusId == campusId).FirstOrDefault();

                if (less != null)
                {
                    if (statusId == (long)EnumUtility.Status.Approved)
                    {
                        less.StatusId = (long)EnumUtility.Status.Approved;

                        return new GenericRespModel { StatusCode = 200, StatusMessage = "Lesson Note Approved Successfully!" };
                    }
                    if (statusId == (long)EnumUtility.Status.Declined)
                    {
                        less.StatusId = (long)EnumUtility.Status.Declined;

                        return new GenericRespModel { StatusCode = 200, StatusMessage = "Lesson Note Declined Successfully!" };

                    }

                    await _context.SaveChangesAsync();
                }

                return new GenericRespModel { StatusCode = 200, StatusMessage = "No Lesson Note With the Specified ID!" };

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

        public async Task<GenericRespModel> deleteLessonNotesAsync(long lessonNoteId, long schoolId, long campusId)
        {
            try
            {
                //checks if a lessonNote Exists
                var lessonNote =  _context.LessonNotes.Where(x => x.Id == lessonNoteId && x.SchoolId == schoolId && x.CampusId == campusId).FirstOrDefault();

                if (lessonNote != null)
                {
                    _context.LessonNotes.Remove(lessonNote);
                    await _context.SaveChangesAsync();

                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Lesson Note Deleted Successfully!" };

                }

                return new GenericRespModel { StatusCode = 200, StatusMessage = "No Lesson Note With the Specified ID!" };

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



        //----------------------------------------------------SUBJECTNOTES-------------------------------------------------------------------------------------------------------

        public async Task<GenericRespModel> createSubjectNotesAsync(SubjectNoteCreateReqModel obj)
        {
            try
            {
                //checks if a SubjectNote exists
                var checkSubjectNote =  _context.SubjectNotes.Where(x => x.Description == obj.Description && x.TeacherId == obj.TeacherId && x.ClassId == obj.ClassId && x.SubjectId == obj.SubjectId
                && x.ClassGradeId == obj.ClassGradeId && x.TermId == obj.TermId && x.SessionId == obj.SessionId && x.SchoolId == obj.SchoolId && x.CampusId == obj.CampusId).FirstOrDefault();

                if (checkSubjectNote == null)
                {
                    var subjNote = new SubjectNotes
                    {
                        Description = obj.Description,
                        FileUrl = obj.FileUrl,
                        SubjectId = obj.SubjectId,
                        TeacherId = obj.TeacherId,
                        ClassId = obj.ClassId,
                        ClassGradeId = obj.ClassGradeId,
                        SchoolId = obj.SchoolId,
                        CampusId = obj.CampusId,
                        TermId = obj.TermId,
                        SessionId = obj.SessionId,
                        DateUploaded = DateTime.Now,
                        LastDateUpdated = DateTime.Now,
                    };

                    await _context.SubjectNotes.AddAsync(subjNote);
                    await _context.SaveChangesAsync();

                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Subject Note Created Successfully" };

                }

                return new GenericRespModel { StatusCode = 200, StatusMessage = "A Subject Note with this description Already Exist" };

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

        public async Task<GenericRespModel> getSubjectNotesByIdAsync(long subjectNoteId, long schoolId, long campusId)
        {
            try
            {
                var result = from ass in _context.SubjectNotes
                             where ass.Id == subjectNoteId && ass.SchoolId == schoolId && ass.CampusId == campusId
                             select new
                             {
                                 ass.Id,
                                 ass.Description,
                                 ass.FileUrl,
                                 ass.SchoolSubjects.SubjectName,
                                 ass.TeacherId,
                                 ass.Classes.ClassName,
                                 ass.ClassGrades.GradeName,
                                 ass.SchoolId,
                                 ass.CampusId,
                                 ass.Sessions.SessionName,
                                 ass.Terms.TermName,
                                 ass.DateUploaded,
                                 ass.LastDateUpdated,
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
                return new GenericRespModel { StatusCode = 500, StatusMessage = "An Error Occured!" };
            }
        }

        public async Task<GenericRespModel> getSubjectNotesBySubjectIdAsync(long subjectId, long schoolId, long campusId, long termId, long sessionId)
        {
            try
            {
                var result = from ass in _context.SubjectNotes
                             where ass.SubjectId == subjectId && ass.TermId == termId && ass.SessionId == sessionId
                             && ass.SchoolId == schoolId && ass.CampusId == campusId
                             select new
                             {
                                 ass.Id,
                                 ass.Description,
                                 ass.FileUrl,
                                 ass.SchoolSubjects.SubjectName,
                                 ass.TeacherId,
                                 ass.Classes.ClassName,
                                 ass.ClassGrades.GradeName,
                                 ass.SchoolId,
                                 ass.CampusId,
                                 ass.Sessions.SessionName,
                                 ass.Terms.TermName,
                                 ass.DateUploaded,
                                 ass.LastDateUpdated,
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

        public async Task<GenericRespModel> getSubjectNotesByClassGradeIdAsync(long classId, long classGradeId, long schoolId, long campusId, long termId, long sessionId)
        {
            try
            {
                var result = from ass in _context.SubjectNotes
                             where ass.ClassId == classId && ass.ClassGradeId == classGradeId && ass.TermId == termId && ass.SessionId == sessionId
                             && ass.SchoolId == schoolId && ass.CampusId == campusId
                             select new
                             {
                                 ass.Id,
                                 ass.Description,
                                 ass.FileUrl,
                                 ass.SchoolSubjects.SubjectName,
                                 ass.TeacherId,
                                 ass.Classes.ClassName,
                                 ass.ClassGrades.GradeName,
                                 ass.SchoolId,
                                 ass.CampusId,
                                 ass.Sessions.SessionName,
                                 ass.Terms.TermName,
                                 ass.DateUploaded,
                                 ass.LastDateUpdated,
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

        public async Task<GenericRespModel> getSubjectNotesByTeacherIdAsync(Guid teacherId, long schoolId, long campusId, long termId, long sessionId)
        {
            try
            {
                var result = from ass in _context.SubjectNotes
                             where ass.TeacherId == teacherId && ass.TermId == termId && ass.SessionId == sessionId
                             && ass.SchoolId == schoolId && ass.CampusId == campusId
                             select new
                             {
                                 ass.Id,
                                 ass.Description,
                                 ass.FileUrl,
                                 ass.SchoolSubjects.SubjectName,
                                 ass.TeacherId,
                                 ass.Classes.ClassName,
                                 ass.ClassGrades.GradeName,
                                 ass.SchoolId,
                                 ass.CampusId,
                                 ass.Sessions.SessionName,
                                 ass.Terms.TermName,
                                 ass.DateUploaded,
                                 ass.LastDateUpdated,
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

        public async Task<GenericRespModel> updateSubjectNotesAsync(long subjectNoteId, SubjectNoteCreateReqModel obj)
        {
            try
            {
                //checks if a Subject Exists
                var subjectNote =  _context.SubjectNotes.Where(x => x.Id == subjectNoteId && x.SchoolId == obj.SchoolId && x.CampusId == obj.CampusId).FirstOrDefault();

                if (subjectNote != null)
                {
                    //checks if a SubjectNote with description exists
                    var subjNote =  _context.LessonNotes.Where(x => x.Description == obj.Description && x.TeacherId == obj.TeacherId && x.ClassId == obj.ClassId && x.SubjectId == obj.SubjectId
                    && x.ClassGradeId == obj.ClassGradeId && x.TermId == obj.TermId && x.SessionId == obj.SessionId && x.SchoolId == obj.SchoolId && x.CampusId == obj.CampusId).FirstOrDefault();

                    if (subjNote == null)
                    {
                        subjNote.Description = obj.Description;
                        subjNote.FileUrl = obj.FileUrl;
                        subjNote.SubjectId = obj.SubjectId;
                        subjNote.TeacherId = obj.TeacherId;
                        subjNote.ClassId = obj.ClassId;
                        subjNote.ClassGradeId = obj.ClassGradeId;
                        subjNote.SchoolId = obj.SchoolId;
                        subjNote.CampusId = obj.CampusId;
                        subjNote.TermId = obj.TermId;
                        subjNote.SessionId = obj.SessionId;
                        subjNote.LastDateUpdated = DateTime.Now;
                        await _context.SaveChangesAsync();

                        return new GenericRespModel { StatusCode = 200, StatusMessage = "Subject Note Updated Successfully!" };
                    }

                    return new GenericRespModel { StatusCode = 200, StatusMessage = "A Subject Note With this Description Already Exists!" };
                }

                return new GenericRespModel { StatusCode = 200, StatusMessage = "No Subject Note With the Specified ID!" };

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

        public async Task<GenericRespModel> deleteSubjectNotesAsync(long subjectNoteId, long schoolId, long campusId)
        {
            try
            {
                //checks if a Subject Exists
                var subjectNote =  _context.SubjectNotes.Where(x => x.Id == subjectNoteId && x.SchoolId == schoolId && x.CampusId == campusId).FirstOrDefault();

                if (subjectNote != null)
                {
                    _context.SubjectNotes.Remove(subjectNote);
                    await _context.SaveChangesAsync();

                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Subject Note Deleted Successfully!" };

                }

                return new GenericRespModel { StatusCode = 200, StatusMessage = "No Subject Note With the Specified ID!" };

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
