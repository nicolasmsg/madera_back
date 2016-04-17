using ConceptionDevisWS.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Web.Http;
using System.Net;
using ConceptionDevisWS.Services.Utils;

namespace ConceptionDevisWS.Services
{
    public static class ClientService
    {
        public async static Task<IEnumerable<Client>> GetAllClients()
        {
            using (ModelsDBContext ctx = new ModelsDBContext())
            {
                return await ctx.Clients.Include( c => c.Projects ).ToListAsync<Client>();
            }
        }

        public async static Task<Client> GetClient(int id)
        {
            if(id==0)
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

        public async static Task RemoveClient(int id)
        {
            using (ModelsDBContext ctx = new ModelsDBContext())
            {
                Client seekedClient = await GetClient(id);
                ctx.Entry(seekedClient).State = EntityState.Deleted;
                await ctx.SaveChangesAsync();
            }
        }

        public async static Task<Client> CreateNew(Client newClient)
        {
            if(newClient == null || newClient.FirstName == null || newClient.LastName == null)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            using (ModelsDBContext ctx = new ModelsDBContext())
            {
                await ServiceHelper<Client>.InitNavigationProperty<Project>(newClient, ctx, _getProjects, _getCtxProjects);
                ctx.Clients.Add(newClient);
                await ctx.SaveChangesAsync();
                return newClient;
            }
        }

        public async static Task<Client> UpdateClient(int id, Client newClient)
        {
            using (ModelsDBContext ctx = new ModelsDBContext())
            {
                Client seekedClient = await GetClient(id);
                ctx.Entry(seekedClient).State = EntityState.Modified;
                ctx.Entry(seekedClient).Collection(c => c.Projects).EntityEntry.State = EntityState.Modified;

                await ServiceHelper<Client>.UpdateNavigationProperty<Project>(newClient, seekedClient, ctx, _getProjects, _getCtxProjects);
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
    }
}