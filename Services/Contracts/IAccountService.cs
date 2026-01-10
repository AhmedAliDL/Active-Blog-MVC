using Active_Blog_Service.ViewModels;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Active_Blog_Service.Services.Contracts
{
    public interface IAccountService : IScopedServiceMarker
    {
        Task<Tuple<IdentityResult,RegisterViewModel>> RegisterServiceAsync(RegisterViewModel registerUserViewModel);
        Task<Tuple<bool, Dictionary<string, string>>> LoginServiceAsync(LoginViewModel loginUserViewModel);
        Task<IdentityResult> EditServiceAsync(ClaimsPrincipal user, AccountEditViewModel accountEditViewModel);
        Task<bool> LogoutServiceAsync();
    }
}