using System;
using System.Net.Http;
using System.Threading.Tasks;
using HttpClientNetCore.Abstract;

namespace HttpClientNetCore.Models
{
    public class WeatherModel:IWeatherService
    {
        private readonly HttpClient _httpClient;

        // #### HttpClient injections are transient, also HttpMessageInvoker implementation makes this class disposable ###
        public WeatherModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<string> GetContentAsync(string city, string aqi)
        {

            try
            {
                //var url = $"http://api.weatherapi.com/v1/current.json?key=b7d6a14df5744325bb872529210207&q={city}&aqi={aqi}";
                var url = $"?key=b7d6a14df5744325bb872529210207&q={city}&aqi={aqi}";//Startup.cs AddHttpClient

                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsStringAsync();
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
                return null;
            }
        }
    }
}
