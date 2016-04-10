using ConceptionDevisWS.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

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
            ICollection<T2> destElements = getCollection(dest);
            if (destElements != null)
            {
                destElements.Clear();
            }
            await AddAllElementsFromContext(src, dest, context, getCollection, getCtxCollection);
        }

        public async static Task InitNavigationProperty<T2>(T src, DbContext context,
            Func<T, ICollection<T2>> getCollection, Func<DbContext, DbSet<T2>> getCtxCollection) 
            where T2 : class, IIdentifiable
        {
            await EnsuresNoNewElement<T2>(src, getCollection, context, getCtxCollection);
            ICollection<T2> srcElements = getCollection(src);
            if (srcElements != null)
            {
                ICollection<T2> elements = new List<T2>(srcElements);
                if (srcElements != null)
                {
                    srcElements.Clear();
                }

                if(elements != null)
                {
                    foreach (T2 element in elements)
                    {
                        T2 trackedElement = await getCtxCollection(context).FindAsync(element.Id);
                        context.Entry(trackedElement).State = EntityState.Unchanged;
                        srcElements.Add(trackedElement);
                    }
                }
                
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

            if (srcElements != null && contextElements != null)
            {
                foreach (T2 srcElement in srcElements)
                {
                    if (await contextElements.FirstOrDefaultAsync(contextElement => contextElement.Id == srcElement.Id) == null)
                    {
                        newElements.Add(srcElement);
                    }
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
            ICollection<T2> srcElements = getCollection(src);
            ICollection<T2> destElements = getCollection(dest);

            if (destElements == null)
            {
                destElements = new List<T2>();
            }

            if (srcElements != null)
            {

                foreach (T2 element in srcElements)
                {
                    T2 trackedElement = await getCtxCollection(context).FindAsync(element.Id);
                    destElements.Add(trackedElement);
                }
            }
        }
        
    }
}