using Microsoft.AspNetCore.Mvc;
using WebApiKalum.Entities;
using Microsoft.EntityFrameworkCore;
using WebApiKalum.DTOs;
using AutoMapper;
using WebApiKalum.Utilites;


namespace WebApiKalum.Controllers
{
    [ApiController]
    [Route("v1/KalumManagement/Aspirantes/")]
    
    public class AspiranteController : ControllerBase
    {
        private readonly KalumDbContext DbContext;
        private readonly ILogger<AspiranteController> Logger;
        private readonly IMapper Mapper;
       

        public AspiranteController(KalumDbContext _DbContext, ILogger<AspiranteController> _Logger, IMapper _Mapper)
        {
            this.DbContext = _DbContext;
            this.Logger = _Logger;
            this.Mapper = _Mapper;
        }
        
        [HttpPost]
        public async Task<ActionResult<Aspirante>> Post([FromBody] Aspirante value)
        {
            Logger.LogDebug("Iniciando proeceso para almacenar un registro de aspirante");
            CarreraTecnica carreraTecnica = await DbContext.CarreraTecnica.FirstOrDefaultAsync(ct=> ct.CarreraId == value.CarreraId);
            if(carreraTecnica == null)
            {
                Logger.LogInformation($"No existe la carrera tecnica con el id {value.CarreraId}");
                return BadRequest();
            }
            Jornada jornada = await DbContext.Jornada.FirstOrDefaultAsync(j => j.JornadaId == value.JornadaId);
            if(jornada == null)
            {
                Logger.LogInformation($"No existe la jornada con el id {value.JornadaId}");
                return BadRequest();                
            }
            ExamenAdmision examenAdmision = await DbContext.ExamenAdmision.FirstOrDefaultAsync(ea => ea.ExamenId == value.ExamenId);
            if(examenAdmision == null)
            {
                Logger.LogWarning($"No existe el examen de admision con el id {value.ExamenId}");
                return BadRequest();
            }
            await DbContext.Aspirante.AddAsync(value);
            await DbContext.SaveChangesAsync();
            Logger.LogInformation($"Se ha creado el aspirante con exito");
            return Ok(value);
        }
        
        [ServiceFilter(typeof(ActionFilter))] // Acciones que se Ejecutan antes y despues del metodo Get()
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AspiranteListDTO>>> Get()
        {
            List<Aspirante> lista = null;
            Logger.LogDebug("Iniciando procedimiento para consulta de Aspirantes");
            lista = await DbContext.Aspirante.Include(a=> a.CarreraTecnica).Include(a => a.Jornada).Include(a => a.ExamenAdmision).ToListAsync();
            if(lista == null || lista.Count == 0)
            {
                Logger.LogWarning("No existen registros en la Base de Datos");
                return new NoContentResult();
            }
            List<AspiranteListDTO> aspirantes = Mapper.Map<List<AspiranteListDTO>>(lista);
            Logger.LogInformation("La consulta se realizo de forma exitosa");
            return Ok(aspirantes);
        }

        [HttpGet("page/{page}")]
        public async Task<ActionResult<IEnumerable<ConsultaCarrerasListDTO>>> GetPaginacion(int page)
        {
            Logger.LogDebug("Iniciando procedimiento para consulta de Aspirantes Paginado");
            var queryable = this.DbContext.Aspirante.Include(a => a.CarreraTecnica).AsQueryable();
            var paginacion = new HttpResponsePaginacion<Aspirante>(queryable,page);
            if(paginacion.Content == null && paginacion.Content.Count == 0)
            {
                Logger.LogWarning("No existen registros en la Base de Datos");
                await DbContext.SaveChangesAsync();
                return NoContent();
            }
            else
            {
                 Logger.LogInformation("La consulta se realizo de forma exitosa");
                 return Ok(paginacion);
            }
        }

        [HttpGet("{id}", Name = "GetAspirante")]
        public async Task<ActionResult<Aspirante>>GetAspirante(string id) //Metodo para listar Aspirante por id
        {
            Logger.LogDebug("Iniciando el proceso de busqueda con el No Expediente " + id);
            var aspirante = await DbContext.Aspirante.Include(a => a.CarreraTecnica).FirstOrDefaultAsync(a => a.NoExpediente == id);
                aspirante = await DbContext.Aspirante.Include(a => a.ExamenAdmision).FirstOrDefaultAsync(a => a.NoExpediente == id);
                aspirante = await DbContext.Aspirante.Include(a => a.Jornada).FirstOrDefaultAsync(a => a.NoExpediente == id);

                
            if(aspirante == null)
            {
                Logger.LogWarning("No existe el Aspirante con No Expediente " + id);
                return new NoContentResult();
            }
            Logger.LogInformation("Finalizando el proceso de busqueda de forma exitosa");
            return Ok(aspirante);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(string id, [FromBody] Aspirante value)
        {
            Logger.LogDebug($"iniciando proceso de actualizacion del aspirante con el No Expediente {id}");
            Aspirante aspirante = await DbContext.Aspirante.FirstOrDefaultAsync(a => a.NoExpediente == id);
            if(aspirante == null)
            {
                Logger.LogWarning($"No existe el aspirante con el No Expediente {id}");
                return BadRequest();
            }
            aspirante.Apellidos = value.Apellidos;
            aspirante.Nombres = value.Nombres;
            aspirante.Direccion = value.Direccion;
            aspirante.Telefono = value.Telefono;
            aspirante.Email = value.Email;
            DbContext.Entry(aspirante).State = EntityState.Modified;
            await DbContext.SaveChangesAsync();
            Logger.LogInformation("Los datos han sido actualizados correctamente");
            return Ok(aspirante);            
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Aspirante>> Delete(string id)
        {
            Logger.LogDebug("Iniciando el proceso de eliminacion del registro");
            Aspirante aspirante = await DbContext.Aspirante.FirstOrDefaultAsync(a => a.NoExpediente == id);
            if(aspirante == null)
            {
                Logger.LogWarning($"No se encontro el aspirante con el No Expediente {id}");
                return NotFound();
            }
            else
            {
                
                DbContext.Aspirante.Remove(aspirante);
                await DbContext.SaveChangesAsync();
                Logger.LogInformation($"Se ha eliminado correctamente la carrera tecnica con el id {id}");
                return (aspirante);
            }
        }

    }
}