using Active_Blog_Service.Models;
using Active_Blog_Service.ValidationAttributes;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Text.RegularExpressions;

namespace Active_Blog_Service.ViewModels
{
    public class RegisterViewModel 
    {
        [MaxLength(30)]
        [MinLength(3)]
        public string FName { get; set; }
        [MaxLength(30)]
        [MinLength(3)]
        public string LName { get; set; }
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"^[^\s@]+@[^\s@]+\.[cC][oO][mM]$", ErrorMessage = "Email must end with .com")]
        public string Email { get; set; }

        [CheckImageExtension(errorMessage: "Invalid image file format. Only .jpg, .png, and .jpeg files are allowed.")]
        [CheckImageSize(maxSizeInMB:5, errorMessage: "Image size must not exceed 5 MB")]
        public IFormFile ImageFile { get; set; }
        [MaxLength(11)]
        [MinLength(11)]
        public string Phone { get; set; }
        public string Address { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage ="Confirmed Password field must match Password field")]
        public string ConfirmPassword {  get; set; }

    } 

}
