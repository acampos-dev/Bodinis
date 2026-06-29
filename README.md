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
- Gestion de metodos de pago.
- Registro de ventas asociadas a pedido, metodo de pago y caja.
- Apertura, consulta y cierre de caja.
- Registro de gastos asociados a la caja abierta.
- Calculo de totales de caja: monto inicial, ingresos por ventas, egresos por gastos y saldo calculado.
- Migraciones EF Core y datos iniciales para probar el sistema.

## Credenciales de prueba

Administrador:

```text
Email: admin@bodinis.com
Password: Admin123
```

Empleado:

```text
Email: empleado@bodinis.com
Password: Empleado@123
```

## Endpoints principales

Autenticacion:

- `POST /api/auth/login`
- `POST /api/auth/logout`

Categorias:

- `GET /api/categorias`
- `GET /api/categorias/{id}`
- `POST /api/categorias`
- `PUT /api/categorias/{id}`
- `DELETE /api/categorias/{id}`

Productos:

- `GET /api/productos`
- `GET /api/productos/{id}`
- `POST /api/productos`
- `PUT /api/productos/{id}`
- `PATCH /api/productos/{id}/desactivar`

Pedidos:

- `GET /api/pedidos`
- `GET /api/pedidos/{id}`
- `POST /api/pedidos`
- `PATCH /api/pedidos/{id}/estado`

Ventas:

- `GET /api/ventas`
- `GET /api/ventas/{id}`
- `POST /api/ventas`

Caja y gastos:

- `GET /api/caja/actual`
- `POST /api/caja/abrir`
- `POST /api/caja/cerrar`
- `GET /api/gastos/caja-actual`
- `POST /api/gastos`

## Gastos y caja

Los gastos funcionan como egresos de caja. Para registrar un gasto debe existir una caja abierta; cada gasto queda asociado a esa caja y se descuenta del saldo calculado.

Datos registrados por gasto:

- Fecha y hora
- Descripcion
- Monto
- Categoria opcional
- Caja asociada

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

Con el perfil `http`, Swagger queda disponible en:

```text
http://localhost:5260/swagger
```

5. Ejecutar la WebApp MVC:

```bash
dotnet run --project Bodinis.WebApp --launch-profile http
```

La WebApp queda disponible en:

```text
http://localhost:5250
```

## Flujo sugerido para probar

1. Hacer login con el usuario administrador y copiar el token JWT.
2. En Swagger, usar `Authorize` con `Bearer {token}`.
3. Consultar categorias, productos y metodos de pago.
4. Abrir caja con `POST /api/caja/abrir`.
5. Crear un pedido con productos existentes.
6. Registrar la venta del pedido con un metodo de pago.
7. Registrar un gasto de caja.
8. Consultar `GET /api/caja/actual` para ver ingresos, egresos y saldo.

## Estado del proyecto

MVP funcional con API y WebApp MVC para operar login, caja, productos, pedidos, ventas y gastos.
Quedan como proximos pasos opcionales: reportes PDF, tests automatizados y mejoras de gestion de usuarios.
