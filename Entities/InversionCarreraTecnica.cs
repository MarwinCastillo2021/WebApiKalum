namespace WebApiKalum.Entities
{
    public class InversionCarreraTecnica
    {
        public string InversionId { get; set; }
        public string CarreraId { get; set; }
        public Decimal MontoInscripcion { get; set; }
        public int NumeroPagos { get; set;}
        public Decimal MontoPagos { get; set; }
        public virtual CarreraTecnica CarreraTecnica { get; set; }
    }
}