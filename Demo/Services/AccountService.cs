using System;
using System.Security.Claims;
using Demo.Models.Entities;
using Demo.Models.Interfaces;
using Demo.ViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Client;

namespace Demo.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> UserManager;
        private readonly RoleManager<IdentityRole> RoleManager;
        private readonly SignInManager<ApplicationUser> SignInManager;
        public AccountService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager
        )
        {
            UserManager = userManager;
            SignInManager = signInManager;
            RoleManager = roleManager;
        }

        public AuthenticationProperties ConfigureExternalLogin(string provider, string redirectUrl)
        {
            return SignInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        }

        public async Task<IdentityResult> CreateExternalUserAsync(ExternalLoginInfo info)
        {
            // Extract email claim (mandatory for identifying the user)
            var email = info.Principal.FindFirstValue(ClaimTypes.Email);

            // Verify email , return identity error if null
            if (string.IsNullOrEmpty(email))
                return IdentityResult.Failed(new IdentityError { Code = "nullEmail", Description = "Email Doesnt Exists in Claims" });
            
            // Check if user with this email already exists
            var user = await UserManager.FindByEmailAsync(email);

            // if user already exists , link him with external provider
            if(user != null)
            {
                // Update the login state of the user and sign him
                var addLoginEntry = await UserManager.AddLoginAsync(user, info);

                if(!addLoginEntry.Succeeded)
                    return addLoginEntry;
                
                await SignInManager.SignInAsync(user, isPersistent: false);

                return IdentityResult.Success;
            }

            // if user is new make an entry in the logins and aspnetusers table

            ApplicationUser appUser = new ApplicationUser
            {
                UserName = email.Substring(0, email.IndexOf("@")),
                Email = email,
                Address = "No Address",
                EmailConfirmed = true,
            };

            var result = await UserManager.CreateAsync(appUser);
            if (!result.Succeeded)
                return result;

            var addLoginResult = await UserManager.AddLoginAsync(appUser, info);
            if (!addLoginResult.Succeeded)
                return addLoginResult;
            
            await SignInManager.SignInAsync(appUser,isPersistent: false);
            return IdentityResult.Success;
            
        }

        public async Task<SignInResult> ExternalLoginSignInAsync(string loginProvider, string providerKey, bool isPresistent)
        {
            return await SignInManager.ExternalLoginSignInAsync(loginProvider, providerKey, isPresistent);
        }

        public async Task<ExternalLoginInfo> GetExternalLoginInfoAsync()
        {
            return await SignInManager.GetExternalLoginInfoAsync();
        }

        public ApplicationUser MapUser(RegisterViewModel newUserVM)
        {
            ApplicationUser applicationUser = new ApplicationUser();
            applicationUser.UserName = newUserVM.Username;
            applicationUser.Address = newUserVM.Address;
            applicationUser.PasswordHash = newUserVM.Password;
            applicationUser.Email = newUserVM.Email;

            return applicationUser;
        }

        public async Task<IdentityResult> RegisterAsync(RegisterViewModel new_user)
        {
            var appUser =  MapUser(new_user);

            IdentityResult result = await UserManager.CreateAsync(appUser, new_user.Password);

            return result;
        }
    }
}
