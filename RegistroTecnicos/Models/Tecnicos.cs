﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RegistroTecnicos.Models;
public class Tecnicos
{
    [Key]
    public int TecnicoId { get; set; }

    [Required(ErrorMessage = "Este campo es requerido")]
    public string Nombres { get; set; }

    [Required(ErrorMessage = "Llenar este campo es hobligatorio.")]
    public decimal? Sueldohora { get; set; }

    public Clientes ? Clientes  { get; set; }


    
}