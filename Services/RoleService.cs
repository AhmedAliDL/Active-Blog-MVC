using Active_Blog_Service.Models;
using Active_Blog_Service.Services.Contracts;
using Active_Blog_Service.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Active_Blog_Service.Services
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<User> _userManager;
        public RoleService(RoleManager<IdentityRole> roleManager, UserManager<User> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }
        public async Task<List<RoleViewModel>> GetAllRolesAsync()
        {
            List<RoleViewModel> roleViewModels = await _roleManager.Roles.Select(role => new RoleViewModel
            {
                RoleName = role.Name!,
                RoleDescription = role.NormalizedName!
            }).ToListAsync();

            return roleViewModels;

        }
        public async Task<IdentityResult> CreateRoleAsync(RoleViewModel roleViewModel)
        {
            var roleExists = await _roleManager.RoleExistsAsync(roleViewModel.RoleName);
            if (roleExists)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Description = $"Role '{roleViewModel.RoleName}' already exists."
                });
            }
            var role = new IdentityRole
            {
                Name = roleViewModel.RoleName,
                NormalizedName = roleViewModel.RoleDescription
            };
            var result = await _roleManager.CreateAsync(role);
            return result;
        }
        public async Task<IdentityResult> AssignRoleToUserAsync(User user, string roleName)
        {
            var result = await _userManager.AddToRoleAsync(user, roleName);
            return result;
        }

    }
}
