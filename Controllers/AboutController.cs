using Active_Blog_Service.Services.Contracts;
using Active_Blog_Service.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Active_Blog_Service.Controllers
{
    public class AboutController : Controller
    {
        private readonly IBlogService _blogService;
        private readonly ICommentService _commentService;
        private readonly IUserService _applicationUserService;
        private readonly ILogger<AboutController> _logger;

        public AboutController(IBlogService blogService, ICommentService commentService, IUserService applicationUserService, ILogger<AboutController> logger)
        {
            _blogService = blogService;
            _commentService = commentService;
            _applicationUserService = applicationUserService;
            _logger = logger;
        }
        public async Task<IActionResult> Index()
        {

            var aboutViewModel = new AboutViewModel
            {
                BlogsCount = await _blogService.TotalBlogsCountAsync(),
                UsersCount = await _applicationUserService.TotalUsersCountAsync(),
                CommentsCount = await _commentService.TotalCommentsCountAsync()
            };
            return View(aboutViewModel);

        }
    }
}
