using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProCareMvc.business;
using ProCareMvc.Database;
using ProCareMvc.Database.Entity;
using ProCareMvc.presentation.Hub;
using ProCareMvc.presentation.Mapper;
using ProCareMvc.presentation.Services;

namespace ProCareMvc.presentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("connection"));
            });

        
            builder.Services.AddTransient<EmailServices>();
            builder.Services.AddIdentity<User, IdentityRole<Guid>>(options =>
               {
                   options.Password.RequireDigit = false;
                   options.Password.RequireNonAlphanumeric = false;
               }).AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddSignalR();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            //services for account controller

      

            builder.Services.AddAutoMapper(typeof(MapperProfile));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            //app.UseAuthentication();  
            app.UseAuthorization();
            app.MapHub<NotifyHub>("/notifyhub");
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
