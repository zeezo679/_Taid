using System.ComponentModel.DataAnnotations;

namespace Demo.ViewModel
{
    public class RegisterViewModel
    {
        [Required]
        [MinLength(3)]
        public string Username { get; set; }

        //Add Email
        [EmailAddress]
        [Required]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        public string Address { get; set; }
    }
}
