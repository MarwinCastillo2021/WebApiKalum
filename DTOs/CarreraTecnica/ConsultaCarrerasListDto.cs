namespace WebApiKalum.DTOs
{
    public class ConsultaCarrerasListDTO
    {
        public string CarreraId { get; set; }
        public string Nombre { get; set; }
        public List<AspiranteConsultaCarrerasDTO> Aspirantes { get; set; }
        public List<InscripcionConsultaCarrerasDTO> Inscripciones { get; set; }
    }
}