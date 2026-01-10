using Active_Blog_Service.Models;
using Active_Blog_Service.ValidationAttributes;
using System.ComponentModel.DataAnnotations;

namespace Active_Blog_Service.ViewModels
{
    public class BlogForEditingOrAddingViewModel
    {
        public int? Id { get; set; }
        [MaxLength(80, ErrorMessage = "Title must be between 3 and 80 characters long.")]
        [MinLength(3, ErrorMessage = "Title must be between 3 and 80 characters long.")]
        public string Title { get; set; }
        [MaxLength(80, ErrorMessage = "Category must be between 3 and 80 characters long.")]
        [MinLength(3, ErrorMessage = "Category must be between 3 and 80 characters long.")]
        public string Category { get; set; }

        [CheckImageExtension(errorMessage: "Invalid image file format. Only .jpg, .png, and .jpeg files are allowed.")]
        [CheckImageSize(maxSizeInMB: 5, errorMessage: "Image size must not exceed 5 MB")]
        public IFormFile? ImageFile { get; set; }
        [MinLength(50, ErrorMessage = "Blog content must be at least 50 characters long.")]
        public string BlogContent { get; set; }
       
    }
}
