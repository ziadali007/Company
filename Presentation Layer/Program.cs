using Business_Logic_Layer.Interfaces;
using Business_Logic_Layer.Repositories;
using Data_Access_Layer.Data.Contexts;
using Data_Access_Layer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Presentation_Layer.Mapping;
using Presentation_Layer.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.Facebook;

namespace Presentation_Layer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            //builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
            //builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddDbContext<CompanyDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            }
            );

            builder.Services.AddAutoMapper(typeof(EmployeeProfile));

            builder.Services.AddScoped<IScopedService, ScopedService>();
            builder.Services.AddTransient<ITransientService, TransientService>();
            builder.Services.AddSingleton<ISingletonService, SingletonService>();

            builder.Services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<CompanyDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Account/SignIn";
                options.AccessDeniedPath = "/Account/AccessDenied";
            });

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignInScheme = IdentityConstants.ApplicationScheme;
            })
           .AddCookie(options =>
           {
             // Configure cookie options if needed
              options.LoginPath = "/Account/Login";
               options.AccessDeniedPath = "/Account/AccessDenied";
           })
           .AddGoogle(options =>
           {
               options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
               options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
           })
           .AddFacebook(options =>
           {
                options.ClientId = builder.Configuration["Authentication:Facebook:ClientId"];
                options.ClientSecret = builder.Configuration["Authentication:Facebook:ClientSecret"];
                options.CallbackPath = "/signin-facebook";
           });

            //// Program.cs
            //builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            //    .AddCookie(options =>
            //    {
            //        options.LoginPath = "/Account/SignIn"; // Your login page
            //        options.AccessDeniedPath = "/Account/AccessDenied";
            //        options.ExpireTimeSpan = TimeSpan.FromDays(30); // Optional: Persistent cookies
            //    });

            //builder.Services.AddAuthentication(options =>
            //{
            //    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            //})                
            //.AddGoogle(options =>
            //{
            //    options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
            //    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
            //});

            /*
             * the problem is in this function :  //builder.Services.AddAuthentication(options =>
 //{
 //    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
 //    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
 //    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
 //});
 //.AddCookie() // 🔐 Your own app's login (username/password)
 //.AddGoogle(options =>
 //{
 //   options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
 //    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
 //})
 //.AddFacebook(options =>
 //{
 //     options.ClientId = builder.Configuration["Authentication:Facebook:ClientId"];
 //     options.ClientSecret = builder.Configuration["Authentication:Facebook:ClientSecret"];
 //});
             * 
             * 
             */


            // builder.Services.AddAuthentication(options =>
            // {
            //     //options.DefaultAuthenticateScheme = GoogleDefaults.AuthenticationScheme;
            //     options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
            // })
            // .AddGoogle(options =>
            // {
            //     options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
            //     options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
            // });

            // builder.Services.AddAuthentication(options =>
            // {
            //     options.DefaultAuthenticateScheme = FacebookDefaults.AuthenticationScheme;
            //     options.DefaultChallengeScheme = FacebookDefaults.AuthenticationScheme;
            // })
            //.AddFacebook(options =>
            //{
            //    options.ClientId = builder.Configuration["Authentication:Facebook:ClientId"];
            //    options.ClientSecret = builder.Configuration["Authentication:Facebook:ClientSecret"];
            //});



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

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
