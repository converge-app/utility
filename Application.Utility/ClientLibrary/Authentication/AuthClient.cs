using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Application.Utility.Exception;
using static System.Environment;
namespace Application.Utility.ClientLibrary.Authentication
{
    public static class AuthClient
    {
        private const string _serviceHttp = "AUTHENTICATION_SERVICE_HTTP";
        private const string _serviceHttps = "AUTHENTICATION_SERVICE_HTTPS";

        public static async Task<AuthRegisteredData> Register(this IClient factory, AuthRegisterData data)
        {
            var clientFactory = factory.HttpClientFactory;
            var client = clientFactory.CreateClient("AuthenticationService");
            var host = GetAuthServiceHost();
            var pingUri = $"{host}/api/health/ping";

            var response = await client.GetAsync(pingUri);
            if (!response.IsSuccessStatusCode)
                throw new ServiceDown(host);

            var uri = $"{host}/api/authentication/register";
            response = await client.PostAsJsonAsync(uri, data);

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<AuthRegisteredData>();

            throw new ServiceDown("Couldn't register user");
        }

        public static async Task<AuthenticatedData> Authenticate(this IClient factory, AuthData data)
        {
            var clientFactory = factory.HttpClientFactory;
            var client = clientFactory.CreateClient("AuthenticationService");
            var host = GetAuthServiceHost();
            var pingUri = $"{host}/api/health/ping";

            var response = await client.GetAsync(pingUri);
            if (!response.IsSuccessStatusCode)
                throw new ServiceDown(host);

            var uri = $"{host}/api/authentication/authenticate";
            response = await client.PostAsJsonAsync(uri, data);

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<AuthenticatedData>();

            throw new ServiceDown("Couldn't authenticate user");
        }

        private static string GetAuthServiceHost()
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
            throw new EnvironmentNotSet("AUTHENTICATION_SERVICE");
        }
    }
}