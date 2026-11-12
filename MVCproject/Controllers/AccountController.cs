using Microsoft.AspNetCore.Authorization;
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
                return RedirectToAction("Login");
            }

            foreach (var error in result.Errors) 
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }

        // LogIn / Inicio de sesión 
        [HttpGet]
        [AllowAnonymous]
        public IActionResult LogIn(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
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

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return RedirectToAction(returnUrl);
            }
            //else
            //{
            //    return RedirectToAction("Index", "Home");
            //}

            return View(model);
        }

        // LogOut
        [HttpPost]
        public async Task<IActionResult> LogOut( string? returnUrl = null) 
        {
            await _singInManager.SignOutAsync();

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return RedirectToAction(returnUrl);

            return RedirectToAction("LogIn", "Account");
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
