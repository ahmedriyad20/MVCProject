using BusinessLogicLayer.IService;
using BusinessLogicLayer.Service;
using DataAccessLayer.Context;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.EntityFrameworkCore;
using MVCProject.Filters;
using MVCProject.Middleware;
using static Microsoft.AspNetCore.Razor.Language.TagHelperMetadata;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace MVCProject
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
            })
                .AddCookie()
                .AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
                {
                    options.ClientId = builder.Configuration.GetSection("GoogleKeys:ClientId").Value;
                    options.ClientSecret = builder.Configuration.GetSection("GoogleKeys:ClientSecret").Value;
                });

            // Add Memory Cache service
            builder.Services.AddMemoryCache();

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            //If you want to use a custom Exception Handling Filter 
            //builder.Services.AddControllersWithViews(op =>
            //{
            //    op.Filters.Add<ExceptionHandleFilter>();
            //});

            //Database Context Configuration and injecting it to the DI Container
            builder.Services.AddDbContext<UniversityContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("UniversityDBConnectionString"));
            });

            //REGISTER ApplicationUser and IdentityRole with the DI Container
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequiredLength = 4;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireDigit = false;
            })
                .AddEntityFrameworkStores<UniversityContext>();
            //.AddDefaultTokenProviders();

            //Dependency Injection for Service Layer
            builder.Services.AddScoped<IStudentService, StudentService>();
            builder.Services.AddScoped<IDepartmentService, DepartmentService>();
            builder.Services.AddScoped<ICourseService, CourseService>();
            builder.Services.AddScoped<IInstructorService, InstructorService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }

            //My Custom LoggingMiddleware
            //app.UseRequestLogging();


            //My Custom GlobalExceptionHandlerMiddleware

            // Program.cs
            // app.UseGlobalExceptionHandler();      // _next will be UseRouting
            // app.UseRouting();                    // _next will be UseAuthorization
            //app.UseAuthorization();              // _next will be MapStaticAssets, and so on.

            //The compiler doesn't figure it out - ASP.NET Core's middleware system builds this chain at runtime based on
            //the order you write in Program.cs.

            //app.UseGlobalExceptionHandler();


            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapStaticAssets();

            // Custom route for Students/All
            app.MapControllerRoute(
                name: "AllStudents",
                pattern: "Students/All",
                defaults: new { controller = "Student", action = "GetAll" });

            // Custom route for Student/Details/{id}
            app.MapControllerRoute(
                name: "StudentDetails",
                pattern: "Student/Details/{id:int:min(1)}",
                defaults: new { controller = "Student", action = "GetById" });

            app.MapControllerRoute(
                name: "Department/Details",
                pattern: "Department/Details/{deptName:alpha:minlength(2):maxlength(30)}",
                defaults: new { controller = "Department", action = "GetByName" });

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
