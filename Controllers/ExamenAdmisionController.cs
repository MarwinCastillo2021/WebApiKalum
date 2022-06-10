using Microsoft.AspNetCore.Mvc;
using WebApiKalum.Entities;
using Microsoft.EntityFrameworkCore;
namespace WebApiKalum.Controllers
{
    [ApiController]
    [Route("v1/KalumManagement/ExamenAdmision/")]
    public class ExamenAdmisionController: ControllerBase
    {
        private readonly KalumDbContext DbContext;
        private readonly ILogger<ExamenAdmisionController>Logger;
        public ExamenAdmisionController(KalumDbContext _DbContext, ILogger<ExamenAdmisionController> _Logger)
        {
            this.DbContext = _DbContext;
            this.Logger = _Logger;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ExamenAdmision>>> Get()
        {
            List<ExamenAdmision> examenAdmision = null;
            Logger.LogDebug("Iniciando proceso de consulta Examen de Admision en BD");
            examenAdmision = await DbContext.ExamenAdmision.Include(ea => ea.Aspirantes).ToListAsync();
            if(examenAdmision == null || examenAdmision.Count == 0)
            {
                Logger.LogWarning("No Existe Examen de Admision");
                return new NoContentResult();
            }
            Logger.LogInformation("Existe Examen de Admision");
            return Ok(examenAdmision);
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
    }
}