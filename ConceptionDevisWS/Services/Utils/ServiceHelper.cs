using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Net.Http;
using System.Net;
using ConceptionDevisWS.Models;

namespace ConceptionDevisWS.Services.Utils
{
    public class ServiceHelper<T> where T : class
    {
        public async static Task UpdateNavigationProperty<T2>(T src, T dest, DbContext context, 
            Func<T, ICollection<T2>> getCollection, 
            Func<DbContext, DbSet<T2>> getCtxCollection) 
            where T2 : class, IIdentifiable
        {
            await EnsuresNoNewElement<T2>(src, getCollection, context, getCtxCollection);
            getCollection(dest).Clear();
            await AddAllElementsFromContext(src, dest, context, getCollection, getCtxCollection);
        }

        public async static Task InitNavigationProperty<T2>(T src, DbContext context,
            Func<T, ICollection<T2>> getCollection, Func<DbContext, DbSet<T2>> getCtxCollection) 
            where T2 : class, IIdentifiable
        {
            await EnsuresNoNewElement<T2>(src, getCollection, context, getCtxCollection);
            ICollection<T2> elements = new List<T2>(getCollection(src));
            getCollection(src).Clear();
            
            foreach (T2 element in elements)
            {
                T2 trackedElement = await getCtxCollection(context).FindAsync(element.Id);
                context.Entry(trackedElement).State = EntityState.Unchanged;
                getCollection(src).Add(trackedElement);
            }
        }
            
        private async static Task EnsuresNoNewElement<T2>(T src, 
            Func<T, ICollection<T2>> getCollection, 
            DbContext context, Func<DbContext, DbSet<T2>> getCtxCollection
        ) 
            where T2 : class, IIdentifiable
        {
            List<T2> newElements = new List<T2>();

            ICollection<T2> srcElements = getCollection(src);
            DbSet<T2> contextElements = getCtxCollection(context);

            foreach(T2 srcElement in srcElements)
            {
                if(await contextElements.FirstOrDefaultAsync( contextElement => contextElement.Id == srcElement.Id  ) == null)
                {
                    newElements.Add(srcElement);
                }
            }

            if (newElements.Count > 0)
            {
                string errorMsg = string.Format("Following entities have not been found: {0}", string.Concat(newElements.ConvertAll( elt => elt.Id )));
                throw new HttpResponseException(new HttpResponseMessage { StatusCode = HttpStatusCode.NotFound, Content = new StringContent(errorMsg) });
            }
        }

        private async static Task AddAllElementsFromContext<T2>(T src, T dest, DbContext context,
            Func<T,ICollection<T2>> getCollection, Func<DbContext, DbSet<T2>> getCtxCollection) 
            where T2 : class, IIdentifiable
        {
            foreach (T2 element in getCollection(src))
            {
                T2 trackedElement = await getCtxCollection(context).FindAsync(element.Id);
                getCollection(dest).Add(trackedElement);
            }
        }
        
    }
}