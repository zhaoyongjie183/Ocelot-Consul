using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace order_api.Controllers
{
    [ApiController]
   // [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [Route("api/orders")]
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "刘明的订单", "王天的订单" };
        }

        [Route("api/save")]
        [HttpPost]
        public string Save()
        {
            return ("成功");
        }
    }
}
