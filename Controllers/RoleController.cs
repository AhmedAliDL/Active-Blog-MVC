using Active_Blog_Service.Services.Contracts;
using Active_Blog_Service.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Active_Blog_Service.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly IRoleService _roleService;
        private readonly IUserService _userService;
        public RoleController(IRoleService roleService, IUserService userService)
        {
            _roleService = roleService;
            _userService = userService;
        }
        public async Task<IActionResult> Index()
        {
            var rolesViewModel = await _roleService.GetAllRolesAsync();

            return View(rolesViewModel);
        }
        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(RoleViewModel roleVM)
        {
            if (!ModelState.IsValid)
            {
                return View(roleVM);
            }
            var result = await _roleService.CreateRoleAsync(roleVM);

            if (result.Succeeded)
                return RedirectToAction("Index");
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(roleVM);
        }
        [HttpGet]
        public async Task<IActionResult> AssignRole()
        {
           List<RoleViewModel> roles = await _roleService.GetAllRolesAsync();
           ViewBag.ApplicationRoles = roles;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AssignRole(AssignRoleViewModel assignRoleViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(assignRoleViewModel);
            }
            var user = await _userService.GetUserByEmailAsync(assignRoleViewModel.UserEmail);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty,"User not found.");
                return View(assignRoleViewModel);
            }
            var result = await _roleService.AssignRoleToUserAsync(user, assignRoleViewModel.RoleName);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(assignRoleViewModel);
        }
    }
}
