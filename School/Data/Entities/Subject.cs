using System.ComponentModel.DataAnnotations;

namespace School.Data.Entities
{
    public class Subject : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }       
        public int Workload { get; set; }

        [Display(Name = "Minimum attendance percentage")]
        [Range(0, 100, ErrorMessage = "The percentage must range from 0 to 100...")]
        public int? AttendancePercentageMin { get; set; }
    }
}
