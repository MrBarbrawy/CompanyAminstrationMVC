
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using CompanyAdminstrationMVC.BLL.Repositories;
using CompanyAdminstrationMVC.Data.Contexts;
using CompanyAdminstrationMVC.BLL.Interfaces;
using CompanyAdminstrationMVC.DAL.Models;
using C42_G01_MVC04.BLL;
using CompanyAdminstrationMVC.PL.Services;
using CompanyAdminstrationMVC.PL.Mapping;
using CompanyAdminstrationMVC.PL.Mapping.User;
namespace C42_G01_MVC04.PL
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);


			// Add services to the container.
			builder.Services.AddControllersWithViews();

			//builder.Services.AddScoped<AppDbContext>();

			builder.Services.AddDbContext<AppDbContext>(options =>
			 options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")),
                         ServiceLifetime.Transient);
			
			builder.Services.AddScoped<IDepartmentRepository, DepartmentRepository>();
			builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
			builder.Services.AddIdentity<AppUser, IdentityRole>()
				.AddEntityFrameworkStores<AppDbContext>()
				.AddDefaultTokenProviders();



			// Life Time
			//builder.Services.AddScoped();     //LifeTime Per Request, Object UnReachable 
			//builder.Services.AddTransient();  // LifeTime Per Operations 
			//builder.Services.AddSingleton();  // LiteTime Per Application


			builder.Services.AddScoped<IScopedService, ScopedService>(); // Per Request
			builder.Services.AddTransient<ITransientService, TransientService>(); // Per Operations 
			builder.Services.AddSingleton<ISingletonService, SingletonService>(); // Per App
			builder.Services.AddAutoMapper(M => M.AddProfile(new EmployeeProfile()));
			builder.Services.AddAutoMapper(M => M.AddProfile(new UserProfile()));
			builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


			builder.Services.ConfigureApplicationCookie(config =>
			{

				config.LoginPath = "/Account/SignIn";

			});


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
