using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ForestAppUI.Areas.Admin.Controllers
{
    [Area(nameof(Admin))]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            var roles = _roleManager.Roles.ToList();
            return View(roles);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Create(IdentityRole identityRole)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }

                var checkRole = await _roleManager.FindByNameAsync(identityRole.Name);

                if (checkRole != null)
                {
                    ViewBag.Error = "The role name is exist";
                    return View();
                }

                await _roleManager.CreateAsync(identityRole);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
        }
    }
}
