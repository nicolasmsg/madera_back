using ConceptionDevisWS.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Web.Http;
using System.Net;
using ConceptionDevisWS.Services.Utils;
using System;

namespace ConceptionDevisWS.Services
{
    public class ProjectService
    {
        public async static Task<IEnumerable<Project>> GetAllProjects()
        {
            using (ModelsDBContext ctx = new ModelsDBContext())
            {
                return await ctx.Projects.Include( p => p.Client ).ToListAsync<Project>();
            }
        }

        public async static Task<Project> GetProject(int id)
        {
            if (id == 0)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            using (ModelsDBContext ctx = new ModelsDBContext())
            {
                Project project = await ctx.Projects.Include(p => p.Client).FirstOrDefaultAsync(c => c.Id == id);
                if (project == null)
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }
                return project;
            }
        }

        public async static Task RemoveProject(int id)
        {
            using (ModelsDBContext ctx = new ModelsDBContext())
            {
                Project seekedProject = await GetProject(id);
                ctx.Entry(seekedProject).State = EntityState.Deleted;
                await ctx.SaveChangesAsync();
            }
        }

        public async static Task<Project> CreateNew(Project newProject)
        {
            if (newProject.Name == null)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            using (ModelsDBContext ctx = new ModelsDBContext())
            {
                await ServiceHelper<Project>.LoadSingleNavigationProperty<Client>(newProject, ctx, p => p.Client, _getCtxClients, _setClient);
                ctx.Projects.Add(newProject);
                await ctx.SaveChangesAsync();
                return newProject;
            }
        }

        public async static Task<Project> UpdateProject(int id, Project newProject)
        {
            using (ModelsDBContext ctx = new ModelsDBContext())
            {
                Project seekedProject = await GetProject(id);
                
                ctx.Entry(seekedProject).State = EntityState.Modified;
                await ServiceHelper<Project>.SetSingleNavigationProperty<Client>(newProject, seekedProject, ctx, p => p.Client, _getCtxClients, _setClient);

                seekedProject.UpdateNonComposedPropertiesFrom(newProject);
                await ctx.SaveChangesAsync();
                return seekedProject;

            }
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