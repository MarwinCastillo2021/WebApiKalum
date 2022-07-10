using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
namespace WebApiKalum.Entities
{
    public class InversionCarreraTecnica
    {
        [Required(ErrorMessage = "El campo {0} es Requerido!!!")]
        public string InversionId { get; set; }
        public string CarreraId { get; set; }
        
        [Required(ErrorMessage = "El campo {0} es Requerido!!!")]
        [Precision(10,2)]
        public decimal MontoInscripcion { get; set; }
        [Required(ErrorMessage = "El campo {0} es Requerido!!!")]
        public int NumeroPagos { get; set;}
        [Required(ErrorMessage = "El campo {0} es Requerido!!!")]
        [Precision(10,2)]
        public decimal MontoPagos { get; set; }
        public virtual CarreraTecnica CarreraTecnica { get; set; }
    }
}