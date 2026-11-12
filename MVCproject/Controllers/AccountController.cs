using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVCproject.Models;

namespace MVCproject.Controllers
{
    public class AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> singInManager) : Controller
    {
        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly SignInManager<AppUser> _singInManager = singInManager;

        // SignUp / Registro nuevo usuario
        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(SignUpViewModel model) 
        {
            if (!ModelState.IsValid) 
            {
                return View(model);
            }

            var user = new AppUser
            {
                UserName = model.Name,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors) 
            {
                ModelState.AddModelError("", error.Description);
            }
            return View(model);

        }

        // LogIn / Inicio de sesión 
        [HttpGet]
        public IActionResult LogIn()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LogIn(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // Comprobamos que existe el usuario:
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                ModelState.AddModelError("", "No existe usuario con ese email");
                return View(model);
            }
            // verifico la contraseña
            var passwordValid = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!passwordValid) 
            {
                ModelState.AddModelError("", "Credenciales incorrectas");
                return View(model);
            }
            // SingIn con email y contraseña
            await _singInManager.SignInAsync(user, model.RememberMe);

            return RedirectToAction("Index", "Home");

        }

        // LogOut
        [HttpPost]
        public async Task<IActionResult> LogOut() 
        {
            await _singInManager.SignOutAsync();
            return RedirectToAction("LogIn", "Account");
        }
    }
}
