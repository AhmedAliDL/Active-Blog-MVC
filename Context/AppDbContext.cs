using Active_Blog_Service.Context.Config;
using Active_Blog_Service.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace Active_Blog_Service.Context
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext():base()
        {
            
        }

        public AppDbContext(DbContextOptions options):base(options) 
        {
            
        }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Comment> Comments { get; set; }

       
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(typeof(BlogConfig).Assembly);
        }
    }
}
