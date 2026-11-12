using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using MVCproject.Data;
using MVCproject.Models;

var builder = WebApplication.CreateBuilder(args);

// Add SQL Server configuration
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
    });
// Add Services to Identity
builder.Services.AddIdentity<AppUser, IdentityRole>(options => 
    { 
        options.Password.RequireDigit = true;
        options.Password.RequiredLength = 6;
        options.User.RequireUniqueEmail = true;
        // SignIn (requisitos para permitir login)
        options.SignIn.RequireConfirmedEmail = false; // en prod: true
        options.SignIn.RequireConfirmedPhoneNumber = false;
        options.SignIn.RequireConfirmedAccount = false; // si usas plantillas con confirmaciçon
        options.Lockout.AllowedForNewUsers = false;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Add Cookie configuration to User session
builder.Services.ConfigureApplicationCookie(options =>
    {
        // add time to expire, this don't start to count while you are using the application. 
        options.ExpireTimeSpan = TimeSpan.FromDays(1);
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
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
