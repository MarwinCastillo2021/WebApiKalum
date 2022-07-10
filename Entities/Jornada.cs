using System.ComponentModel.DataAnnotations;
namespace WebApiKalum.Entities
{
    public class Jornada 
    {
        [Required(ErrorMessage = "El campo {0} es Requerido!!!")]
        public string JornadaId { get; set;}
        [Required(ErrorMessage = "El campo {0} es Requerido!!!")]
        [StringLength(2, MinimumLength=2, ErrorMessage = "La cantidad mínima de caracteres es {2} y la máxima {1} para el campo {0}" )]
        public string jornada { get; set; }
        public string Descripcion { get; set; }
        public virtual List<Aspirante> Aspirantes { get; set; } 
        public virtual List<Inscripcion> Inscripciones { get; set; }
        
    }
}