using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Services;
using Web.Models.Entities;
using Web.Models.Interfaces;
using Web.ViewModel;

namespace Web.Controllers;

public class ProfileController : Controller
{
    
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ICrsResultRepository _crsResultRepository;
    private readonly IInstructorRepository _instructorRepository;
    private readonly SignInManager<ApplicationUser> _signInManager;
    
    public ProfileController(IInstructorRepository instructorRepository,UserManager<ApplicationUser> userManager,  ICrsResultRepository crsResultRepository,  SignInManager<ApplicationUser> signInManager)
    {
        _instructorRepository = instructorRepository;
        _userManager = userManager;
        _crsResultRepository = crsResultRepository;
        _signInManager = signInManager;
    }
    
    // GET
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        
        var applicationUser = await _userManager.GetUserAsync(User);
        var uid = applicationUser!.Id;
        
        var enrolledCourses = await _crsResultRepository.FilterCoursesByCurrentUserAsync(uid);

        List<Course> assignedCourses = new List<Course>();
        if (User.IsInRole("Instructor"))
        {
            assignedCourses = await _instructorRepository.FilterCoursesByCurrentInstructorAsync(uid);
        }

        var imageFile = ImageService.ConvertToIFormFile(applicationUser.Image);
        
        var profileViewModel = new ProfileViewModel
        {
            UserName = User.Identity?.Name,
            JoinDate = applicationUser!.RegistrationDate,
            Email = applicationUser.Email,
            Image = imageFile,
            EnrolledCourses = enrolledCourses,
            //AssignedCourses = assignedCourses,
        };
        
        return View(profileViewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateProfile(ProfileViewModel profileViewModel)
    {
        //dont forget to get enrolledCourses again and assign in to the viewmodel
        var applicationUser = await _userManager.GetUserAsync(User);
        var uid = applicationUser!.Id;
        var enrolledCourses = await _crsResultRepository.FilterCoursesByCurrentUserAsync(uid);
         List<Course> assignedCourses = new List<Course>();
        if (User.IsInRole("Instructor"))
        {
            assignedCourses = await _instructorRepository.FilterCoursesByCurrentInstructorAsync(uid);
        }

        if (profileViewModel.Image is not null)
        {
            applicationUser.Image = profileViewModel.Image.FileName;
        }
       
        var saveLocation = Path.Combine(Directory.GetCurrentDirectory(), $"wwwroot\\images");
        if(profileViewModel.Image is not null)
            ImageService.UploadImageToDirectory(profileViewModel.Image, saveLocation,  profileViewModel.Image.FileName);
        
        var imageFile = ImageService.ConvertToIFormFile(applicationUser.Image);
        profileViewModel.Image = imageFile;
        
        
        
        // Update properties
        applicationUser.UserName = profileViewModel.UserName;
        applicationUser.Image = profileViewModel.Image?.FileName;

        // Save changes to database
        var result = await _userManager.UpdateAsync(applicationUser);

        if (!result.Succeeded)
        {
            // Handle errors
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            profileViewModel.EnrolledCourses = enrolledCourses;
            return View("Index", profileViewModel);
        }

        //refresh cookie for updates to appear
        await _signInManager.RefreshSignInAsync(applicationUser);
        return RedirectToAction("Index");
    }
}