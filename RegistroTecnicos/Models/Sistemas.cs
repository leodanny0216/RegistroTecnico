using System.ComponentModel.DataAnnotations;

namespace RegistroTecnicos.Modelsss
{
    public class Sistemas
    {
        [Key]
        public int SistemaId { get; set; }

        [Required(ErrorMessage = "Este campo es requerido")]
        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "El campo es requerido")]
        public string? Complejidad { get; set; }
    }
}
