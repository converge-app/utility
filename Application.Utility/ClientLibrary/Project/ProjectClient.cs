using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Application.Utility.Exception;
using Microsoft.AspNetCore.Authentication;
using Newtonsoft.Json;

namespace Application.Utility.ClientLibrary.Project
{
    public static class ProjectClient
    {
        public static async Task<ProjectData> GetProjectAsync(this IClient factory, string projectId)
        {
            var clientFactory = factory.HttpClientFactory;
            var client = clientFactory.CreateClient("ProjectsService");
            var host = GetProjectsServiceHost();
            var pingUri = $"http://{host}/api/health/ping";
            var response = await client.GetAsync(pingUri);
            if (!response.IsSuccessStatusCode)
                throw new ServiceDown(host);

            var uri = $"http://{host}/api/projects/{projectId}";
            response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<ProjectData>();
            throw new ProjectNotFound();
        }

        public static async Task<bool> UpdateProjectAsync(this IClient factory, string authorizationToken,
            ProjectData project)
        {
            var clientFactory = factory.HttpClientFactory;
            
            var host = GetProjectsServiceHost();
            var uri = $"http://{host}/api/projects/{project.Id}";
            var content = project;
            
            using (var request = new HttpRequestMessage(HttpMethod.Put, uri))
            {
                var client = clientFactory.CreateClient("ProjectsServicePut");

                var jsonContent = JsonConvert.SerializeObject(content);
                request.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorizationToken);

                var response = await client.PutAsJsonAsync(uri, content);
                
                return response.IsSuccessStatusCode;
            }
        }

        private static string GetProjectsServiceHost()
        {
            var host = Environment.GetEnvironmentVariable("PROJECTS_SERVICE_HTTP");
            if (string.IsNullOrEmpty(host))
                throw new EnvironmentNotSet("PROJECTS_SERVICE_HTTP");
            return host;
        }
    }
}