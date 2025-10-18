using Demo.Infrastructure;
using Demo.Models.Entities;
using Demo.Models.Interfaces;
using Demo.Models.Repository;
using Demo.Services;
using Demo.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Runtime.Serialization;
using System.Security.Claims;

namespace Demo.Controllers;


public class CourseController : Controller
{

    private ICourseRepository CourseRepository;
    private IInstructorRepository InstructorRepository;
    private IDepartmentRepository DepartmentRepository;
    private ITraineeRepository TraineeRepository;
    private ICrsResultRepository CrsResultRepository;
    private UserManager<ApplicationUser> _userManager;
    public readonly EmailService _emailService;

    public CourseController(
        ICourseRepository courseRepository, 
        IDepartmentRepository departmentRepository,
        IInstructorRepository instructorRepository,
        ITraineeRepository traineeRepository,
        ICrsResultRepository crsResultRepository,
        UserManager<ApplicationUser> userManager,
        EmailService emailService
        )
    {
        CourseRepository = courseRepository;
        DepartmentRepository = departmentRepository;
        InstructorRepository = instructorRepository;
        TraineeRepository = traineeRepository;
        CrsResultRepository = crsResultRepository;
        _userManager = userManager;
        _emailService = emailService;
    }

      [HttpGet]
    //[Route("courses")] -> this route has higher priority than the routing in the program.cs
    public IActionResult Index(int? deptId)
    {
        List<Course> courses = CourseRepository.FilterByDept(deptId);
        List<string> Errors = new List<string>();
        List<Department> departments = DepartmentRepository.Load();
        List<Instructor> instructors = InstructorRepository.Load();


        var deptItems = new List<SelectListItem>
        {
            new SelectListItem { Text = "All Departments", Value="0", Selected = !deptId.HasValue || deptId == 0 }
        };
        deptItems.AddRange(departments.Select(d => new SelectListItem
        {
            Text = d.Name,
            Value = d.Id.ToString(),
            Selected = deptId.HasValue && d.Id == deptId.Value
        }));

        if (courses is null)
        {
            Errors.Add("Cannot fetch courses from the database");
            ViewBag.Errors = Errors;
            return RedirectToAction("Index", "Home");
        }

        ViewBag.Departments = departments;
        ViewBag.DeptItems = deptItems;
        ViewBag.Instructors = instructors;
        ViewBag.SelectedDept = deptId;
        return View("Index", courses);
    }



    public IActionResult AddCourse()
    {

        CourseViewModel courseView = new CourseViewModel();

        var instructors = InstructorRepository.LoadSelectItems();

        courseView.Instructors = instructors;
        courseView.Departments = DepartmentRepository.Load();

        return View("AddCourse", courseView);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AddCourse(int id,CourseViewModel CourseView)
    {
        CourseView.Departments = DepartmentRepository.Load();
        CourseView.Instructors = InstructorRepository.LoadSelectItems();

        if (ModelState.IsValid) //server side validation
        {
            try
            {
                var course = new Course
                {
                    Name = CourseView.Name,
                    Degree = CourseView.Degree,
                    MinDegree = CourseView.MinDegree,
                    DeptId = CourseView.DeptId,
                    CourseDescription = CourseView.CourseDescription,
                    Instructors = InstructorRepository.LoadInstructorsWithTheirCourses(CourseView, false)
                };

                int oldCourseCount = CourseRepository.Count();

                CourseRepository.Insert(course);

                int newCourseCount = CourseRepository.Count();

                if (newCourseCount > oldCourseCount)
                    TempData["Message"] = "Course Added Successfully";
                else 
                    TempData["Message"] = string.Empty;

                
                return RedirectToAction("Index", new { deptId = 0});
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
            }
        }
        return View(CourseView);
    }
    [HttpGet]
    public IActionResult Edit(int id)
    {
        CourseViewModel courseView = new CourseViewModel();
        var course = CourseRepository.Get(id);


        var instructors = InstructorRepository.LoadSelectItems();

        courseView.Instructors = instructors;
        courseView.Departments = DepartmentRepository.Load();
        courseView.Name = course.Name;
        courseView.DeptId = course.DeptId;
        courseView.Id = course.Id;
        courseView.Degree = course.Degree;
        courseView.MinDegree = course.MinDegree;
        courseView.CourseDescription = course.CourseDescription;

        return View("Edit", courseView);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, CourseViewModel newCourse)
    {
        var oldCourse = CourseRepository.Get(id);

        newCourse.Departments = DepartmentRepository.Load();
        newCourse.Instructors = InstructorRepository.LoadSelectItems();

        oldCourse.Name = newCourse.Name;
        oldCourse.DeptId = newCourse.DeptId;
        oldCourse.Degree = newCourse.Degree;
        oldCourse.MinDegree = newCourse.MinDegree;
        oldCourse.Instructors = InstructorRepository.LoadInstructorsWithTheirCourses(newCourse, true);
        oldCourse.CourseDescription = newCourse.CourseDescription;

        CourseRepository.Update(id, oldCourse);

        return RedirectToAction("Index");
    }

    public JsonResult IsValidMinDegree(decimal minDegree, decimal degree)
    {
        if(minDegree < degree)
            return Json(true);
        else
            return Json("Minimum Degree must be less than the degree you entered");
    }

    [HttpGet]
    public IActionResult ViewCourseDetails(int id)
    {
        var course = CourseRepository.Get(id);
        var uid = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        //check if the courseId is present in the crsResults with the current uid
        bool isEnrolled = CrsResultRepository.CheckEnrollStatus(id, uid);
        
        ViewData["IsEnrolled"] = isEnrolled;
        return View(course);
    }
    
    [HttpGet]
    public IActionResult CourseDetails(int id)
    {
        var course = CourseRepository.Get(id);
        var instructors = InstructorRepository.Load();
        
        course.Instructors = instructors;

        return PartialView("CourseDetailsPartial", course);
    }


    public async Task<IActionResult> Enroll(int id)
    {
        var currentLoggedUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(currentLoggedUserId))
            return RedirectToAction("Login", "Account");


        var course = CourseRepository.Get(id);
        var user = await _userManager.FindByIdAsync(currentLoggedUserId);

        if (course is null || user is null)
            return Content("user or course not found");

        var trainee = new Trainee
        {
            UserId = currentLoggedUserId,
            Name = user.UserName,
            Address = user.Address,
            Image = null,
            Grade = 0.00m,
            Department = course.Department,
        };

        if (!TraineeRepository.IsAlreadyTrainee(currentLoggedUserId))
            TraineeRepository.InsertTraineeCrsResult(trainee, course.Id);

       
        CrsResultRepository.Insert(trainee, id);

        if(!await _userManager.IsInRoleAsync(user, "Trainee"))
        {
            await _userManager.AddToRoleAsync(user, "Trainee");
        }

        TempData["EnrollSuccess"] = true;

        await _emailService.SendEmailAsync(
               user.Email,
               "Course Enrollment",
               $"<h3>Hello {user.UserName},</h3><p>Thank you for enrolling in the course 🎉</p>"
           );

        return RedirectToAction("Index", new { deptId = course.DeptId });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        CourseRepository.Delete(id);
        return RedirectToAction("Index");
    }
}
