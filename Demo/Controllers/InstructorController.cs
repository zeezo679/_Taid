using Demo.Infrastructure;
using Demo.Models.Entities;
using Demo.Models.Interfaces;
using Demo.Models.Repository;
using Demo.Services;
using Demo.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Demo.Controllers
{
    [Authorize(Roles = "Admin")]
    public class InstructorController : Controller
    {

        private ICourseRepository CourseRepository;
        private IInstructorRepository InstructorRepository;
        private IDepartmentRepository DepartmentRepository;
        private UserManager<ApplicationUser> UserManager;
        private RoleManager<IdentityRole> RoleManager;

        public InstructorController(
            ICourseRepository courseRepository, 
            IInstructorRepository instructorRepository,
            IDepartmentRepository departmentRepository,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager
            )
        {
            CourseRepository = courseRepository;
            InstructorRepository = instructorRepository;
            DepartmentRepository = departmentRepository;
            UserManager = userManager;
            RoleManager = roleManager;
        }

        public IActionResult Index()
        {
            List<Instructor> instructors = InstructorRepository.Load();

            return View(instructors);

        }

        public IActionResult GetInstructor(int id)
        {
            List<Instructor> instructors = InstructorRepository.Load();

            var instructor = InstructorRepository.Get(id);

            return View(instructor);
        }

        public IActionResult Edit(int id)
        {
            List<Instructor> instructors = InstructorRepository.Load();
            var depts = DepartmentRepository.LoadDeferred();
            
            var instructor = InstructorRepository.Get(id);
            ViewBag.Departments = depts
                     .Select(i => new SelectListItem
                     {
                         Value = i.Id.ToString(),
                         Text = i.Name
                     }).ToList();

            
            var file = ImageService.ConvertToIFormFile(instructor.Image); 
            var instructorFromUsers = UserManager.FindByNameAsync(instructor.Name).Result;
            


            var InstructorToEdit = new EditInstructorViewModel
            {
                Name = instructor.Name,
                Image = file,
                Salary = instructor.Salary,
                Address = instructor.Address,
                Email = instructorFromUsers.Email
            };
            
            
            return View(InstructorToEdit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save(Instructor newInstructor)
        {
            List<Instructor> instructors = InstructorRepository.Load();
            var oldInstructor = InstructorRepository.Get(newInstructor.Id);
            var selectedDept = DepartmentRepository.Get(newInstructor.DeptId);
            var selectedCourse = CourseRepository.Get(newInstructor.CourseId);

            oldInstructor.Department = selectedDept;
            oldInstructor.Course = selectedCourse;
            if (oldInstructor.Department is null)
                Console.WriteLine("The department is still null");
            else
                Console.WriteLine("The Department is not null YAYAYAYAYA");

            if (newInstructor is null)
                return RedirectToAction("Edit");

            if (newInstructor.Name == null || newInstructor.Image == null)
                return RedirectToAction("Edit");

            InstructorRepository.Update(oldInstructor, newInstructor);
            //InstructorRepository.Update(newInstructor.Id, oldInstructor);

            TempData["success-update"] = true;
            return RedirectToAction("Index");
        }


        public IActionResult addInstructor()
        {
            List<Course> courses = CourseRepository.Load();
            List<Department> departments = DepartmentRepository.Load();

            var instructorVm = new InstructorViewModel();
            instructorVm.courses = courses;
            instructorVm.departments = departments;

            return View(instructorVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SuccessAdd(InstructorViewModel newInstructorvm)
        {

            //store image in directory before using it
            var saveLocation = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot\\images");
            ImageService.UploadImageToDirectory(newInstructorvm.Image, saveLocation,  newInstructorvm.Image.FileName);
            
            //adding instructor to instructors table
            var newInstructor = new Instructor
            {
                Id = newInstructorvm.Id,
                Name = newInstructorvm.Name,
                Image = newInstructorvm.Image.FileName,
                Salary = newInstructorvm.Salary,
                Address = newInstructorvm.Address,
                CourseId = newInstructorvm.CourseId,
                DeptId = newInstructorvm.DeptId,
            };


            var department = DepartmentRepository.Get(newInstructor.DeptId);
            newInstructor.Department = department;


            InstructorRepository.Insert(newInstructor);
            TempData["success"] = true;

            if (ModelState.IsValid)
            {
                var applicationUser = new ApplicationUser
                {
                    UserName = newInstructorvm.Name,
                    Address = newInstructorvm.Address,
                    PasswordHash = newInstructorvm.Password
                };

                var create = await UserManager.CreateAsync(applicationUser, applicationUser.PasswordHash);
                if(create.Succeeded)
                {
                    await UserManager.AddToRoleAsync(applicationUser, "Instructor");
                } else
                {
                    var errors = create.Errors;
                    foreach (var error in errors)
                        ModelState.AddModelError("IE", error.Description);
                }

               
                return RedirectToAction("Index");
            } else
            {
                ModelState.AddModelError("", "Something went wrong");
                return View("addinstructor", newInstructorvm);
            }
        }

        //creating action to show the courses for that depeartment
        public IActionResult ShowCoursesPerDept(int deptId)
        {
            var department = DepartmentRepository.Get(deptId);
            var allCourses = CourseRepository.Load();


            var itemList = allCourses.Where(c => c.DeptId == deptId).ToList();

            var ivm = new InstructorViewModel
            {
                itemList = itemList,
            };
            
            return PartialView("ShowCoursesPerDeptPartial",ivm);
        }

        public IActionResult ShowSuccess()
        {
            return View("_ShowSuccessPartial");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            InstructorRepository.Delete(id);
            return RedirectToAction("Index");
        }
    }
}

//goal - display the courses for the selected department using ajax