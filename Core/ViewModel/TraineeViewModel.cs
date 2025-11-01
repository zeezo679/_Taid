using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Web.Models.Entities;
using Web.Models.Interfaces;

namespace Web.ViewModel
{
    public class TraineeViewModel : IDeletable
    {
        //Trainee
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        
        [EmailAddress]
        public string Email { get; set; }
        
        [DataType(DataType.Password)]
        public string Password { get; set; }
        
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        public IFormFile Image { get; set; }
        public string Address { get; set; } = null!;
        public decimal Grade { get; set; }
        public int DeptId { get; set; }

        //Extra
        public List<Department> departments { get; set; } = new();
        //SelectList departList;
    }
}
