using Microsoft.AspNetCore.Mvc;
using WebApiKalum.Entities;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using WebApiKalum.Utilites;
using WebApiKalum.DTOs;

namespace WebApiKalum.Controllers
{
    [ApiController]
    [Route("v1/KalumManagement/ExamenAdmision/")]
    public class ExamenAdmisionController: ControllerBase
    {
        private readonly KalumDbContext DbContext;
        private readonly ILogger<ExamenAdmisionController>Logger;
        private readonly IMapper Mapper;
        public ExamenAdmisionController(KalumDbContext _DbContext, ILogger<ExamenAdmisionController> _Logger, IMapper _Mapper)
        {
            this.DbContext = _DbContext;
            this.Logger = _Logger;
            this.Mapper = _Mapper;
        }
        public async Task<ActionResult<ExamenAdmision>> Post([FromBody] ExamenAdmision value)
        {
            Logger.LogDebug("Iniciando procedimiento para agregar una nueva fecha para Examen de Admision");
            //ExamenAdmision nuevo = Mapper.Map<ExamenAdmision>(value);
            value.ExamenId = Guid.NewGuid().ToString().ToUpper();
            await DbContext.ExamenAdmision.AddAsync(value);
            await DbContext.SaveChangesAsync();
            Logger.LogInformation("Se agrego una fecha de examen nueva admision");
            return new CreatedAtRouteResult("GetExamenAdmision", new{id = value.ExamenId}, value);
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExamenAdmisionListDTO>>> Get()
        {
            List<ExamenAdmision> examenAdmision = null;
            Logger.LogDebug("Iniciando proceso de consulta Examen de Admision en BD");
            examenAdmision = await DbContext.ExamenAdmision.Include(ea => ea.Aspirantes).ToListAsync();
            if(examenAdmision == null || examenAdmision.Count == 0)
            {
                Logger.LogWarning("No Existe Examen de Admision");
                return new NoContentResult();
            }
            List<ExamenAdmisionListDTO> examenesAdmision = Mapper.Map<List<ExamenAdmisionListDTO>>(examenAdmision);
            Logger.LogInformation("Existe Examen de Admision");
            return Ok(examenesAdmision);
        }

        [HttpGet("page/{page}")]
        public async Task<ActionResult<IEnumerable<ExamenAdmision>>> GetPaginacion(int page)
        {
            Logger.LogDebug("Iniciando procedimiento para consulta de Examen de Admision Paginado");
            var queryable = this.DbContext.ExamenAdmision.Include(ea => ea.Aspirantes).AsQueryable();
            var paginacion = new HttpResponsePaginacion<ExamenAdmision>(queryable,page);
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

        [HttpGet("{id}", Name="GetExamenAdmision")]
        public async Task<ActionResult<ExamenAdmision>> GetExamenAdmision(string id)
        {
            Logger.LogDebug("Iniciando proceso de busqueda con examen id : " + id);
            var examen = await DbContext.ExamenAdmision.Include(ea => ea.Aspirantes).FirstOrDefaultAsync(ea => ea.ExamenId == id);
            if(examen == null)
            {
                Logger.LogWarning("No existe el Examen de Admision con el id :" + id);
                return new NoContentResult();
            }
            Logger.LogInformation("Existe Examen de Admision con id" + id);
            return Ok(examen);
        }
        
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(string id, [FromBody] ExamenAdmision value)
        {
            Logger.LogDebug($"Iniciando proceso de actualizacion para fecha de examen con el Id {id}");
            ExamenAdmision examenAdmision = await DbContext.ExamenAdmision.FirstOrDefaultAsync(ea => ea.ExamenId == id);
            if(examenAdmision == null)
            {
                Logger.LogWarning($"No existe la fecha de examen con el Id {id}, el registro no puede actualizarse");
                return BadRequest();
            }
            examenAdmision.FechaExamen = value.FechaExamen;
            DbContext.Entry(examenAdmision).State = EntityState.Modified;
            await DbContext.SaveChangesAsync();
            Logger.LogInformation($"El cambmio de fecha con el Id {id} se realizo exitosamente!!");
            return Ok(examenAdmision);
        }

        [HttpDelete ("{id}")]
        public async Task<ActionResult<ExamenAdmision>> Delete(string id)
        {
            Logger.LogDebug($"Iniciando el proceso para eliminar la fecha de examen con el Id {id}");
            ExamenAdmision examenAdmision = await DbContext.ExamenAdmision.FirstOrDefaultAsync(ea => ea.ExamenId == id);
            if(examenAdmision == null)
            {
                Logger.LogWarning($"No se encontro la fecha de examen con el id {id}, el registro no se puede eliminar");
                return NotFound();
            }
            else
            {
                DbContext.ExamenAdmision.Remove(examenAdmision);
                await DbContext.SaveChangesAsync();
                Logger.LogInformation($"Se elimino correctamente la fecha de examen con el id {id}");
                return (examenAdmision);
            }
        }
        
    }
}