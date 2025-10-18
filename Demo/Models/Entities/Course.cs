using Demo.Models.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demo.Models.Entities
{
    public class Course : IDeletable
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public decimal Degree { get; set; }
        public decimal MinDegree { get; set; }
        public string? CourseDescription { get; set; }

        [ForeignKey("Department")]
        public int DeptId { get; set; }
        public virtual Department Department { get; set; } = null!;
        public virtual ICollection<Instructor> Instructors { get; set; } = new List<Instructor>();

        public override string ToString()
        {
            return $"{this.Name}";
        }
    }
}
