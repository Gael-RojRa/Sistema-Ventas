using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<Proyecto_Programado.BL.IAdministradorDeUsuarios, Proyecto_Programado.BL.AdministradorDeUsuarios>();
builder.Services.AddScoped<Proyecto_Programado.BL.IAdministradorDeInventarios, Proyecto_Programado.BL.AdministradorDeInventarios>();
builder.Services.AddScoped<Proyecto_Programado.BL.IAdministradorDeAjustes, Proyecto_Programado.BL.AdministradorDeAjustes>();
builder.Services.AddScoped<Proyecto_Programado.BL.IAdministradorDeCaja, Proyecto_Programado.BL.AdministradorDeCaja>();
builder.Services.AddScoped<Proyecto_Programado.BL.IAdministradorDeVentas, Proyecto_Programado.BL.AdministradorDeVenta>();
builder.Services.AddScoped<Proyecto_Programado.BL.IAdministradorDeSolicitudes, Proyecto_Programado.BL.AdministradorDeSolicitudes>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<Proyecto_Programado.DA.DBContexto>(x => x.UseSqlServer(connectionString));

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;

})
.AddCookie(options =>
{
    options.LoginPath = "/LoginRegistrarse/InicieSesion";
    options.AccessDeniedPath = "/LoginRegistrarse/InicieSesion";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
