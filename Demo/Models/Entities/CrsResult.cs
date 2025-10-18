namespace Demo.Models.Entities
{
    public class CrsResult
    {
        public int Id { get; set; }    
        public decimal Degree { get; set; }

        public int CourseId { get; set; }
        public string UserId { get; set; } = null!;  
        public virtual Course Course { get; set; } = null!;
        public virtual Trainee Trainee { get; set; } = null!;
    }
}
