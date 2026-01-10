using Active_Blog_Service.ViewModels;
using System.Security.Claims;

namespace Active_Blog_Service.Services.Contracts
{
    public interface IContactService : IScopedServiceMarker
    {
        Task<object> SendEmailServiceAsync(ClaimsPrincipal user, SendMailViewModel sendMailViewModel);
    }
}