using Demo.Models.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Demo.Models.Entities
{
    public class Trainee : IDeletable
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Image { get; set; }
        public string Address { get; set; } = null!;
        public decimal Grade { get; set; }

        [ForeignKey("Department")]
        [DisplayName("Department")]
        public int DeptId { get; set; }
        public virtual Department Department { get; set; } = null!;

        //Added navigation properties with the Identity Users so i can navigate between the Users and assign UID to trainee (we may need the UID for future purposes)
        public string UserId { get; set; } = null!;
        public ApplicationUser User { get; set; }
        public ICollection<CrsResult> CrsResults { get; set; } = new List<CrsResult>();

    }
}
