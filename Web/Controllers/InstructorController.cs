using Web.Infrastructure;
using Web.Models.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Web.Models.Entities;
using Web.Models.Interfaces;
using Web.ViewModel;
using Services;

namespace Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class InstructorController : Controller
    {

        private ICourseRepository CourseRepository;
        private IInstructorRepository InstructorRepository;
        private IDepartmentRepository DepartmentRepository;
        private UserManager<ApplicationUser> UserManager;
        private RoleManager<IdentityRole> RoleManager;
        private AppDbContext _context;

        public InstructorController(
            ICourseRepository courseRepository, 
            IInstructorRepository instructorRepository,
            IDepartmentRepository departmentRepository,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            AppDbContext context
            )
        {
            CourseRepository = courseRepository;
            InstructorRepository = instructorRepository;
            DepartmentRepository = departmentRepository;
            UserManager = userManager;
            RoleManager = roleManager;
            _context = context;
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
                         Text = i.Name,
                         Selected = i.Id == instructor.DeptId // Set selected department
                     }).ToList();

            
            var file = ImageService.ConvertToIFormFile(instructor.Image); 
            var instructorFromUsers = UserManager.FindByNameAsync(instructor.Name).Result;
            

            var InstructorToEdit = new EditInstructorViewModel
            {
                Id = instructor.Id,
                Name = instructor.Name,
                Image = file,
                Email = instructor.Email,
                Salary = instructor.Salary,
                Address = instructor.Address,
                DeptId = instructor.DeptId,
                CourseId = instructor.CourseId
            };
            
            // Pass current image filename to ViewBag so it can be displayed
            ViewBag.CurrentImage = instructor.Image;
            
            return View(InstructorToEdit);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save(EditInstructorViewModel model)
        {


            if (ModelState.IsValid)
            {
                var oldInstructor = InstructorRepository.Get(model.Id);
                
                if (oldInstructor == null)
                {
                    ModelState.AddModelError("", "Instructor not found");
                    return View("Edit", model);
                }

                if (model.Image != null && model.Image.Length > 0)
                {
                    var saveLocation = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot\\images");
                    ImageService.UploadImageToDirectory(model.Image, saveLocation, model.Image.FileName);
                    oldInstructor.Image = model.Image.FileName;
                }

                oldInstructor.Name = model.Name;
                oldInstructor.Salary = model.Salary;
                oldInstructor.Address = model.Address;
                oldInstructor.DeptId = model.DeptId;
                oldInstructor.CourseId = model.CourseId;
                oldInstructor.Email = model.Email;
                
                var department = DepartmentRepository.Get(model.DeptId);
                oldInstructor.Department = department;

                InstructorRepository.Update(model.Id, oldInstructor);

                TempData["edit_success"] = true;
                return RedirectToAction("Index");
            }
            
            var depts = DepartmentRepository.LoadDeferred();
            ViewBag.Departments = depts
                .Select(i => new SelectListItem
                {
                    Value = i.Id.ToString(),
                    Text = i.Name,
                    Selected = i.Id == model.DeptId
                }).ToList();
            
            return View("Edit", model);
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

            if (ModelState.IsValid)
            {
                var applicationUser = new ApplicationUser
                {
                    UserName = newInstructorvm.Name,
                    Address = newInstructorvm.Address,
                    PasswordHash = newInstructorvm.Password,
                    Email = newInstructorvm.Email,
                    Image = newInstructorvm.Image?.FileName
                };

                var create = await UserManager.CreateAsync(applicationUser, applicationUser.PasswordHash);
                if (create.Succeeded)
                {
                    await UserManager.AddToRoleAsync(applicationUser, "Instructor");
                }
                else
                {
                    var errors = create.Errors;
                    foreach (var error in errors)
                        ModelState.AddModelError("IE", error.Description);
                }

                //store image in directory before using it
                var saveLocation = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot\\images");
                ImageService.UploadImageToDirectory(newInstructorvm.Image, saveLocation, newInstructorvm.Image.FileName);

                //adding instructor to instructors table
                var newInstructor = new Instructor
                {
                    Id = newInstructorvm.Id,
                    Name = newInstructorvm.Name,
                    Image = newInstructorvm.Image.FileName,
                    Salary = newInstructorvm.Salary,
                    Email = newInstructorvm.Email,
                    Address = newInstructorvm.Address,
                    CourseId = newInstructorvm.CourseId,
                    DeptId = newInstructorvm.DeptId,
                    User = applicationUser, 
                };


                var department = DepartmentRepository.Get(newInstructor.DeptId);
                newInstructor.Department = department;


                InstructorRepository.Insert(newInstructor);
                TempData["success"] = true;

                return RedirectToAction("Index");
            }
            else
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
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var instructor = InstructorRepository.Get(id);
            var UID = instructor.UserId;
            var appUser = await UserManager.FindByIdAsync(UID);
            
            InstructorRepository.Delete(id);
            

            await UserManager.DeleteAsync(appUser);
            return RedirectToAction("Index");
        }
    }
}

//goal - display the courses for the selected department using ajax