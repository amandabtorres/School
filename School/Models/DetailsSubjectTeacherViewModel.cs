using School.Data.Entities;

namespace School.Models
{
    public class DetailsSubjectTeacherViewModel
    {
        public User User { get; set; }

        public IEnumerable<StudentsClassDetail> StudentsInSubject { get; set; }
    }
}
