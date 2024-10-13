using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using School.Data.Entities;

namespace School.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly IStudentsClassDetailRepository _studentsClassDetailRepository;

        public UserRepository(
            DataContext context,
            IStudentsClassDetailRepository studentsClassDetailRepository)
        {
            _context = context;
            _studentsClassDetailRepository = studentsClassDetailRepository;
        }        
        public IEnumerable<User> GetAll()
        {
            return _context.Users.OrderBy(u => u.FirstName);
        }

        public async Task<IEnumerable<SelectListItem>> GetComboTeachersAsync()
        {
            var teacherRole = await _context.Roles.FirstOrDefaultAsync(t => t.Name == "Teacher");
            if (teacherRole == null)
            {
                return null; 
            }
            var listId = await _context.UserRoles
                .Where(u => u.RoleId == teacherRole.Id)
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
               
        public async Task<IEnumerable<User>> GetUserByRoleInSchool(string roleName)
        {
            var role = await _context.Roles.FirstOrDefaultAsync(t => t.Name == roleName);
            if (role == null)
            {
                return null;
            }
            var listId = await _context.UserRoles
                .Where(u => u.RoleId == role.Id)
                .Select(u => u.UserId)
                .ToListAsync();

            return await _context.Users.Where(u => listId.Contains(u.Id)).ToListAsync();
        }

        public async Task<bool> IsUserInClass(User user)
        {
            var list = await _studentsClassDetailRepository.GetClassesSchoolByStudent(user);
            var listTeacher = await _studentsClassDetailRepository.GetSubjectsClassDetailByTeacherAsync(user.Email);
            if(list.Count() > 0 || listTeacher.Count() > 0)
            {
                return true;
            }
            return false;
        }
    }
}
