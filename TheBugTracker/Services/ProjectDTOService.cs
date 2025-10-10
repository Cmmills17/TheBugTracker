﻿using Microsoft.CodeAnalysis;
using TheBugTracker.Client.Models;
using TheBugTracker.Client.Services.Interfaces;
using TheBugTracker.Services.Interfaces;
using TheBugTracker.Models;

using Project = TheBugTracker.Models.Project;
using TheBugTracker.Client;
namespace TheBugTracker.Services
{
    public class ProjectDTOService(IProjectRepository repository) : IProjectDTOService
    {
        public async Task<IEnumerable<ProjectDTO>> GetProjectsAsync(UserInfo user)
        {

            IEnumerable<Project> projects = await repository.GetProjectsAsync(user);

            IEnumerable<ProjectDTO> dtos = projects.Select(p => p.ToDTO());

            return dtos;

        }

        public async Task<ProjectDTO> CreateProjectAsync(ProjectDTO project, UserInfo user)
        {
            Project dbProject = new Project()
            {

                Name = project.Name,
                Description = project.Description,
                Created = DateTimeOffset.UtcNow,
                StartDate = project.StartDate ?? DateTimeOffset.UtcNow,
                EndDate = project.EndDate ?? DateTimeOffset.UtcNow + TimeSpan.FromDays(7),
                Priority = project.Priority,
                Archive = false,
                CompanyId = user.CompanyId,
                
            };

            dbProject = await repository.CreateProjectAsync(dbProject, user);
            return dbProject.ToDTO();

        }
    }
}
