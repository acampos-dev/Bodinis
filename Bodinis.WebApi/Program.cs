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
using Bodinis.LogicaAplicacion.CasosDeUso.Caja;
using Bodinis.LogicaAplicacion.CasosDeUso.Gastos;
using Bodinis.LogicaAplicacion.DTOs.Caja;
using Bodinis.LogicaAplicacion.DTOs.Gastos;
using Bodinis.LogicaAplicacion.CasosDeUso.Categorias;
using Bodinis.LogicaAplicacion.CasosDeUso.MetodoPago;
using Bodinis.LogicaAplicacion.CasosDeUso.Pedidos;
using Bodinis.LogicaAplicacion.CasosDeUso.Ventas;
using Bodinis.LogicaAplicacion.DTOs.Categorias;
using Bodinis.LogicaAplicacion.DTOs.MetodoPago;
using Bodinis.LogicaAplicacion.DTOs.Pedidos;
using Bodinis.LogicaAplicacion.DTOs.Ventas;

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
builder.Services.AddScoped<IRepositorioCategoria, RepositorioCategoria>();
builder.Services.AddScoped<IRepositorioCaja, RepositorioCaja>();
builder.Services.AddScoped<IRepositorioGasto, RepositorioGasto>();
builder.Services.AddScoped<IPedidoRepositorio, RepositorioPedido>();
builder.Services.AddScoped<IRepositorioMetodoPago, RepositorioMetodoPago>();
builder.Services.AddScoped<IRepositorioVenta, RepositorioVenta>();

// =======================
// Casos de Uso
// =======================

// Caso de uso: Login Usuario
builder.Services.AddScoped<ILogin<LoginDtoRequest, LoginDtoResponse>, LoginUsuario>();

// Caso de uso: CRUD Producto
builder.Services.AddScoped<ICUAdd<ProductoDtoAlta>, AddProducto>();
builder.Services.AddScoped<ICUDeactivate, DesactivarProducto>();
builder.Services.AddScoped<ICUGetAll<ProductoDtoListado>, GetAllProductos>();
builder.Services.AddScoped<ICUGetById<ProductoDtoListado>, GetProductoById>();
builder.Services.AddScoped<ICUUpdate<ProductoDtoModificar>, UpdateProducto>();

// Caso de uso: CRUD Categoria
builder.Services.AddScoped<ICUAdd<CategoriaDtoAlta>, AddCategoria>();
builder.Services.AddScoped<ICUGetAll<CategoriaDtoListado>, GetAllCategorias>();
builder.Services.AddScoped<ICUGetById<CategoriaDtoListado>, GetCategoriaById>();
builder.Services.AddScoped<ICUUpdate<CategoriaDtoModificar>, UpdateCategoria>();
builder.Services.AddScoped<ICUDelete<CategoriaDtoListado>, DeleteCategoria>();

// Caso de uso: CRUD Metodo de Pago
builder.Services.AddScoped<ICUAdd<MetodoPagoDtoAlta>, AddMetodoPago>();
builder.Services.AddScoped<ICUGetAll<MetodoPagoDtoListado>, GetAllMetodosPago>();
builder.Services.AddScoped<ICUGetById<MetodoPagoDtoListado>, GetMetodoPagoById>();
builder.Services.AddScoped<ICUUpdate<MetodoPagoDtoModificar>, UpdateMetodoPago>();
builder.Services.AddScoped<ICUDelete<MetodoPagoDtoListado>, DeleteMetodoPago>();

// Caso de uso: Pedidos
builder.Services.AddScoped<ICUAdd<PedidoDtoAlta>, AddPedido>();
builder.Services.AddScoped<ICUGetAll<PedidoDtoListado>, GetAllPedidos>();
builder.Services.AddScoped<ICUGetById<PedidoDtoListado>, GetPedidoById>();
builder.Services.AddScoped<ICUCambiarEstadoPedido, CambiarEstadoPedido>();

// Caso de uso: Ventas
builder.Services.AddScoped<ICUAdd<VentaDtoAlta>, RegistrarVenta>();
builder.Services.AddScoped<ICUGetAll<VentaDtoListado>, GetAllVentas>();
builder.Services.AddScoped<ICUGetById<VentaDtoListado>, GetVentaById>();

