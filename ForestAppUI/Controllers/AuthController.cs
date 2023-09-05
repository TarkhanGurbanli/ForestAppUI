using ForestAppUI.Dtos;
using ForestAppUI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;

namespace ForestAppUI.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
    

        public AuthController(UserManager<User> userManager, SignInManager<User> signInManager)
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
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
            return View();
            }

            var checkEmail = await _userManager.FindByEmailAsync(loginDto.Email);

            if(checkEmail == null)
            {
                ModelState.AddModelError("Error", "Email or Password is not valid!");
                return View();
            }

            Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(checkEmail, loginDto.Password, loginDto.RememberMe,true);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("Error", "Email or Password is not valid!");
                return View();
            }
            return RedirectToAction("Index", "Home");
        }


        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var checkEmail = await _userManager.FindByEmailAsync(registerDto.Email);
            if (checkEmail != null)
            {
                ModelState.AddModelError("Error", "Email is exist!");
                return View();
            }

            User newUser = new()
            {
                FirstName = registerDto.Firstname,
                LastName = registerDto.Lastname,
                Email = registerDto.Email,
                UserName = registerDto.Firstname + registerDto.Lastname,
                PhotoUrl = "/"
                
            };

            var result = await _userManager.CreateAsync(newUser, registerDto.Password);

            if (result.Succeeded)
            {

                await _signInManager.SignInAsync(newUser, isPersistent: true);
                return RedirectToAction("Index", "Home");

            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("Error", error.Description);
                }
            }
            return View(registerDto);
        }


        [HttpPost]
        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

    }
}
