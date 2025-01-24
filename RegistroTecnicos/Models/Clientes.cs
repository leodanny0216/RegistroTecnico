using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RegistroTecnicos.Models;

public class Clientes
{
    [Key]
    public int ClienteId { get; set; }

    [Required(ErrorMessage = "Llenar este campo por favor.")]
    public string? Nombres { get; set; }
  
    [Required(ErrorMessage = "Llenar este campo por favor.")]
    public string? Direccion { get; set; }

    [Required(ErrorMessage = "Llenar este campo por favor.")]
    [MaxLength(10, ErrorMessage = "El RNC no debe exceder los 10 caracteres.")]
    public string? Rnc { get; set; }

    [Required(ErrorMessage = "Llenar este campo por favor.")]
    [Range(0, double.MaxValue, ErrorMessage = "El límite de crédito debe ser un número positivo.")]
    public decimal LimiteCredito { get; set; }

    [Required(ErrorMessage = "Llenar este campo por favor.")]
    public DateTime FechaIngreso { get; set; }

    [ForeignKey("TecnicoId")]
    public int TecnicoId { get; set; }

    public Tecnicos? TecnicosId { get; set; }
}