using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Web.Models.Interfaces;

namespace Web.Models.Entities
{
    public class Instructor : IDeletable
    {
        public int Id { get; set; }

        [Display(Name = "Instructor Name")]
        public string Name { get; set; } = null!;
        public string? Image { get; set; }
        public decimal Salary { get; set; }
        public string Address { get; set; } = null!;


        public int CourseId { get; set; }
        [ForeignKey("Department")]
        public int DeptId { get; set; }

        public string UserId { get; set; }

        public virtual Course Course { get; set; } = null!;
        public virtual Department Department { get; set; } = null!;

        public virtual ApplicationUser User { get; set; } = null!;
    }
}
