using Microsoft.AspNetCore.Mvc;
using WebApiKalum.Entities;
using Microsoft.EntityFrameworkCore;
using WebApiKalum.DTOs;
using AutoMapper;
using WebApiKalum.Utilites;

namespace WebApiKalum.Controllers
{
    [ApiController]
    [Route("v1/KalumManagement/CuentasPorCobrar/")]
    
    public class CuentaPorCobrarController : ControllerBase
    {
        private readonly KalumDbContext DbContext;
        private readonly ILogger<AspiranteController> Logger;
        private readonly IMapper Mapper;
       

        public CuentaPorCobrarController(KalumDbContext _DbContext, ILogger<AspiranteController> _Logger, IMapper _Mapper)
        {
            this.DbContext = _DbContext;
            this.Logger = _Logger;
            this.Mapper = _Mapper;
        }
                       
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CuentaPorCobrarListDTO>>> Get()
        {
            List<CuentaPorCobrar> cuenta = null;
            Logger.LogDebug("Iniciando procedimiento para consulta de Cuentas por Cobrar");
            cuenta = await DbContext.CuentaPorCobrar.ToListAsync();
            if(cuenta == null || cuenta.Count == 0)
            {
                Logger.LogWarning("No existen registros en la Base de Datos");
                return new NoContentResult();
            }
            List<CuentaPorCobrarListDTO> cuentas = Mapper.Map<List<CuentaPorCobrarListDTO>>(cuenta);
            Logger.LogInformation("La consulta se realizo de forma exitosa");
            return Ok(cuentas);
        }

        [HttpGet("page/{page}")]
        public async Task<ActionResult<IEnumerable<CuentaPorCobrar>>> GetPaginacion(int page)
        {
            Logger.LogDebug("Iniciando procedimiento para consulta de Cuentas por Cobrar Paginado");
            var queryable = this.DbContext.CuentaPorCobrar.AsQueryable();
            var paginacion = new HttpResponsePaginacion<CuentaPorCobrar>(queryable,page);
            if(paginacion.Content == null && paginacion.Content.Count == 0)
            {
                Logger.LogWarning("No existen registros en la Base de Datos");
                return NoContent();
            }
            else
            {
                 Logger.LogInformation("La consulta se realizo de forma exitosa");
                 await DbContext.SaveChangesAsync();
                 return Ok(paginacion);
            }
        }

        [HttpGet("{id}", Name = "GetCuentaPorCobrar")]
        public async Task<ActionResult<Aspirante>>GetCuentaPorCobrar(string id, [FromBody] CuentaPorCobrar value) 
        {
            Logger.LogDebug("Iniciando el proceso de busqueda con el id " + id);
            var cuenta = await DbContext.CuentaPorCobrar.FirstOrDefaultAsync(cxc => cxc.Cargo == id && cxc.Carne == value.Carne);
                           
            if(cuenta == null)
            {
                Logger.LogWarning($"No existe La Cuenta por Cobrar con el Cargo {id}  y el Carne {value.Carne}");
                return new NoContentResult();
            }
            Logger.LogInformation("Finalizando el proceso de busqueda de forma exitosa");
            return Ok(cuenta);
        }
                   

    }
}