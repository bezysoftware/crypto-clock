using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace CryptoClock.Extensions
{
    public static class HttpExtensions
    {
        public static async Task<T> GetObjectAsync<T>(this HttpClient httpClient, string requestUri)
        {
            var response = await httpClient.GetStringAsync(requestUri);

            return JsonSerializer.Deserialize<T>(response);
        }
    }
}
