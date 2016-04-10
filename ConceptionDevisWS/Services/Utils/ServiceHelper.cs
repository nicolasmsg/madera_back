using ConceptionDevisWS.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace ConceptionDevisWS.Services.Utils
{
    public class ServiceHelper<T> where T : class, IIdentifiable
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

        public async static Task LoadSingleNavigationProperty<T2>(T src, DbContext context,
            Expression<Func<T, T2>> singlePropExpr, Func<DbContext, DbSet<T2>> getCtxSingleProp, Action<T, T2> setSingleProp)
            where T2 :class, IIdentifiable
        {
            Func<T, T2> getSingleProp = singlePropExpr.Compile();
            await EnsuresNotNew<T2>(src, getSingleProp, context, getCtxSingleProp);
            T2 srcProperty = getSingleProp(src);
            if (srcProperty != null)
            {
                T2 trackedProperty = await getCtxSingleProp(context).FirstOrDefaultAsync(prop => prop.Id == srcProperty.Id);
                context.Entry(trackedProperty).State = EntityState.Unchanged;
                setSingleProp(src, trackedProperty);
            }
        }


        public async static Task SetSingleNavigationProperty<T2>(T src, T dest, DbContext context,
          Expression<Func<T, T2>> singlePropExpr, Func<DbContext, DbSet<T2>> getCtxSingleProp, Action<T, T2> setSingleProp)
            where T2 : class, IIdentifiable
        {
            Func<T, T2> getSingleProp = singlePropExpr.Compile();
            await EnsuresNotNew<T2>(src, getSingleProp, context, getCtxSingleProp);
            T2 srcProperty = getSingleProp(src);
            if (srcProperty != null)
            {
                T2 trackedProperty = await getCtxSingleProp(context).FirstOrDefaultAsync(prop => prop.Id == srcProperty.Id);
                context.Entry(trackedProperty).State = EntityState.Unchanged;
                setSingleProp(dest, trackedProperty);
            } else
            {
               setSingleProp(dest, null);
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


        private async static Task EnsuresNotNew<T2>(T src,
            Func<T, T2> getSingleProp, 
            DbContext context, Func<DbContext, DbSet<T2>> getCtxSingleProp)
            where T2:class, IIdentifiable
        {
            T2 srcProperty = getSingleProp(src);
            if(srcProperty == null)
            {
                return;
            }    
            T2 trackedProperty = await getCtxSingleProp(context).FirstOrDefaultAsync(p => p.Id == srcProperty.Id);


            if (trackedProperty == null)
            {
                string errorMsg = string.Format("Following entity has not been found: {0}", srcProperty.Id);
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