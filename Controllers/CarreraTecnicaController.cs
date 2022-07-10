using Microsoft.AspNetCore.Mvc;
using WebApiKalum.Entities;
using Microsoft.EntityFrameworkCore;
using WebApiKalum.DTOs;
using AutoMapper;
using WebApiKalum.Utilites;

namespace WebApiKalum.Controllers
{
    [ApiController]
    [Route("v1/KalumManagement/CarrerasTecnicas/")]  // v1 : version  KalumManagement : ruta

    public class CarreraTecnicaController : ControllerBase
    {
        private const string V = "GetCarreraTecnica,";
        private readonly KalumDbContext DbContext;
        private readonly ILogger<CarreraTecnicaController> Logger;
        private readonly IMapper Mapper;
        public CarreraTecnicaController(KalumDbContext _DbContext, ILogger<CarreraTecnicaController> _Logger, IMapper _Mapper) 
        {
            this.DbContext = _DbContext;
            this.Logger = _Logger;
            this.Mapper = _Mapper;
        }
        
        [HttpPost]
        //public async Task<ActionResult<CarreraTecnica>> Post([FromBody]) CarreraTecnica value)
        public async Task<ActionResult<CarreraTecnica>> Post([FromBody] CarreraTecnicaCreateDTO value) 
        {
            Logger.LogDebug("Iniciando proceso de agregar una carrera tecnica nueva");
            CarreraTecnica nuevo = Mapper.Map<CarreraTecnica>(value);
            //value.CarreraId =Guid.NewGuid().ToString().ToUpper();
            nuevo.CarreraId =Guid.NewGuid().ToString().ToUpper();
            //await DbContext.CarreraTecnica.AddAsync(value);
            await DbContext.CarreraTecnica.AddAsync(nuevo);
            await DbContext.SaveChangesAsync();
            Logger.LogInformation("Finalizando el proceso de agregar una Carrera Tecnica");
            //return new CreatedAtRouteResult("GetCarreraTecnica", new{id = value.CarreraId}, value);
            return new CreatedAtRouteResult("GetCarreraTecnica", new{id = nuevo.CarreraId}, nuevo);
        }

        [HttpGet]                                            
        public async Task<ActionResult<IEnumerable<ConsultaCarrerasListDTO>>> Get() //Metodo para Listar todas las carreras
        {
            List<CarreraTecnica> carrerasTecnicas = null;
            Logger.LogDebug("Iniciando Procesos de consulta de carreras tecnicas en la BD");
            //tarea 1
            carrerasTecnicas = await DbContext.CarreraTecnica.Include(c => c.Aspirantes).Include(c => c.Inscripciones).ToListAsync();
           //tarea2
            if(carrerasTecnicas == null || carrerasTecnicas.Count == 0)
            {
                Logger.LogWarning("No Existe Carrera Tecnica");
                return new NoContentResult();
            }
            List<ConsultaCarrerasListDTO> carreras = Mapper.Map<List<ConsultaCarrerasListDTO>>(carrerasTecnicas);
            Logger.LogInformation("Se ejecuto la petición de forma exitosa");
            return Ok(carreras);
        }

        [HttpGet("page/{page}")]
        public async Task<ActionResult<IEnumerable<ConsultaCarrerasListDTO>>> GetPaginacion(int page)
        {
            var queryable = this.DbContext.CarreraTecnica.Include(ct => ct.Aspirantes).Include(ct=>ct.Inscripciones).AsQueryable();
            var paginacion = new HttpResponsePaginacion<CarreraTecnica>(queryable,page);
            await DbContext.SaveChangesAsync();
            
            if(paginacion.Content == null && paginacion.Content.Count == 0)
            {
                return NoContent();
            }
            else
            {

                return Ok(paginacion);
            }
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
            return Ok(carreraTecnica);            
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
    }
}
