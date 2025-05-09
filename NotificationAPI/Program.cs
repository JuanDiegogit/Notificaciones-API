using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;  
using Microsoft.IdentityModel.Tokens;
using NotificationAPI.Data;
using NotificationAPI.Middleware;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configurar el contexto de base de datos
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("SupabaseConnection")));

// Agregar controladores y Swagger
builder.Services.AddControllers();
builder.Services.AddScoped<Servicios.ServicioAutenticacion>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var claveJwt = "lX8bJ/m/oR7Tu+j45vdi39My5afiA1A9zjjQrMCY4OF70KsJLQe8oAFGaiqJSqiLksPku7n2EPGX9V+nmyTmyA==";

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    

    .AddJwtBearer(opciones =>
    {
        opciones.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(claveJwt)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });
builder.Services.AddScoped<ServicioAutenticacion>();
var app = builder.Build();
app.UseMiddleware<NotificationAPI.Middleware.ErrorHandlingMiddleware>();

// Configurar middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
