using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;

namespace FuelStation.Infrastructure.Filters
{
    //Фильтр исключений
    public class ExceptionFilterAttribute : Attribute, IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            string actionName = context.ActionDescriptor.DisplayName; //имя метода действия
            // подробности пишем в лог, пользователю их не показываем
            ILogger logger = (ILogger)context.HttpContext.RequestServices
                .GetService(typeof(ILogger<ExceptionFilterAttribute>));
            logger?.LogError(context.Exception, "Исключение в методе {Action}", actionName);
            context.Result = new ContentResult
            {
                Content = $"В методе {actionName} возникло исключение.",
                StatusCode = 500
            };
            context.ExceptionHandled = true;
        }
    }
}
