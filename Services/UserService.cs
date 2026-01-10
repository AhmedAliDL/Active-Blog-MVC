using Active_Blog_Service.Models;
using Active_Blog_Service.Services.Contracts;
using System.Runtime.CompilerServices;

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
        public bool CheckFoundOfEmail(string email)
        {
            var user = _applicationUserRepository.GetUserByEmail(email);
            if (user != null)
                return true;
            return false;
        }
    }
}
