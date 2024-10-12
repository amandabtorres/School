using System.ComponentModel.DataAnnotations;

namespace School.Data.Entities
{
    public class StudentsClassDetail : IEntity
    {
        public int Id { get; set; }

        public string StudentId { get; set; }
        public User Student { get; set; }

        public int SubjectsClassDetailId { get; set; }
        public SubjectsClassDetail SubjectsClassDetail { get; set; }


        public decimal? Grade { get; set; }

        public int? Absence { get; set; }

        [Display(Name = "Status")]
        public string Status => 
            !IsApprovedByPresence() ? "Failed due to absence" :
            Grade == null ? "Ongoing" : 
            Grade >= 10  ? "Approved"
            : "Failed";

        private bool IsApprovedByPresence()
        {
            double percentagemAbsence = ((double)Absence / (double)SubjectsClassDetail.Subject.Workload) * 100;
            if (percentagemAbsence > (100 - SubjectsClassDetail.Subject.AttendancePercentageMin))
            {
                return false;
            }
            return true;
        }
    }
}
