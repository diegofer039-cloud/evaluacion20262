using System.ComponentModel.DataAnnotations;

namespace TecnoGas.Hogar.Models;

public class SolicitudServicio
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El cliente es obligatorio")]
    public string Cliente { get; set; } = string.Empty;

    [Required(ErrorMessage = "El teléfono es obligatorio")]
    [Phone(ErrorMessage = "Ingrese un teléfono válido")]
    [StringLength(20, ErrorMessage = "El teléfono no puede superar los 20 caracteres")]
    public string Telefono { get; set; } = string.Empty;

    [Required(ErrorMessage = "El distrito es obligatorio")]
    public string Distrito { get; set; } = string.Empty;

    [Required(ErrorMessage = "El tipo de servicio es obligatorio")]
    public string TipoServicio { get; set; } = string.Empty;

    public string? Descripcion { get; set; }

    public DateTime FechaRegistro { get; set; } = DateTime.Now;
}
