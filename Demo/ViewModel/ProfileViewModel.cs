using System.ComponentModel.DataAnnotations;
using Demo.Models.Entities;

namespace Demo.ViewModel;

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
}