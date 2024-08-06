using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;

namespace FuelStation.Infrastructure.Filters
{
    //Фильтр действий для запись в сессию данных из ModelState
    public class SetToSessionAttribute(string name) : Attribute, IActionFilter
    {

        private readonly string _name = name;//имя ключа

        // Выполняется до выполнения метода контроллера, но после привязки данных передаваемых в контроллер
        public void OnActionExecuting(ActionExecutingContext context)
        {

        }

        // Выполняется после выполнения метода контроллера
        public void OnActionExecuted(ActionExecutedContext context)
        {
            Dictionary<string, string> dict = [];
            // считывание данных из ModelState и запись в сессию
            if (context.ModelState != null)
            {
                foreach (var item in context.ModelState)
                {
                    dict.Add(item.Key, item.Value.AttemptedValue);
                }
                context.HttpContext.Session.Set(_name, dict);
            }

        }
    }
}
