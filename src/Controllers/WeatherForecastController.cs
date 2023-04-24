using ApiKeyAuthentication.Model;
using ApiKeyAuthentication.Secure.Key.RestAPI.Strategy.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace ApiKeyAuthentication.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]    
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [ActionName("weather/key")]
        [ApiKey]
        public IEnumerable<WeatherForecast> GetWeatherWithKey()
        {
            return WeatherService.GetWeather();
        }

        [HttpGet]
        [ActionName("weather")]
        public IEnumerable<WeatherForecast> GetWeather()
        {
            return WeatherService.GetWeather();
        }
    }
}