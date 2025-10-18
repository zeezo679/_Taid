using Demo.Models.Entities;
using Demo.Models.Interfaces;
using Demo.Models.Repository;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demo.ViewModel
{
    public class InstructorViewModel : IDeletable
    {
        //instructor
        public int Id { get; set; }

        [Display(Name = "Instructor Name")]
        public string Name { get; set; } = null!;
        public string? Image { get; set; }
        public decimal Salary { get; set; }
        public string Address { get; set; } = null!;
        
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        public int CourseId { get; set; }
        public int DeptId { get; set; }


        public List<Course> courses { get; set; } = new();
        public List<Department> departments { get; set; } = new();
        public List<Course> itemList;

    }
}
