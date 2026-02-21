using Microsoft.EntityFrameworkCore;
using TrackanDrive.Dashcam.Interfaces;
using TrackanDrive.Dashcam.Models;
using TrackanDrive.Dashcam.Services;
using TrackanDrive.Web.Interfaces;
using TrackanDrive.Web.Services;

namespace trackandrive.dashcam
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddControllersWithViews();
            builder.Services.AddDbContext<TrackanDriveDbContext>(o => o.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddScoped<IHome, HomeServices>();
            builder.Services.AddScoped<IDapper, Dapperr>();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddSession();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseSession();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=LiveStream}/{id?}");

            app.Run();
        }
    }
}
