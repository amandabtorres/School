using Microsoft.AspNetCore.Mvc.Rendering;
using School.Data.Entities;

namespace School.Data
{
    public interface IClassSchoolRepository : IGenericRepository<ClassSchool>
    {
        Task<IEnumerable<SubjectsClassDetail>> GetSubjectsInClassAsync(int idClass);

        Task<IEnumerable<SelectListItem>> GetComboSubjectsNotInClassAsync(int idClass);

        Task<IEnumerable<SelectListItem>> GetComboTeachersAsync();

        SubjectsClassDetail GetSubjectClassDetail(int id);
          
        Task<int> DeleteSubjectClassDetailAsync(int idSubjectClass);

        
    }
}
