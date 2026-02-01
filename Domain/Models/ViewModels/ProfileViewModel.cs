using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using Web.Models.Entities;

namespace Web.ViewModel;

public class ProfileViewModel
{
    [Display(Name = "Username")]
    public string? UserName {get; set;}
    
    [Display(Name = "Email")]
    public string? Email {get; set;}
    
    [Display(Name = "Profile Image")]
    public IFormFile? Image { get; set; }
    
    [Display(Name = "Member Since")]
    public DateTime? JoinDate { get; set; }
    
    public List<CrsResult> EnrolledCourses { get; set; } = new List<CrsResult>();
    public List<Course> AssignedCourses { get; set; } = new List<Course>();
}