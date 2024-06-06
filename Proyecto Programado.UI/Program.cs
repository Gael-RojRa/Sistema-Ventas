using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.EntityFrameworkCore;
using Proyecto_Programado.BL;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Configuración de autenticación
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.LoginPath = "/Login/InicieSesion";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
})
.AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
{
    var googleAuthNSection = builder.Configuration.GetSection("Authentication:Google");
    options.ClientId = googleAuthNSection["ClientId"];
    options.ClientSecret = googleAuthNSection["ClientSecret"];

    options.Events = new Microsoft.AspNetCore.Authentication.OAuth.OAuthEvents
    {
        OnCreatingTicket = async context =>
        {
            var identity = (ClaimsIdentity)context.Principal.Identity;
            var email = context.Principal.FindFirst(ClaimTypes.Email)?.Value;

            if (!string.IsNullOrEmpty(email))
            {
                identity.AddClaim(new Claim(ClaimTypes.Name, email));
            }
        }
    };
});

// Otros servicios
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<IAdministradorDeUsuarios, AdministradorDeUsuarios>();
builder.Services.AddScoped<IAdministradorDeInventarios, AdministradorDeInventarios>();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<Proyecto_Programado.DA.DBContexto>(options => options.UseSqlServer(connectionString));

var app = builder.Build();

// Configuración de la tubería HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=InicieSesion}/{id?}");

app.Run();
