using Core.Interfaces;
using Core.Services;
using Infrastructure;
using Infrastructure.IdentityContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

InfrastructureDependencies.ConfigureInfastructue(builder.Configuration, builder.Services);
builder.Services.AddScoped<IRestaurantService, RestaurantService>();
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddLogging(builder =>
    {
        builder.AddDebug()
            .AddConsole()
            .SetMinimumLevel(LogLevel.Warning)
            .AddFilter("Aguacongas.Identity.Redis", LogLevel.Trace);
    });
}
builder.Services.AddAuthentication();
builder.Services.AddAuthorization() ;

builder.Services.Configure<RazorViewEngineOptions>(o =>
{
    // {2} is area, {1} is controller,{0} is the action    
   
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddRedisStores("redis:6379")
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
    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

    options.LoginPath = "/Identity/Client/Login";
   
    options.SlidingExpiration = true;
});

// Add services to the container.
builder.Services.AddControllersWithViews();


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

//app.MapControllerRoute(
  //  name: "MyArea",
    //pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");



app.Run();
