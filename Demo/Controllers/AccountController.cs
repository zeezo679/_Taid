using Castle.Components.DictionaryAdapter.Xml;
using Demo.Models.Entities;
using Demo.Models.Interfaces;
using Demo.Services;
using Demo.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using NuGet.Packaging;
using System.Security.Claims;

namespace Demo.Controllers
{
    public class AccountController : Controller
    {
        public readonly UserManager<ApplicationUser> UserManager;
        public readonly RoleManager<IdentityRole> RoleManager;
        public readonly SignInManager<ApplicationUser> SignInManager;
        public readonly IAccountService AccountService;


        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IAccountService service
            )
        {
            UserManager = userManager;
            SignInManager = signInManager;
            RoleManager = roleManager;
            AccountService = service;
        }

        [HttpGet]
        public IActionResult Register() => View();
        

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel newUserVM)
        {
            if (newUserVM == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid request.");
                return View();
            }

            if (!ModelState.IsValid)
                return View(newUserVM);

            var result = await AccountService.RegisterAsync(newUserVM);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);
                return View(newUserVM);
            }

           
            return RedirectToAction(nameof(Login), "Account");
        }


        [HttpGet]
        public async Task<IActionResult> RegisterAdmin()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterAdmin(RegisterViewModel newAdminVM)
        {
            if(!ModelState.IsValid)
                return View(newAdminVM);

            ApplicationUser applicationUser = new ApplicationUser();
            applicationUser.UserName = newAdminVM.Username;
            applicationUser.Address = newAdminVM.Address;
            applicationUser.PasswordHash = newAdminVM.Password;
            applicationUser.Email = newAdminVM.Email;

            IdentityResult result = await UserManager.CreateAsync(applicationUser, newAdminVM.Password);

            if(!result.Succeeded)
            {
                var errors = result.Errors;

                foreach (var errorItem in errors)
                    ModelState.AddModelError("Passowrd", errorItem.Description);
            }


            //create cookie that has id, name, role ONLY
            await UserManager.AddToRoleAsync(applicationUser, "Admin");
            await SignInManager.SignInAsync(applicationUser, true);


            return RedirectToAction("Login", "Account");
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            return View(new LoginViewModel { ReturnUrl = returnUrl});
        }

        [HttpPost]
        [ValidateAntiForgeryToken] //!!!!!!!! issue here the returnUrl is null which makes the ModelState invalid
        public async Task<IActionResult> Login(LoginViewModel loggedUser, string? returnUrl = "/home/index")
        {
            if(!ModelState.IsValid)
                return View(loggedUser);


            //check if user is in db
            ApplicationUser userRowFromDB = await UserManager.FindByNameAsync(loggedUser.Username);

            if (userRowFromDB is null || !await UserManager.CheckPasswordAsync(userRowFromDB, loggedUser.Password))
            {
                ModelState.AddModelError("", "Username or Password is incorrect.");
                return View(loggedUser);
            }

            await SignInManager.SignInAsync(userRowFromDB, loggedUser.RememberMe);
            
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> ExternalLogin(string provider, string? returnUrl = null)
        {
            var redirectUrl = Url.Action(
                action: "ExternalLoginCallBack",
                controller: "Account",
                values: new { returnUrl }
            );

            var props = AccountService.ConfigureExternalLogin(provider, redirectUrl);

            return new ChallengeResult(provider,props);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> ExternalLoginCallBack(string returnUrl, string? remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            if(remoteError != null)
            {
                ModelState.AddModelError(string.Empty, "Error from external provider");
                return RedirectToAction("login");
            }

            var info = await AccountService.GetExternalLoginInfoAsync();

            if(info == null)
            {
                ModelState.AddModelError(string.Empty, "The Provider Failed to load external information");
                return RedirectToAction("login");
            }

            //attempt to sign in the user
            var signInResult = await AccountService.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPresistent: false);

            if(signInResult.Succeeded)
            {
                return Redirect(returnUrl ?? "/"); //this returns the user directly to the action they came from
            }

            var createResult = await AccountService.CreateExternalUserAsync(info);
            if(createResult.Succeeded)
                return Redirect(returnUrl ?? "/"); //this returns the user directly to the action they came from

            foreach(var error in createResult.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View();
        }
        public async Task<IActionResult> Logout()
        {
            await SignInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }
    }
}
