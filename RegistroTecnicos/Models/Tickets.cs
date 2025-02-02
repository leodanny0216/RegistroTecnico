using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RegistroTecnicos.Models;

public class Tickets
{
    [Key]
    public int TicketId { get; set; }

    public DateTime Fecha { get; set; } = DateTime.Now;

    public string? Prioridad { get; set; }

    public string? Asunto { get; set; }

    public string? Descripcion { get; set; }

    public int TiempoInvertido { get; set; }

    // Relación con Tecnico (uno a uno)
    [ForeignKey("Tecnicos")]
    public int TecnicoId { get; set; }
    public Tecnicos? Tecnicos { get; set; }

    // Relación con Cliente (uno a uno)
    [ForeignKey("Cliente")]
    public int ClienteId { get; set; }
    public Clientes? Cliente { get; set; }  // Corrección: Tipo `Clientes` en vez de `Tecnicos`

}