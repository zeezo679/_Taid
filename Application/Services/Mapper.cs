using System;
using Web.Models.Entities;
using Web.ViewModel;
using Core.Models.Interfaces;



namespace Services
{
    public class Mapper : IMapper
    {
        public ApplicationUser MapToUser(RegisterViewModel newUserVM)
        {
            ApplicationUser applicationUser = new ApplicationUser();
            applicationUser.UserName = newUserVM.Username;
            applicationUser.Address = newUserVM.Address;
            applicationUser.PasswordHash = newUserVM.Password;
            applicationUser.Email = newUserVM.Email;
            applicationUser.RegistrationDate = DateTime.Now;
            applicationUser.Image = null;

            return applicationUser;
        }
    }
}
