using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;

namespace FuelStation.Infrastructure.Filters
{
    //Фильтр действий
    public class SetToSessionAttribute : Attribute, IActionFilter
    {
        private string _name;
        public SetToSessionAttribute(string name)
        {
            _name = name;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>();            
            foreach (var item in context.ModelState)
            {
                dict.Add(item.Key, item.Value.AttemptedValue);
            }
            context.HttpContext.Session.Set(_name, dict);
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {

        }
    }
}
