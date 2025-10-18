using Microsoft.CodeAnalysis.Elfie.Serialization;
using MVCProject.Middleware;
using static Microsoft.AspNetCore.Razor.Language.TagHelperMetadata;

namespace MVCProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }

            //My Custom LoggingMiddleware
            app.UseRequestLogging();


            //My Custom GlobalExceptionHandlerMiddleware

            // Program.cs
            // app.UseGlobalExceptionHandler();      // _next will be UseRouting
            // app.UseRouting();                    // _next will be UseAuthorization
            //app.UseAuthorization();              // _next will be MapStaticAssets, and so on.

            //The compiler doesn't figure it out - ASP.NET Core's middleware system builds this chain at runtime based on
            //the order you write in Program.cs.

            app.UseGlobalExceptionHandler();


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
