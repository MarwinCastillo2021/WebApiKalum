using System.ComponentModel.DataAnnotations;
namespace WebApiKalum.Entities
{
        public class CarreraTecnica
        {
            //#nullable disable warnings
            [Required(ErrorMessage = "El campo {0} es Requerido!!!")]
            public string CarreraId { get; set; }
            [Required(ErrorMessage = "El campo {0} es Requerido!!!") ]
            [StringLength(128, MinimumLength = 5, ErrorMessage = "La cantidad minima de caracteres es {2} y maxima {1} para el campo {0}")]
            public string Nombre { get; set;}
            public virtual List<Aspirante> Aspirantes { get; set; } // Aspirantes es le nombre de la lista tipo Aspirante
            public virtual List<Inscripcion> Inscripciones {get;set;}
            public virtual List<InversionCarreraTecnica> InversionesCarreraTecnica {get; set; }

            //#nullable enable warnings
        }
}