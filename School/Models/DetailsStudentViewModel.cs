using School.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace School.Models
{
    public class DetailsStudentViewModel
    {
        public User User { get; set; }              

        public IEnumerable<ClassSchool> ClassesOfStudent { get; set; }       
    }
}
