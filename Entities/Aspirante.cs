namespace WebApiKalum.Entities
{
    public class Aspirante
    {
        public string NoExpediente { get; set; }
        public string Apellidos { get; set; }
        public string Nombres { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string Estatus { get; set; }
        public string CarreraId { get; set;} //llave foranea
        public string JornadaId { get; set;} //llave foranea
        public virtual CarreraTecnica CarreraTecnica { get; set; } //objeto con informacion de Carrera Tecnica
        public virtual Jornada Jornada { get; set; } //objeto con informacion de  Jornada      
    }
}