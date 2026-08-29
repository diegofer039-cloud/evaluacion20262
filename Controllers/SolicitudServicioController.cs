using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TecnoGas.Hogar.Data;
using TecnoGas.Hogar.Models;

namespace TecnoGas.Hogar.Controllers;

public class SolicitudServicioController : Controller
{
    private readonly SolicitudDbContext _context;

    public SolicitudServicioController(SolicitudDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Cliente,Telefono,Distrito,TipoServicio,Descripcion")] SolicitudServicio solicitud)
    {
        if (ModelState.IsValid)
        {
            solicitud.FechaRegistro = DateTime.Now;
            _context.Add(solicitud);
            await _context.SaveChangesAsync();
            TempData["Mensaje"] = "Solicitud de servicio registrada correctamente.";
            return RedirectToAction(nameof(Index));
        }
        return View(solicitud);
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var solicitudes = await _context.SolicitudesServicio
            .OrderByDescending(s => s.FechaRegistro)
            .ToListAsync();
        return View(solicitudes);
    }
}
