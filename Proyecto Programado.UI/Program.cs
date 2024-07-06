using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.CodeAnalysis.Options;
using Proyecto_Programado.BL;
var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();


builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    
})
.AddCookie(options =>
{
    options.LoginPath = "/Login/InicieSesion";
    options.AccessDeniedPath = "/Login/InicieSesion";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
})
.AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
{
    var googleAuthNSection = builder.Configuration.GetSection("Authentication:Google");
    options.ClientId = googleAuthNSection["ClientId"];
    options.ClientSecret = googleAuthNSection["ClientSecret"];

    options.Events.OnRedirectToAuthorizationEndpoint = context =>
    {
        context.Response.Redirect(context.RedirectUri + "&prompt=select_account");
        return Task.CompletedTask;
    };
})

.AddFacebook(options =>
{
    var facebookAuthNSection = builder.Configuration.GetSection("Authentication:Facebook");
    options.AppId = facebookAuthNSection["AppId"];
    options.AppSecret = facebookAuthNSection["AppSecret"];
    options.Events.OnRedirectToAuthorizationEndpoint = context =>
    {
        context.Response.Redirect(context.RedirectUri + "&auth_type=rerequest");
        return Task.CompletedTask;
    };
});


var app = builder.Build();


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
    pattern: "{controller=LoginRegistro}/{action=InicieSesion}/{id?}");

app.Run();
