using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using School.Data.Entities;
using System.Collections.Generic;

namespace School.Data
{
    public class ClassSchoolRepository : GenericRepository<ClassSchool>, IClassSchoolRepository
    {
        private readonly DataContext _context;

        public ClassSchoolRepository(DataContext context) : base(context)
        {
            _context = context;
        }             

        public async Task<IEnumerable<SubjectsClassDetail>> GetSubjectsInClassAsync(int idClass)
        {
            var classSchool =  await _context.ClassSchool.FindAsync(idClass);
            if (classSchool == null) 
            {
                return null;
            }

            return _context.SubjectsClassDetails
                .Include(s => s.Subject)
                .Include(u=> u.Teacher)
                .Where(c => c.ClassSchoolId == classSchool.Id).AsNoTracking();
        }

        public async Task<IEnumerable<SelectListItem>> GetComboSubjectsNotInClassAsync(int idClass)
        {
            var classSchool = await _context.ClassSchool.FindAsync(idClass);
            if (classSchool == null)
            {
                return null;
            }
            var subjectsInClass = await _context.SubjectsClassDetails
                .Where(s => s.ClassSchoolId == idClass)
                .Select(s => s.SubjectId).ToListAsync();

            var subjectsNotInClass = await _context.Subjects
                .Where(s => !subjectsInClass.Contains(s.Id))
                .Select(s => new SelectListItem
                {
                    Value = s.Id.ToString(),
                    Text = s.Name
                })
            .ToListAsync();

            subjectsNotInClass.Insert(0, new SelectListItem
            {
                Text = "(Select a subject...)",
                Value = "0"
            });

            return subjectsNotInClass;
        }

        public async Task<IEnumerable<SelectListItem>> GetComboTeachersAsync()
        {
            var teacher = await _context.Roles.FirstOrDefaultAsync(t => t.Name == "Teacher");
            if(teacher== null)
            {
                return null; ;
            }
            var listId = await _context.UserRoles
                .Where(u => u.RoleId == teacher.Id)
                .Select(u => u.UserId)
                .ToListAsync();

            var listTeacher = await _context.Users
                .Where(u => listId.Contains(u.Id)).Select(s => new SelectListItem
                {
                    Value = s.Id,
                    Text = s.FullName
                })
            .ToListAsync();

            listTeacher.Insert(0, new SelectListItem
            {
                Text = "(Select a teacher...)",
                Value = ""
            });

            return listTeacher;
        }

        public SubjectsClassDetail GetSubjectClassDetail(int id)
        {
            return _context.SubjectsClassDetails.FirstOrDefault(s => s.Id == id);
        }
           

        public async Task<int> DeleteSubjectClassDetailAsync(int id)
        {
            var subjectclass = await _context.SubjectsClassDetails.FindAsync(id);
            if (subjectclass == null)
            {
                return 0;
            }
            var classId = subjectclass.ClassSchoolId;
            _context.SubjectsClassDetails.Remove(subjectclass);
            await _context.SaveChangesAsync();
            return classId;
        }

        
    }
}
