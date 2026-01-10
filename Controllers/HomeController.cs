using Active_Blog_Service.Models;
using Active_Blog_Service.Services.Contracts;
using Active_Blog_Service.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Active_Blog_Service.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBlogService _blogService;
        private readonly IUserService _userService;

        public HomeController(ILogger<HomeController> logger, IBlogService blogService, IUserService userService)
        {
            _logger = logger;
            _blogService = blogService;
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            var homeViewModel = new HomeViewModel();
            homeViewModel.SimpleOfBlogs = await _blogService.TakeCountOfBlogsAsync(3);
            homeViewModel.UsersBlogs = await _userService.GetAllUsersBlogsAsync();
            return View(homeViewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var exceptionDetails = HttpContext.Features.Get<IExceptionHandlerFeature>();

            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                ErrorMessage = exceptionDetails?.Error.Message
            });
        }
    }
}
