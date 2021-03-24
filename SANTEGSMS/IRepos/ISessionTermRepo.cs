using SANTEGSMS.RequestModels;
using SANTEGSMS.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.IRepos
{
    public interface ISessionTermRepo
    {
        Task<GenericRespModel> getAllTermsAsync();
        Task<GenericRespModel> getTermByIdAsync(long termId);
        Task<GenericRespModel> createSessionAsync(SessionReqModel obj);
        Task<GenericRespModel> getAllSessionsAsync(long schoolId);
        Task<GenericRespModel> getSessionByIdAsync(long schoolId, long sessionId);
        Task<GenericRespModel> createAcademicSessionAsync(AcademicSessionReqModel obj);
        Task<GenericRespModel> getAllAcademicSessionsAsync(long schoolId);
        Task<GenericRespModel> setAcademicSessionAsCurrentAsync(long schoolId, long academicSessionId);
        Task<GenericRespModel> closeAcademicSessionAsync(long schoolId, long academicSessionId);
        Task<GenericRespModel> openAcademicSessionAsync(long schoolId, long academicSessionId);
        Task<GenericRespModel> getCurrentSessionAsync(long schoolId);
        Task<GenericRespModel> getCurrentTermAsync(long schoolId);
        Task<GenericRespModel> getCurrentAcademicSessionAsync(long schoolId);

        Task<GenericRespModel> updateSessionAsync(long sessionId, SessionReqModel obj);
        Task<GenericRespModel> updateAcademicSessionAsync(long academicSessionId, AcademicSessionReqModel obj);
        Task<GenericRespModel> deleteSessionAsync(long sessionId);
        Task<GenericRespModel> deleteAcademicSessionAsync(long academicSessionId);
    }
}
