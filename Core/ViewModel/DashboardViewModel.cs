using Web.Models.Entities;

namespace Web.ViewModel;

public class DashboardViewModel
{
    public int TotalCourses { get; set; }
    public int TotalInstructors { get; set; }
    public int TotalTrainees { get; set; }
    public int TotalDepartments { get; set; }
    public int TotalEnrollements { get; set; }
    
    public List<(string DeptName, int CourseCount)> CoursesByDept =  new List<(string DeptName, int CourseCount)> ();
    public List<Course> RecentAddedCourses { get; set; } = new List<Course>();
    public List<Course> RecentEnrollements { get; set; } = new List<Course>();
}