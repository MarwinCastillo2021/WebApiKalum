using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace WebApiKalum.DTOs
{
    public class CuentaPorCobrarConsultaAlumnoDTO
    {
         public string Cargo { get; set; }
         public string Descripcion { get; set; }
         [Required]
         [Precision(10,2)]
         public decimal Monto { get; set; }
    }

}