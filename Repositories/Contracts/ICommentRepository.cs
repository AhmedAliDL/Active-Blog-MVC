using Active_Blog_Service.Models;

namespace Active_Blog_Service.Repositories.Contracts
{
    public interface ICommentRepository : IScopedRepositoryMarker
    {
        Task<int> TotalCommentsCountAsync { get; }
        Task<List<Comment>> GetCommentsOfBlogOrderByDateTimeAsync(int blogId);
        Task AddCommentAsync(Comment comment);
        Task<Comment> GetCommentAsync(int commentId);
        Task EditCommentAsync(Comment oldComment, string commentContent);
        Task DeleteCommentAsync(Comment comment);
        Task<List<Comment>> GetCommentsAsync();


    }
}
