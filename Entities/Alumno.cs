using System.ComponentModel.DataAnnotations;
namespace WebApiKalum.Entities
{
    public class Alumno
    {
        [Required(ErrorMessage = "El campo {0} es Requerido!!!")]
        public string Carne { get; set; }
        [Required(ErrorMessage = "El campo {0} es Requerido!!!")]
        [StringLength(128, MinimumLength=3, ErrorMessage="La cantidad mínima de caracteres es {2} y la máxima {1} para el campo {0}")]
        public string Apellidos { get; set; }
        [Required(ErrorMessage = "El campo {0} es Requerido!!!")]
        [StringLength(128, MinimumLength=3, ErrorMessage="La cantidad mínima de caracteres es {2} y la máxima {1} para el campo {0}")]
        public string Nombres { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public virtual List<Inscripcion> Inscripciones { get; set; }
        public virtual List<CuentaPorCobrar> CuentasPorCobrar { get; set; }

    }
}