using Active_Blog_Service.Context;
using Active_Blog_Service.Models;
using Active_Blog_Service.Repositories;
using Active_Blog_Service.Repositories.Contracts;
using Active_Blog_Service.Services;
using Active_Blog_Service.Services.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using System.Collections;

namespace Active_Blog_Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<AppDbContext>(
                optionsBuilder =>
                {
                    var config = builder.Configuration;
                    var constr = config["constr"];
                    optionsBuilder.UseSqlServer(constr);
                }
                );
            var repositoryAssembly = typeof(BlogRepository).Assembly;

            builder.Services.Scan(s => s.FromAssemblies(repositoryAssembly)
            .AddClasses(c => c.AssignableTo<IScopedRepositoryMarker>())
            .AsImplementedInterfaces()
            .WithScopedLifetime()
            );
            
            var serviceAssembly = typeof(BlogService).Assembly;
            builder.Services.Scan(s => s.FromAssemblies(serviceAssembly)
            .AddClasses(c => c.AssignableTo<IScopedServiceMarker>())
            .AsImplementedInterfaces()
            .WithScopedLifetime()
            );

            builder.Services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            else
            {
                // Show detailed errors in Development to help debugging
                app.UseDeveloperExceptionPage();
            }
            app.UseStaticFiles();

            app.UseRouting();
            // Ensure authentication runs before authorization
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
           
            app.Run();
        }
    }
}

