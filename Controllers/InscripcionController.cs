using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using RabbitMQ.Client;
using System.Text.Json;
using System.Text;
using WebApiKalum.Entities;
using WebApiKalum.DTOs;
using WebApiKalum.Utilites;

namespace WebApiKalum.Controllers
{
    [ApiController]
    [Route("v1/KalumManagement/Inscripciones")]
    public class InscripcionController : ControllerBase
    {
        private readonly KalumDbContext DbContext;
        private readonly ILogger<InscripcionController> Logger;
        private readonly IMapper Mapper;
        public InscripcionController(KalumDbContext _DbContext,ILogger<InscripcionController> _Logger, IMapper _Mapper)
        {
            this.DbContext = _DbContext;
            this.Logger = _Logger;
            this.Mapper = _Mapper;
        }
        
        [HttpPost("Enrollments")]
        public async Task<ActionResult<ResponseEnrollmentDTO>> EnrollmentCreateAsync([FromBody] EnrollmentDTO value)
        {
            Aspirante aspirante = await DbContext.Aspirante.FirstOrDefaultAsync(a=>a.NoExpediente == value.NoExpediente);
            if(aspirante == null)
            {
                return NoContent();
            }
            CarreraTecnica carreraTecnica = await DbContext.CarreraTecnica.FirstOrDefaultAsync(ct=>ct.CarreraId == value.CarreraId);
            if(carreraTecnica == null)
            {
                return NoContent();
            }
            bool respuesta = await CrearSolicitudAsync(value);
            if(respuesta == true)
            {
                ResponseEnrollmentDTO response = new ResponseEnrollmentDTO();
                response.HttpStatus = 201;
                response.Message = "El Proceso de inscripcion se realizo con exito";
                return Ok(response);
            }
            else
            {
                return StatusCode(503,value);
            }
        }
        private async Task<bool> CrearSolicitudAsync(EnrollmentDTO value)
        {
            bool proceso = false;
            ConnectionFactory factory = new ConnectionFactory();
            IConnection conexion = null;
            IModel channel = null;
            factory.HostName = "localhost";
            factory.VirtualHost = "/";
            factory.Port = 5672;
            factory.UserName = "guest";
            factory.Password = "guest";
            try
            {
                conexion = factory.CreateConnection();
                channel = conexion.CreateModel();
                channel.BasicPublish("kalum.exchange.enrollment","",null,Encoding.UTF8.GetBytes(JsonSerializer.Serialize(value)));
                proceso = true;
                //await DbContext.SaveChangesAsync();
            }
            catch(Exception e)
            {
                Logger.LogError(e.Message);
            }
            finally
            {
                channel.Close();
                conexion.Close();
            }
            return proceso;
        }

        [HttpGet]

         public async Task<ActionResult<IEnumerable<Inscripcion>>> Get()
        {
            List<Inscripcion> inscripcion = null;
            Logger.LogDebug("Iniciando proceso de consulta Inscripcion en BD");
            inscripcion = await DbContext.Inscripcion.Include(i => i.Alumno).Include(i => i.CarreraTecnica).Include(i=>i.Jornada).ToListAsync();
            if(inscripcion == null || inscripcion.Count == 0)
            {
                Logger.LogWarning("No Existe Inscripcion");
                return new NoContentResult();
            }
            //List<ExamenAdmisionListDTO> examenesAdmision = Mapper.Map<List<ExamenAdmisionListDTO>>(examenAdmision);
            Logger.LogInformation("Existe Inscripcion");
            return Ok(inscripcion);
        }

        [HttpGet("page/{page}")]
        public async Task<ActionResult<IEnumerable<Inscripcion>>> GetPaginacion(int page)
        {
            Logger.LogDebug("Iniciando procedimiento para consulta de Inscripcion Paginado");
            var queryable = this.DbContext.Inscripcion.Include(i => i.Alumno).Include(i => i.CarreraTecnica).Include(i => i.Jornada).AsQueryable();
            var paginacion = new HttpResponsePaginacion<Inscripcion>(queryable,page);
            //await DbContext.SaveChangesAsync();
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

        [HttpGet("{id}", Name="GetInscripcion")]
        public async Task<ActionResult<Inscripcion>> GetInscripcion(string id)
        {
            Logger.LogDebug("Iniciando proceso de busqueda con Inscripcion id : " + id);
            var inscripcion = await DbContext.Inscripcion.Include(i => i.Alumno).Include(i=>i.CarreraTecnica).Include(i=>i.Jornada).FirstOrDefaultAsync(i => i.InscripcionId == id);
            if(inscripcion == null)
            {
                Logger.LogWarning("No existe la Inscripcion con el id :" + id);
                return new NoContentResult();
            }
            Logger.LogInformation("Existe Inscripcion con id" + id);
            return Ok(inscripcion);
        }
        
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(string id, [FromBody] Inscripcion value)
        {
            Logger.LogDebug($"Iniciando proceso de actualizacion de Inscripcion con el Id {id}");
            Inscripcion inscripcion = await DbContext.Inscripcion.FirstOrDefaultAsync(i => i.InscripcionId == id);
            if(inscripcion == null)
            {
                Logger.LogWarning($"No existe la Inscripcion con el Id {id}, el registro no puede actualizarse");
                return BadRequest();
            }
            inscripcion.Ciclo = value.Ciclo;
            inscripcion.FechaInscripcion = value.FechaInscripcion;
            DbContext.Entry(inscripcion).State = EntityState.Modified;
            await DbContext.SaveChangesAsync();
            Logger.LogInformation($"El cambmio de inscripcion con el Id {id} se realizo exitosamente!!");
            return Ok(inscripcion);
        }

        [HttpDelete ("{id}")]
        public async Task<ActionResult<Inscripcion>> Delete(string id, [FromBody] Inscripcion value)
        {
            Logger.LogDebug($"Iniciando el proceso para eliminar inscripcion con el Id {id}");
            Inscripcion inscripcion = await DbContext.Inscripcion.FirstOrDefaultAsync(i => i.InscripcionId == id);
            if(inscripcion == null)
            {
                Logger.LogWarning($"No se encontro la Inscripcion con el id {id}, el registro no se puede eliminar");
                return NotFound();
            }
            else
            {
                CuentaPorCobrar cuenta = null;
                for(int i=0; i<7; i++)
                {
                    cuenta = await DbContext.CuentaPorCobrar.FirstOrDefaultAsync(cxc => cxc.Carne == value.Carne);
                    DbContext.CuentaPorCobrar.Remove(cuenta);
                    await DbContext.SaveChangesAsync();
                }
                  
                Logger.LogDebug($"Se elimino correctamente Cuentas por Conbrar con el Carne {value.Carne}");
                
                DbContext.Inscripcion.Remove(inscripcion);
                await DbContext.SaveChangesAsync();
                Logger.LogInformation($"Se elimino correctamente la Inscripcion con el id {id}");
               
                Alumno alumno = await DbContext.Alumno.FirstOrDefaultAsync(al => al.Carne == value.Carne);
                DbContext.Alumno.Remove(alumno);
                await DbContext.SaveChangesAsync();
                Logger.LogDebug($"Se elimino correctamente el Alumno con Carne {value.Carne}"); 
                
                return (inscripcion);
            }
        }

    }
}