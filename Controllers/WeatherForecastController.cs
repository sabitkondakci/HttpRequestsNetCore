using System.Text;
using System.Threading.Tasks;
using HttpClientNetCore.Abstract;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace HttpClientNetCore.Controllers
{
    [ApiController]
    [Route("[controller]")] // [Route("[controller]/[action]") [action] is replaced with method names, "-" separation included.
    public class WeatherForecastController : ControllerBase
    {
        //private readonly IHttpClientFactory _httpClientFactory;
        private readonly IWeatherService _weatherService;
        private readonly IGitHubService _gitHubService;

        // #### Typed Clients: Apply IHttpClientFactory in Controllers ####
        public WeatherForecastController(/*IHttpClientFactory httpClientFactory,*/IWeatherService weatherService,IGitHubService gitHubService)
        {
            //_httpClientFactory = httpClientFactory;
            _weatherService = weatherService;
            _gitHubService = gitHubService;
        }

        [HttpGet]
        public async Task<string> Get(string city, string aqi = "no") => await BindWeatherInfo(city, aqi);

        [HttpGet]
        [Route("/github/users/{username}")]
        public async Task<string> GetUserFromGitHub(string username) => await BindGithubInfo(username);

        private async Task<string> BindGithubInfo(string username)
        {
            #region TypedClients
            //var httpClient = _httpClientFactory.CreateClient("github");
            //var url = $"/users/{username}";
            //var response = await httpClient.GetAsync(url);

            //var content = await response.Content.ReadAsStringAsync(); 
            #endregion

            var content = await _gitHubService.GetContentAsync(username);

            if (content == null) return null;
            dynamic json = JsonConvert.DeserializeObject(content);

            #region DisplayString
            var strBuilder = new StringBuilder();
            strBuilder.Append("\nGitHub Information\n\n");
            strBuilder.Append($"User Name:{json?.login}\nName:{json?.name}\n");
            strBuilder.Append($"Company:{json?.company}\nContact:{json?.blog}\nLocation:{json?.location}\n");
            strBuilder.Append($"Looking For Job?:{json?.hireable}\n");
            strBuilder.Append($"Biography:{json?.bio}\n");
            strBuilder.Append($"Followers:{json?.followers}\nFollowing:{json?.following}\n\n");
            strBuilder.Append($"Registry Date:{json?.created_at}\nLast Updated:{json?.updated_at}"); 
            #endregion

            return await Task.FromResult(strBuilder.ToString());

        }
        private async Task<string> BindWeatherInfo(string city,string aqi)
        {
            #region TypedClients
            //var httpClient = _httpClientFactory.CreateClient("WeatherAPI_v1");
            ////var url = $"http://api.weatherapi.com/v1/current.json?key=b7d6a14df5744325bb872529210207&q={city}&aqi={aqi}";
            //var url = $"?key=b7d6a14df5744325bb872529210207&q={city}&aqi={aqi}";//Startup.cs AddHttpClient

            //var response = await httpClient.GetAsync(url);
            //var content = await response.Content.ReadAsStringAsync(); 
            #endregion

            var content = await _weatherService.GetContentAsync(city,aqi);

            if (content == null) return null;
            dynamic json = JsonConvert.DeserializeObject(content);

            #region DisplayString
            var strBuilder = new StringBuilder();
            strBuilder.Append("\nWeather Forecast API\n\n");
            strBuilder.Append("Location\n\n");
            strBuilder.Append($"City:{json?.location.name}\nCountry:{json?.location.country}\nLatitude:{json?.location.lat}\nLongitude:{json?.location.lon}\n");
            strBuilder.Append(
                $"Time Zone:{json?.location.tz_id}\nLocal Time:{json?.location.localtime}\nLocal Time Epoch:{json?.location.localtime_epoch}\n\n");
            strBuilder.Append("Weather\n\n");
            strBuilder.Append($"Date:{json?.current.last_updated}\nDate_Epoch:{json?.current.last_updated_epoch}\n");
            strBuilder.Append($"Temperature(C):{json?.current.temp_c}\nTemperature(F):{json?.current.temp_f}\n");
            strBuilder.Append(
                $"Condition:{json?.current.condition.text}\nWind Speed(kph):{json?.current.wind_kph}\nWind Degree:{json?.current.wind_degree}\nWind Direction:{json?.current.wind_dir}\n");
            strBuilder.Append(
                $"Humidity:{json?.current.humidity}\nFeels Like(C):{json?.current.feelslike_c}\nFeels Like(F):{json?.current.feelslike_f}"); 
            #endregion

            return await Task.FromResult(strBuilder.ToString());

        }
    }
    
}
