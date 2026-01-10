using Active_Blog_Service.Models;
using Active_Blog_Service.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace Active_Blog_Service.Repositories.Contracts
{
    public interface IBlogRepository : IScopedRepositoryMarker
    {   
        Task<int> TotalBlogsCountAsync { get; }
        IQueryable<Blog> GetBlogs();
        Task AddBlogAsync(Blog blog);
        Task<Blog> GetBlogByIdAsync(int id);
        Task<List<Blog>> GetBlogsByDateAsync(DateOnly date);
        Task<List<Blog>> GetBlogsByUserIdAsync(string userId);
        Task DeleteBlogAsync(int blogId);
        Task UpdateBlogAsync(int id, BlogForEditingOrAddingViewModel newBlog);
    }
}
