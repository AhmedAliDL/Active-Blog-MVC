using Active_Blog_Service.Models;
using Active_Blog_Service.Services.Contracts;
using Active_Blog_Service.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol;
using System.Data;
using System.Threading.Tasks;
namespace Active_Blog_Service.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IUserService _userService;

        public AccountController(IAccountService accountService, IUserService userService)
        {
            _accountService = accountService;
            _userService = userService;
        }
        public IActionResult Index()
        {

            ViewBag.User = _userService.GetUserByEmailAsync(User.Identity!.Name!).Result;

            return View();

        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel registerUserViewModel)
        {
            if (ModelState.IsValid)
            {

                var tupleData = await _accountService.RegisterServiceAsync(registerUserViewModel);

                var registeredUserViewModel = tupleData.Item2;
                var resultData = tupleData.Item1;
                if (registeredUserViewModel != null)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in resultData.Errors)
                        ModelState.AddModelError("Password", error.Description);
                }


                return View(registerUserViewModel);
            }
            return View(registerUserViewModel);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginUserViewModel)
        {
            if (ModelState.IsValid)
            {

                var tupleData = await _accountService.LoginServiceAsync(loginUserViewModel);
                var userFound = tupleData.Item1;
                var dataErrors = tupleData.Item2;
                if (userFound)
                    return RedirectToAction("Index", "Home");
                else
                    foreach (var error in dataErrors)
                    {
                        ModelState.AddModelError(error.Key, error.Value);
                    }

            }
            return View(loginUserViewModel);

        }
        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            AccountEditViewModel accountEditViewModel = new AccountEditViewModel();
            accountEditViewModel.User = await _userService.GetUserByEmailAsync(User.Identity!.Name!);
            return View(accountEditViewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(AccountEditViewModel accountEditViewModel)
        {
            if (ModelState.IsValid)
            {

                var resultData = await _accountService.EditServiceAsync(User, accountEditViewModel);

                if (resultData.Succeeded)
                    return RedirectToAction("Index", "Home");
                else
                    foreach (var error in resultData!.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
            }
            return View(accountEditViewModel);
        }
        public async Task<IActionResult> Logout()
        {
            bool isLoggedOut = await _accountService.LogoutServiceAsync();
            if (isLoggedOut)
                return RedirectToAction("Login");
            return RedirectToAction("Index", "Home");
        }
    }
}
