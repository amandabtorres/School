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
               
        /// <summary>
        /// Obtem todas as turma em que um aluno está
        /// </summary>
        /// <param name="user">Aluno</param>
        /// <returns>Todas as turma que o aluno participa</returns>
        public async Task<IEnumerable<ClassSchool>> GetClassesSchoolByStudent(User user)
        {
            return await _context.StudentsClassDetails
                        .Where(scd => scd.Student.Id == user.Id)
                        .Select(scd => scd.SubjectsClassDetail.ClassSchool)
                        .Distinct()
                        .ToListAsync();                        
        }

        /// <summary>
        /// Obtem todos os detalhes de um aluno de acordo com sua turma, para mostrar materias e notas
        /// </summary>
        /// <param name="user">Aluno</param>
        /// <param name="classId">Turma</param>
        /// <returns>Detalhes do estudante</returns>
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

        /// <summary>
        /// Obtem todos os detalhes dos alunos de acordo com uma disciplina na turma atraves do seu ID
        /// </summary>
        /// <param name="idScd">id de uma disciplina na turma</param>
        /// <returns>Detalhes dos estudantes na disciplina</returns>
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

        /// <summary>
        /// Obtem todos os detalhes dos alunos de acordo com uma disciplina na turma
        /// </summary>
        /// <param name="scd">disciplina na turma</param>
        /// <returns>Detalhes dos estudantes na disciplina</returns>
        public async Task<IEnumerable<StudentsClassDetail>> GetStudentsClassDetailsBySubjectClassDetailAsync(SubjectsClassDetail scd)
        {                      
            return await _context.StudentsClassDetails                
                .Include(s=> s.Student)                
                .Where(s=> s.SubjectsClassDetailId == scd.Id)
                .ToListAsync();
        }

        /// <summary>
        /// Obtem todos as disciplinas que um professor leciona 
        /// </summary>
        /// <param name="userNameTeacher">username do professor</param>
        /// <returns>IEnumerable de detalhes das disciplinas </returns>
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

        /// <summary>
        /// Aumenta uma quantidade de faltas 
        /// </summary>
        /// <param name="id">Id do StudentClassDetail</param>
        /// <param name="qtd">quantidade de falta</param>
        /// <returns>Response</returns>
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

        /// <summary>
        /// Diminui uma quantidade de faltas 
        /// </summary>
        /// <param name="id">Id do StudentClassDetail</param>
        /// <param name="qtd">quantidade de falta</param>
        /// <returns>Response</returns>
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

        /// <summary>
        /// Salva a nota do aluno em uma disciplina de uma turma
        /// </summary>
        /// <param name="id">Student Class Detail</param>
        /// <param name="grade">Nota</param>
        /// <returns>Response</returns>
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
