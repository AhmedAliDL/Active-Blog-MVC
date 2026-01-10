using Active_Blog_Service.Context;
using Active_Blog_Service.Exceptions;
using Active_Blog_Service.Models;
using Active_Blog_Service.Repositories;
using Active_Blog_Service.Repositories.Contracts;
using Active_Blog_Service.Services.Contracts;
using Active_Blog_Service.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.Drawing.Printing;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Active_Blog_Service.Services
{
    public class BlogService : IBlogService
    {
        private readonly UserManager<User> _userManager;
        private readonly IBlogRepository _blogRepository;

        public BlogService(UserManager<User> userManager, IBlogRepository blogRepository)
        {
            _userManager = userManager;
            _blogRepository = blogRepository;
        }
        public async Task<int> TotalBlogsCountAsync() => await _blogRepository.TotalBlogsCountAsync;
        public PaginationBlogViewModel ShowBlogs(int page = 1, int pageSize = 3)
        {
            List<ShowBlogViewModel> blogs = _blogRepository.GetBlogs().Select(b => new ShowBlogViewModel
            {
                Id = b.Id,
                Title = b.Title,
                CreatedDate = b.CreatedDate,
                Image = b.Image,
                UserId = b.UserId,
            }).ToList();
            var blogViewModel = new PaginationBlogViewModel();
            var totalBlogs = blogs.Count;

            blogViewModel.TotalPages = (int)Math.Ceiling(totalBlogs / (double)pageSize);
            blogViewModel.PageSize = pageSize;
            blogViewModel.Currentpage = page;
            blogViewModel.ShowBlogList = blogs;

            return blogViewModel;
        }
        public async Task AddBlogAsync(ClaimsPrincipal user, BlogForEditingOrAddingViewModel addBlogViewModel)
        {

            var appUser = await _userManager.FindByEmailAsync(user.Identity!.Name!);
            string userId = appUser!.Id;
            string imagePath = "";
            try
            {
                if (addBlogViewModel.ImageFile != null && addBlogViewModel.ImageFile.Length > 0)
                {
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(addBlogViewModel.ImageFile.FileName);
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/blogImages");
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await addBlogViewModel.ImageFile.CopyToAsync(stream);
                    }

                    imagePath = $"/blogImages/{uniqueFileName}";
                }
                else
                {
                    imagePath = $"/blogImages/Default.png";
                }


            }
            catch (IOException ex)
            {
                throw new ImageUploadException("An error occurred while uploading the image.", ex);
            }
            Blog blog = new()
            {
                Title = addBlogViewModel.Title,
                Category = addBlogViewModel.Category,
                BlogContent = addBlogViewModel.BlogContent,
                Image = imagePath,
                UserId = userId,
            };


            await _blogRepository.AddBlogAsync(blog);
        }

        public async Task<BlogCommentViewModel> ShowBlogAsync(ClaimsPrincipal user, int blogId)
        {
            var blog = await _blogRepository.GetBlogByIdAsync(blogId);
            var activeUser = await _userManager.FindByEmailAsync(user.Identity!.Name!);

            var recentBlogs = await _blogRepository.GetBlogsByDateAsync(DateOnly.FromDateTime(DateTime.Now).AddDays(-1));


            BlogCommentViewModel blogCommentViewModel = new()
            {
                Blog = blog,
                ActiveUser = activeUser!,
                BlogsAtDate = recentBlogs
            };
            return blogCommentViewModel;
        }
        public async Task<Blog> GetBlogByIdAsync(int id)
        {
            return await _blogRepository.GetBlogByIdAsync(id);
        }
        public async Task<List<Blog>> GetBlogsByDateAsync(DateOnly date)
        {
            return await _blogRepository.GetBlogsByDateAsync(date);
        }

        public async Task<List<Blog>> TakeCountOfBlogsAsync(int count)
        {
            return await _blogRepository.GetBlogs()
                .OrderByDescending(b => b.Id)
                .Take(count)
                .ToListAsync();
        }
        public async Task<List<Blog>> ShowBlogsOfUserAsync(ClaimsPrincipal user)
        {
            var appUser = await _userManager.FindByEmailAsync(user.Identity!.Name!);

            return await _blogRepository.GetBlogsByUserIdAsync(appUser!.Id);
        }
        public async Task DeleteBlogAsync(int blogId)
        {
            await _blogRepository.DeleteBlogAsync(blogId);
        }
        public async Task<BlogForEditingOrAddingViewModel> ShowBlogForEditingAsync(int blogId)
        {
            var blog = await _blogRepository.GetBlogByIdAsync(blogId);

            var viewModel = new BlogForEditingOrAddingViewModel();

            viewModel.Id = blogId;
            viewModel.Title = blog.Title;
            viewModel.BlogContent = blog.BlogContent;
            viewModel.Category = blog.Category;
            return viewModel;
        }
        public async Task EditBlogAsync(int id, BlogForEditingOrAddingViewModel blogViewModel)
        {
            var blog = await GetBlogByIdAsync(id);
            try
            {
                if (blogViewModel.ImageFile != null && blogViewModel.ImageFile.Length > 0)
                {
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(blogViewModel.ImageFile.FileName);
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/blogImages");
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await blogViewModel.ImageFile.CopyToAsync(stream);
                    }

                    var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", blog.Image.TrimStart('/'));
                    if (File.Exists(oldImagePath))
                    {
                        File.Delete(oldImagePath);
                    }

                    blog.Image = $"/blogImages/{uniqueFileName}";
                }

            }
            catch (IOException ex)
            {
                throw new ImageUploadException("An error occurred while uploading the image.", ex);
            }

            await _blogRepository.UpdateBlogAsync(id, blogViewModel);
        }

    }
}
