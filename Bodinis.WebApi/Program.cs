using Bodinis.Infraestructura.AccesoDatos.EF;
using Bodinis.Infraestructura.AccesoDatos.EF.Seed;
using Bodinis.LogicaAplicacion.DTOs.Usuarios;
using Bodinis.LogicaAplicacion.Interfaces;
using Bodinis.LogicaNegocio.InterfacesLogicaNegocio;
using Bodinis.LogicaNegocio.InterfacesRepositorio;
using Bodinis.WebApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Bodinis.Infraestructura.Seguridad;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Bodinis.LogicaAplicacion.CasosDeUso.Usuarios;
using Bodinis.LogicaNegocio.InterfacesLogicaAplicacion;
using Bodinis.LogicaAplicacion.DTOs.Productos;
using Bodinis.LogicaAplicacion.CasosDeUso.Productos;

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
// Repositorios
// =======================


builder.Services.AddScoped<IRepositorioUsuario, RepositorioUsuario>();
builder.Services.AddScoped<IRepositorioProducto, RepositorioProducto>();

// =======================
// Casos de Uso
// =======================

// Caso de uso: Login Usuario
builder.Services.AddScoped<ILogin<LoginDtoRequest>, LoginUsuario>();

// Caso de uso: CRUD Producto
builder.Services.AddScoped<ICUAdd<ProductoDtoAlta>, AddProducto>();
builder.Services.AddScoped<ICUDeactivate, DesactivarProducto>();
builder.Services.AddScoped<ICUGetAll<ProductoDtoListado>, GetAllProductos>();
builder.Services.AddScoped<ICUGetById<ProductoDtoListado>, GetProductoById>();
builder.Services.AddScoped<ICUUpdate<ProductoDtoModificar>, UpdateProducto>();


// Cargar datos iniciales a la base de datos
builder.Services.AddScoped<SeedData>();

builder.Services.AddDbContext<BodinisContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("Bodinis")
    )
);




// =======================
// Seguridad JWT
// =======================
builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection("Jwt")
);


//Servicio que genera el token JWT
builder.Services.AddScoped<IJwtGenerator, JwtGenerator>();

// Servicio para hashear contraseñas
builder.Services.AddScoped<IPasswordHasher, PasswordHasherBodinis>();

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
