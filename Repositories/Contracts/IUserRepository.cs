using Active_Blog_Service.Models;

namespace Active_Blog_Service.Repositories.Contracts
{
    public interface IUserRepository : IScopedRepositoryMarker
    {
        Task<int> CountOfUsersAsync { get; }
        Task<User> GetUserByIdAsync(string userId);
        Task<User> GetUserByEmailAsync(string email);
        Task<User> GetUserByBlogIdAsync(int blogId);
        Task<List<User>> GetAllUsersBlogsAsync();
        User GetUserByEmail(string email);
    }
}
