using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Demo.Models.Entities
{
    public class ApplicationUser:IdentityUser
    {
        [Required]
        public string Address { get; set; }
        public  DateTime? RegistrationDate { get; set; }

        [MaxLength(50)]
        public string? Image { get; set; }
    }
}
