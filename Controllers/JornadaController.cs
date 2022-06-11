using Microsoft.AspNetCore.Mvc;
using WebApiKalum.Entities;
using Microsoft.EntityFrameworkCore;

namespace WebApiKalum.Controllers
{
    [ApiController]
    [Route("v1/KalumManagement/Jornada/")]
    public class JornadaController : ControllerBase
    {
        private readonly KalumDbContext DbContext;
        private readonly ILogger<JornadaController> Logger;
        public JornadaController(KalumDbContext _DbContext, ILogger<JornadaController> _Logger)
        {
            this.DbContext = _DbContext;
            this.Logger = _Logger;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Jornada>>> Get()
        {
            List<Jornada> jornada = null;
            Logger.LogDebug("Iniciando proceso de consulta de Jornada en Base de Datos");
            jornada = await DbContext.Jornada.Include(j => j.Aspirantes).ToListAsync();
            jornada = await DbContext.Jornada.Include(j => j.Inscripciones).ToListAsync();
            if(jornada == null || jornada.Count == 0)
            {
                Logger.LogWarning("No Existe la Jornada en base de Datos");
                return new NoContentResult();
            }
            Logger.LogInformation("Se ejecuto la peticion de forma exitosa");
            return Ok(jornada);
        }
        [HttpGet("{id}", Name = "GetJornada")]
        public async Task<ActionResult<Jornada>>GetJornada(string id)
        {
            Logger.LogDebug("Iniciando el proceso de busqueda de la Jornada con el id :" + id);
            var jornada = await DbContext.Jornada.Include(j => j.Aspirantes).FirstOrDefaultAsync(j => j.JornadaId == id);
                jornada = await DbContext.Jornada.Include(j => j.Inscripciones).FirstOrDefaultAsync(j => j.JornadaId == id);
            if(jornada == null)
            {
                Logger.LogWarning("No Existe la Jornada con el id : " + id);
                return new NoContentResult();
            }
            Logger.LogInformation("Se ejecuto la peticion de forma exitosa");
            return Ok(jornada);
        }
    }
}