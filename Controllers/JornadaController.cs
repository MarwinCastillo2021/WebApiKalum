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
        public async Task<ActionResult<Jornada>> Post([FromBody] Jornada value)
        {
            Logger.LogDebug("Iniciando proceso para agregar una jornada nueva : ");
            value.JornadaId = Guid.NewGuid().ToString().ToUpper();
            await DbContext.Jornada.AddAsync(value);
            await DbContext.SaveChangesAsync();
            Logger.LogInformation("La jornada nueva se ha agregado con exito!!! ");
            return new CreatedAtRouteResult("GetJornada", new{id = value.JornadaId}, value);
        }
        [HttpDelete ("{id}")]
        public async Task<ActionResult<Jornada>> Delete(string id)
        {
            Logger.LogDebug($"Iniciando proceso para eliminar Jornada con Id : {id}");
            Jornada jornada = await DbContext.Jornada.FirstOrDefaultAsync(j => j.JornadaId == id);
            if(jornada == null)
            {
                Logger.LogWarning($"No se encontro la Jornada con el Id {id}, la Jornada no puede ser eliminada");
                return NotFound();
            }
            else
            {
                DbContext.Jornada.Remove(jornada);
                await DbContext.SaveChangesAsync();
                Logger.LogInformation($"La Jornada con id {id} ha sido eliminada de forma correcta!!");
                return(jornada);
            }        
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(string id, [FromBody] Jornada value)
        {
            Logger.LogDebug($"Iniciando el proceso de actualizacion del registro con el Id : {id} ");
            Jornada jornadas = await DbContext.Jornada.FirstOrDefaultAsync(j => j.JornadaId == id);
            if(jornadas == null)
            {
                Logger.LogWarning($"La Jornada con el Id {id} no existe, no se puede eliminar la Jornada");
                return BadRequest();
            }
            jornadas.jornada = value.jornada;
            jornadas.Descripcion = value.Descripcion;
            DbContext.Entry(jornadas).State = EntityState.Modified;
            await DbContext.SaveChangesAsync();
            Logger.LogInformation("Los Datos fueron actualizados correctamente");
            return Ok(jornadas);
        }
    }
}