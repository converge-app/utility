using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Application.Utility.Exception;

using static System.Environment;
namespace Application.Utility.ClientLibrary.Collaboration
{
    public static class BidClient
    {
        private const string _serviceHttp = "BIDDING_SERVICE_HTTP";
        private const string _serviceHttps = "BIDDING_SERVICE_HTTPS";

        public static async Task<List<BidData>> GetBidsForProject(this IClient factory, string projectId)
        {
            var clientFactory = factory.HttpClientFactory;
            var client = clientFactory.CreateClient("BiddingService");
            var host = GetProjectsServiceHost();
            var pingUri = $"{host}/api/health/ping";

            var response = await client.GetAsync(pingUri);
            if (!response.IsSuccessStatusCode)
                throw new ServiceDown(host);

            var uri = $"{host}/api/biddings/project/{projectId}";
            response = await client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<List<BidData>>();

            throw new ServiceDown("Couldn't fetch projects at projectId: " + projectId);
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
            throw new EnvironmentNotSet("BIDDING_SERVICE");
        }
    }
}