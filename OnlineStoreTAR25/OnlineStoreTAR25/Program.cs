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


            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddControllersWithViews();

            builder.Services.AddScoped<IFileServices, FileServices>();
            builder.Services.AddScoped<IKindergardenServices, KindergardenServices>(); // LISATUD: Lasteaia teenuse registreerimine

            builder.Services.AddDbContext<ShopTARpe25Context>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
 
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