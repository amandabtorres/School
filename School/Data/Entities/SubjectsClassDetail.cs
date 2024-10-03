
using System.ComponentModel.DataAnnotations;

namespace School.Data.Entities
{
    public class SubjectsClassDetail : IEntity
    {
        public int Id { get; set; }

        public int SubjectId { get; set; }
        public Subject Subject { get; set; }

        public string TeacherId { get; set; }
        [Display(Name ="Teacher")]
        public User Teacher { get; set; }

        public int ClassSchoolId { get; set; }

    }
}
