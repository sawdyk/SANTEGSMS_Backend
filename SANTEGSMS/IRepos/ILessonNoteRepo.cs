using SANTEGSMS.RequestModels;
using SANTEGSMS.ResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SANTEGSMS.IRepos
{
    public interface ILessonNoteRepo
    {
        //-----------------------------------LESSONNOTES---------------------------------------------------------------
        Task<GenericRespModel> createLessonNotesAsync(LessonNoteCreateReqModel obj);
        Task<GenericRespModel> getLessonNotesByIdAsync(long lessonNoteId, long schoolId, long campusId);
        Task<GenericRespModel> getLessonNotesBySubjectIdAsync(long subjectId, long schoolId, long campusId, long termId, long sessionId);
        Task<GenericRespModel> getLessonNotesByClassGradeIdAsync(long classId, long classGradeId, long schoolId, long campusId, long termId, long sessionId);
        Task<GenericRespModel> getLessonNotesByTeacherIdAsync(Guid teacherId, long schoolId, long campusId, long termId, long sessionId);
        Task<GenericRespModel> deleteLessonNotesAsync(long lessonNoteId, long schoolId, long campusId);
        Task<GenericRespModel> updateLessonNotesAsync(long lessonNoteId, LessonNoteCreateReqModel obj);
        Task<GenericRespModel> approveLessonNotesAsync(long lessonNoteId, long statusId, long schoolId, long campusId);


        //-------------------------------------------------SUBJECTNOTES---------------------------------------------------------------
        Task<GenericRespModel> createSubjectNotesAsync(SubjectNoteCreateReqModel obj);
        Task<GenericRespModel> getSubjectNotesByIdAsync(long subjectNoteId, long schoolId, long campusId);
        Task<GenericRespModel> getSubjectNotesBySubjectIdAsync(long subjectId, long schoolId, long campusId, long termId, long sessionId);
        Task<GenericRespModel> getSubjectNotesByClassGradeIdAsync(long classId, long classGradeId, long schoolId, long campusId, long termId, long sessionId);
        Task<GenericRespModel> getSubjectNotesByTeacherIdAsync(Guid teacherId, long schoolId, long campusId, long termId, long sessionId);
        Task<GenericRespModel> deleteSubjectNotesAsync(long subjectNoteId, long schoolId, long campusId);
        Task<GenericRespModel> updateSubjectNotesAsync(long subjectNoteId, SubjectNoteCreateReqModel obj);
    }
}
