using Microsoft.AspNetCore.Mvc;
using WebApiKalum.Entities;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using WebApiKalum.Utilites;
using WebApiKalum.DTOs;

namespace WebApiKalum.Controllers
{
    [ApiController]
    [Route("v1/KalumManagement/Inversiones/")]
    public class InversionCarreraTecnicaController : ControllerBase
    {
        private readonly KalumDbContext DbContext;
        private readonly ILogger<JornadaController> Logger;
        private readonly IMapper Mapper;
        public InversionCarreraTecnicaController(KalumDbContext _DbContext, ILogger<JornadaController> _Logger, IMapper _Mapper)
        {
            this.DbContext = _DbContext;
            this.Logger = _Logger;
            this.Mapper = _Mapper;
        }
        public async Task<ActionResult<InversionCarreraTecnica>> Post([FromBody] InversionCarreraTecnica value)
        {
            Logger.LogDebug("Iniciando proceso para agregar una inversion de carrera tecnica nueva : ");
            value.InversionId = Guid.NewGuid().ToString().ToUpper();
            await DbContext.InversionCarreraTecnica.AddAsync(value);
            await DbContext.SaveChangesAsync();
            Logger.LogInformation("La Inversion nueva se ha agregado con exito!!! ");
            return new CreatedAtRouteResult("GetInversion", new{id = value.InversionId}, value);
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InversionCarreraTecnicaListDTO>>> Get()
        {
            List<InversionCarreraTecnica> inversion = null;
            Logger.LogDebug("Iniciando proceso de consulta de Inversion Carrera Tecnica en Base de Datos");
            inversion = await DbContext.InversionCarreraTecnica.Include(inv => inv.CarreraTecnica).ToListAsync();
            
            if(inversion == null || inversion.Count == 0)
            {
                Logger.LogWarning("No Existe la Inversion de Carrera Tecnica base de Datos");
                return new NoContentResult();
            }
            Logger.LogInformation("Se ejecuto la peticion de forma exitosa");
            List<InversionCarreraTecnicaListDTO> inversiones = Mapper.Map<List<InversionCarreraTecnicaListDTO>>(inversion);
            return Ok(inversiones);
        }

        [HttpGet("page/{page}")]
        public async Task<ActionResult<IEnumerable<InversionCarreraTecnica>>> GetPaginacion(int page)
        {
            Logger.LogDebug("Iniciando procedimiento para consulta de Inversion Carrera Tecnica Paginado");
            var queryable = this.DbContext.InversionCarreraTecnica.Include(inv => inv.CarreraTecnica).AsQueryable();
            var paginacion = new HttpResponsePaginacion<InversionCarreraTecnica>(queryable,page);
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

        [HttpGet("{id}", Name = "GetInversion")]
        public async Task<ActionResult<InversionCarreraTecnica>>GetInversionCarreraTecnica(string id)
        {
            Logger.LogDebug("Iniciando el proceso de busqueda de la Inversion con el id :" + id);
            var inversion = await DbContext.InversionCarreraTecnica.Include(inv => inv.CarreraTecnica).FirstOrDefaultAsync(inv => inv.InversionId == id);
                
            if(inversion == null)
            {
                Logger.LogWarning("No Existe la Inversion de Carrera Tecnica con el id : " + id);
                return new NoContentResult();
            }
            Logger.LogInformation("Se ejecuto la peticion de forma exitosa");
            return Ok(inversion);
        }
        
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(string id, [FromBody] InversionCarreraTecnica value)
        {
            Logger.LogDebug($"Iniciando el proceso de actualizacion del registro con el Id : {id} ");
            InversionCarreraTecnica inversion = await DbContext.InversionCarreraTecnica.FirstOrDefaultAsync(inv => inv.InversionId == id);
            if(inversion == null)
            {
                Logger.LogWarning($"La Inversion con el Id {id} no existe, no se puede actualizar la Inversion de Carrera Tecnica");
                return BadRequest();
            }
            inversion.MontoInscripcion = value.MontoInscripcion;
            inversion.NumeroPagos = value.NumeroPagos;
            inversion.MontoPagos = value.MontoPagos;
            DbContext.Entry(inversion).State = EntityState.Modified;
            await DbContext.SaveChangesAsync();
            Logger.LogInformation("Los Datos fueron actualizados correctamente");
            return Ok(inversion);
        }

        [HttpDelete ("{id}")]
        public async Task<ActionResult<InversionCarreraTecnica>> Delete(string id)
        {
            Logger.LogDebug($"Iniciando proceso para eliminar Inversion con Id : {id}");
            InversionCarreraTecnica inversion = await DbContext.InversionCarreraTecnica.FirstOrDefaultAsync(inv => inv.InversionId == id);
            if(inversion == null)
            {
                Logger.LogWarning($"No se encontro la Inversion con el Id {id}, la Inversion de Carrera Tecnica no puede ser eliminada");
                return NotFound();
            }
            else
            {
                DbContext.InversionCarreraTecnica.Remove(inversion);
                await DbContext.SaveChangesAsync();
                Logger.LogInformation($"La Inversion con id {id} ha sido eliminada de forma correcta!!");
                return(inversion);
            }        
        }
        
    }
}