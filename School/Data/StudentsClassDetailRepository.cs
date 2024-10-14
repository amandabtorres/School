using Microsoft.EntityFrameworkCore;
using School.Data.Entities;
using School.Helpers;

namespace School.Data
{
    public class StudentsClassDetailRepository : GenericRepository<StudentsClassDetail>, IStudentsClassDetailRepository
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private readonly IClassSchoolRepository _classSchoolRepository;

        public StudentsClassDetailRepository(
            DataContext context, 
            IUserHelper userHelper,
            IClassSchoolRepository classSchoolRepository) : base(context)
        {
            _context = context;
            _userHelper = userHelper;
            _classSchoolRepository = classSchoolRepository;
        }

        public async Task<Response> DecreaseAbsenceAsync(int id, int qtd)
        {
            try
            {
                var studentClassDetail = await _context.StudentsClassDetails.FindAsync(id);
                if (studentClassDetail != null)
                {
                    studentClassDetail.Absence -= qtd;
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

        public async Task<IEnumerable<ClassSchool>> GetClassesSchoolByStudent(User user)
        {
            return await _context.StudentsClassDetails
                        .Where(scd => scd.Student.Id == user.Id)
                        .Select(scd => scd.SubjectsClassDetail.ClassSchool)
                        .Distinct()
                        .ToListAsync();                        
        }

        public async Task<IEnumerable<StudentsClassDetail>> GetStudentClassDetailByClassStudentAsync(User user, int classId )
        {
            return await _context.StudentsClassDetails
                .Include(s=> s.SubjectsClassDetail)
                .ThenInclude(t=> t.Teacher)
                .Include(s=> s.SubjectsClassDetail)
                .ThenInclude(s=> s.Subject)               
                .Where(u=> u.StudentId == user.Id && u.SubjectsClassDetail.ClassSchoolId == classId)
                .ToListAsync();               
        }

        public async Task<IEnumerable<StudentsClassDetail>> GetStudentsClassDetailsByIdSubjectClassDetailAsync(int idScd)
        {
            var scd = _classSchoolRepository.GetSubjectClassDetail(idScd);
            if(scd == null)
            {
               return Enumerable.Empty<StudentsClassDetail>();
            }
            return await _context.StudentsClassDetails
                .Include(scd=> scd.SubjectsClassDetail).ThenInclude(s => s.Subject)
                .Include(scd => scd.SubjectsClassDetail).ThenInclude(t=> t.Teacher)                
                .Include(s => s.Student)
                .Where(s => s.SubjectsClassDetailId == scd.Id)
                .ToListAsync();
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

        public async Task<Response> IncreaseAbsenceAsync(int id, int qtd)
        {
            try
            {
                var studentClassDetail = await _context.StudentsClassDetails.FindAsync(id);
                if (studentClassDetail != null)
                {
                    studentClassDetail.Absence += qtd;
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
