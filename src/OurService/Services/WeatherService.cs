using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace OurService.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _httpClient;

        public WeatherService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<WeatherForecast>> GetWeather()
        {
            var uri = "WeatherForecast";

            var responseString = await _httpClient.GetStringAsync(uri);

            var weatherForecasts = JsonSerializer.Deserialize<List<WeatherForecast>>(responseString);
            return weatherForecasts;
        }
    }
}
