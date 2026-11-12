using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVCproject.Models;

namespace MVCproject.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _singInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> singInManager)
        {
            _userManager = userManager;
            _singInManager = singInManager;
        }

        [HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }
        // SignUp Registro nuevo usuario
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

    }
}
