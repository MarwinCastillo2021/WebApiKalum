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
    }
}