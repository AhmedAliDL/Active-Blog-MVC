using Active_Blog_Service.Models;
using Active_Blog_Service.Services.Contracts;

namespace Active_Blog_Service.Services
{
    public class UserService : IUserService
    {
        private readonly Repositories.Contracts.IUserRepository _applicationUserRepository;
        public UserService(Repositories.Contracts.IUserRepository applicationUserRepository)
        {
            _applicationUserRepository = applicationUserRepository;
        }
        public async Task<int> TotalUsersCountAsync()
        {
            return await _applicationUserRepository.CountOfUsersAsync;
        }

        public async Task<User> GetUserByIdAsync(string userId)
        {
            return await _applicationUserRepository.GetUserByIdAsync(userId);
        }
        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _applicationUserRepository.GetUserByEmailAsync(email);
        }
        public async Task<User> GetUserByBlogIdAsync(int blogId)
        {
            return await _applicationUserRepository.GetUserByBlogIdAsync(blogId);
        }
        public async Task<List<User>> GetAllUsersBlogsAsync()
        {
            return await _applicationUserRepository.GetAllUsersBlogsAsync();
        }
    }
}
