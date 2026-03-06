using Bodinis.Infraestructura.AccesoDatos.EF;
using Bodinis.Infraestructura.AccesoDatos.EF.Seed;
using Bodinis.Infraestructura.Seguridad;
using Bodinis.LogicaAplicacion.CasosDeUso.Cajas;
using Bodinis.LogicaAplicacion.CasosDeUso.Categorias;
using Bodinis.LogicaAplicacion.CasosDeUso.Pedidos;
using Bodinis.LogicaAplicacion.CasosDeUso.Productos;
using Bodinis.LogicaAplicacion.CasosDeUso.Usuarios;
using Bodinis.LogicaAplicacion.CasosDeUso.Ventas;
using Bodinis.LogicaAplicacion.DTOs.Categorias;
using Bodinis.LogicaAplicacion.DTOs.Productos;
using Bodinis.LogicaAplicacion.DTOs.Usuarios;
using Bodinis.LogicaAplicacion.Interfaces;
using Bodinis.LogicaNegocio.Entidades;
using Bodinis.LogicaNegocio.InterfacesLogicaAplicacion;
using Bodinis.LogicaNegocio.InterfacesLogicaNegocio;
using Bodinis.LogicaNegocio.InterfacesRepositorio;
using Bodinis.WebApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

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

// Repositorios
builder.Services.AddScoped<IRepositorioUsuario, RepositorioUsuario>();
builder.Services.AddScoped<IRepositorioProducto, RepositorioProducto>();
builder.Services.AddScoped<IRepositorioCategoria, RepositorioCategoria>();
builder.Services.AddScoped<IRepositorioPedido, RepositorioPedido>();
builder.Services.AddScoped<IRepositorioVenta, RepositorioVenta>();
builder.Services.AddScoped<IRepositorioCaja, RepositorioCaja>();

// Casos de uso base
builder.Services.AddScoped<ILogin<LoginDtoRequest>, LoginUsuario>();

builder.Services.AddScoped<ICUAdd<ProductoDtoAlta>, AddProducto>();
builder.Services.AddScoped<ICUDeactivate, DesactivarProducto>();
builder.Services.AddScoped<ICUGetAll<ProductoDtoListado>, GetAllProductos>();
builder.Services.AddScoped<ICUGetById<ProductoDtoListado>, GetProductoById>();
builder.Services.AddScoped<ICUUpdate<ProductoDtoModificar>, UpdateProducto>();

builder.Services.AddScoped<ICUAdd<CategoriaDtoAlta>, AddCategoria>();
builder.Services.AddScoped<ICUGetAll<CategoriaDtoListado>, GetAllCategorias>();
builder.Services.AddScoped<ICUGetById<CategoriaDtoListado>, GetCategoriaById>();
builder.Services.AddScoped<ICUUpdate<CategoriaDtoModificar>, UpdateCategoria>();
builder.Services.AddScoped<ICUDelete<Categoria>, DeleteCategoria>();

// Casos de uso pedidos/ventas/caja
builder.Services.AddScoped<ICUCrearPedido, CrearPedido>();
builder.Services.AddScoped<ICUGetPedidoById, GetPedidoById>();
builder.Services.AddScoped<ICUGetResumenPedidos, GetResumenPedidos>();
builder.Services.AddScoped<ICUGetResumenVentasDia, GetResumenVentasDia>();
builder.Services.AddScoped<ICUGetResumenVentasMes, GetResumenVentasMes>();
builder.Services.AddScoped<ICUAbrirCaja, AbrirCaja>();
builder.Services.AddScoped<ICUCerrarCaja, CerrarCaja>();
builder.Services.AddScoped<ICUGetCajaAbierta, GetCajaAbierta>();

builder.Services.AddScoped<SeedData>();

builder.Services.AddDbContext<BodinisContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Bodinis")));

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));

builder.Services.AddScoped<IJwtGenerator, JwtGenerator>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasherBodinis>();

var jwtSettings = builder.Configuration.GetSection("Jwt").Get<JwtSettings>();
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

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<BodinisContext>();
    context.Database.EnsureCreated();

    var seed = services.GetRequiredService<SeedData>();
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

