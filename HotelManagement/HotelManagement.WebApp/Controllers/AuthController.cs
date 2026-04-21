//using HotelManagement.WebApp.Domain.Models;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;

//[Route("auth")]
//public class AuthController : Controller
//{
//    private readonly SignInManager<ApplicationEmployee> _signInManager;

//    public AuthController(SignInManager<ApplicationEmployee> signInManager)
//    {
//        _signInManager = signInManager;
//    }

//    [HttpGet("login")]
//    public IActionResult Login(string? returnUrl = null)
//    {
//        ViewBag.ReturnUrl = returnUrl;
//        return View();
//    }

//    [HttpPost("login")]
//    public async Task<IActionResult> Login(string username, string password, string? returnUrl = null)
//    {
//        var result = await _signInManager.PasswordSignInAsync(
//            username, password, false, lockoutOnFailure: false);

//        if (result.Succeeded)
//            return Redirect(returnUrl ?? "/");

//        ModelState.AddModelError("", "Invalid login attempt");
//        return View();
//    }

//    [HttpPost("logout")]
//    public async Task<IActionResult> Logout()
//    {
//        await _signInManager.SignOutAsync();
//        return Redirect("/");
//    }
//}
