namespace WebApiKalum.DTOs
{
    public class InversionCarreraTecnicaListDTO
    {
        public string InversionId { get; set; }
        public string MontoInscripcion { get; set; }
        public string NumeroPagos { get; set; }
        public string MontoPagos { get; set; }
        public  CarreraTecnicaCreateDTO CarreraTecnica {get; set; }
    }
}