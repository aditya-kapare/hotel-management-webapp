using HotelManagement.WebApp.Domain.Models;
using HotelManagement.WebApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HotelManagement.WebApp.Presentation.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationEmployee> _userManager;
        private readonly SignInManager<ApplicationEmployee> _signInManager;

        public AccountController(
            UserManager<ApplicationEmployee> userManager,
            SignInManager<ApplicationEmployee> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // ✅ Find user in DB
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid credentials");
                return View(model);
            }

            // ✅ Check hashed password
            var result = await _signInManager.PasswordSignInAsync(
                user,
                model.Password,
                isPersistent: false,
                lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Invalid credentials");
                return View(model);
            }

            // ✅ Role-based redirection
            if (await _userManager.IsInRoleAsync(user, "Admin"))
                return Redirect("/admin");

            if (await _userManager.IsInRoleAsync(user, "Receptionist"))
                return Redirect("/reception/home");

            // ❌ Unauthorized role
            await _signInManager.SignOutAsync();
            ModelState.AddModelError(string.Empty, "Unauthorized role");
            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            // ✅ Redirect to actual home page
            return RedirectToAction("Index", "Home");
        }

    }
}