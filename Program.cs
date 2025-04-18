using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebApp.Areas.Admin.Repository;
using WebApp.Models;
using WebApp.Repository;

namespace WebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //Connect db
            builder.Services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(builder.Configuration["ConnectionStrings:ConnectedDb"]);
            });
            builder.Services.AddTransient<IEmailSender, EmailSender>();
            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.IsEssential = true;
            } );

			builder.Services.AddIdentity<AppUserModel, IdentityRole>().AddEntityFrameworkStores<DataContext>().AddDefaultTokenProviders();

			builder.Services.Configure<IdentityOptions>(options =>
			{
				// Password settings.
				options.Password.RequireDigit = true;
				options.Password.RequireLowercase = true;
				options.Password.RequireNonAlphanumeric = true;
				options.Password.RequireUppercase = true;
				options.Password.RequiredLength = 6;
				options.Password.RequiredUniqueChars = 1;

				// Lockout settings.
				//options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
				//options.Lockout.MaxFailedAccessAttempts = 5;
				//options.Lockout.AllowedForNewUsers = true;

				// User settings.
				//options.User.AllowedUserNameCharacters =
				//"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
				options.User.RequireUniqueEmail = false;
			});
			var app = builder.Build();

            app.UseStatusCodePagesWithRedirects("/Home/Error?statuscode={0}");
            
            app.UseSession();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication(); //xác thực
            app.UseAuthorization(); // quyền

            app.MapStaticAssets();
			app.UseStaticFiles();

			app.MapControllerRoute(
				name: "Areas",
				pattern: "{area:exists}/{controller=Product}/{action=Index}/{id?}")
				.WithStaticAssets();

            app.MapControllerRoute(
                name: "Category",
                pattern: "/Category/{Slug?}",
                defaults: new { controller = "Category", action = "Index" });

			app.MapControllerRoute(
				name: "Brand",
				pattern: "/Brand/{Slug?}",
				defaults: new { controller = "Brand", action = "Index" });

			app.MapControllerRoute(
				name: "default",
				pattern: "{controller=Home}/{action=Index}/{id?}")
				.WithStaticAssets();

			//Seeding Data
			var context = app.Services.CreateScope().ServiceProvider.GetRequiredService<DataContext>();
            //SeedData.SeedingData(context);

            app.Run();
        }
    }
}
