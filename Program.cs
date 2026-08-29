using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.Json;
using TecnoGas.Hogar.Data;

var builder = WebApplication.CreateBuilder(args);

// En contenedores con límite bajo de inotify (p. ej. el tier Free de Render), el
// FileSystemWatcher que activa el recargado automático de appsettings.json revienta
// el arranque con "IOException: inotify instances has been reached". Desactivamos la
// recarga de configuración de archivos, que es la que crea esos watchers.
foreach (var source in builder.Configuration.Sources.OfType<JsonConfigurationSource>())
    source.ReloadOnChange = false;

builder.Services.AddDbContext<SolicitudDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Aplicar migraciones al arrancar para asegurar que la base de datos exista
// (útil en entornos desplegados como Render, donde no se ejecuta dotnet ef).
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<SolicitudDbContext>();
    db.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
