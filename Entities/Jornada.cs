namespace WebApiKalum.Entities
{
    public class Jornada 
    {
        public string JornadaId { get; set; }
        public string jornada { get; set; }
        public string Description { get; set; }
        public virtual List<Aspirante> Aspirantes { get; set; } 
        public virtual List<Inscripcion> Inscripciones { get; set; }

    }
}