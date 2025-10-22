using System.ComponentModel.DataAnnotations;
using Demo.Models.Entities;

namespace Demo.ViewModel;

public class EditInstructorViewModel
{
    public int Id { get; set; }

    [Display(Name = "Instructor Name")]
    public string Name { get; set; } = null!;
    public IFormFile Image { get; set; }
    public decimal Salary { get; set; }
    public string Address { get; set; } = null!;
    public string Email { get; set; }

    public int CourseId { get; set; }
    public int DeptId { get; set; }


    public List<Course> courses { get; set; } = new();
    public List<Department> departments { get; set; } = new();
    public List<Course> itemList;
}