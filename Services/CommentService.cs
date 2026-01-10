using Active_Blog_Service.Models;
using Active_Blog_Service.Repositories.Contracts;
using Active_Blog_Service.Services.Contracts;
using Active_Blog_Service.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Active_Blog_Service.Services
{
    public class CommentService : ICommentService
    {
        private readonly UserManager<User> _userManager;
        private readonly ICommentRepository _commentRepository;
        public CommentService(UserManager<User> userManager, ICommentRepository commentRepository)
        {
            _userManager = userManager;
            _commentRepository = commentRepository;
        }
        public async Task<int> TotalCommentsCountAsync() => await _commentRepository.TotalCommentsCountAsync;  
        public async Task<List<Comment>> GetCommentsAsync()
        {
            return await _commentRepository.GetCommentsAsync();
        }
        public async Task<List<Comment>> GetCommentsOfBlogOrderByDateTimeAsync(int blogId)
        {
            return await _commentRepository.GetCommentsOfBlogOrderByDateTimeAsync(blogId);
        }
        public async Task AddCommentAsync(ClaimsPrincipal user ,BlogCommentViewModel blogCommentViewModel)
        {
            var appUser = await _userManager.FindByEmailAsync(user.Identity!.Name!);
            blogCommentViewModel.Comment.UserId = appUser!.Id;
            await _commentRepository.AddCommentAsync(blogCommentViewModel.Comment);
        }
            
        public async Task<Comment> GetCommentAsync(int commentId)
        {
            return await _commentRepository.GetCommentAsync(commentId);
        }
        public async Task EditCommentAsync(Comment oldComment, string commentContent)
        {
            await _commentRepository.EditCommentAsync(oldComment, commentContent);
        }
        public async Task DeleteCommentAsync(Comment comment)
        {
            await _commentRepository.DeleteCommentAsync(comment);

        }

    }
}
