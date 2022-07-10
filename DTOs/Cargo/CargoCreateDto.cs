namespace WebApiKalum.DTOs
{
    public class CargoCreateDTO
    {
        
        public string Descripcion { get; set; }
        public string Prefijo { get; set; }
        public decimal Monto { get; set; }
        public bool GeneraMora { get; set; }
        public int PorcentajeMora { get; set; }
    }
}