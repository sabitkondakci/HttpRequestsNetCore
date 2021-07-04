using System;
using System.Net.Http;
using System.Threading.Tasks;
using HttpClientNetCore.Abstract;

namespace HttpClientNetCore.Models
{
    public class GitHubModel:IGitHubService
    {
        private readonly HttpClient _httpClient;

        // #### HttpClient injections are transient, also HttpMessageInvoker implementation makes this class disposable ###
        public GitHubModel(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<string> GetContentAsync(string username)
        {
            try
            {
                var url = $"/users/{username}";
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
