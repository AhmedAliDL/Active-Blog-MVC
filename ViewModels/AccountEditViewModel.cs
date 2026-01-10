using Active_Blog_Service.Models;
using Active_Blog_Service.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace Active_Blog_Service.ViewModels
{
    public class AccountEditViewModel 
    {
        [MaxLength(30, ErrorMessage = "First Name must be between 3 and 30 characters long.")]
        [MinLength(3, ErrorMessage = "First Name must be between 3 and 30 characters long.")]
        public string? FName { get; set; }
        [MaxLength(30, ErrorMessage = "Last Name must be between 3 and 30 characters long.")]
        [MinLength(3, ErrorMessage = "Last Name must be between 3 and 30 characters long.")]
        public string? LName { get; set; }
        [RegularExpression(@"^[^\s@]+@[^\s@]+\.[cC][oO][mM]$",ErrorMessage ="Email must end with .com")]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }

        [CheckImageExtension(errorMessage: "Invalid image file format. Only .jpg, .png, and .jpeg files are allowed.")]
        [CheckImageSize(maxSizeInMB: 5, errorMessage: "Image size must not exceed 5 MB")]
        public IFormFile? ImageFile { get; set; }
        [MaxLength(11, ErrorMessage = "Phone number must be 11 digits long.")]
        [MinLength(11, ErrorMessage = "Phone number must be 11 digits long.")]
        public string? Phone { get; set; }
        public string? Address { get; set; }
        [DataType(DataType.Password)]
        public string? CurrentPassword { get; set; }
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        [DataType(DataType.Password)]
        [Compare("Password",ErrorMessage ="Confirmed Password field must match Password field")]
        public string? ConfirmPassword {  get; set; }

        public User? User {get; set;}  
    }

}
