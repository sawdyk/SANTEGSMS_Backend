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
    public class SessionTermRepo : ISessionTermRepo
    {
        private readonly AppDbContext _context;

        public SessionTermRepo(AppDbContext context)
        {
            _context = context;
        }


        public async Task<GenericRespModel> createAcademicSessionAsync(AcademicSessionReqModel obj)
        {
            try
            {
                //checks if the school already has the Academic session created
                var academicSessionExists = _context.AcademicSessions.Where(s => s.SessionId == obj.SessionId && s.TermId == obj.TermId && s.SchoolId == obj.SchoolId).FirstOrDefault();
                if (academicSessionExists == null)
                {
                    //creates the new Academic session
                    var acdsession = new AcademicSessions
                    {
                        SessionId = obj.SessionId,
                        TermId = obj.TermId,
                        SchoolId = obj.SchoolId,
                        UserId = obj.UserId,
                        DateStart = obj.DateStart,
                        DateEnd = obj.DateEnd,
                        IsApproved = false,
                        IsCurrent = false,
                        IsClosed = true,
                        IsOpened = false,
                        DateCreated = DateTime.Now,
                    };

                    await _context.AcademicSessions.AddAsync(acdsession);
                    await _context.SaveChangesAsync();

                    //returns all the academic session created by the school
                    var getAllSchAcademicSession = from s in _context.AcademicSessions
                                                   where s.SchoolId == obj.SchoolId orderby s.Id descending
                                                   select new
                                                   {
                                                       s.Id,
                                                       s.SchoolId,
                                                       s.Terms.TermName,
                                                       s.Sessions.SessionName,
                                                       s.UserId,
                                                       s.DateStart,
                                                       s.DateEnd,
                                                       s.IsApproved,
                                                       s.IsCurrent,
                                                       s.IsClosed,
                                                       s.IsOpened,
                                                       s.DateCreated
                                                   };

                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Academic Session Created Successfully", Data = getAllSchAcademicSession.ToList()};
                }

                return new GenericRespModel { StatusCode = 200, StatusMessage = "Academic Session Already Exist" };

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

        public async Task<GenericRespModel> createSessionAsync(SessionReqModel obj)
        {
            try
            {
                //checks if the school already has the session created
                var sessionExist = _context.Sessions.Where(s => s.SessionName == obj.SessionName && s.SchoolId == obj.SchoolId).FirstOrDefault();
                if (sessionExist == null)
                {
                    //creates the new session
                    var session = new Sessions
                    {
                        SessionName = obj.SessionName,
                        SchoolId = obj.SchoolId,
                        DateCreated = DateTime.Now,
                    };

                    await _context.Sessions.AddAsync(session);
                    await _context.SaveChangesAsync();

                    //returns all the session created by the school
                    var getAllSchSession = from ses in _context.Sessions where ses.SchoolId == obj.SchoolId orderby ses.Id descending select ses;

                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Session Created Successfully", Data = getAllSchSession.ToList()};
                }

                return new GenericRespModel { StatusCode = 200, StatusMessage = "Session Already Exist" };

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

        public async Task<GenericRespModel> getAllAcademicSessionsAsync(long schoolId)
        {
            try
            {
                CheckerValidation check = new CheckerValidation(_context);
                var checkResult = check.checkSchoolById(schoolId);

                if (checkResult == true)
                {
                    //returns all the academic session created by the school
                    var result = from s in _context.AcademicSessions
                                 where s.SchoolId == schoolId
                                 select new
                                 {
                                     s.Id,
                                     s.SchoolId,
                                     s.Terms.TermName,
                                     s.Sessions.SessionName,
                                     s.UserId,
                                     s.DateStart,
                                     s.DateEnd,
                                     s.IsApproved,
                                     s.IsCurrent,
                                     s.IsClosed,
                                     s.IsOpened,
                                     s.DateCreated
                                 };
                    if (result.Count() > 0)
                    {
                        return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList(), };
                    }

                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful, No Record Available", };
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

        public async Task<GenericRespModel> getAllSessionsAsync(long schoolId)
        {
            try
            {
                CheckerValidation check = new CheckerValidation(_context);
                var checkResult = check.checkSchoolById(schoolId);

                if (checkResult == true)
                {
                    //returns all the session created by the school
                    var result = from ses in _context.Sessions
                                 where ses.SchoolId == schoolId
                                 select new
                                 {
                                     ses.Id,
                                     ses.SchoolId,
                                     ses.SessionName,
                                     ses.DateCreated
                                 };

                    if (result.Count() > 0)
                    {
                        return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList(), };
                    }

                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful, No Record Available", };
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

        public async Task<GenericRespModel> getAllTermsAsync()
        {
            try
            {
                //returns all the Terms
                var result = from trm in _context.Terms select trm;

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


        public async Task<GenericRespModel> getSessionByIdAsync(long schoolId, long sessionId)
        {
            try
            {
                CheckerValidation check = new CheckerValidation(_context);
                var checkResult = check.checkSchoolById(schoolId);

                if (checkResult == true)
                {
                    //returns all the session created by the school
                    var result = from ses in _context.Sessions
                                 where ses.Id == sessionId && ses.SchoolId == schoolId
                                 select new
                                 {
                                     ses.Id,
                                     ses.SchoolId,
                                     ses.SessionName,
                                     ses.DateCreated
                                 };

                    if (result.Count() > 0)
                    {
                        return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = result.FirstOrDefault(), };
                    }

                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful, No Record Available", };
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

        public async Task<GenericRespModel> getTermByIdAsync(long termId)
        {
            try
            {
                //returns all the Terms
                var result = from trm in _context.Terms where trm.Id == termId select trm;

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

        public async Task<GenericRespModel> setAcademicSessionAsCurrentAsync(long schoolId, long academicSessionId)
        {
            try
            {
                //get the current session that is set to true
                var getCurrentSession = _context.AcademicSessions.Where(s => s.SchoolId == schoolId && s.IsCurrent == true).FirstOrDefault();

                if (getCurrentSession != null)
                {
                    //update the session to false
                    getCurrentSession.IsCurrent = false;
                    getCurrentSession.IsOpened = false;
                    getCurrentSession.IsClosed = true;

                    //update the new session whose parameter is supplied to true
                    var setSessionCurrent = _context.AcademicSessions.FirstOrDefault(s => s.Id == academicSessionId && s.SchoolId == schoolId);
                    setSessionCurrent.IsCurrent = true;
                    setSessionCurrent.IsOpened = true;
                    setSessionCurrent.IsClosed = false;

                    await _context.SaveChangesAsync();

                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Academic Session Set Successfully" };

                }
                else
                {
                    //update the new session whose parameter is supplied to true
                    var setSessionCurrent = _context.AcademicSessions.FirstOrDefault(s => s.Id == academicSessionId && s.SchoolId == schoolId);
                    setSessionCurrent.IsCurrent = true;
                    setSessionCurrent.IsOpened = true;
                    setSessionCurrent.IsClosed = false;

                    await _context.SaveChangesAsync();

                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Academic Session Set Successfully" };

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


        public async Task<GenericRespModel> closeAcademicSessionAsync(long schoolId, long academicSessionId)
        {
            try
            {
                //get the session
                var result = _context.AcademicSessions.Where(s => s.Id == academicSessionId && s.SchoolId == schoolId).FirstOrDefault();
                if (result != null)
                {
                    result.IsClosed = true;
                    result.IsOpened = false;
                    result.IsCurrent = false;

                    await _context.SaveChangesAsync();

                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Academic Session Closed/Terminated Successfully" };
                }

                return new GenericRespModel { StatusCode = 409, StatusMessage = "No Academic Session with the specified ID" };

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

        public async Task<GenericRespModel> openAcademicSessionAsync(long schoolId, long academicSessionId)
        {
            try
            {
                //get the current session that is set to true
                var getCurrentSession = _context.AcademicSessions.Where(s => s.SchoolId == schoolId && s.IsCurrent == true).FirstOrDefault();

                if (getCurrentSession == null)
                {
                    //get the session
                    var result = _context.AcademicSessions.Where(s => s.Id == academicSessionId && s.SchoolId == schoolId).FirstOrDefault();
                    if (result != null)
                    {
                        result.IsOpened = true;
                        result.IsCurrent = true;
                        result.IsClosed = false;

                        await _context.SaveChangesAsync();

                        return new GenericRespModel { StatusCode = 200, StatusMessage = "Academic Session Opened Successfully" };
                    }

                    return new GenericRespModel { StatusCode = 409, StatusMessage = "No Academic Session with the specified ID" };
                }

                return new GenericRespModel { StatusCode = 409, StatusMessage = "An Academic Session is Current and Opened" };

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

        public async Task<GenericRespModel> getCurrentSessionAsync(long schoolId)
        {
            try
            {
                CheckerValidation check = new CheckerValidation(_context);
                var checkResult = check.checkSchoolById(schoolId);

                if (checkResult == true)
                {
                    //returns the current academic session
                    var result = from s in _context.AcademicSessions
                                 where s.SchoolId == schoolId && s.IsCurrent == true
                                 select new
                                 {
                                     s.Id,
                                     s.SchoolId,
                                     s.SessionId,
                                     s.Sessions.SessionName,
                                 };

                    if (result.Count() > 0)
                    {
                        return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = result.FirstOrDefault(), };
                    }

                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful, No Record Available", };
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

        public async Task<GenericRespModel> getCurrentTermAsync(long schoolId)
        {
            try
            {
                CheckerValidation check = new CheckerValidation(_context);
                var checkResult = check.checkSchoolById(schoolId);

                if (checkResult == true)
                {
                    //returns the current academic term
                    var result = from s in _context.AcademicSessions
                                 where s.SchoolId == schoolId && s.IsCurrent == true
                                 select new
                                 {
                                     s.Id,
                                     s.SchoolId,
                                     s.TermId,
                                     s.Terms.TermName,
                                 };

                    if (result.Count() > 0)
                    {
                        return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = result.FirstOrDefault(), };
                    }

                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful, No Record Available", };
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
        public async Task<GenericRespModel> getCurrentAcademicSessionAsync(long schoolId)
        {
            try
            {
                CheckerValidation check = new CheckerValidation(_context);
                var checkResult = check.checkSchoolById(schoolId);

                if (checkResult == true)
                {
                    //returns all the academic session created by the school
                    var result = from s in _context.AcademicSessions
                                 where s.SchoolId == schoolId && s.IsCurrent == true
                                 select new
                                 {
                                     s.Id,
                                     s.SchoolId,
                                     s.Terms.TermName,
                                     s.Sessions.SessionName,
                                     s.UserId,
                                     s.DateStart,
                                     s.DateEnd,
                                     s.IsApproved,
                                     s.IsCurrent,
                                     s.IsClosed,
                                     s.IsOpened,
                                     s.DateCreated
                                 };
                    if (result.Count() > 0)
                    {
                        return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful", Data = result.ToList(), };
                    }

                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Successful, No Record Available", };
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

        public async Task<GenericRespModel> updateSessionAsync(long sessionId, SessionReqModel obj)
        {
            try
            {
                //checks the session
                var result = _context.Sessions.Where(s => s.Id == sessionId).FirstOrDefault();
                if (result != null)
                {
                    //check if ther session already exists before updating
                    var sessionExist = _context.Sessions.Where(s => s.SessionName == obj.SessionName && s.SchoolId == obj.SchoolId).FirstOrDefault();
                    if (sessionExist == null)
                    {
                        result.SessionName = obj.SessionName;
                        result.SchoolId = obj.SchoolId;

                        await _context.SaveChangesAsync();
                        return new GenericRespModel { StatusCode = 200, StatusMessage = "Session Updated Successfully" };
                    }

                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Session Already Exists" };

                }

                return new GenericRespModel { StatusCode = 200, StatusMessage = "No Session with the specified ID" };

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

        public async Task<GenericRespModel> updateAcademicSessionAsync(long academicSessionId, AcademicSessionReqModel obj)
        {
            try
            {
                //checks the session
                var result = _context.AcademicSessions.Where(s => s.Id == academicSessionId).FirstOrDefault();
                if (result != null)
                {
                    result.SessionId = obj.SessionId;
                    result.TermId = obj.TermId;
                    result.SchoolId = obj.SchoolId;
                    result.UserId = obj.UserId;
                    result.DateStart = obj.DateStart;
                    result.DateEnd = obj.DateEnd;

                    await _context.SaveChangesAsync();
                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Academic Session Updated Successfully" };
                }

                return new GenericRespModel { StatusCode = 200, StatusMessage = "No Academic Session with the specified ID" };

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

        public async Task<GenericRespModel> deleteSessionAsync(long sessionId)
        {
            try
            {
                //checks the session
                var result = _context.Sessions.Where(s => s.Id == sessionId).FirstOrDefault();
                if (result != null)
                {
                    _context.Sessions.Remove(result);
                    await _context.SaveChangesAsync();

                    return new GenericRespModel { StatusCode = 200, StatusMessage = "Session Deleted Successfully" };
                }

                return new GenericRespModel { StatusCode = 200, StatusMessage = "No Session with the specified ID" };

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

        public async Task<GenericRespModel> deleteAcademicSessionAsync(long academicSessionId)
        {
            try
            {
                //checks the Academic session
                var result = _context.AcademicSessions.Where(s => s.Id == academicSessionId).FirstOrDefault();
                if (result != null)
                {
                    //check if the academic session is current
                    if (result.IsCurrent == true)
                    {
                        return new GenericRespModel { StatusCode = 200, StatusMessage = "This is a current Academic Session" };
                    }
                    //check if the academic session is Closed/Terminated
                    else if (result.IsClosed == true)
                    {
                        return new GenericRespModel { StatusCode = 200, StatusMessage = "This is a Closed/Terminated Academic Session" };
                    }
                    else
                    {
                        _context.AcademicSessions.Remove(result);
                        await _context.SaveChangesAsync();

                        await _context.SaveChangesAsync();
                        return new GenericRespModel { StatusCode = 200, StatusMessage = "Academic Session Deleted Successfully" };
                    }
                }

                return new GenericRespModel { StatusCode = 200, StatusMessage = "No Academic Session with the specified ID" };

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
