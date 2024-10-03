using School.Data.Entities;

namespace School.Models
{
    public class StudentsClassDetailViewModel
    {
        public int ClassSchoolId { get; set; }
        public IEnumerable<StudentsClassDetail> StudentsInClass { get; set; }
    }
}
