using Microsoft.CodeAnalysis.CSharp.Syntax;
using School.Data.Entities;

namespace School.Data
{
    public class SubjectRepository : GenericRepository<Subject>, ISubjectRepository
    {
        private readonly DataContext _context;

        public SubjectRepository(DataContext context):base(context)
        {
            _context = context;
        }

        /// <summary>
        /// Verifica se uma disciplina está em alguma turma
        /// </summary>
        /// <param name="subject">disciplina</param>
        /// <returns>True se ela estiver em alguma turma, senao false</returns>
        public bool IsSubjectInClass(Subject subject)
        {
            var result = _context.SubjectsClassDetails.Select(s=> s.SubjectId).Contains(subject.Id);
            if (result)
            {
                return true;
            }
            return false;        }
    }
}
