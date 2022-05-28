namespace WebApiKalum.Entities
{
        public class CarreraTecnica
        {
            //#nullable disable warnings
            public string CarreraId { get; set; }
            public string Nombre { get; set;}
            public virtual List<Aspirante> Aspirantes { get; set; } // Aspirantes es le nombre de la lista tipo Aspirante
            //#nullable enable warnings
        }
}