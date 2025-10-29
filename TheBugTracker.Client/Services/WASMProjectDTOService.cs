using System.Net.Http.Json;
using TheBugTracker.Client.Models;
using TheBugTracker.Client.Services.Interfaces;

namespace TheBugTracker.Client.Services
{
    public class WASMProjectDTOService(HttpClient http) : IProjectDTOService
    {
        public async Task<IEnumerable<ProjectDTO>> GetProjectsAsync(UserInfo user)
        {
            try
            {
                List<ProjectDTO> projects = await http.GetFromJsonAsync<List<ProjectDTO>>("api/Projects") ?? [];

                return projects;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

                return [];
            }
        }

        public async Task<ProjectDTO?> GetProjectByIdAsync(int id, UserInfo user)
        {
            try
            {
                var project = await http.GetFromJsonAsync<ProjectDTO>($"api/Projects/{id}");

                return project;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null; 
            }
        }

        public async Task<ProjectDTO> CreateProjectAsync(ProjectDTO project, UserInfo user)
        {
            var response = await http.PostAsJsonAsync("api/Projects", project);
            response.EnsureSuccessStatusCode();

            ProjectDTO createdProject = await response.Content.ReadFromJsonAsync<ProjectDTO>()
                ?? throw new HttpIOException(HttpRequestError.InvalidResponse);

            return createdProject;
        }

        public async Task UpdateProjectAsync(ProjectDTO project, UserInfo user)
        {
            var response = await http.PutAsJsonAsync($"api/Projects/{project.Id}", project);
            response.EnsureSuccessStatusCode();


        }

        public async Task ArchiveProjectAsync(int projectId, UserInfo user)
        {
            var response = await http.PatchAsync($"api/Projects/{projectId}/archive", null);
            response.EnsureSuccessStatusCode();

        }

        public async Task RestoreProjectAsync(int projectId, UserInfo user)
        {
            var response = await http.PatchAsync($"api/Projects/{projectId}/restore", null);
            response.EnsureSuccessStatusCode();
        }

        public Task AddProjectMemberAsync(int projectId, string userId, UserInfo user)
        {
            throw new NotImplementedException();
        }

        public Task RemoveProjectMemberAsync(int projectId, string userId, UserInfo user)
        {
            throw new NotImplementedException();
        }
    }
}
