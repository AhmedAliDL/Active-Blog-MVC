
using Active_Blog_Service.Models;
using Active_Blog_Service.ViewModels;
using System.Security.Claims;

namespace Active_Blog_Service.Services.Contracts
{
    public interface ICommentService : IScopedServiceMarker
    {
        Task<int> TotalCommentsCountAsync();
        Task<List<Comment>> GetCommentsAsync();
        Task<List<Comment>> GetCommentsOfBlogOrderByDateTimeAsync(int blogId);
        Task AddCommentAsync(ClaimsPrincipal user,BlogCommentViewModel blogCommentViewModel);
        Task<Comment> GetCommentAsync(int commentId);
        Task EditCommentAsync(Comment oldComment, string commentContent);
        Task DeleteCommentAsync(Comment comment);

    }
}