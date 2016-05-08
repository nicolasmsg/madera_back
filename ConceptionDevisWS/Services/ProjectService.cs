using ConceptionDevisWS.Models;
using ConceptionDevisWS.Models.Converters;
using ConceptionDevisWS.Services.Utils;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using System;

namespace ConceptionDevisWS.Services
{
    /// <summary>
    /// A Service to manage a <see cref="ConceptionDevisWS.Models.Project"/>'s business.
    /// </summary>
    public class ProjectService
    {
        /// <summary>
        /// Retrieve a <see cref="ConceptionDevisWS.Models.Client"/>'s <see cref="ConceptionDevisWS.Models.Project"/>s.
        /// </summary>
        /// <param name="clientId">the client's identity</param>
        /// <param name="lang">the culture to get projects into (fr-FR or en-US)</param>
        /// <returns>a list of projects</returns>
        /// <exception cref="HttpResponseException">In case something went wrong (for example when the given client doesn't exist).</exception>
        public async static Task<IEnumerable<Project>> GetClientProjects(int clientId, string lang)
        {
            using (ModelsDBContext ctx = new ModelsDBContext())
            {
                Client seekedClient = await _searchClient(clientId, lang);
                return seekedClient.Projects;

            }
        }

        /// <summary>
        /// Retrieve a <see cref="ConceptionDevisWS.Models.Client"/>'s <see cref="ConceptionDevisWS.Models.Project"/>.
        /// </summary>
        /// <param name="clientId">the client's identity</param>
        /// <param name="id">the project's identity</param>
        /// <param name="lang">the culture to get the project into (fr-FR or en-US)</param>
        /// <returns>a project</returns>
        /// <exception cref="HttpResponseException">In case either the client or project doesn't exist.</exception>
        public async static Task<Project> GetClientProject(int clientId, int id, string lang)
        {
            
            using (ModelsDBContext ctx = new ModelsDBContext())
            {
                return await _searchClientProject(clientId, id, lang);
            }
        }

