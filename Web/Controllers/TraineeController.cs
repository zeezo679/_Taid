using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using Core.Models.Interfaces.Trainee;
using Services;
using Microsoft.EntityFrameworkCore;
using Web.Models.Entities;
using Web.Models.Interfaces;
using Web.ViewModel;

namespace Web.Controllers
{
    public class TraineeController : Controller
    {
        private ITraineeRepository _traineeRepository;
        private IDepartmentRepository _departmentRepository;
        private ICrsResultRepository _crsResultRepository;
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private IAccountService _accountService;
        private readonly ITraineeService _traineeService;
        public TraineeController(ITraineeService traineeService,SignInManager<ApplicationUser> signInManager,IAccountService accountService ,ITraineeRepository traineeRepository, IDepartmentRepository departmentRepository, ICrsResultRepository crsResultRepository, UserManager<ApplicationUser> userManager) 
        {
            _traineeRepository = traineeRepository;
            _departmentRepository = departmentRepository;
            _crsResultRepository = crsResultRepository;
            _userManager = userManager;
            _accountService = accountService;   
            _signInManager = signInManager;
            _traineeService = traineeService;
        }


        [HttpGet]
        public IActionResult Index()
        {
            List<Trainee> trainees = _traineeRepository.Load(true);

            return View(trainees);
        }

        [HttpGet]
        public IActionResult AddTrainee()
        {
            var departments = _departmentRepository.Load();

            var traineeVm = new TraineeViewModel();
            traineeVm.departments = departments;

            return View(traineeVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTrainee(TraineeViewModel newTrainee)
        {

            if(!ModelState.IsValid)
                return RedirectToAction("AddTrainee");

            //Add trainee service must handle the logic
            var result = await _traineeService.AddTraineeAsync(newTrainee);   
            
            if(!result)
                return RedirectToAction("AddTrainee");

            return RedirectToAction("Index");
                
        }

        [HttpGet]
        public IActionResult Edit(int id){

            Trainee trainee = _traineeRepository.Get(id);
            List<Department> departments = _departmentRepository.Load();

            if (trainee == null || departments == null)
                ModelState.AddModelError("", "Trainee or Departments Not Found in Database");
            
            var file = ImageService.ConvertToIFormFile(trainee.Image); 
            var traineeFromUsers = _userManager.FindByIdAsync(trainee.UserId).Result;
            
            TraineeViewModel traineeVM = new TraineeViewModel
            {
                Name = trainee.Name,
                Image = file,
                Email = traineeFromUsers.Email,
                Address = trainee.Address,
                Grade = trainee.Grade,
                departments = departments
            };

            // Pass current image filename to ViewBag so it can be displayed
            ViewBag.CurrentImage = trainee.Image;

            //dont forget to check for errors in the view
            return View(traineeVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, TraineeViewModel newTrainee)
        {
            ModelState.Remove("Password");
            ModelState.Remove("ConfirmPassword");   
            if(ModelState.IsValid)
            {
                Department traineeDept = _departmentRepository.Get(newTrainee.DeptId);

                var saveLocation = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot\\images");
                ImageService.UploadImageToDirectory(newTrainee.Image, saveLocation, newTrainee.Image.FileName); //TODO: Dont forget to see if file exists already or no
                
                Trainee trainee = new Trainee
                {
                    Name = newTrainee.Name,
                    Image = newTrainee.Image.FileName,
                    Address = newTrainee.Address,
                    Grade = newTrainee.Grade,
                    Department = traineeDept,
                };

                _traineeRepository.Update(id, trainee);

                TempData["edit_success"] = true;    
                return RedirectToAction("Index");
            }
            else
                return RedirectToAction("Edit");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var trainee = _traineeRepository.Get(id);
            var uid = trainee.UserId;
            var appUser  = _userManager.FindByIdAsync(uid).Result;
            
            if(appUser is null || trainee is null)
                throw new Exception("Trainee or user not found");
            
            await _userManager.DeleteAsync(appUser);  
            return RedirectToAction("Index");
        }
    }
}

