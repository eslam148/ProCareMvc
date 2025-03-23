using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProCareMvc.business;
using ProCareMvc.Database;
using ProCareMvc.Database.Entity;
using ProCareMvc.presentation.Mapper;

namespace ProCareMvc.presentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<AppDbContext>(optiens =>
            {
                optiens.UseSqlServer(builder.Configuration.GetConnectionString("connection"));
            });
            builder.Services.AddIdentity<User, IdentityRole<Guid>>();

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddAutoMapper(typeof(MapperProfile));
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
