namespace School.Data.Entities
{
    public class Subject : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }       
        public int Workload { get; set; }
    }
}
