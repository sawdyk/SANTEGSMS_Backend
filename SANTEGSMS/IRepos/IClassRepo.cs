using SANTEGSMS.RequestModels;
using SANTEGSMS.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.IRepos
{
    public interface IClassRepo
    {
        //-----------------------------Classes and Class Grades/Arms-------------------------------------------------
        Task<GenericRespModel> createClassAsync(ClassReqModel obj);
        Task<GenericRespModel> createClassGradesAsync(ClassGradeReqModel obj);
        Task<GenericRespModel> getAllClassesAsync(long schoolId, long campusId);
        Task<GenericRespModel> getAllClassGradesAsync(long schoolId, long campusId);
        Task<GenericRespModel> getClassByClassIdAsync(long classId, long schoolId, long campusId);
        Task<GenericRespModel> getClassGradesByClassGradeIdAsync(long classGradeId, long schoolId, long campusId);
        Task<GenericRespModel> getClassGradesByClassIdAsync(long classId, long schoolId, long campusId);
        Task<GenericRespModel> updateClassAsync(long classId, ClassReqModel obj);
        Task<GenericRespModel> updateClassGradeAsync(long classGradeId, ClassGradeReqModel obj);
        Task<GenericRespModel> deleteClassAsync(long classId, long schoolId, long campusId);
        Task<GenericRespModel> deleteClassGradeAsync(long classGradeId, long schoolId, long campusId);

        //-----------------------------Students In Class And ClassGrades-------------------------------------------------
        Task<GenericRespModel> getAllStudentInClassAsync(long classId, long schoolId, long campusId, long sessionId);
        Task<GenericRespModel> getAllStudentInClassGradeAsync(long classId, long classGradeId, long schoolId, long campusId, long sessionId);
        Task<GenericRespModel> getAllStudentInClassForCurrentSessionAsync(long classId, long schoolId, long campusId);
        Task<GenericRespModel> getAllStudentInClassGradeForCurrentSessionAsync(long classId, long classGradeId, long schoolId, long campusId);
    }
}
