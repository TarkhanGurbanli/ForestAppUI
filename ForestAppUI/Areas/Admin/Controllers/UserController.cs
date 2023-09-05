using ForestAppUI.Areas.Admin.ViewModels;
using ForestAppUI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ForestAppUI.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    public class UserController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            var users = _userManager.Users.ToList();
            return View(users);
        }

        public async Task<IActionResult> AddRole(string id)
        {
            if (id == null) return NotFound();

            User user = await _userManager.FindByIdAsync(id);

            if (user == null) return NotFound();
            var userRoles = (await _userManager.GetRolesAsync(user)).ToList();
            var roles = _roleManager.Roles.Select(x => x.Name).ToList();

            UserRoleVM userRoleVM = new()
            {
                User = user,
                Roles = roles.Except(userRoles)
            };
            return View(userRoleVM);
        }

        [HttpPost]
        public async Task<IActionResult> AddRole(string id, string role)
        {
          

            try
            {
                if (id == null) return NotFound();
                User user = await _userManager.FindByIdAsync(id);
                if (user == null) return NotFound();
                var userAddRole = await _userManager.AddToRoleAsync(user, role);


                if (!userAddRole.Succeeded)
                {
                    ModelState.AddModelError("Error", "Something went wromg");
                    return View();
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
        }

        public async Task<IActionResult> Edit(string userId)
        {
            if (userId == null) return NotFound();
            User user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            return View(user);
        }

        public async Task<IActionResult> Delete(string userId, string role)
        {
            if (userId == null || role == null) return NotFound();

            User user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            IdentityResult result = await _userManager.RemoveFromRoleAsync(user, role);

            if (!result.Succeeded)
            {
                ViewBag.Error = "Somthing went wromg!";
                return View();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
