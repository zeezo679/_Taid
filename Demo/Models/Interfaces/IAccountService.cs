using System.Threading.Tasks;
using Demo.Models.Entities;
using Demo.ViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace Demo.Models.Interfaces
{
    public interface IAccountService
    {
        ApplicationUser MapUser(RegisterViewModel newUserVM);
        Task<IdentityResult> RegisterAsync(RegisterViewModel new_user);

        //Google Auth Methods
        AuthenticationProperties ConfigureExternalLogin(string provider, string redirectUrl);
        Task<ExternalLoginInfo> GetExternalLoginInfoAsync();
        Task<SignInResult> ExternalLoginSignInAsync(
            string loginProvider, 
            string providerKey,
            bool isPresistent
        );

        Task<IdentityResult> CreateExternalUserAsync(ExternalLoginInfo info);
    }
}