using System.Web.Mvc;

namespace MicroClimateControllSystem.Infrastructure
{
    public class ExceptionAttrIbuteAttribute : FilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            // Все исключения
            context.Result =
                new RedirectResult("~/Shared/Error.cshtml");
            context.ExceptionHandled = true;
        }
    }
}