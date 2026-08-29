# TecnoGas Hogar

Prototipo de portal interno de **TecnoGas Hogar**, empresa peruana dedicada al mantenimiento e instalación de artefactos a gas en el hogar.

Aplicación web **ASP.NET Core MVC (.NET 10)** con **C#**, que permite al personal de atención:

- **Registrar** solicitudes de servicio (Insert)
- **Listar** las solicitudes registradas (Select)

La información se persiste en una base de datos **SQLite** usando **Entity Framework Core**.

## Requisitos

- [SDK .NET 10](https://dotnet.microsoft.com/download/dotnet/10.0)
- (Opcional) Docker para el despliegue en Render

## Ejecución local

```bash
dotnet restore
dotnet ef database update
dotnet run
```

La aplicación queda disponible en `https://localhost:5001` (o el puerto que indique la consola).

## Estructura del repositorio

Flujo de ramas utilizado (cada funcionalidad se desarrolló en su propia rama e integró a `develop` vía Pull Request; al final se integró `develop` en `main`):

- `main` → rama de producción (integración final)
- `develop` → rama de integración
- `feature/modelo-sqlite` → modelo de datos, EF Core y SQLite (Pregunta 1)
- `feature/registro-solicitud` → registro de solicitudes (Pregunta 2)
- `feature/listado-solicitudes` → listado de solicitudes (Pregunta 3)

## Modelo de datos

```csharp
public class SolicitudServicio
{
    public int Id { get; set; }
    [Required] public string Cliente { get; set; }
    [Required] public string Telefono { get; set; }
    [Required] public string Distrito { get; set; }
    [Required] public string TipoServicio { get; set; } // Instalación, Mantenimiento, Revisión, Fuga
    public string Descripcion { get; set; }
    public DateTime FechaRegistro { get; set; } = DateTime.Now;
}
```

Cadena de conexión (`appsettings.json`):

```json
"ConnectionStrings": {
  "DefaultConnection": "Data Source=tecnogas.db"
}
```

## Despliegue en Render (Web Service)

El proyecto incluye un **Dockerfile** multi-etapa que compila y publica la aplicación para el despliegue.

### 1. Preparar el repositorio

La aplicación y el `Dockerfile` deben estar subidos a un repositorio público de GitHub (los merges del flujo ya quedaron reflejados).

### 2. Crear el Web Service en Render

1. Ingresar a [Render](https://render.com) → **New** → **Web Service**.
2. Conectar el repositorio GitHub (**evaluacion20262**).
3. Configurar:
   - **Name**: `tecnogas-hogar`
   - **Region**: la más cercana (p. ej. `Oregon (US West)`)
   - **Branch**: `main`
   - **Runtime**: `Docker`
   - **Plan**: gratis (Free) es suficiente para el demo
4. Hacer clic en **Create Web Service**.

Render detecta automáticamente el `Dockerfile` y construye la imagen.

### 3. Detalles del Dockerfile

```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["TecnoGas.Hogar.csproj", "./"]
RUN dotnet restore "TecnoGas.Hogar.csproj"

COPY . .
RUN dotnet publish "TecnoGas.Hogar.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production
EXPOSE 8080

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "TecnoGas.Hogar.dll"]
```

- El puerto de escucha es **8080** (`ASPNETCORE_URLS`), que es el que Render usa por defecto para Web Services.
- No requiere comandos de build/run adicionales; se usa la ruta `/healthz` de Render si se desea healthcheck.

### 4. Acceso y verificación

Una vez desplegado, Render entrega una URL pública del tipo:

```
https://tecnogas-hogar.onrender.com
```

Para verificar el demo:

- **Registrar (Insert)**: `https://tecnogas-hogar.onrender.com/SolicitudServicio/Create`
- **Listar (Select)**: `https://tecnogas-hogar.onrender.com/SolicitudServicio`

### 5. Persistencia de SQLite en Render

> Importante: el filesystem de un Web Service gratuito de Render es **efímero**: se reinicia en cada deploy o ciclo de dormido. El archivo `tecnogas.db` se crea en el contenedor y **no persiste** entre reinicios.

Para persistir los datos en este plan gratuito, montar un **Render Disk** en el directorio de trabajo de la app (`/app`):

1. En el Web Service, sección **Disks**, agregar un disco montado en `/app`.
2. Opcionalmente, definir la variable de entorno `DefaultConnection` apuntando a una ruta dentro del disco, por ejemplo:
   ```
   DefaultConnection=Data Source=/app/tecnogas.db
   ```
3. El deploy inicial crea la base de datos automáticamente al arrancar (la migración se aplica en el primer uso).

Para un demo sin persistencia, no se necesita ningún cambio adicional.
