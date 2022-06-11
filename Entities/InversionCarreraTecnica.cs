using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
namespace WebApiKalum.Entities
{
    public class InversionCarreraTecnica
    {
        public string InversionId { get; set; }
        public string CarreraId { get; set; }
        [Required]
        [Precision(10,2)]
        public decimal MontoInscripcion { get; set; }
        public int NumeroPagos { get; set;}
        [Required]
        [Precision(10,2)]
        public decimal MontoPagos { get; set; }
        public virtual CarreraTecnica CarreraTecnica { get; set; }
    }
}