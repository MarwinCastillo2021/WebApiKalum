using Microsoft.AspNetCore.Mvc;
using WebApiKalum.Entities;
using Microsoft.EntityFrameworkCore;
using WebApiKalum.Utilites;
using WebApiKalum.DTOs;
using AutoMapper;

namespace WebApiKalum.Controllers
{
    [ApiController]
    [Route("v1/KalumManagement/Cargos/")]
    public class CargoController : ControllerBase
    {
        private readonly KalumDbContext DbContext;
        private readonly ILogger <CargoController> Logger;
        private readonly IMapper Mapper;

        public CargoController(KalumDbContext _DbContext, ILogger<CargoController> _Logger, IMapper _Mapper)
        {
            this.DbContext = _DbContext;
            this.Logger = _Logger;
            this.Mapper = _Mapper;
        }
       
        [HttpPost]
        public async Task<ActionResult<Cargo>> Post([FromBody] CargoCreateDTO value)
         {
            Logger.LogDebug("Iniciando proceso para agregar un Cargo nuevo");
            Cargo nuevo = Mapper.Map<Cargo>(value);
            nuevo.CargoId = Guid.NewGuid().ToString().ToUpper();
            await DbContext.Cargo.AddAsync(nuevo);
            await DbContext.SaveChangesAsync();
            Logger.LogInformation("Se agrego correctamente el nuevo cargo");
            return new CreatedAtRouteResult("GetCargo", new{id= nuevo.CargoId},nuevo);
         }
       
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CargoListDTO>>> Get()
        {
            List<Cargo> cargo = null;
            Logger.LogDebug("Iniciando Proceso de Consulta de Cargos en Base de Datos");
            cargo = await DbContext.Cargo.ToListAsync();
            if(cargo == null || cargo.Count == 0)
            {
                Logger.LogWarning("No Existen Cargos en la Base de Datos");
                return new NoContentResult();
            }
            Logger.LogInformation("Se ejecuto la peticion de forma exitosa");
            List<CargoListDTO> cargos = Mapper.Map<List<CargoListDTO>>(cargo);
            return Ok(cargos);
        }

        [HttpGet("page/{page}")]
        public async Task<ActionResult<IEnumerable<Cargo>>> GetPaginacion(int page)
        {
            Logger.LogDebug("Iniciando procedimiento para consulta de Cargos Paginado");
            var queryable = this.DbContext.Cargo.AsQueryable();
            var paginacion = new HttpResponsePaginacion<Cargo>(queryable,page);
            await DbContext.SaveChangesAsync();
            if(paginacion.Content == null && paginacion.Content.Count == 0)
            {
                Logger.LogWarning("No existen registros en la Base de Datos");
                return NoContent();
            }
            else
            {
                 Logger.LogInformation("La consulta se realizo de forma exitosa");
                 return Ok(paginacion);
            }
        }

        [HttpGet("{id}", Name = "GetCargo")]
        public async Task<ActionResult<Cargo>> GetCargo(string id)
        {
            Logger.LogDebug("Iniciando el proceso de busqueda del Cargo con el Id : " + id);
            var cargo = await DbContext.Cargo.FirstOrDefaultAsync(c => c.CargoId == id);
            if(cargo == null)
            {
                Logger.LogWarning("No Existe Cargo con el Id : " + id);
                return new NoContentResult();
            }
            Logger.LogInformation("Se ejecuto la peticion de forma exitosa");
            return Ok(cargo);
         }
         
         [HttpPut("{id}")]
         public async Task<ActionResult>Put(string id, [FromBody] Cargo value)
         {
            Logger.LogDebug($"Iniciando proceso de actualizacion de cargo con el id {id}");
            Cargo cargo = await DbContext.Cargo.FirstOrDefaultAsync(ca => ca.CargoId == id);
            if(cargo == null)
            {
                Logger.LogWarning($"No existe el cargo con el Id {id}, no se puede hacer la actualizacion requerida");
                return BadRequest();
            }
            cargo.Descripcion = value.Descripcion;
            cargo.Prefijo = value.Prefijo;
            cargo.Monto = value.Monto;
            DbContext.Entry(cargo).State = EntityState.Modified;
            await DbContext.SaveChangesAsync();
            Logger.LogInformation($"La actualizacion del cargo con el id {id} fue realizada de forma exitosa");
            return Ok(cargo);
         }
         
         [HttpDelete ("{id}")]
         public async Task<ActionResult<Cargo>> Delete(string id)
         {
            Logger.LogDebug($"iniciando el proceso de eliminacion del registro con el Id : {id}");
            Cargo cargo = await DbContext.Cargo.FirstOrDefaultAsync(ca => ca.CargoId == id);
            if(cargo == null)
            {
                Logger.LogWarning($"El cargo con Id {id} no se encontró, no se puede eliminar el registro indicado");
                return NotFound();
            }
            else
            {
                DbContext.Cargo.Remove(cargo);
                await DbContext.SaveChangesAsync().ConfigureAwait(false);
                Logger.LogInformation($"Se elemnino el cargo con el id {id}");
                return Ok(cargo);
            }            
         }
                  
    } 
}