using Bodinis.Infraestructura.AccesoDatos.EF;
using Bodinis.Infraestructura.AccesoDatos.EF.Seed;
using Bodinis.LogicaAplicacion.CasosDeUso;
using Bodinis.LogicaAplicacion.DTOs.Usuarios;
using Bodinis.LogicaAplicacion.Interfaces;
using Bodinis.LogicaNegocio.InterfacesRepositorio;
using Bodinis.WebApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Swagger + JWT
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Bodini's API",
        Version = "v1"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Bearer {token}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});


// =======================
// Infraestructura
// =======================
builder.Services.AddDbContext<BodinisContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Bodinis"))
);

builder.Services.AddScoped<IRepositorioUsuario, RepositorioUsuario>();

// =======================
// Casos de Uso
// =======================
builder.Services.AddScoped<SeedData>();
builder.Services.AddScoped<ILogin<LoginRequestDto>, LoginUsuario>();

// =======================
// Seguridad JWT
// =======================

// =======================
// Seguridad JWT
// =======================
builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("Jwt")
);

builder.Services.AddScoped<IJwtGenerator, JwtGenerator>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();

var jwtSettings = builder.Configuration
    .GetSection("Jwt")
    .Get<JwtSettings>();

var key = Encoding.ASCII.GetBytes(jwtSettings.Key);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false
    };
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var seed = scope.ServiceProvider.GetRequiredService<SeedData>();
    seed.Run();
}




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
