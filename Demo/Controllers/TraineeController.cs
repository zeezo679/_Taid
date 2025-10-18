using Demo.Models.Entities;
using Demo.Models.Interfaces;
using Demo.Models.Repository;
using Demo.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Demo.Controllers
{
    public class TraineeController : Controller
    {
        private ITraineeRepository _traineeRepository;
        private IDepartmentRepository _departmentRepository;
        private ICrsResultRepository _crsResultRepository;
        private UserManager<ApplicationUser> _userManager;
        public TraineeController(ITraineeRepository traineeRepository, IDepartmentRepository departmentRepository, ICrsResultRepository crsResultRepository, UserManager<ApplicationUser> userManager) 
        {
            _traineeRepository = traineeRepository;
            _departmentRepository = departmentRepository;
            _crsResultRepository = crsResultRepository;
            _userManager = userManager;
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

            Trainee trainee = new Trainee
            {
                Name = newTrainee.Name,
                Image = newTrainee.Image,
                Address = newTrainee.Address,
                Grade = newTrainee.Grade,
                Department = dept,
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

            TraineeViewModel traineeVM = new TraineeViewModel
            {
                Name = trainee.Name,
                Image = trainee.Image,
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
            if(ModelState.IsValid)
            {
                Department traineeDept = _departmentRepository.Get(newTrainee.DeptId);

                Trainee trainee = new Trainee
                {
                    Name = newTrainee.Name,
                    Image = newTrainee.Image,
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
        public IActionResult DeleteConfirmed(int id)
        {
            _traineeRepository.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
