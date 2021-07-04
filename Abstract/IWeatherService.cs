using System.Threading.Tasks;

namespace HttpClientNetCore.Abstract
{
    public interface IWeatherService
    {
        Task<string> GetContentAsync(string city, string aqi);
    }
}
