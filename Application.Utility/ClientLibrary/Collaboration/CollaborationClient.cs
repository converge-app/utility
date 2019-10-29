using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Application.Utility.Exception;

using static System.Environment;
namespace Application.Utility.ClientLibrary.Collaboration
{
    public static class CollaborationClient
    {
        private const string _serviceHttp = "COLLABORATION_SERVICE_HTTP";
        private const string _serviceHttps = "COLLABORATION_SERVICE_HTTPS";

        public static async Task<bool> PostEvent(this IClient factory, string authorizationToken,
            EventData @event)
        {
            var clientFactory = factory.HttpClientFactory;
            var client = clientFactory.CreateClient("CollaborationService");
            var host = GetProjectsServiceHost();
            var pingUri = $"{host}/api/health/ping";

            var response = await client.GetAsync(pingUri);
            if (!response.IsSuccessStatusCode)
                throw new ServiceDown(host);

            var content = @event;
            var uri = $"{host}/api/collaborations";
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorizationToken);

            response = await client.PostAsJsonAsync(uri, content);
            return response.IsSuccessStatusCode;
        }

        private static string GetProjectsServiceHost()
        {
            var host = "";
            try
            {
                if (!string.IsNullOrEmpty(GetEnvironmentVariable(_serviceHttp)))
                {
                    host = "http://" + GetEnvironmentVariable(_serviceHttp);
                    return host;

                }
                throw new EnvironmentNotSet("HTTP not set");
            }
            catch (System.Exception e)
            {
                try
                {
                    if (!string.IsNullOrEmpty(GetEnvironmentVariable(_serviceHttps)))
                    {
                        host = "https://" + GetEnvironmentVariable(_serviceHttps);
                        return host;

                    }
                    throw new EnvironmentNotSet("HTTPS not set");
                }
                catch (System.Exception ex)
                {
                    throw ex;
                }
            }
            throw new EnvironmentNotSet("COLLABORATION_SERVICE");
        }
    }
}