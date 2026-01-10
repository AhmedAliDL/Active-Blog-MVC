using Active_Blog_Service.Models;
using Active_Blog_Service.ViewModels;
using System.Security.Claims;

namespace Active_Blog_Service.Services.Contracts
{
    public interface IBlogService : IScopedServiceMarker
    {
        Task<int> TotalBlogsCountAsync();
        PaginationBlogViewModel ShowBlogs(int page = 1, int pageSize = 3);
        Task AddBlogAsync(ClaimsPrincipal user, BlogForEditingOrAddingViewModel addBlogViewModel);
        Task<Blog> GetBlogByIdAsync(int id);
        Task<List<Blog>> GetBlogsByDateAsync(DateOnly date);
        Task<List<Blog>> TakeCountOfBlogsAsync(int count);
        Task<BlogCommentViewModel> ShowBlogAsync(ClaimsPrincipal user, int blogId);
        Task<List<Blog>> ShowBlogsOfUserAsync(ClaimsPrincipal user);
        Task DeleteBlogAsync(int blogId);
        Task<BlogForEditingOrAddingViewModel> ShowBlogForEditingAsync(int blogId);
        Task EditBlogAsync(int id, BlogForEditingOrAddingViewModel blogViewModel);


    }
}