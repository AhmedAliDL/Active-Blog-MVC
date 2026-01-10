using Active_Blog_Service.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Active_Blog_Service.Services.Contracts;
using System.Threading.Tasks;

namespace Active_Blog_Service.Controllers
{
    [Authorize]
    public class ContactController : Controller
    {
        private readonly IContactService _contactService;
        private readonly IUserService _userService;

        public ContactController(IContactService contactService, IUserService userService)
        {
            _contactService = contactService;
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            var sendMailViewModel = new SendMailViewModel();
            sendMailViewModel.User = await _userService.GetUserByEmailAsync(User.Identity!.Name!);
            return View(sendMailViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SendEmail(SendMailViewModel sendMailViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new
                {
                    success = false,
                    message = "Invalid form data",
                    errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                });
            }

            var statusOfEmail = await _contactService.SendEmailServiceAsync(User, sendMailViewModel);
            return Json(statusOfEmail);
        }
    }
}