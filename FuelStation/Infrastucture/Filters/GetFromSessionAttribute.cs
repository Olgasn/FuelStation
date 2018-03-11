using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using FuelStation.Infrastucture;

namespace FuelStation.Infrastructure.Filters
{
    public class GetFromSessionAttribute : Attribute, IActionFilter
    {
        private string _name;
        public GetFromSessionAttribute(string name)
        {
            _name = name;
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var session = context.HttpContext.Session.Get<Dictionary<string, string>>(_name);
            var model = context.ModelState;
            foreach (var item in model)
            {
                if (item.Value.AttemptedValue == "")
                {
                    item.Value.AttemptedValue = session[item.Key];
                }

            }


        }
    }
}
