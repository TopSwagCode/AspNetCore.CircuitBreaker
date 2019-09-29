using System.Collections.Generic;
using System.Threading.Tasks;

namespace OurService.Services
{
    public interface IWeatherService
    {
        Task<IEnumerable<WeatherForecast>> GetWeather();
    }
}