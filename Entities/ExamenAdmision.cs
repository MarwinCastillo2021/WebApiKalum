using System.ComponentModel.DataAnnotations;
namespace WebApiKalum.Entities
{
    public class ExamenAdmision
    {
        [Required(ErrorMessage = "El campo {0} es Requerido!!!")]
        public string ExamenId { get; set; }
        [Required(ErrorMessage = "El campo {0} es Requerido!!!")]
        //[DisplayFormat(DataFormatString = "{yyyy-mm-dd}")]
        //[DataType(DataType.Date)]
        public DateTime FechaExamen { get; set;}
        public virtual List<Aspirante> Aspirantes { get; set; }
    }
}
