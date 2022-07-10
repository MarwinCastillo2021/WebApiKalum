using Microsoft.AspNetCore.Mvc.Filters;
namespace WebApiKalum.Utilites
{
    public class ActionFilter : IActionFilter
    {
        private readonly ILogger<ActionFilter> Logger;
        public ActionFilter (ILogger<ActionFilter> _Logger)
        {
            this.Logger = _Logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            Logger.LogInformation("Esto se ejecuta antes de la accion");
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            Logger.LogInformation("Esto se ejecuta despues de la accion");
        }
    }
}