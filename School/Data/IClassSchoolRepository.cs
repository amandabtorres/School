using Microsoft.AspNetCore.Mvc.Rendering;
using School.Data.Entities;
using School.Helpers;

namespace School.Data
{
    public interface IClassSchoolRepository : IGenericRepository<ClassSchool>
    {
        Task<IEnumerable<SubjectsClassDetail>> GetSubjectsInClassAsync(int idClass);

        Task<IEnumerable<SelectListItem>> GetComboSubjectsNotInClassAsync(int idClass);
               
        SubjectsClassDetail GetSubjectClassDetail(int id);

        Task<Response> AddSubjectClassDetailInClass(SubjectsClassDetail subjectClassDetail, ClassSchool classSchool);

        Task<Response> RemoveSubjectClassDetailAsync(SubjectsClassDetail scd);

        Task<IEnumerable<User>> GetStudentsInClassAsync(int idClass);

        Task<IEnumerable<User>> GetStudentsNotEnrolledInClassAsync(int idClass);

        Task<Response> AddStudentInClass(User user, ClassSchool classSchool);
        Task<Response> RemoveStudentInClass(User user, ClassSchool classSchool);
        

    }
}
