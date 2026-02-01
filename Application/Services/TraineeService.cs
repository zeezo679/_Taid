using Core.Models.Interfaces.Trainee;
using Microsoft.AspNetCore.Identity;
using Web.Models.Entities;
using Web.Models.Interfaces;
using Web.ViewModel;

namespace Services;

public class TraineeService : ITraineeService
{
    private ITraineeRepository _traineeRepository;
    private IDepartmentRepository _departmentRepository;
    private UserManager<ApplicationUser> _userManager;

    
    public TraineeService(ITraineeRepository traineeRepository, IDepartmentRepository departmentRepository, UserManager<ApplicationUser> userManager) 
    {
        _traineeRepository = traineeRepository;
        _departmentRepository = departmentRepository;
        _userManager = userManager;
    }
    
    
    public async Task<bool> AddTraineeAsync(TraineeViewModel  newTrainee)
    {
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

        if (!createResult.Succeeded)
            return false;
            
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

        return true;
    }
}