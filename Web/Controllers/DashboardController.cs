using Microsoft.AspNetCore.Mvc;
using Web.Models.Interfaces;
using Web.ViewModel;

namespace Web.Controllers;

public class DashboardController : Controller
{
    
    private readonly ICourseRepository  _courseRepository;
    private readonly IInstructorRepository _instructorRepository;
    private readonly ITraineeRepository _traineeRepository;
    private readonly IDepartmentRepository _departmentRepository;
    private readonly ICrsResultRepository _crsResultRepository;

    public DashboardController(
        ICourseRepository courseRepository, 
        IInstructorRepository instructorRepository,
        ITraineeRepository traineeRepository, IDepartmentRepository departmentRepository,
        ICrsResultRepository crsResultRepository)
    {
        _courseRepository = courseRepository;
        _instructorRepository = instructorRepository;
        _traineeRepository = traineeRepository;
        _departmentRepository = departmentRepository;
        _crsResultRepository = crsResultRepository;
    }
    
    public IActionResult Index()
    {

        var totalCourses = _courseRepository.Load().Count;
        var totalInstructors = _instructorRepository.Load().Count;
        var totalTrainees = _traineeRepository.Load(false).Count;
        var totalDepartments = _departmentRepository.Load().Count;
        var totalEnrollments = _crsResultRepository.Load().Count;

        var CountCoursesPerDept = _departmentRepository.CountCoursesPerDepartment();
        
        var model = new DashboardViewModel
        {
            TotalCourses = totalCourses,
            TotalInstructors = totalInstructors,
            TotalTrainees = totalTrainees,
            TotalDepartments = totalDepartments,
            TotalEnrollements = totalEnrollments,
            CoursesByDept = CountCoursesPerDept
        };
        
        return View(model);
    }
}