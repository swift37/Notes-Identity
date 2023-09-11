using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Notes.Identity.Models;

namespace Notes.Identity.Controllers
{
    public class AuthController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IIdentityServerInteractionService _interactionService;

        public AuthController(
            SignInManager<AppUser> signInManager, 
            UserManager<AppUser> userManager,
            IIdentityServerInteractionService interactionService) => 
            (_signInManager, _userManager, _interactionService) = 
            (signInManager, userManager, interactionService);

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            var model = new LoginViewModel { ReturnUrl = returnUrl };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid || 
                string.IsNullOrEmpty(model.Username) || 
                string.IsNullOrEmpty(model.Password) ||
                string.IsNullOrEmpty(model.ReturnUrl))
                return View(model);

            var user = await _userManager.FindByNameAsync(model.Username);
            if (user is null)
            {
                ModelState.AddModelError(string.Empty, "User not found");
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, false, false);
            if (result.Succeeded)
                return Redirect(model.ReturnUrl);

            ModelState.AddModelError(string.Empty, "Login error");

            return View(model);
        }

        [HttpGet]
        public IActionResult Register(string returnUrl)
        {
            var model = new RegisterViewModel { ReturnUrl = returnUrl };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid ||
                string.IsNullOrEmpty(model.Username) ||
                string.IsNullOrEmpty(model.Password) ||
                string.IsNullOrEmpty(model.ConfirmPassword) ||
                string.IsNullOrEmpty(model.ReturnUrl)) 
                return View(model);

            var user = new AppUser { UserName = model.Username };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, false);
                return Redirect(model.ReturnUrl);
            }

            ModelState.AddModelError(string.Empty, "Error occured");
            return View(model);
        }

        public async Task<IActionResult> Logout(string logoutId)
        {
            await _signInManager.SignOutAsync();
            var logoutRequest = await _interactionService.GetLogoutContextAsync(logoutId);
            return RedirectToAction(logoutRequest.PostLogoutRedirectUri);
        }
    }
}
