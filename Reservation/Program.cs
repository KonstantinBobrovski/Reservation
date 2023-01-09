using Core.Interfaces;
using Core.Services;
using Infrastructure;
using Infrastructure.EntiesContext;
using Infrastructure.IdentityContext;
using Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Reservation.consts;
using Reservation.Core.Models;
using System.Configuration;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        ConfigureServices(builder);

        var app = builder.Build();



        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }
        else
        {
          
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();



        app.MapControllerRoute(
          name: "MyArea",
        pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

        if(app.Environment.IsDevelopment())
        {
            app.MapControllerRoute(name: "Dev",
                pattern: "{area=Developer}/{controller=Home}/{action=Index}");
        }
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;

           // var identiyContext = services.GetRequiredService<IdentityDbContext>();
           // if (identiyContext.Database.GetPendingMigrations().Any())
           // {
           //     identiyContext.Database.Migrate();
           // }
           //
           // var entitiesContext = services.GetRequiredService<EntitiesDbContext>();
           // if (entitiesContext.Database.GetPendingMigrations().Any())
           // {
           //     entitiesContext.Database.Migrate();
           // }

            Seed(services);
        }

        app.Run();
    }

    private static void ConfigureServices(WebApplicationBuilder builder)
    {
         builder.Services.AddDbContext<IdentityDbContext>(c =>
          c.UseSqlServer("Server=sqlServer;Database=Identity;User=sa;Password=S3cur3P@ssW0rd!;TrustServerCertificate=true;"));
         builder.Services.AddDbContext<EntitiesDbContext>(c =>
         c.UseSqlServer("Server=sqlServer;Database=Application;User=sa;Password=S3cur3P@ssW0rd!;TrustServerCertificate=true;"));

       // builder.Services.AddDbContext<IdentityDbContext>(c => c.UseInMemoryDatabase("Identity"));
       // builder.Services.AddDbContext<EntitiesDbContext>(c => c.UseInMemoryDatabase("Enitites"));


        builder.Services.AddScoped(typeof(IFileService), typeof(FileService));

        builder.Services.AddScoped(typeof(IRepository<>), typeof(EfCoreRepository<>));
       // builder.Services.AddScoped(typeof(IRepository<Table>), typeof(EfCoreRepository));
       // builder.Services.AddScoped(typeof(IRepository<Reservation.Core.Models.Reservation>), typeof(EfCoreRepository));


        builder.Services.AddScoped<IRestaurantService, RestaurantService>();

        if (builder.Environment.IsDevelopment())
        {
            builder.Services.AddLogging(builder =>
            {
                builder.AddDebug()
                    .AddConsole();


            });
        }
        builder.Services.AddAuthentication();
        builder.Services.AddAuthorization();

        builder.Services.Configure<RazorViewEngineOptions>(o =>
        {
            // {2} is area, {1} is controller,{0} is the action    
           
        });

        builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                        .AddEntityFrameworkStores<IdentityDbContext>()
                        .AddDefaultTokenProviders();

        builder.Services.Configure<IdentityOptions>(options =>
        {
            // Password settings.
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredLength = 6;



            options.User.RequireUniqueEmail = true;
        });

        builder.Services.ConfigureApplicationCookie(options =>
        {
            // Cookie settings
            options.Cookie.HttpOnly = true;
            options.ExpireTimeSpan = TimeSpan.FromMinutes(30);

            options.LoginPath = "/Identity/Client/Login";

            options.SlidingExpiration = true;
        });

        // Add services to the container.
        builder.Services.AddControllersWithViews();
    }

#pragma warning disable CS1998 // Only once never mind
    private async static void Seed(IServiceProvider provider)
#pragma warning restore CS1998 
    {
        var userManager = provider.GetRequiredService<UserManager<ApplicationUser>>();
        var rolesManager = provider.GetRequiredService<RoleManager<IdentityRole>>();
        if (!rolesManager.Roles.Any())
        {
            foreach (UserTypeEnum userType in (UserTypeEnum[])Enum.GetValues(typeof(UserTypeEnum)))
            {
                if (!rolesManager.CreateAsync(new IdentityRole() { Name = userType.ToString() }).Result.Succeeded)
                {
                    throw new Exception("The role is not created " + userType.ToString());
                }
            }
        }

        if (userManager.FindByNameAsync("manager2@mail.com").Result == null)
        {

            ApplicationUser Manager = new ApplicationUser
            {
                UserName = "manager2@mail.com",
                Email = "manager2@mail.com"
            };

            IdentityResult result = userManager.CreateAsync(Manager, "Password").Result;

            if (result.Succeeded)
            {
                userManager.AddClaimAsync(Manager,new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Role, UserTypeEnum.System_Administrator.ToString())).Wait();
            }
            else
                throw new Exception("THE ADMIN USER IS NOT CREATED");
        }
    }
}