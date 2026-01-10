using Active_Blog_Service.Context;
using Active_Blog_Service.Models;
using Active_Blog_Service.Repositories.Contracts;
using Active_Blog_Service.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Active_Blog_Service.Repositories
{
    public class BlogRepository : IBlogRepository
    {
        private readonly AppDbContext _context;

        public BlogRepository(AppDbContext context)
        {
            _context = context;
        }
        public Task<int> TotalBlogsCountAsync=> _context.Blogs.CountAsync();
        public IQueryable<Blog> GetBlogs()
        {
            return _context.Blogs.AsQueryable();
        }
        public async Task AddBlogAsync(Blog blog)
        {
            
            await _context.Blogs.AddAsync(blog);
            await _context.SaveChangesAsync();

        }
        public async Task<Blog> GetBlogByIdAsync(int id)
        {
            
            return  await _context.Blogs
                .Include(b => b.User)
                .Include(b=>b.Comments)
                .ThenInclude(c=>c.User)
                .FirstOrDefaultAsync(b => b.Id == id);
        }
        public async Task<List<Blog>> GetBlogsByDateAsync(DateOnly date)
        {
            return await _context.Blogs.Where(b => b.CreatedDate >= date).ToListAsync();
        }
        public async Task<List<Blog>> GetBlogsByUserIdAsync(string userId)
        {
            return await _context.Blogs.Where(b => b.UserId == userId).ToListAsync();
        }
      
        public async Task DeleteBlogAsync(int blogId)
        {
            var blog = await GetBlogByIdAsync(blogId);
            _context.Blogs.Remove(blog);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateBlogAsync(int id,BlogForEditingOrAddingViewModel newBlog)
        {
            var oldBlog = await GetBlogByIdAsync(id);

            oldBlog.Title = newBlog.Title;
            oldBlog.Category = newBlog.Category;
            oldBlog.BlogContent = newBlog.BlogContent;

            await _context.SaveChangesAsync();
        }

       

    }
}
