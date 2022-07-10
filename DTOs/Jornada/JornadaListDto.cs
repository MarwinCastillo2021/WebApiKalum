namespace WebApiKalum.DTOs
{
    public class JornadaListDTO
    {
        public string jornada { get; set; }
        public string Descripcion { get; set; }
        public List<AspiranteConsultaJornadaDTO> Aspirantes { get; set; }
        public List<InscripcionConsultaJornadaDTO> Inscripciones { get; set; }
        
    }
}