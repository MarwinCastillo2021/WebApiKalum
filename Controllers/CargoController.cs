using Microsoft.AspNetCore.Mvc;
using WebApiKalum.Entities;
using Microsoft.EntityFrameworkCore;
namespace WebApiKalum.Controllers
{
    [ApiController]
    [Route("v1/KalumManagement/Cargo/")]
    public class CargoController : ControllerBase
    {
        private readonly KalumDbContext DbContext;
        private readonly ILogger <CargoController> Logger;
        public CargoController(KalumDbContext _DbContext, ILogger<CargoController> _Logger)
        {
            this.DbContext = _DbContext;
            this.Logger = _Logger;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cargo>>> Get()
        {
            List<Cargo> cargo = null;
            Logger.LogDebug("Iniciando Proceso de Consulta de Cargos en Base de Datos");
            cargo = await DbContext.Cargo.Include(c => c.CuentasPorCobrar).ToListAsync();
            if(cargo == null || cargo.Count == 0)
            {
                Logger.LogWarning("No Existen Cargos en la Base de Datos");
                return new NoContentResult();
            }
            Logger.LogInformation("Se ejecuto la peticion de forma exitosa");
            return Ok(cargo);
        }
        [HttpGet("{id}", Name = "GetCargo")]
        public async Task<ActionResult<Cargo>> GetCargo(string id)
        {
            Logger.LogDebug("Iniciando el proceso de busqueda del Cargo con el Id : " + id);
            var cargo = await DbContext.Cargo.Include(c => c.CuentasPorCobrar).FirstOrDefaultAsync(c => c.CargoId == id);
            if(cargo == null)
            {
                Logger.LogWarning("No Existe Cargo con el Id : " + id);
                return new NoContentResult();
            }
            Logger.LogInformation("Se ejecuto la peticion de forma exitosa");
            return Ok(cargo);
         }
         public async Task<ActionResult<Cargo>> Post([FromBody] Cargo value)
         {
            Logger.LogDebug("Iniciando proceso para agregar un Cargo nuevo");
            value.CargoId = Guid.NewGuid().ToString().ToUpper();
            await DbContext.Cargo.AddAsync(value);
            await DbContext.SaveChangesAsync();
            Logger.LogInformation("Se agrego correctamente el nuevo cargo");
            return new CreatedAtRouteResult("GetCargo", new{id= value.CargoId},value);
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
                return(cargo);
            }            
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
            DbContext.Entry(cargo).State = EntityState.Modified;
            await DbContext.SaveChangesAsync();
            Logger.LogInformation($"La actualizacion del cargo con el id {id} fue realizada de forma exitosa");
            return Ok(cargo);
         }
    } 
}