// Caso de uso: Caja
builder.Services.AddScoped<ICUAdd<CajaDtoAbrir>, AbrirCaja>();
builder.Services.AddScoped<ICUCerrarCaja, CerrarCaja>();
builder.Services.AddScoped<ICUGetCajaActual, GetCajaActual>();

// Caso de uso: Gastos
builder.Services.AddScoped<ICUAdd<GastoDtoAlta>, RegistrarGasto>();
builder.Services.AddScoped<ICUGetGastosPorCajaActual, GetGastosCajaActual>();


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
    .Get<JwtSettings>()
    ?? throw new InvalidOperationException("La configuracion Jwt es obligatoria");

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

    RegistrarBaselineSiLaBaseFueCreadaConEnsureCreated(context);

    // Aplica migraciones antes de ejecutar datos iniciales.
    context.Database.Migrate();

    RepararEsquemaLocalDesfasado(context);

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

static void RegistrarBaselineSiLaBaseFueCreadaConEnsureCreated(BodinisContext context)
{
    context.Database.ExecuteSqlRaw("""
        IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NOT NULL
           AND OBJECT_ID(N'[Categorias]') IS NOT NULL
           AND NOT EXISTS (
                SELECT 1
                FROM [__EFMigrationsHistory]
                WHERE [MigrationId] = N'20260301211924_InitialCreate'
           )
        BEGIN
            INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
            VALUES (N'20260301211924_InitialCreate', N'8.0.20')
        END
        """);
}

