using School.Data.Entities;

namespace School.Models
{
    public class DetailsClassViewModel
    {
        public ClassSchool ClassSchool { get; set; }

        public IEnumerable<SubjectsClassDetail> SubjectsInClass { get; set; }

        public IEnumerable<User> StudentsInClass    { get; set; }
    }
}
