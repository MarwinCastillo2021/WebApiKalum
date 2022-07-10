namespace WebApiKalum.DTOs
{
    public class ExamenAdmisionListDTO
    {
         public DateTime FechaExamen { get; set;}
         public List<AspiranteConsultaExamenAdmisionDTO> Aspirantes { get; set; }

    }
}