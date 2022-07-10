using Microsoft.AspNetCore.Mvc;
using WebApiKalum.Entities;
using Microsoft.EntityFrameworkCore;
using WebApiKalum.Utilites;
using WebApiKalum.DTOs;
using AutoMapper;

namespace WebApiKalum.Controllers
{
    [ApiController]
    [Route("v1/KalumManagement/Alumnos/")]
    public class AlumnoController : ControllerBase
    {
        private readonly KalumDbContext DbContext;
        private readonly ILogger<AlumnoController> Logger;
        private readonly IMapper Mapper;
        public AlumnoController(KalumDbContext _DbContext, ILogger<AlumnoController> _Logger, IMapper _Mapper)
        {
            this.DbContext = _DbContext;
            this.Logger = _Logger;
            this.Mapper = _Mapper;
        }

        [HttpPost]
        public async Task<ActionResult<Alumno>> Post([FromBody] Alumno value)
        {
            Logger.LogDebug($"Iniciando proceso para agregar una nuevo Alumno");
            Console.ReadLine();
            await DbContext.Alumno.AddAsync(value);
            await DbContext.SaveChangesAsync();
            Logger.LogInformation("Finalizado el proceso de agregar un nuevo Alumno");
            return new CreatedAtRouteResult("GetAlumnos",new{id = value.Carne}, value);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AlumnoListDTO>>> Get()
        {
            List<Alumno> alumnos = null;
            Logger.LogDebug("Iniciando consulta de Alumnos en Base de Datos");
            alumnos = await DbContext.Alumno.Include(al => al.Inscripciones).ToListAsync();
            alumnos = await DbContext.Alumno.Include(al => al.CuentasPorCobrar).ToListAsync();
            if (alumnos == null || alumnos.Count == 0)
            {
                Logger.LogWarning("No Existe Alumno");
                return new NoContentResult();
            }
            Logger.LogInformation("Se ejecuto la peticion de forma exitosa");
            List<AlumnoListDTO> listaAlumnos = Mapper.Map<List<AlumnoListDTO>>(alumnos);
            return Ok(listaAlumnos);
        }

        [HttpGet("page/{page}")]
        public async Task<ActionResult<IEnumerable<Alumno>>> GetPaginacion(int page)
        {
            Logger.LogDebug("Iniciando procedimiento para consulta de Alumnos Paginado");
            var queryable = this.DbContext.Alumno.Include(a => a.CuentasPorCobrar).AsQueryable();
            var paginacion = new HttpResponsePaginacion<Alumno>(queryable,page);
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
        
        [HttpGet("{id}", Name = "GetAlumnos")]
        public async Task<ActionResult<Alumno>> GetAlumno(string id)
        {
            Logger.LogDebug("Iniciando el proceso de busqueda de Alumno con el Carne :" + id);
            var alumnos = await DbContext.Alumno.Include(al => al.Inscripciones).FirstOrDefaultAsync(al => al.Carne == id);
                alumnos = await DbContext.Alumno.Include(al => al.CuentasPorCobrar).FirstOrDefaultAsync(al => al.Carne == id);
            if(alumnos == null)
            {
                Logger.LogWarning("No Existe el Alumno con el Carne : " + id);
                return new NoContentResult();
            }
            Logger.LogInformation("Finalizado el proceso de busqueda de forma exitosa");
            return Ok(alumnos);
        }
        
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(string id, [FromBody] Alumno value)
        {
            Logger.LogDebug($"Iniciando el proceso de actualizacion del registro con el numero de Carne : {id} ");
            Alumno alumno = await DbContext.Alumno.FirstOrDefaultAsync(al => al.Carne == id);
            if(alumno == null)
            {
                Logger.LogWarning($"El Alumno con el Id {id} no existe, no se puede eliminar el Alumno");
                return BadRequest();
            }
            alumno.Nombres = value.Nombres;
            alumno.Apellidos = value.Apellidos;
            alumno.Direccion = value.Direccion;
            alumno.Telefono = value.Telefono;
            alumno.Email = value.Email;
            DbContext.Entry(alumno).State = EntityState.Modified;
            await DbContext.SaveChangesAsync();
            Logger.LogInformation("Los Datos fueron actualizados correctamente");
            return Ok(alumno);
        }

        [HttpDelete ("{id}")]
        public async Task<ActionResult<Alumno>> Delete(string id)
        {
            Logger.LogDebug("Iniciando proceso para eliminar el registro");
            Alumno alumno = await DbContext.Alumno.FirstOrDefaultAsync(al => al.Carne == id);
            if(alumno == null)
            {
                Logger.LogWarning($"No se encontro el registro a eliminar con el Carne : {id}");
                return NotFound();
            }
            else
            {
                Inscripcion inscripcion = await DbContext.Inscripcion.FirstOrDefaultAsync(i => i.Carne == id);
                DbContext.Inscripcion.Remove(inscripcion);
                
                
                CuentaPorCobrar cuenta = await DbContext.CuentaPorCobrar.FirstOrDefaultAsync(cxc => cxc.Carne == id);
                if(cuenta != null)
                {
                    bool flag = true;
                    while(flag)
                    {
                        DbContext.CuentaPorCobrar.Remove(cuenta);
                        await DbContext.SaveChangesAsync();
                        cuenta = await DbContext.CuentaPorCobrar.FirstOrDefaultAsync(cxc => cxc.Carne == id);
                        Logger.LogDebug($"Procesando Eliminacion de Cuentas por Cobrar con Carne {id}");
                        if(cuenta == null)
                        {
                            flag = false;
                        }
                    }    
                }
                

                DbContext.Alumno.Remove(alumno);
                Logger.LogInformation($"Se elimino el Alumno con el Carne : {id}");
                await DbContext.SaveChangesAsync();
                return(alumno);
            }
        }        
    }
}