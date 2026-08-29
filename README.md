# TecnoGas Hogar

Prototipo de portal interno de **TecnoGas Hogar**, empresa peruana dedicada al mantenimiento e instalación de artefactos a gas en el hogar.

Aplicación web **ASP.NET Core MVC (.NET 10)** con **C#**, que permite al personal de atención:

- **Registrar** solicitudes de servicio (Insert)
- **Listar** las solicitudes registradas (Select)

La información se persiste en una base de datos **SQLite** usando **Entity Framework Core**.

## Requisitos

- [SDK .NET 10](https://dotnet.microsoft.com/download/dotnet/10.0)

## Ejecución local

```bash
dotnet restore
dotnet ef database update
dotnet run
```

## Estructura del repositorio

- `feature/modelo-sqlite` → modelo de datos, EF Core y SQLite (Pregunta 1)
- `feature/registro-solicitud` → registro de solicitudes (Pregunta 2)
- `feature/listado-solicitudes` → listado de solicitudes (Pregunta 3)

## Despliegue

_(Instrucciones de despliegue en Render próximamente)_
