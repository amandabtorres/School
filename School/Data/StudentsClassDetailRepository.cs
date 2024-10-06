using Microsoft.EntityFrameworkCore;
using School.Data.Entities;
using School.Helpers;

namespace School.Data
{
    public class StudentsClassDetailRepository : GenericRepository<StudentsClassDetail>, IStudentsClassDetailRepository
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public StudentsClassDetailRepository(DataContext context, IUserHelper userHelper) : base(context)
        {
            _context = context;
            _userHelper = userHelper;
        }

        public async Task<IEnumerable<StudentsClassDetail>> GetStudentsClassDetailsBySubjectClassDetailAsync(SubjectsClassDetail scd)
        {                      
            return await _context.StudentsClassDetails                
                .Include(s=> s.Student)                
                .Where(s=> s.SubjectsClassDetailId == scd.Id)
                .ToListAsync();
        }

        public async Task<IEnumerable<SubjectsClassDetail>> GetSubjectsClassDetailByTeacherAsync(string userNameTeacher)
        {
            var user = await _userHelper.GetUserByEmailAsync(userNameTeacher);
            if (user == null)
            {
                return Enumerable.Empty<SubjectsClassDetail>();
            }

            if(await _userHelper.IsUserInRoleAsync(user, "Admin"))
            {
                return await _context.SubjectsClassDetails
               .Include(s => s.Subject)
               .Include(c => c.ClassSchool)
               .Include(t => t.Teacher)
               .ToListAsync();

            }

            return await _context.SubjectsClassDetails
                .Include(s=> s.Subject)
                .Include(c=> c.ClassSchool)
                .Include(t=> t.Teacher)
                .Where(u=> u.TeacherId == user.Id).ToListAsync();
        }

        public async Task<Response> SaveGradeAsync(int id, decimal grade)
        {
            try
            {
                var studentClassDetail = await _context.StudentsClassDetails.FindAsync(id);
                if(studentClassDetail != null)
                {
                    studentClassDetail.Grade = grade;
                    _context.StudentsClassDetails.Update(studentClassDetail);
                    await _context.SaveChangesAsync();
                    return new Response { IsSuccess = true };
                }
                return new Response { IsSuccess = false };
            }
            catch (Exception ex)
            {
                return new Response { Message = "An error occurred in the process: " + ex.Message, IsSuccess = false };
            }
           
        }
    }
}
