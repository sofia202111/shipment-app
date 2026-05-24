using System.ComponentModel.DataAnnotations;

namespace ShipmentsApp.Models;

public class Shipment
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El número de guía es obligatorio")]
    public string TrackingNumber { get; set; } = "";

    [Required(ErrorMessage = "Obligatorio")]
    public string PaisOrigen { get; set; } = "";

    [Required(ErrorMessage = "Obligatorio")]
    public string PaisDestino { get; set; } = "";

    [Required(ErrorMessage = "Obligatorio")]
    public string CiudadOrigen { get; set; } = "";

    [Required(ErrorMessage = "Obligatorio")]
    public string CiudadDestino { get; set; } = "";

    [Required(ErrorMessage = "Obligatorio")]
    public string NombreRemitente { get; set; } = "";

    [Required(ErrorMessage = "Obligatorio")]
    public string NombreDestinatario { get; set; } = "";

    [Required(ErrorMessage = "Obligatorio")]
    public string DescripcionMercancia { get; set; } = "";

    [Range(0.01, 9999, ErrorMessage = "El peso debe ser mayor que cero")]
    public decimal PesoKg { get; set; }

    public int Estado { get; set; } = 0;
    // 0=Creado 1=EnTransito 2=Entregado 3=Cancelado

    public DateTime FechaCreacion { get; set; } = DateTime.Now;

    [Required(ErrorMessage = "Obligatorio")]
    public DateTime FechaEstimadaEntrega { get; set; }

    // Solo para mostrar el texto del estado en la vista
    public string EstadoTexto => Estado switch
    {
        0 => "Creado",
        1 => "En tránsito",
        2 => "Entregado",
        3 => "Cancelado",
        _ => "Desconocido"
    };
}
