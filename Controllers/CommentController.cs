using Active_Blog_Service.Models;
using Active_Blog_Service.Repositories.Contracts;
using Active_Blog_Service.Services.Contracts;
using Active_Blog_Service.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Active_Blog_Service.Controllers
{
    public class CommentController : Controller
    {
        
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddComment(BlogCommentViewModel blogCommentViewModel)
        {
            await _commentService.AddCommentAsync(User,blogCommentViewModel);

            return RedirectToAction("Details", "Blog", new { id = blogCommentViewModel.Comment.BlogId });

        }
        [HttpPost]
        public async Task<IActionResult> EditComment(int commentId)
        {
            var comment = await _commentService.GetCommentAsync(commentId);
            return View(comment);
        }
        [HttpPost]
        public async Task<IActionResult> SaveCommentEdit(int commentId, string commentContent)
        {
            var comment = await _commentService.GetCommentAsync(commentId);

            await _commentService.EditCommentAsync(comment, commentContent);

            return RedirectToAction("Details", "Blog", new { id = comment.BlogId });
        }
        [HttpPost]
        public async Task<IActionResult> DeleteComment(int commentId)
        {
            var comment = await _commentService.GetCommentAsync(commentId);
            int blogId = comment.BlogId;
            await _commentService.DeleteCommentAsync(comment);

            return RedirectToAction("Details","Blog", new { id = blogId });
        }
    }
}
