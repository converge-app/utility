using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Application.Utility.Exception;
using Microsoft.AspNetCore.Authentication;
using Newtonsoft.Json;
using static System.Environment;

namespace Application.Utility.ClientLibrary.Project
{
    public static class ProjectClient
    {
        public static async Task<ProjectData> GetProjectAsync(this IClient factory, string projectId)
        {
            var clientFactory = factory.HttpClientFactory;
            var client = clientFactory.CreateClient("ProjectsService");
            var host = GetProjectsServiceHost();
            var pingUri = $"{host}/api/health/ping";
            var response = await client.GetAsync(pingUri);
            if (!response.IsSuccessStatusCode)
                throw new ServiceDown(host);

            var uri = $"{host}/api/projects/{projectId}";
            response = await client.GetAsync(uri);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<ProjectData>();
            throw new ProjectNotFound();
        }

        public static async Task<bool> UpdateProjectAsync(this IClient factory, string authorizationToken,
            ProjectData project)
        {
            var clientFactory = factory.HttpClientFactory;
            var client = clientFactory.CreateClient("ProjectsService");
            var host = GetProjectsServiceHost();
            var pingUri = $"{host}/api/health/ping";
            
            var response = await client.GetAsync(pingUri);
            if (!response.IsSuccessStatusCode)
                throw new ServiceDown(host);
            
            var content = project;
            var uri = $"{host}/api/projects/{project.Id}";
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authorizationToken);

            response = await client.PutAsJsonAsync(uri, content);
            return response.IsSuccessStatusCode;
        }

        private static string GetProjectsServiceHost()
        {
            var host = "";
            if (!string.IsNullOrEmpty(GetEnvironmentVariable("PROJECTS_SERVICE_HTTP")))
                host = "http://" + GetEnvironmentVariable("PROJECTS_SERVICE_HTTP");
            else if (!string.IsNullOrEmpty(GetEnvironmentVariable("PROJECTS_SERVICE_HTTPS")))
                host = "https://" + GetEnvironmentVariable("PROJECTS_SERVICE_HTTPS");
            else 
                throw new EnvironmentNotSet("PROJECTS_SERVICE");
            return host;
        }
    }
}