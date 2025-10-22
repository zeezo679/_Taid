using Demo.Models.Entities;
using Demo.Models.Interfaces;
using Demo.Models.Repository;
using Demo.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using Demo.Services;
using Microsoft.EntityFrameworkCore;

namespace Demo.Controllers
{
    public class TraineeController : Controller
    {
        private ITraineeRepository _traineeRepository;
        private IDepartmentRepository _departmentRepository;
        private ICrsResultRepository _crsResultRepository;
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private IAccountService _accountService;
        public TraineeController(SignInManager<ApplicationUser> signInManager,IAccountService accountService ,ITraineeRepository traineeRepository, IDepartmentRepository departmentRepository, ICrsResultRepository crsResultRepository, UserManager<ApplicationUser> userManager) 
        {
            _traineeRepository = traineeRepository;
            _departmentRepository = departmentRepository;
            _crsResultRepository = crsResultRepository;
            _userManager = userManager;
            _accountService = accountService;   
            _signInManager = signInManager;
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


            Department dept = _departmentRepository.Get(newTrainee.DeptId);
            
            var applicationUser = new ApplicationUser
            {
                UserName = newTrainee.Name,
                Address = newTrainee.Address,
                Email =  newTrainee.Email,
                PasswordHash = newTrainee.Password
            };

            //adding trainee to aspnetusers
            var createResult = await  _userManager.CreateAsync(applicationUser, newTrainee.Password);
            
            if(!createResult.Succeeded)
                return RedirectToAction("AddTrainee");
            
            await _userManager.AddToRoleAsync(applicationUser, "Trainee");

            //store image in directory before using it
            var saveLocation = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot\\images");
            ImageService.UploadImageToDirectory(newTrainee.Image, saveLocation, newTrainee.Image.FileName);
            
            //adding trainee to trainees table
            Trainee trainee = new Trainee
            {
                Name = newTrainee.Name,
                Image = newTrainee.Image.FileName,
                Address = newTrainee.Address,
                Grade = newTrainee.Grade,
                Department = dept,
                UserId = applicationUser.Id,
            };


            _traineeRepository.Insert(trainee);


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


        //[HttpGet]
        //public IActionResult Delete(Trainee trainee)
        //{

        //    return PartialView("_ConfirmDeletePartial",trainee);
        //}


        


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

//ui problems in trainee index