static void RepararEsquemaLocalDesfasado(BodinisContext context)
{
    context.Database.ExecuteSqlRaw("""
        IF OBJECT_ID(N'[MetodosPago]') IS NULL
        BEGIN
            CREATE TABLE [MetodosPago] (
                [Id] int NOT NULL IDENTITY,
                [Nombre] nvarchar(80) NOT NULL,
                [Activo] bit NOT NULL,
                CONSTRAINT [PK_MetodosPago] PRIMARY KEY ([Id])
            );

            CREATE UNIQUE INDEX [IX_MetodosPago_Nombre] ON [MetodosPago] ([Nombre]);
        END

        IF OBJECT_ID(N'[Gastos]') IS NULL AND OBJECT_ID(N'[Cajas]') IS NOT NULL
        BEGIN
            CREATE TABLE [Gastos] (
                [Id] int NOT NULL IDENTITY,
                [FechaHora] datetime2 NOT NULL,
                [Descripcion] nvarchar(200) NOT NULL,
                [Monto] int NOT NULL,
                [Categoria] nvarchar(80) NULL,
                [CajaId] int NOT NULL,
                CONSTRAINT [PK_Gastos] PRIMARY KEY ([Id])
            );

            CREATE INDEX [IX_Gastos_CajaId] ON [Gastos] ([CajaId]);
        END
        """);

    context.Database.ExecuteSqlRaw("""
        IF OBJECT_ID(N'[Pedidos]') IS NOT NULL
        BEGIN
            IF COL_LENGTH(N'[Pedidos]', N'NombreCliente') IS NULL
                ALTER TABLE [Pedidos] ADD [NombreCliente] nvarchar(100) NULL;

            IF COL_LENGTH(N'[Pedidos]', N'TelefonoCliente') IS NULL
                ALTER TABLE [Pedidos] ADD [TelefonoCliente] nvarchar(30) NULL;

            IF COL_LENGTH(N'[Pedidos]', N'DireccionCliente') IS NULL
                ALTER TABLE [Pedidos] ADD [DireccionCliente] nvarchar(200) NULL;
        END
        """);

    context.Database.ExecuteSqlRaw("""
        IF OBJECT_ID(N'[Pedidos]') IS NOT NULL
           AND EXISTS (
               SELECT 1
               FROM sys.columns
               WHERE object_id = OBJECT_ID(N'[Pedidos]')
                 AND name = N'TipoPedido'
                 AND system_type_id = TYPE_ID(N'int')
           )
           AND COL_LENGTH(N'[Pedidos]', N'TipoPedidoNuevo') IS NULL
        BEGIN
                ALTER TABLE [Pedidos] ADD [TipoPedidoNuevo] nvarchar(30) NULL;
        END
        """);

    context.Database.ExecuteSqlRaw("""
        IF OBJECT_ID(N'[Pedidos]') IS NOT NULL
           AND EXISTS (
               SELECT 1
               FROM sys.columns
               WHERE object_id = OBJECT_ID(N'[Pedidos]')
                 AND name = N'TipoPedido'
                 AND system_type_id = TYPE_ID(N'int')
           )
           AND COL_LENGTH(N'[Pedidos]', N'TipoPedidoNuevo') IS NOT NULL
        BEGIN
            UPDATE [Pedidos]
               SET [TipoPedidoNuevo] = CASE [TipoPedido]
                    WHEN 2 THEN N'Delivery'
                    ELSE N'Mostrador'
               END;
            ALTER TABLE [Pedidos] DROP COLUMN [TipoPedido];
            EXEC sp_rename N'Pedidos.TipoPedidoNuevo', N'TipoPedido', N'COLUMN';
            ALTER TABLE [Pedidos] ALTER COLUMN [TipoPedido] nvarchar(30) NOT NULL;
        END
        """);

    context.Database.ExecuteSqlRaw("""
        IF OBJECT_ID(N'[Pedidos]') IS NOT NULL
           AND EXISTS (
               SELECT 1
               FROM sys.columns
               WHERE object_id = OBJECT_ID(N'[Pedidos]')
                 AND name = N'Estado'
                 AND system_type_id = TYPE_ID(N'int')
           )
           AND COL_LENGTH(N'[Pedidos]', N'EstadoNuevo') IS NULL
        BEGIN
                ALTER TABLE [Pedidos] ADD [EstadoNuevo] nvarchar(30) NULL;
        END
        """);

    context.Database.ExecuteSqlRaw("""
        IF OBJECT_ID(N'[Pedidos]') IS NOT NULL
           AND EXISTS (
               SELECT 1
               FROM sys.columns
               WHERE object_id = OBJECT_ID(N'[Pedidos]')
                 AND name = N'Estado'
                 AND system_type_id = TYPE_ID(N'int')
           )
           AND COL_LENGTH(N'[Pedidos]', N'EstadoNuevo') IS NOT NULL
        BEGIN
            UPDATE [Pedidos]
               SET [EstadoNuevo] = CASE [Estado]
                    WHEN 2 THEN N'Preparacion'
                    WHEN 3 THEN N'Entregado'
                    WHEN 4 THEN N'Cancelado'
                    ELSE N'Pendiente'
               END;
            ALTER TABLE [Pedidos] DROP COLUMN [Estado];
            EXEC sp_rename N'Pedidos.EstadoNuevo', N'Estado', N'COLUMN';
            ALTER TABLE [Pedidos] ALTER COLUMN [Estado] nvarchar(30) NOT NULL;
        END
        """);

    context.Database.ExecuteSqlRaw("""
        IF OBJECT_ID(N'[Ventas]') IS NOT NULL
        BEGIN
            IF COL_LENGTH(N'[Ventas]', N'MetodoPagoId') IS NULL
                ALTER TABLE [Ventas] ADD [MetodoPagoId] int NOT NULL CONSTRAINT [DF_Ventas_MetodoPagoId] DEFAULT 1;

            IF COL_LENGTH(N'[Ventas]', N'PedidoId') IS NULL
                ALTER TABLE [Ventas] ADD [PedidoId] int NOT NULL CONSTRAINT [DF_Ventas_PedidoId] DEFAULT 1;
        END
        """);

    context.Database.ExecuteSqlRaw("""
        IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NOT NULL
        BEGIN
            IF NOT EXISTS (
                SELECT 1 FROM [__EFMigrationsHistory]
                WHERE [MigrationId] = N'20260522120000_AddGastosCaja'
            )
                INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
                VALUES (N'20260522120000_AddGastosCaja', N'8.0.20');

            IF NOT EXISTS (
                SELECT 1 FROM [__EFMigrationsHistory]
                WHERE [MigrationId] = N'20260522123000_AddPedidosMetodosPagoVentas'
            )
                INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
                VALUES (N'20260522123000_AddPedidosMetodosPagoVentas', N'8.0.20');
        END
        """);
}
