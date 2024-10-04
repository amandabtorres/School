using School.Data.Entities;

namespace School.Models
{
    public class StudentsClassDetailViewModel
    {
        public int ClassSchoolId { get; set; }
        public IEnumerable<User> StudentsInClass { get; set; }
        public IEnumerable<User> StudentsAvailable { get; set; }
    }
}
