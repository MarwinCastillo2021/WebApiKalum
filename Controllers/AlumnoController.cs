using Microsoft.AspNetCore.Mvc;
using WebApiKalum.Entities;
using Microsoft.EntityFrameworkCore;
namespace WebApiKalum.Controllers
{
    [ApiController]
    [Route("v1/KalumManagement/Alumno/")]
    public class AlumnoController : ControllerBase
    {
        private readonly KalumDbContext DbContext;
        private readonly ILogger<AlumnoController> Logger;
        public AlumnoController(KalumDbContext _DbContext, ILogger<AlumnoController> _Logger)
        {
            this.DbContext = _DbContext;
            this.Logger = _Logger;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Alumno>>> Get()
        {
            List<Alumno> Alumnos = null;
            Logger.LogDebug("Iniciando consulta de Alumnos en Base de Datos");
            Alumnos = await DbContext.Alumno.Include(al => al.Inscripciones).ToListAsync();
            Alumnos = await DbContext.Alumno.Include(al => al.CuentasPorCobrar).ToListAsync();
            if (Alumnos == null || Alumnos.Count == 0)
            {
                Logger.LogWarning("No Existe Alumno");
                return new NoContentResult();
            }
            Logger.LogInformation("Se ejecuto la peticion de forma exitosa");
            return Ok(Alumnos);
        }
        [HttpGet("{id}", Name = "GetAlumnos")]
        public async Task<ActionResult<Alumno>> GetAlumno(string id)
        {
            Logger.LogDebug("Iniciando el proceso de busqueda de Alumno con el Carne :" + id);
            var alumnos = await DbContext.Alumno.Include(al => al.Inscripciones)
                                .Include(al => al.CuentasPorCobrar)
                                .FirstOrDefaultAsync(al => al.Carne == id);
            if(alumnos == null)
            {
                Logger.LogWarning("No Existe el Alumno con el Carne : " + id);
                return new NoContentResult();
            }
            Logger.LogInformation("Finalizado el proceso de busqueda de forma exitosa");
            return Ok(alumnos);
        }
    }
}