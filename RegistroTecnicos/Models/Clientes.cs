using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RegistroTecnicos.Models
{
    public class Clientes
    {
        [Key]
        public int ClienteId { get; set; }

        // Fechalngreso should be DateTime if it's meant to store a date
        public DateTime? Fechalngreso { get; set; }  // Change to DateTime if it's meant to represent a date

        public string? Nombres { get; set; }

        // Fixed typo: changed "Direcion" to "Direccion"
        public string? Direccion { get; set; }

        public string? Rnc { get; set; }

        // Ensure LimiteCredito is decimal
        public decimal LimiteCredito { get; set; }

        // TecnicoId is likely a foreign key for a related entity
        
        [ForeignKey("Tecnicos")]
        public int TecnicoId { get; set; }
        public Tecnicos? Tecnicos { get; set; }
    }
}
