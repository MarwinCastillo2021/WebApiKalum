using System.ComponentModel.DataAnnotations;


namespace WebApiKalum.Entities
{
    public class Inscripcion
    {
         [Required(ErrorMessage = "El campo {0} es Requerido!!!")]
        public string InscripcionId { get; set; }
        public string Carne { get; set; }
        public string CarreraId { get; set; }
        public string JornadaId { get; set; }
        [Required(ErrorMessage = "El campo {0} es Requerido!!!")]
        [StringLength(4, MinimumLength=4, ErrorMessage = "La cantidad mínima de caracteres es {2} y la máxima {1} para el campo {0}" )]
        public string Ciclo { get; set; }
        public DateTime FechaInscripcion { get; set; }
        public virtual CarreraTecnica CarreraTecnica { get; set; }
        public virtual Jornada Jornada { get; set; }
        public virtual Alumno Alumno { get; set; }
    }
}