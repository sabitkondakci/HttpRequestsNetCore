using System.Threading.Tasks;

namespace HttpClientNetCore.Abstract
{
    public interface IGitHubService
    {
        Task<string> GetContentAsync(string username);
    }
}
