using ConceptionDevisWS.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Web.Http;
using System.Net;
using ConceptionDevisWS.Services.Utils;
using System.Linq;

namespace ConceptionDevisWS.Services
{
    public class ProjectService
    {
        public async static Task<IEnumerable<Project>> GetClientProjects(int clientId)
        {
            using (ModelsDBContext ctx = new ModelsDBContext())
            {
                Client seekedClient = await _searchClient(clientId);
                return seekedClient.Projects;

            }
        }

        public async static Task<Project> GetClientProject(int clientId, int id)
        {
            
            using (ModelsDBContext ctx = new ModelsDBContext())
            {
                return await _searchClientProject(clientId, id);
            }
        }

        public async static Task RemoveProject(int clientId, int id)
        {
            using (ModelsDBContext ctx = new ModelsDBContext())
            {
                Project seekedProject = await _searchClientProject(clientId, id);
                ctx.Entry(seekedProject).State = EntityState.Deleted;
                await ctx.SaveChangesAsync();
            }
        }

        public async static Task<Project> CreateNew(int clientId, Project newProject)
        {
            if (newProject.Name == null)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            await _searchClient(clientId);
            newProject.Client.Id = clientId;

            using (ModelsDBContext ctx = new ModelsDBContext())
            {
                await ServiceHelper<Project>.LoadSingleNavigationProperty<Client>(newProject, ctx, p => p.Client, _getCtxClients, _setClient);
                ctx.Projects.Add(newProject);
                await ctx.SaveChangesAsync();
                return newProject;
            }
        }

        public async static Task<Project> UpdateProject(int clientId, int id, Project newProject)
        {
            using (ModelsDBContext ctx = new ModelsDBContext())
            {
                newProject.Client.Id = clientId;

                Project seekedProject = await _searchClientProject(clientId, id);
                ctx.Entry(seekedProject).State = EntityState.Modified;
                await ServiceHelper<Project>.SetSingleNavigationProperty<Client>(newProject, seekedProject, ctx, p => p.Client, _getCtxClients, _setClient);

                seekedProject.UpdateNonComposedPropertiesFrom(newProject);
                await ctx.SaveChangesAsync();
                return seekedProject;

            }
        }

        private async static Task<Client> _searchClient(int clientId)
        {
            return await ClientService.GetClient(clientId);
        }

        private async static Task<Project> _searchClientProject(int clientId, int id)
        {
            if (id == 0)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            Client seekedClient = await _searchClient(clientId);
            Project seekedProject = seekedClient.Projects.FirstOrDefault(p => p.Id == id);
            if(seekedClient == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            return seekedProject;
        }

        private static void _setClient(Project project, Client client)
        {
            project.Client = client;
        }

        private static DbSet<Client> _getCtxClients(DbContext ctx)
        {
            return ((ModelsDBContext)ctx).Clients;
        }

    }
}