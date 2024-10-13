using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace School.Data.Entities
{
    public class ClassSchool : IEntity
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "The field {0} can contain {1} characters.")]
        public string Name { get; set; }
        public string? Description { get; set; }

        [Display(Name = "Start Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = false)]
        public DateTime StartDate { get; set; }

        [Display(Name = "End Date")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = false)]
        public DateTime EndDate { get; set; }

        public List<SubjectsClassDetail> Subjects { get; set; } = new List<SubjectsClassDetail>();

        [Display(Name = "Status")]
        public string Status => (StartDate <= DateTime.Now && EndDate >= DateTime.Now) ? "Ongoing" : (StartDate > DateTime.Now) ? "To Start" : "Finished";
    }

}
