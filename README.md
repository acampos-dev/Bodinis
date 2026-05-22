# Bodini's

Sistema de gestion para una rotiseria con ventas presenciales, pedidos con delivery propio, caja diaria y control de gastos.

## Objetivo

Bodini's busca centralizar la operacion diaria del negocio: productos, pedidos, ventas, caja y gastos. El backend esta construido como una Web API REST y preparado para ser consumido por una aplicacion MVC.

## Tecnologias

- C# / .NET 8
- ASP.NET Core Web API
- ASP.NET Core MVC
- Entity Framework Core
- SQL Server
- JWT Bearer Authentication
- Swagger

## Arquitectura

La solucion esta organizada por capas:

- `Bodinis.LogicaNegocio`: entidades, value objects, excepciones de negocio e interfaces de repositorio.
- `Bodinis.LogicaAplicacion`: casos de uso, DTOs y mappers.
- `Bodinis.Infraestructura`: Entity Framework Core, configuraciones, repositorios y seguridad.
- `Bodinis.WebApi`: controladores, autenticacion, Swagger y configuracion de dependencias.
- `Bodinis.WebApp`: cliente MVC.

## Funcionalidades del MVP

- Autenticacion de usuarios con JWT.
- Gestion de productos y categorias.
- Registro y gestion de pedidos.
- Registro de ventas asociado a caja.
- Apertura, consulta y cierre de caja.
- Registro de gastos asociados a la caja abierta.
- Calculo de totales de caja: monto inicial, ingresos por ventas, egresos por gastos y saldo calculado.

## Gastos y caja

Los gastos funcionan como egresos de caja. Para registrar un gasto debe existir una caja abierta; cada gasto queda asociado a esa caja y se descuenta del saldo calculado.

Datos registrados por gasto:

- Fecha y hora
- Descripcion
- Monto
- Categoria opcional
- Caja asociada

Endpoints principales:

- `GET /api/caja/actual`: obtiene la caja abierta con totales.
- `POST /api/caja/abrir`: abre una caja nueva.
- `POST /api/caja/cerrar`: cierra la caja abierta.
- `GET /api/gastos/caja-actual`: lista los gastos de la caja abierta.
- `POST /api/gastos`: registra un gasto en la caja abierta.

Ejemplo para registrar gasto:

```json
{
  "descripcion": "Compra de insumos",
  "monto": 1500,
  "categoria": "Insumos"
}
```

## Puesta en marcha

1. Configurar la cadena de conexion `Bodinis` en `Bodinis.WebApi/appsettings.json`.
2. Compilar la solucion:

```bash
dotnet build Bodinis.sln
```

3. Ejecutar la Web API:

```bash
dotnet run --project Bodinis.WebApi
```

4. Abrir Swagger en la URL configurada por `launchSettings.json`.

## Estado del proyecto

En desarrollo. El backend ya cuenta con la base para caja y gastos; quedan pendientes integraciones completas de ventas, reportes PDF y vistas MVC segun el alcance final del MVP.
