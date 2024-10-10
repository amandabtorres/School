using School.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace School.Models
{
    public class DetailsTeacherViewModel
    {
        public User User { get; set; }       

        public IEnumerable<SubjectsClassDetail> Subjects { get; set; }

        [Display(Name = "Image")]
        public string? ImageUrl { get; set; }
    }
}