        /// <summary>
        /// Remove the given <see cref="ConceptionDevisWS.Models.Project"/> from storage.
        /// </summary>
        /// <param name="clientId">the client's identity</param>
        /// <param name="id">the project's identity</param>
        /// <exception cref="HttpResponseException">In case either the client or project doesn't exist.</exception>
        public async static Task RemoveProject(int clientId, int id)
        {
            using (ModelsDBContext ctx = new ModelsDBContext())
            {
                Project seekedProject = await _searchClientProject(clientId, id, "fr-FR");
                ctx.Entry(seekedProject).State = EntityState.Deleted;
                await ctx.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Create a new <see cref="ConceptionDevisWS.Models.Project"/> for an existing <see cref="ConceptionDevisWS.Models.Client"/>.
        /// </summary>
        /// <param name="clientId">the client's identity</param>
        /// <param name="newProject">the project to store</param>
        /// <param name="lang">the culture to create the project into (fr-FR or en-US)</param>
        /// <returns>the created project</returns>
        public async static Task<Project> CreateNew(int clientId, Project newProject, string lang)
        {
            if (newProject == null || newProject.Name == null)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            await _searchClient(clientId, lang);
            newProject.Client.Id = clientId;

            using (ModelsDBContext ctx = new ModelsDBContext())
            {
                await ServiceHelper<Project>.LoadSingleNavigationProperty<Client>(newProject, ctx, p => p.Client, _getCtxClients, _setClient);
                ctx.Projects.Add(newProject);
                await ctx.SaveChangesAsync();
                return newProject;
            }
        }

        /// <summary>
        /// Update completely a given <see cref="ConceptionDevisWS.Models.Project"/>.
        /// </summary>
        /// <param name="clientId">the client's identity</param>
        /// <param name="id">the project's identity</param>
        /// <param name="newProject">the updated project </param>
        /// <param name="lang">the culture to update the project into (fr-FR or en-US)</param>
        /// <returns>the updated project</returns>
        /// <exception cref="HttpResponseException">In case either the client or project doesn't exist.</exception>
        public async static Task<Project> UpdateProject(int clientId, int id, Project newProject, string lang)
        {
            using (ModelsDBContext ctx = new ModelsDBContext())
            {
                if(newProject == null || newProject.Name == null)
                {
                    throw new HttpResponseException(HttpStatusCode.BadRequest);
                }

                if (newProject.Client == null)
                {
                    newProject.Client = new Client();
                }
                newProject.Client.Id = clientId;

                Project seekedProject = await _searchClientProject(clientId, id, lang);
                ctx.Entry(seekedProject).State = EntityState.Modified;
                ctx.Entry(seekedProject).Collection(p => p.Products).EntityEntry.State = EntityState.Modified;
                await ServiceHelper<Project>.SetSingleNavigationProperty<Client>(newProject, seekedProject, ctx, p => p.Client, _getCtxClients, _setClient);
                await ServiceHelper<Project>.UpdateNavigationProperty<Product>(newProject, seekedProject, ctx, _getProducts, _getCtxProducts);

                seekedProject.UpdateNonComposedPropertiesFrom(newProject);
                bool updateSuccess = false;
                do
                {
                    try
                    {
                        await ctx.SaveChangesAsync();
                        updateSuccess = true;
                    } catch(DbUpdateConcurrencyException dbuce)
                    {
                        DbEntityEntry entry = dbuce.Entries.Single();
                        entry.OriginalValues.SetValues(await entry.GetDatabaseValuesAsync());
                    }
                } while (!updateSuccess);
                return seekedProject;

            }
        }

        private static DbSet<Product> _getCtxProducts(DbContext context)
        {
            return ((ModelsDBContext)context).Products;
        }

        private static ICollection<Product> _getProducts(Project project)
        {
            return project.Products;
        }

        /// <summary>
        /// Internal use only, Update completely a given <see cref="ConceptionDevisWS.Models.Project"/> Including its <see cref="ConceptionDevisWS.Models.Client"/>.
        /// </summary>
        /// <param name="originalClientId">the original client's identity</param>
        /// <param name="newClientId">the new client's identity</param>
        /// <param name="id">the project's identity</param>
        /// <param name="newProject">the updated project (possibly with a different client) </param>
        /// <param name="lang">the culture to udpate the project into (fr-FR or en-US)</param>
        /// <returns>the updated project</returns>
        /// <exception cref="HttpResponseException">In case either the client or project doesn't exist.</exception>
        public async static Task<Project> SpecialUpdateProject(int originalClientId, int newClientId, int id, Project newProject, string lang)
        {
            using (ModelsDBContext ctx = new ModelsDBContext())
            {
                if (newProject.Client == null)
                {
                    newProject.Client = new Client();
                }
                newProject.Client.Id = newClientId;

                if(newProject == null || newProject.Name == null)
                {
                    throw new HttpResponseException(HttpStatusCode.BadRequest);
                }

                Project seekedProject = await _searchClientProject(originalClientId, id, lang);
                ctx.Entry(seekedProject).State = EntityState.Modified;
                await ServiceHelper<Project>.SetSingleNavigationProperty<Client>(newProject, seekedProject, ctx, p => p.Client, _getCtxClients, _setClient);

                seekedProject.UpdateNonComposedPropertiesFrom(newProject);
                await ctx.SaveChangesAsync();
                return seekedProject;

            }
        }

        private async static Task<Client> _searchClient(int clientId, string lang)
        {
            CulturalEnumStringConverter.Culture = new CultureInfo(lang);
            return await ClientService.GetClient(clientId);
        }

        private async static Task<Project> _searchClientProject(int clientId, int id, string lang)
        {
            if (id == 0)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            Client seekedClient = await _searchClient(clientId, lang);
            Project seekedProject = seekedClient.Projects.FirstOrDefault(p => p.Id == id);
            if(seekedClient == null || seekedProject == null)
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