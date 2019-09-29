using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OurService.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace OurService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IWeatherService _weatherServiceWithRetry;
        private readonly WeatherService _weatherService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, WeatherServiceWithRetry weatherServiceWithRetry, WeatherService weatherService)
        {
            _weatherServiceWithRetry = weatherServiceWithRetry;
            _weatherService = weatherService;
            _logger = logger;
        }


        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            var weatherForecasts = await _weatherService.GetWeather();
            
            return weatherForecasts;
        }

        [HttpGet]
        [Route("retry")]
        public async Task<IEnumerable<WeatherForecast>> GetRetry()
        {
            var weatherForecasts = await _weatherServiceWithRetry.GetWeather();

            return weatherForecasts;
        }
    }
}
