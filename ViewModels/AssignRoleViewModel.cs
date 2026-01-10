using Microsoft.AspNetCore.Identity;

namespace Active_Blog_Service.ViewModels
{
    public class AssignRoleViewModel
    {
        public string UserEmail { get; set; }
        public string RoleName { get; set; } 
    }
}
