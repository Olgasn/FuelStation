using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace FuelStation.Infrastructure.Filters
{
    //Фильтр ресурсов
    public class TimingLogAttribute : Attribute, IResourceFilter
    {
        ILogger _logger;
        public TimingLogAttribute(ILoggerFactory loggerFactory)
        {
            //loggerFactory.AddFile(Path.Combine(Directory.GetCurrentDirectory(), "logger.txt"));
            _logger = loggerFactory.CreateLogger("TimingResourceFilter");
        }
        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            _logger.LogInformation($"Path - {context.HttpContext.Request.Path}");
            _logger.LogInformation($"OnResourceExecuted - {DateTime.Now}");
        }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            _logger.LogInformation($"OnResourceExecuting - {DateTime.Now}");
        }
    }
}
