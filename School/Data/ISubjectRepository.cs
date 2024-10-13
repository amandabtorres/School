using School.Data.Entities;

namespace School.Data
{
    public interface ISubjectRepository : IGenericRepository<Subject>
    {
        bool IsSubjectInClass (Subject subject);
    }
}
