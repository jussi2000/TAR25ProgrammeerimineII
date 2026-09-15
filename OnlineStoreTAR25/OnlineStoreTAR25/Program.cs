using Microsoft.EntityFrameworkCore;
using ShopTARpe25.ApplicationServices.Services;
using ShopTARpe25.Core.Serviceinterface;
using ShopTARpe25.Data;

namespace OnlineStoreTAR25
{
    public class Program
    {
        public static void Main(string[] args)
        {

            //Update-Database: EntityFrameworkCore\Update-Database

            var builder = WebApplication.CreateBuilder(args);



            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddScoped<IspaceshipServices, SpaceshipServices>();

            //ühendame andmebaasiga vvv
            //selleks, et tuleb installida Microsoft.EntityFrameworkCore,SqlServer
            //kuiinstallitud, siis tuleb viidata namespacesis Mircrosoft.EntityFrameworkCore
            builder.Services.AddDbContext<ShopTARpe25Context>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
