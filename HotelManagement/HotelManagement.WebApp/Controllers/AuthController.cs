using HotelManagement.WebApp.Domain.Models;
using HotelManagement.WebApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

[Route("auth")]
public class AuthController : Controller
{
    private readonly UserManager<ApplicationEmployee> _userManager;
    private readonly SignInManager<ApplicationEmployee> _signInManager;

    public AuthController(
        UserManager<ApplicationEmployee> userManager,
        SignInManager<ApplicationEmployee> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    // GET: /auth/login
    [HttpGet("login")]
    public IActionResult Login(string? returnUrl = null)
    {
        ViewBag.ReturnUrl = returnUrl;
        return View(new LoginViewModel());
    }

    // POST: /auth/login
    [HttpPost("login")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
    {
        if (!ModelState.IsValid)
            return View(model);

        // ✅ Find user
        var user = await _userManager.FindByNameAsync(model.Username);
        if (user == null)
        {
            ModelState.AddModelError(string.Empty, "Invalid credentials");
            return View(model);
        }

        // ✅ Sign in
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

        // ✅ ROLE‑BASED REDIRECTION
        if (await _userManager.IsInRoleAsync(user, "Admin"))
            return Redirect("/admin");

        if (await _userManager.IsInRoleAsync(user, "Receptionist"))
            return Redirect("/reception");

        // ❌ Unauthorized role
        await _signInManager.SignOutAsync();
        ModelState.AddModelError(string.Empty, "Unauthorized role");
        return View(model);
    }

    // POST: /auth/logout
    [Authorize]
    [HttpPost("logout")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    [HttpGet("denied")]
    public IActionResult Denied()
    {
        return View();
    }
}