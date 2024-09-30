namespace School.Data.Entities
{
    public class StudentsClassDetail : IEntity
    {
        public int Id { get; set; }
        public User Student { get; set; }

        public SubjectsClassDetail SubjectsClassDetail { get; set; }

        public decimal? Grade { get; set; }
    }
}
