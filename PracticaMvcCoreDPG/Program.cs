using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using PracticaMvcCoreDPG.Data;
using PracticaMvcCoreDPG.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews
    (options => options.EnableEndpointRouting = false)
    .AddSessionStateTempDataProvider(); 
builder.Services.AddMemoryCache();
builder.Services.AddSession();
builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<RepositoryLibros>();
string conecctionString = builder.Configuration.GetConnectionString("SqlConecction");
builder.Services.AddDbContext<LibrosContext>
    (options => options.UseSqlServer(conecctionString));
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme =
    CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme =
    CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme =
    CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(
    CookieAuthenticationDefaults.AuthenticationScheme,
    config =>
    {
        config.AccessDeniedPath = "/Managed/ErrorAcceso";
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

app.UseAuthorization();

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();
app.UseMvc(routes =>
{
    routes.MapRoute(name: "default", template: "{controller=Libros}/{action=Libros}/{id?}");
    routes.MapRoute(name: "perfil", template: "{controller=Libros}/{action=Perfil}/{id?}");
});



app.Run();
