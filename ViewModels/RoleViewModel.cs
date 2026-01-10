using System.ComponentModel.DataAnnotations;

namespace Active_Blog_Service.ViewModels
{
    public class RoleViewModel
    {
        [MaxLength(15)]
        [MinLength(3)]
        public string RoleName { get; set; }
        [MinLength(10)]
        public string? RoleDescription { get; set; }
    }
}
