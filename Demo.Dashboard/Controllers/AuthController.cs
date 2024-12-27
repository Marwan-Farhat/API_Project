using Demo.Core.Domain.Entities.Identity;
using Demo.Shared.Models.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Dashboard.Controllers
{
    public class AuthController(SignInManager<ApplicationUser> _signInManager, UserManager<ApplicationUser> _userManager) : Controller
	{
		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
        public async Task<IActionResult> Login(LoginDto login)
        {
			var user = await _userManager.FindByEmailAsync(login.Email);
			if(user == null) 
			{
				ModelState.AddModelError("Email", "Invalid Login");
				return RedirectToAction(nameof(Login));
			}

			var result = await _signInManager.CheckPasswordSignInAsync(user, login.Password, false);
			if(!result.Succeeded || !await _userManager.IsInRoleAsync(user,"Admin"))
			{
				ModelState.AddModelError(string.Empty,"You are not authorized");
                return RedirectToAction(nameof(Login));
            }
			else
               return RedirectToAction("Index","Home");  // If Succeeded will go to our website
        }

        public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction(nameof(Login));
		}
    }
}
