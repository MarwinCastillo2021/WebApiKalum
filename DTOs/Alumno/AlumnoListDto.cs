
namespace WebApiKalum.DTOs
{
    public class AlumnoListDTO
    {
       public string Carne { get; set; }
       public string NombreCompleto { get; set; }
       public string Email { get; set; }
       public List<InscripcionConsultaAlumnoDTO> Inscripciones { get; set; }
       public List<CuentaPorCobrarConsultaAlumnoDTO> CuentasPorCobrar { get; set; }

    }
}