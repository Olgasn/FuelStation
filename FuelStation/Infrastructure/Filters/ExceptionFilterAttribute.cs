using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace FuelStation.Infrastructure.Filters
{
    //Фильтр исключений
    public class ExceptionFilterAttribute : Attribute, IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            string actionName = context.ActionDescriptor.DisplayName; //имя метода действия
            string exceptionStack = context.Exception.StackTrace;
            string exceptionMessage = context.Exception.Message;
            context.Result = new ContentResult
            {
                Content = $"В методе {actionName} возникло исключение: \n {exceptionMessage} \n {exceptionStack}"
            };
            context.ExceptionHandled = true;
        }
    }
}