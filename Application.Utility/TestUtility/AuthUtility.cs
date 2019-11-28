using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Application.Utility.ClientLibrary.Authentication;

namespace ApplicationModulTests.TestUtility
{
    public static class AuthUtility
    {
        public static async Task<AuthRegisteredData> RegisterUser(HttpClient client, AuthRegisterData user, bool isLocal = false)
        {
            string url;

            if (isLocal)
                url = "/api/authentication/register";
            else
                url = "https://authentication-service.api.converge-app.net/api/authentication/register";

            var response = await client.PostAsJsonAsync(url, user);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<AuthRegisteredData>();
            else
                throw new Exception("Was unsuccessful");
        }

        public static async Task<AuthenticatedData> AuthenticateUser(HttpClient client, AuthData data, bool isLocal = false)
        {
            string url;

            if (isLocal)
                url = "/api/authentication/authenticate";
            else
                url = "https://authentication-service.api.converge-app.net/api/authentication/authenticate";

            var response = await client.PostAsJsonAsync(url, data);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<AuthenticatedData>();
            else
                throw new Exception("Was unsuccessful");
        }

        public static AuthRegisterData GenerateUser()
        {
            return new AuthRegisterData
            {
                FirstName = Guid.NewGuid().ToString(),
                LastName = Guid.NewGuid().ToString(),
                Email = Guid.NewGuid().ToString() + "test@gmail.com",
                Password = Guid.NewGuid().ToString()
            };
        }

        public static AuthData GenerateCredentials(AuthRegisterData data)
        {
            return new AuthData
            {
                Email = data.Email,
                Password = data.Password
            };
        }

        public static AuthData GenerateCredentials(string email, string password)
        {
            return new AuthData
            {
                Email = email,
                Password = password
            };
        }

        public static async Task<AuthenticatedData> GenerateAndAuthenticate(HttpClient client, bool isLocal = false)
        {
            var user = AuthUtility.GenerateUser();
            await AuthUtility.RegisterUser(client, user, isLocal);
            var authUser = await AuthUtility.AuthenticateUser(client, AuthUtility.GenerateCredentials(user), isLocal);
            return authUser;
        }

        public static HttpClient AddAuthorization(HttpClient client, string token)
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return client;
        }
    }
}