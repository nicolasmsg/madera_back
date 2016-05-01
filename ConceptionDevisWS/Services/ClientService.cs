using ConceptionDevisWS.Models;
using ConceptionDevisWS.Services.Utils;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

//! \brief Business layer handling the data access.
//!
//! It's justified here as the only business is to provide data.
namespace ConceptionDevisWS.Services
{
    /// <summary>
    /// A Service to manage a <see cref="ConceptionDevisWS.Models.Client"/>'s business.
    /// </summary>
    public static class ClientService
    {
        /// <summary>
        /// Retrieve all existing <see cref="ConceptionDevisWS.Models.Client"/>s with their <see cref="ConceptionDevisWS.Models.Project"/>s.
        /// </summary>
        /// <returns>a list of clients</returns>
        public async static Task<IEnumerable<Client>> GetAllClients()
        {
            using (ModelsDBContext ctx = new ModelsDBContext())
            {
                return await ctx.Clients.Include( c => c.Projects )
                    .Where( c => c.Id != 1)
                    .ToListAsync<Client>();
            }
        }

        /// <summary>
        /// Retrieve the given <see cref="ConceptionDevisWS.Models.Client"/>.
        /// </summary>
        /// <param name="id">the client's identity</param>
        /// <returns>the client</returns>
        /// <exception cref="HttpResponseException">In case something went wrong (for example, when the requested client does not exists).</exception>
        public async static Task<Client> GetClient(int id)
        {
            if(id == 0 || id == 1)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            using (ModelsDBContext ctx = new ModelsDBContext())
            {
                Client client =  await ctx.Clients.Include( c => c.Projects ).FirstOrDefaultAsync( c => c.Id == id);
                if(client == null)
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }
                return client;
            }
        }

        /// <summary>
        /// Remove the given <see cref="ConceptionDevisWS.Models.Client"/> from storage.
        /// </summary>
        /// <param name="id">the client's identity</param>
        /// <returns>The request's HttpStatusCode</returns>
        /// <exception cref="HttpResponseException">In case something went wrong (for example, when the requested client does not exists).</exception>
        public async static Task RemoveClient(int id)
        {
            using (ModelsDBContext ctx = new ModelsDBContext())
            {
                Client seekedClient = await GetClient(id);
                Client maderaClient = await ctx.Clients.FindAsync(1);
                IEnumerable<Task> tasks = seekedClient.Projects.Select(p => {
                    return ProjectService.SpecialUpdateProject(p.Client.Id, maderaClient.Id, p.Id, p);
                });
                await Task.WhenAll(tasks);
                ctx.Entry(seekedClient).Collection(c => c.Projects).EntityEntry.State = EntityState.Modified;
                seekedClient.Projects.Clear();
                ctx.Entry(seekedClient).State = EntityState.Deleted;
                await ctx.SaveChangesAsync();
            }
        }

        public async static Task<Client> GetMaderaClient()
        {
            using (ModelsDBContext ctx = new ModelsDBContext())
            {
                return await ctx.Clients.FirstAsync(c => c.FirstName == "Madera" && c.ZipCode == -1);
            }
        }

        /// <summary>
        /// Create a new <see cref="ConceptionDevisWS.Models.Client"/>.
        /// </summary>
        /// <param name="newClient">the client to store</param>
        /// <returns>the stored client</returns>
        /// <exception cref="HttpResponseException">In case something went wrong (when the given client is null).</exception>
        public async static Task<Client> CreateNew(Client newClient)
        {
            if(newClient == null || newClient.FirstName == null || newClient.LastName == null)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            using (ModelsDBContext ctx = new ModelsDBContext())
            {
                await ServiceHelper<Client>.InitNavigationProperty<Project>(newClient, ctx, _getProjects, _getCtxProjects);
                await ServiceHelper<Client>.LoadSingleNavigationProperty<User>(newClient, ctx, c => c.User, _getCtxUsers, _setUser);
                ctx.Clients.Add(newClient);
                await ctx.SaveChangesAsync();
                return newClient;
            }
        }

        /// <summary>
        /// Update completely the given <see cref="ConceptionDevisWS.Models.Client"/>.
        /// </summary>
        /// <param name="id">the client's identity</param>
        /// <param name="newClient">the updated client to store</param>
        /// <returns>the updated client</returns>
        /// <exception cref="HttpResponseException">In case something went wrong (for example, when the requested client does not exists).</exception>
        public async static Task<Client> UpdateClient(int id, Client newClient)
        {
            using (ModelsDBContext ctx = new ModelsDBContext())
            {
                Client seekedClient = await GetClient(id);
                ctx.Entry(seekedClient).State = EntityState.Modified;
                ctx.Entry(seekedClient).Collection(c => c.Projects).EntityEntry.State = EntityState.Modified;
                ctx.Entry(seekedClient).Reference(c => c.User).EntityEntry.State = EntityState.Modified;

                await ServiceHelper<Client>.UpdateNavigationProperty<Project>(newClient, seekedClient, ctx, _getProjects, _getCtxProjects);
                await ServiceHelper<Client>.SetSingleNavigationProperty<User>(newClient, seekedClient, ctx, c => c.User, _getCtxUsers, _setUser);
                seekedClient.UpdateNonComposedPropertiesFrom(newClient);
                await ctx.SaveChangesAsync();
                return seekedClient;
       
            }
        }

        private static ICollection<Project> _getProjects(Client client)
        {
            return client.Projects;
        }

        private static DbSet<Project> _getCtxProjects(DbContext dbContext)
        {
            return ((ModelsDBContext)dbContext).Projects;
        }

        private static DbSet<User> _getCtxUsers(DbContext dbContext)
        {
            return ((ModelsDBContext)dbContext).Users;
        }

        private static void _setUser(Client client, User user)
        {
            client.User = user;
        }
    }
}