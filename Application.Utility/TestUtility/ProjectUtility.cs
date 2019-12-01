using System;
using System.Net.Http;
using System.Threading.Tasks;
using Application.Utility.ClientLibrary.Project;

namespace ApplicationModulTests.TestUtility
{
    public static class ProjectUtility
    {
        public static async Task<ProjectData> CreateProject(HttpClient project, ProjectData user, bool isLocal = false)
        {
            string url;

            if (isLocal)
                url = "/api/Projects";
            else
                url = "https://projects-service.api.converge-app.net/api/Projects";

            var response = await project.PostAsJsonAsync(url, user);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadAsAsync<ProjectData>();
            else
                throw new Exception("Was unsuccessful");
        }

        public static ProjectData GenerateProject(string OwnerId)
        {
            return new ProjectData
            {
                OwnerId = OwnerId,
                ProjectContent = new ProjectContentData
                {
                    Title = Guid.NewGuid().ToString(),
                    Description = Guid.NewGuid().ToString(),
                    Category = Guid.NewGuid().ToString(),
                    SubCategory = Guid.NewGuid().ToString(),
                    Amount = 100,
                }

            };
        }
    }
}