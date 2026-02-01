namespace Web.Models.Entities
{
    public class CrsResult
    {
        public int Id { get; set; }    
        public decimal Degree { get; set; }

        public int CourseId { get; set; }
        public string UserId { get; set; } = null!;  
        
        public DateTime EnrollmentDate { get; set; }
        public virtual Course Course { get; set; } = null!;
        public virtual Trainee Trainee { get; set; } = null!;
    }
}
