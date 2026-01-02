using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace SignalRDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        RedisService _redisService;
        public WeatherForecastController(ILogger<WeatherForecastController> logger, RedisService redisService)
        {
            _logger = logger;
            _redisService = redisService;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
         var s = await _redisService.GetCachedDataAsync("mykey");
           await _redisService.PublishMessageAsync("Hello, Redis!");
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary =  Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
