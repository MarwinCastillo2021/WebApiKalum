using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
namespace WebApiKalum.Entities
{
    public class CuentaPorCobrar
    {
        public string Cargo { get; set; }
        public string Carne { get; set; }
        public string Anio { get; set; }
        public string CargoId { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaCargo { get; set;}
        public DateTime FechaAplica { get; set;}
        [Required]
        [Precision(10,2)]
        public decimal Monto { get; set; }
        [Required]
        [Precision(10,2)]
        public decimal Mora { get; set;}
        [Required]
        [Precision(10,2)]
        public decimal Descuento { get; set; }
        public virtual Alumno Alumno{ get; set;}
        public virtual Cargo Cargos { get; set; }

    }
}