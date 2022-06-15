using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace WebApiKalum.Entities
{
    public class Cargo
    {
        [Required(ErrorMessage = "El campo {0} es Requerido!!!")]
        public string CargoId { get; set; }
        [Required(ErrorMessage = "El campo {0} es Requerido!!!")]
        [StringLength(128,MinimumLength=10, ErrorMessage="La cantidad mínima de caracteres es {2} y la máxima {1} para el campo {0}")]
        public string Descripcion { get; set; }
        [Required(ErrorMessage = "El campo {0} es Requererido!!!")]
        [StringLength(64,MinimumLength=3, ErrorMessage="La cantidad mínima de caracteres es {2} y la máxima {1} para el campo {0}")]
        public string Prefijo { get; set; }
        [Required]
        [Precision(10,2)]
        public decimal Monto { get; set; }
        public bool GeneraMora { get; set; }
        public int PorcentajeMora { get; set; }
        public virtual List<CuentaPorCobrar> CuentasPorCobrar { get; set; }

    }
}