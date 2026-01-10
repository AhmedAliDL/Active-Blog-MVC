using Active_Blog_Service.Models;

namespace Active_Blog_Service.ViewModels
{
    public class HomeViewModel
    {
        public List<Blog> SimpleOfBlogs { get; set; }
        public List<User> UsersBlogs { get; set; }
    }
}
