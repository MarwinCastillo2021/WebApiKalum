using Microsoft.AspNetCore.Mvc;
using WebApiKalum.Entities;
using Microsoft.EntityFrameworkCore;

namespace WebApiKalum.Controllers
{
    [ApiController]
    [Route("v1/KalumManagement/CarreraTecnica/")]  // v1 : version  KalumManagement : ruta

    public class CarreraTecnicaController : ControllerBase
    {
        private const string V = "GetCarreraTecnica,";
        private readonly KalumDbContext DbContext;
        private readonly ILogger<CarreraTecnicaController> Logger;
        public CarreraTecnicaController(KalumDbContext _DbContext, ILogger<CarreraTecnicaController> _Logger) 
        {
            this.DbContext = _DbContext;
            this.Logger = _Logger;
        }
        [HttpGet]
        //public ActionResult<List<CarreraTecnica>> Get()
        public async Task<ActionResult<IEnumerable<CarreraTecnica>>> Get() //Metodo para Listar todas las carreras
        {
            List<CarreraTecnica> carrerasTecnicas = null;
            Logger.LogDebug("Iniciando Procesos de consulta de carreras tecnicas en la BD");
            //tarea 1
            carrerasTecnicas = await DbContext.CarreraTecnica.Include(c => c.Aspirantes).ToListAsync();
            carrerasTecnicas = await DbContext.CarreraTecnica.Include(c => c.Inscripciones).ToListAsync();
            
            //tarea2
            if(carrerasTecnicas == null || carrerasTecnicas.Count == 0)
            {
                Logger.LogWarning("No Existe Carrera Tecnica");
                return new NoContentResult();
            }
            Logger.LogInformation("Se ejecuto la petición de forma exitosa");
            return Ok(carrerasTecnicas);
        }
        [HttpGet("{id}", Name = "GetCarreraTecnica")]
        public async Task<ActionResult<CarreraTecnica>>GetCarreraTecnica(string id) //Metodo para listar carrera por id
        {
            Logger.LogDebug("Iniciando el proceso de busqueda con el id" + id);
            var carrera = await DbContext.CarreraTecnica.Include(c => c.Aspirantes).FirstOrDefaultAsync(c => c.CarreraId == id);
                carrera = await DbContext.CarreraTecnica.Include(c => c.Inscripciones).FirstOrDefaultAsync(c => c.CarreraId == id);
            if(carrera == null)
            {
                Logger.LogWarning("No existe la carrera tecnica con el id" + id);
                return new NoContentResult();
            }
            Logger.LogInformation("Finalizando el proceso de busqueda de forma exitosa");
            return Ok(carrera);
        }
        public async Task<ActionResult<CarreraTecnica>> Post([FromBody] CarreraTecnica value) 
        {
            Logger.LogDebug("Iniciando procso de agregar una carrera tecnica nueva");
            value.CarreraId =Guid.NewGuid().ToString().ToUpper();
            await DbContext.CarreraTecnica.AddAsync(value);
            await DbContext.SaveChangesAsync();
            Logger.LogInformation("Finalizando el proceso de agregar una Carrera Tecnica");
            return new CreatedAtRouteResult("GetCarreraTecnica", new{id = value.CarreraId}, value);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<CarreraTecnica>> Delete(string id)
        {
            Logger.LogDebug("Iniciando el proceso de eliminacion del registro");
            CarreraTecnica carreraTecnica = await DbContext.CarreraTecnica.FirstOrDefaultAsync(ct => ct.CarreraId == id);
            if(carreraTecnica == null)
            {
                Logger.LogWarning($"No se encontro la carrera tecnica con el id {id}");
                return NotFound();
            }
            else
            {
                DbContext.CarreraTecnica.Remove(carreraTecnica);
                await DbContext.SaveChangesAsync();
                Logger.LogInformation($"Se ha eliminado correctamente la carrera tecnica con el id {id}");
                return (carreraTecnica);
            }
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(string id, [FromBody] CarreraTecnica value)
        {
            Logger.LogDebug($"iniciando proceso de actualizacion de la carrera tecnica con el id {id}");
            CarreraTecnica carreraTecnica = await DbContext.CarreraTecnica.FirstOrDefaultAsync(ct => ct.CarreraId == id);
            if(carreraTecnica == null)
            {
                Logger.LogWarning($"No existe la carrera tecnica con el Id {id}");
                return BadRequest();
            }
            carreraTecnica.Nombre = value.Nombre;
            DbContext.Entry(carreraTecnica).State = EntityState.Modified;
            await DbContext.SaveChangesAsync();
            Logger.LogInformation("Los datos han sido actualizados correctamente");
            return NoContent();            
        }
    }
}
