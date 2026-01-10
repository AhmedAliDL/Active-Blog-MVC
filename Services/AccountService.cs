using Active_Blog_Service.Exceptions;
using Active_Blog_Service.Models;
using Active_Blog_Service.Services.Contracts;
using Active_Blog_Service.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace Active_Blog_Service.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountService(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task<Tuple<IdentityResult, RegisterViewModel>> RegisterServiceAsync(RegisterViewModel registerUserViewModel)
        {

            var user = new User();
            user.FName = registerUserViewModel.FName;
            user.LName = registerUserViewModel.LName;
            user.Email = registerUserViewModel.Email;
            user.PhoneNumber = registerUserViewModel.Phone;
            user.Address = registerUserViewModel.Address;

            try
            {
                if (registerUserViewModel.ImageFile != null && registerUserViewModel.ImageFile.Length > 0)
                {
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(registerUserViewModel.ImageFile.FileName);
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    if (!Directory.Exists(uploadsFolder))
                        Directory.CreateDirectory(uploadsFolder);
                    // Save the file to the specified path

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await registerUserViewModel.ImageFile.CopyToAsync(stream);
                    }


                    // Save the image path to the user model
                    user.Image = $"/images/{uniqueFileName}"; // Store relative path for later use
                }
            }
            catch(IOException ex)
            {
                throw new ImageUploadException("There was an error uploading the image file.", ex);
            }
            user.UserName = registerUserViewModel.Email;

            var result = await _userManager.CreateAsync(user,
                registerUserViewModel.Password);


            if (result.Succeeded)
            {
                try
                {
                    await _userManager.AddToRoleAsync(user, "User");
                }
                catch (InvalidOperationException ex)
                {

                    throw new InvalidRoleException("The default role 'User' does not exist.", ex);
                }
                await _signInManager.SignInAsync(user, true);
            }

            Tuple<IdentityResult, RegisterViewModel> resultTuple = new Tuple<IdentityResult, RegisterViewModel>(result, registerUserViewModel);


            return resultTuple;
        }
        public async Task<Tuple<bool, Dictionary<string, string>>> LoginServiceAsync(LoginViewModel loginUserViewModel)
        {

            var userModel = await _userManager.FindByEmailAsync(loginUserViewModel.Email);

            Dictionary<string, string> errors = new Dictionary<string, string>();
            bool userFound = false;
            if (userModel != null)
            {
                //create cookie
                var passwordCheck = await _userManager.CheckPasswordAsync(userModel, loginUserViewModel.Password);
                if (passwordCheck)
                {
                    await _signInManager.SignInAsync(userModel, loginUserViewModel.RememberMe);
                    userFound = true;
                    return new Tuple<bool, Dictionary<string, string>>(userFound, errors);
                }
            }
            errors["Error"] = "Email or password are not correct!";

            return new Tuple<bool, Dictionary<string, string>>(userFound, errors);
        }
        public async Task<IdentityResult> EditServiceAsync(ClaimsPrincipal user, AccountEditViewModel accountEditViewModel)
        {
            var appUser = await _userManager.FindByEmailAsync(user.Identity!.Name!);
            if (appUser != null)
            {
                appUser.Email = accountEditViewModel.Email ?? appUser.Email;
                appUser.Address = accountEditViewModel.Address ?? appUser.Address;
                appUser.PhoneNumber = accountEditViewModel.Phone ?? appUser.PhoneNumber;
                appUser.FName = accountEditViewModel.FName ?? appUser.FName;
                appUser.LName = accountEditViewModel.LName ?? appUser.LName;
                if (accountEditViewModel.Password != null && accountEditViewModel.CurrentPassword != null)
                {
                    var resultOfChangePassword = await _userManager.ChangePasswordAsync(appUser, accountEditViewModel.CurrentPassword, accountEditViewModel.Password);
                    if (!resultOfChangePassword.Succeeded)
                    {
                        return resultOfChangePassword;
                    }
                }
                accountEditViewModel.User = appUser;
                try
                {
                    if (accountEditViewModel.ImageFile != null && accountEditViewModel.ImageFile.Length > 0)
                    {
                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(accountEditViewModel.ImageFile.FileName);
                        var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                        var filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        if (!Directory.Exists(uploadsFolder))
                            Directory.CreateDirectory(uploadsFolder);

                        // Save the file to the specified path
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await accountEditViewModel.ImageFile.CopyToAsync(stream);
                        }
                        var oldImagePath = @$"wwwroot{appUser.Image}";
                        if (File.Exists(oldImagePath))
                            File.Delete(oldImagePath);
                        // Save the image path to the user model
                        appUser.Image = $"/images/{uniqueFileName}"; // Store relative path for later use
                    }
                }
                catch (IOException ex)
                {
                    throw new ImageUploadException("There was an error uploading the image file.", ex);
                }

                appUser.UserName = accountEditViewModel.Email ?? appUser.Email;
                var result = await _userManager.UpdateAsync(appUser);
                if (result.Succeeded)
                {
                    await _signInManager.SignOutAsync();
                    await _signInManager.SignInAsync(appUser, true);
                }
                return result;
            }
            return IdentityResult.Failed(new IdentityError
            {
                Description = "User not found."
            });
        }
        public async Task<bool> LogoutServiceAsync()
        {
            await _signInManager.SignOutAsync();
            return true;
        }
    }

}
