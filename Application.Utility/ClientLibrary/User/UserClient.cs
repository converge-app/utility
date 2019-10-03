using System;
using System.Net.Http;
using System.Threading.Tasks;
using Application.Utility.Exception;

namespace Application.Utility.ClientLibrary
{
    public static class UserClient
    {
        public static async Task<UserData> GetUserAsync(this IClient factory, string ownerId)
        {
            var clientFactory = factory.HttpClientFactory;
            var client = clientFactory.CreateClient("UsersService");
            var host = GetUsersServiceHost();
            var pingUri = $"http://{host}/api/health/ping";
            var response = await client.GetAsync(pingUri);
            if (!response.IsSuccessStatusCode)
                throw new ServiceDown(host);

            var uri = $"http://{host}/api/users/{ownerId}";
            response = await client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<UserData>();
            else
            {
                throw new UserNotFound();
            }
        }

        private static string GetUsersServiceHost()
        {
            var host = Environment.GetEnvironmentVariable("USERS_SERVICE_HTTP");
            if (string.IsNullOrEmpty(host))
                throw new EnvironmentNotSet("USERS_SERVICE_HTTP");
            return host;
        }
    }
}