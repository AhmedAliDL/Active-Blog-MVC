using Active_Blog_Service.Models;
using Active_Blog_Service.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace Active_Blog_Service.Services.Contracts
{
    public interface IRoleService : IScopedServiceMarker
    {
        Task<List<RoleViewModel>> GetAllRolesAsync();
        Task<IdentityResult> CreateRoleAsync(RoleViewModel roleViewModel);
        Task<IdentityResult> AssignRoleToUserAsync(User user, string roleName);
    }
}
