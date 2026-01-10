using Active_Blog_Service.Context;
using Active_Blog_Service.Models;
using Active_Blog_Service.Repositories.Contracts;
using Microsoft.AspNetCore.Mvc.Formatters.Xml;
using Microsoft.EntityFrameworkCore;

namespace Active_Blog_Service.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly AppDbContext _context;

        public CommentRepository(AppDbContext context)
        {
            _context = context;
        }
        public Task<int> TotalCommentsCountAsync => _context.Comments.CountAsync();
        public async Task<List<Comment>> GetCommentsAsync()
        {
            return await _context.Comments.ToListAsync();
        }
        public async Task<List<Comment>> GetCommentsOfBlogOrderByDateTimeAsync(int blogId)
        {
            return await _context.Comments.Where(c=>c.BlogId == blogId).OrderBy(c=>c.CreatedDateTime).ToListAsync();
        }
        public async Task AddCommentAsync(Comment comment)
        {
            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();
        }
          
        public async Task<Comment> GetCommentAsync(int commentId)
        {
            return await _context.Comments.FirstOrDefaultAsync(c => c.Id == commentId);
        }
        public async Task EditCommentAsync(Comment oldComment,string commentContent)
        {
            oldComment.CommentContent = commentContent;
            await _context.SaveChangesAsync();
        }
        public async Task DeleteCommentAsync(Comment comment)
        {

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
        }
         
       
    }
}
