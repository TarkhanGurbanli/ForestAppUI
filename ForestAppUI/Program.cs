using ForestAppUI.Controllers;
using ForestAppUI.Data;
using ForestAppUI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});


builder.Services.AddDefaultIdentity<User>().AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Auth/Login";
});

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = true;  //reqem olmasi mecburdu
    options.Password.RequireLowercase = true; //kicik herf olmasi mecburdu
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false; //Ancaq boyuk herif 
    options.Password.RequiredLength = 6; //minimum uzunluq ne qeder olmalidir passwordda
    options.Password.RequiredUniqueChars = 1; //Unikal Xarakter sayi
    options.Lockout.MaxFailedAccessAttempts = 5;//5 ugursuz giris ede bilersen sonra 2 dq gozlemelisen
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(2); //5 ugursuz girisden sonra gozleme vaxti
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

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}/{seoUrl?}"
    );
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}/{seoUrl?}"
    );

app.Run();
