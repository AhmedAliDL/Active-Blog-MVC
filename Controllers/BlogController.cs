using Active_Blog_Service.Models;
using Active_Blog_Service.Repositories.Contracts;
using Active_Blog_Service.Services.Contracts;
using Active_Blog_Service.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Active_Blog_Service.Controllers
{
    [Authorize]
    public class BlogController : Controller
    {

        private readonly IBlogService _blogService;
        private readonly IUserService _userService;
        private readonly ILogger<BlogController> _logger;

        public BlogController(IBlogService blogService, IUserService userService, ILogger<BlogController> logger)
        {
            _blogService = blogService;
            _userService = userService;
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, int pageSize = 3)
        {

            var blogs = _blogService.ShowBlogs(page, pageSize);
            blogs.UsersOfBlogs = await _userService.GetAllUsersBlogsAsync();
            return View(blogs);

        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(BlogForEditingOrAddingViewModel addBlogViewModel)
        {
            if (ModelState.IsValid)
            {


                await _blogService.AddBlogAsync(User, addBlogViewModel);


                return RedirectToAction("Index", "Home");


            }
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {

            var blogCommentViewModel = await _blogService.ShowBlogAsync(User, id);
            return View(blogCommentViewModel);

        }

        [HttpGet]
        public async Task<IActionResult> DeleteBlog()
        {

            var blogs = await _blogService.ShowBlogsOfUserAsync(User);

            return View(blogs);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteBlog(int blogId)
        {

            await _blogService.DeleteBlogAsync(blogId);


            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<ActionResult> ShowBlogs()
        {

            var blogs = await _blogService.ShowBlogsOfUserAsync(User);
            return View(blogs);

        }
        [HttpGet]
        public async Task<IActionResult> EditBlog(int blogId)
        {

            var viewModel = await _blogService.ShowBlogForEditingAsync(blogId);
            return View(viewModel);

        }
        [HttpPost]
        public async Task<IActionResult> SaveEdit(int blogId, BlogForEditingOrAddingViewModel blogViewModel)
        {
            if (ModelState.IsValid)
            {

                await _blogService.EditBlogAsync(blogId, blogViewModel);
                return RedirectToAction("Details", new { id = blogId });
            }
            return RedirectToAction("EditBlog", new { blogId = blogId });
        }


    }
}
