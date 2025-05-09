using System;
using System.Text;
using NotificationAPI.Data;
using NotificationAPI.Servicios;
using NotificationAPI.Middleware;
using Microsoft.AspNetCore.Builder;        
using Microsoft.Extensions.Hosting; 
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization; 
using Microsoft.Extensions.DependencyInjection; 
using Microsoft.AspNetCore.Authentication.JwtBearer;


var builder = WebApplication.CreateBuilder(args);

// 1. CORS (solo para desarrollo; en prod restringe orígenes)
builder.Services.AddCors(opts =>
{
    opts.AddPolicy("DevCors", p => p
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod());
});

// 2. DbContext con Npgsql y cadena desde configuración
var supaConn = builder.Configuration.GetConnectionString("Supabase");
builder.Services.AddDbContext<AppDbContext>(opts =>
    opts.UseNpgsql(supaConn));

// 3. JWT: lee key, issuer y audience de configuration (appsettings / secrets / env vars)
var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIss = builder.Configuration["Jwt:Issuer"];
var jwtAud = builder.Configuration["Jwt:Audience"];
var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

// 4. Autenticación y validación de tokens
builder.Services
  .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
  .AddJwtBearer(opts =>
  {
      opts.RequireHttpsMetadata = false;
      opts.SaveToken = true;
      opts.TokenValidationParameters = new TokenValidationParameters
      {
          ValidateIssuerSigningKey = true,
          IssuerSigningKey = signingKey,
          ValidateIssuer = true,
          ValidIssuer = jwtIss,
          ValidateAudience = true,
          ValidAudience = jwtAud,
          ClockSkew = TimeSpan.Zero
      };
  });

builder.Services.AddAuthorization();

// 5. Controladores con autorización global (todos requieren JWT salvo que uses [AllowAnonymous])
builder.Services.AddControllers(opts =>
{
    var policy = new AuthorizationPolicyBuilder()
                     .RequireAuthenticatedUser()
                     .Build();
    opts.Filters.Add(new AuthorizeFilter(policy));
});

// 6. Registra tu servicio de autenticación
builder.Services.AddScoped<ServicioAutenticacion>();

// 7. Swagger (OpenAPI)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 8. Middleware de manejo de errores antes de todo
app.UseMiddleware<ErrorHandlingMiddleware>();

// 9. Swagger UI solo en desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// 10. CORS
app.UseCors("DevCors");

// 11. Autenticación / Autorización
app.UseAuthentication();
app.UseAuthorization();

// 12. Mapea tus controladores
app.MapControllers();

app.Run();
