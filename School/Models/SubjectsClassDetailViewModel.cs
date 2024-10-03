using Microsoft.AspNetCore.Mvc.Rendering;
using School.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace School.Models
{
    public class SubjectsClassDetailViewModel
    {
        public int ClassSchoolId { get; set; }
        public IEnumerable<SubjectsClassDetail> SubjectsInClass { get; set; }
       

    }
}
