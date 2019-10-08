using System.Net.Http;
using System.Threading.Tasks;
using Application.Utility.Exception;
using static System.Environment;

namespace Application.Utility.ClientLibrary
{
    public static class UserClient
    {
        public static async Task<UserData> GetUserAsync(this IClient factory, string ownerId)
        {
            var clientFactory = factory.HttpClientFactory;
            var client = clientFactory.CreateClient("UsersService");
            var host = GetUsersServiceHost();
            var pingUri = $"{host}/api/health/ping";
            var response = await client.GetAsync(pingUri);
            if (!response.IsSuccessStatusCode)
                throw new ServiceDown(host);

            var uri = $"{host}/api/users/{ownerId}";
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
            var host = "";
            if (!string.IsNullOrEmpty(GetEnvironmentVariable("USERS_SERVICE_HTTP")))
                host = "http://" + GetEnvironmentVariable("USERS_SERVICE_HTTP");
            else if (!string.IsNullOrEmpty(GetEnvironmentVariable("USERS_SERVICE_HTTPS")))
                host = "https://" + GetEnvironmentVariable("USERS_SERVICE_HTTPS");
            else
                throw new EnvironmentNotSet("USERS_SERVICE");
            return host;
        }
    }
}