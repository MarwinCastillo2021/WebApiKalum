namespace WebApiKalum.DTOs
{
    public class InscripcionConsultaJornadaDTO
    {
        public string NombreCompleto { get; set; }
        public string Carne { get; set; }
        public string Ciclo { get; set; }
        public DateTime FechaInscripcion { get; set; }

        public List<AspiranteConsultaJornadaDTO> Aspirantes { get; set; }
    
    }
}