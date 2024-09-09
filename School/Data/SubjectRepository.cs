using School.Data.Entities;

namespace School.Data
{
    public class SubjectRepository : GenericRepository<Subject>, ISubjectRepository
    {
        public SubjectRepository(DataContext context):base(context)
        {
            
        }
    }
}
