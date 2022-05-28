namespace WebApiKalum.Entities
{
    public class Jornada 
    {
        public string JornadaId { get; set; }
        public string jornada { get; set; }
        public string Description { get; set; }
        public virtual List<Jornada> Jornadas { get; set; }

    }
}