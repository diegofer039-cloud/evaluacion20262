using Microsoft.EntityFrameworkCore;
using TecnoGas.Hogar.Models;

namespace TecnoGas.Hogar.Data;

public class SolicitudDbContext : DbContext
{
    public SolicitudDbContext(DbContextOptions<SolicitudDbContext> options)
        : base(options)
    {
    }

    public DbSet<SolicitudServicio> SolicitudesServicio { get; set; }
}
