using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace School.Models
{
    public class AddSubjectViewModel
    {
        public int ClassSchoolId { get; set; }

        [Display(Name = "Subject")]
        [Range(1, int.MaxValue, ErrorMessage = "You must select a subject.")]
        public int SubjectId { get; set; }

        public IEnumerable<SelectListItem> Subjects { get; set; }

        [Display(Name = "Teacher")]
        [Required(AllowEmptyStrings = false)]
        public string TeacherId { get; set; }

        public IEnumerable<SelectListItem> Teachers { get; set; }
    }
}
