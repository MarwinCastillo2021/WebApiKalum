using Microsoft.AspNetCore.Mvc.Filters;
using WebApiKalum.Models;
using Microsoft.AspNetCore.Mvc;
namespace WebApiKalum.Utilites
{
    public class ErrorFilterException : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            ApiResponse apiResponse = new ApiResponse(){TipoError = "Error en el servicio legado", HttpStatusCode = "503", Mensaje = context.Exception.Message};
            context.Result = new JsonResult(apiResponse);
            base.OnException(context);
        }
    }
}