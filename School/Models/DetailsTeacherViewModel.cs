using School.Data.Entities;

namespace School.Models
{
    public class DetailsTeacherViewModel
    {
        public User User { get; set; }       

        public IEnumerable<SubjectsClassDetail> Subjects { get; set; }
    }
}
