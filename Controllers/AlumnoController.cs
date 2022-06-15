using Microsoft.AspNetCore.Mvc;
using WebApiKalum.Entities;
using Microsoft.EntityFrameworkCore;
namespace WebApiKalum.Controllers
{
    [ApiController]
    [Route("v1/KalumManagement/Alumno/")]
    public class AlumnoController : ControllerBase
    {
        private readonly KalumDbContext DbContext;
        private readonly ILogger<AlumnoController> Logger;
        public AlumnoController(KalumDbContext _DbContext, ILogger<AlumnoController> _Logger)
        {
            this.DbContext = _DbContext;
            this.Logger = _Logger;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Alumno>>> Get()
        {
            List<Alumno> Alumnos = null;
            Logger.LogDebug("Iniciando consulta de Alumnos en Base de Datos");
            Alumnos = await DbContext.Alumno.Include(al => al.Inscripciones).ToListAsync();
            Alumnos = await DbContext.Alumno.Include(al => al.CuentasPorCobrar).ToListAsync();
            if (Alumnos == null || Alumnos.Count == 0)
            {
                Logger.LogWarning("No Existe Alumno");
                return new NoContentResult();
            }
            Logger.LogInformation("Se ejecuto la peticion de forma exitosa");
            return Ok(Alumnos);
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
        public async Task<ActionResult<Alumno>> Post([FromBody] Alumno value)
        {
            Logger.LogDebug("Iniciando proceso para agregar una nueva Carrera Tecnicao");
            //value.Carne = Guid.NewGuid().ToString().ToUpper();
            await DbContext.Alumno.AddAsync(value);
            await DbContext.SaveChangesAsync();
            Logger.LogInformation("Finalizado el proceso de agregar una carrera tecnica nueva");
            return new CreatedAtRouteResult("GetAlumnos",new{id = value.Carne}, value);
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
                DbContext.Alumno.Remove(alumno);
                await DbContext.SaveChangesAsync();
                Logger.LogInformation($"Se elimino el Alumno con el Carne : {id}");
                return(alumno);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(string id, [FromBody] Alumno value, string message)
        {
            Logger.LogDebug($"Iniciando el proceso de actualizacion del registro con el numero de Carne : {id} ");
            Alumno alumno = await DbContext.Alumno.FirstOrDefaultAsync(al => al.Carne == id);
            if(alumno == null)
            {
                Logger.LogWarning(message);
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
    }
}