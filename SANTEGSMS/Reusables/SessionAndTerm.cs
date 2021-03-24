using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SANTEGSMS.DatabaseContext;


namespace SANTEGSMS.Reusables
{
    public class SessionAndTerm
    {
        private readonly AppDbContext _context;

        public SessionAndTerm(AppDbContext context)
        {
            _context = context;
        }
        public long getCurrentSessionId(long schoolId) //get the current SessionId
        {
            try
            {
                long returnCurrentSessionId;

                var getCurrentSessionId = from currentSession in _context.AcademicSessions
                                          where currentSession.SchoolId == schoolId && currentSession.IsCurrent == true
                                          select currentSession.SessionId;

                if (getCurrentSessionId.Count() > 0)
                {
                    returnCurrentSessionId = getCurrentSessionId.FirstOrDefault();
                }
                else
                {
                    returnCurrentSessionId = 0;
                }

                return returnCurrentSessionId;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public long getCurrentTermId(long schoolId) //get the current TermId 
        {
            try
            {
                long returnCurrentTermId;
                var getCurrentTermId = from currentTerm in _context.AcademicSessions
                                       where currentTerm.SchoolId == schoolId && currentTerm.IsCurrent == true
                                       select currentTerm.TermId;

                if (getCurrentTermId.Count() > 0)
                {
                    returnCurrentTermId = getCurrentTermId.FirstOrDefault();
                }
                else
                {
                    returnCurrentTermId = 0;
                }

                return returnCurrentTermId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
