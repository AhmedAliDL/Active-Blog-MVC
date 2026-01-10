using Active_Blog_Service.Models;
using System.Security.Claims;

namespace Active_Blog_Service.ViewModels
{
    public class BlogCommentViewModel
    {
        public Blog Blog { get; set; }
        public Comment Comment { get; set; } = new Comment();
        public User ActiveUser { get; set; }

        public List<Blog> BlogsAtDate { get; set; } = new List<Blog>();
    }
}
