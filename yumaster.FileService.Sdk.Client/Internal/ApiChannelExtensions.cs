using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace yumaster.FileService.Sdk.Client.Internal
{
    internal static class ApiChannelExtensions
    {
        public static async Task<JObject> GetJObjectAsync(this IApiChannel apiChannel, HttpMethod method, string apiPath, HttpContent reqContent = null)
        {
            using (var tr = await apiChannel.GetTextReaderAsync(method, apiPath, reqContent))
            {
                return await JObject.LoadAsync(new JsonTextReader(tr));
            }
        }
    }
}
