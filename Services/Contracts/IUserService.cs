using Active_Blog_Service.Models;

namespace Active_Blog_Service.Services.Contracts
{
    public interface IUserService : IScopedServiceMarker
    {
        Task<int> TotalUsersCountAsync();
        Task<User> GetUserByIdAsync(string userId);
        Task<User> GetUserByEmailAsync(string email);
        Task<User> GetUserByBlogIdAsync(int blogId);
        Task<List<User>> GetAllUsersBlogsAsync();
        bool CheckFoundOfEmail(string email);
    }
}
