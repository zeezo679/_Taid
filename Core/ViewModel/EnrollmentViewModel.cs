using Web.Models.Entities;

namespace Web.ViewModel;

public class EnrollmentViewModel
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? TraineeName { get; set; }
    public DateTime EnrollmentDate { get; set; }
    public virtual Department Department { get; set; } = null!;
    public virtual ICollection<Instructor> Instructors { get; set; } = new List<Instructor>();
    
}