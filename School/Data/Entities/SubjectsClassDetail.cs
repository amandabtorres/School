namespace School.Data.Entities
{
    public class SubjectsClassDetail : IEntity
    {
        public int Id { get; set; }

        public Subject Subject { get; set; }

        public User Teacher { get; set; }

    }
}
