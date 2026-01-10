using Active_Blog_Service.Context;
using Active_Blog_Service.Models;
using Active_Blog_Service.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Active_Blog_Service.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task<int> CountOfUsersAsync => _context.Users.CountAsync();

        public async Task<User> GetUserByIdAsync(string userId)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
        }
        public User GetUserByEmail(string email)
        {
            return _context.Users.FirstOrDefault(u => u.Email == email)!;
        }
        public async Task<User> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
        public async Task<User> GetUserByBlogIdAsync(int blogId)
        {
            var blog = await _context.Blogs.FirstOrDefaultAsync(b => b.Id == blogId);
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Id == blog.UserId);
        }
        public async Task<List<User>> GetAllUsersBlogsAsync()
        {
               return await _context.Users
                .Where(u => u.Blogs.Any())
                .Include(u => u.Blogs)
                .ToListAsync();
        }
    }
}
