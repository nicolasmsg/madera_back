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
    /// <summary>
    /// Handles Navigation property within EntityFramework in a disconnected scenario.
    /// </summary>
    /// <typeparam name="T">the type of the <see cref="ConceptionDevisWS.Models">Model</see> to manage</typeparam>
    public class ServiceHelper<T> where T : class, IIdentifiable
    {
        /// <summary>
        /// Update the destination's collection from the source and the storage
        /// </summary>
        /// <typeparam name="T2">type of the navigation property</typeparam>
        /// <param name="src">the source</param>
        /// <param name="dest">the destination</param>
        /// <param name="context">the storage context to use</param>
        /// <param name="getCollection">a function to retrieve the source's collection</param>
        /// <param name="getCtxCollection">a function to retrieve the context's stored collection of all elements of type T2</param>
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

        /// <summary>
        /// Let EntityFramework know a collection of related entities already exists.
        /// </summary>
        /// <remarks>
        /// When we create an entity X we do not create any associated entities in a single request,
        /// but we can still associate our new entity with others pre-existing ones in one go, that's what this method is intended for.
        /// </remarks>
        /// <typeparam name="T2">a collection type</typeparam>
        /// <param name="src">the object which collection will be initialized in EntityFramework's context management</param>
        /// <param name="context">the storage context</param>
        /// <param name="getCollection">a function to retrieve the collection from the source</param>
        /// <param name="getCtxCollection">a function to retrieve the collection of existing entities from the storage context</param>
        /// <returns></returns>
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

        /// <summary>
        /// Let EntityFramework knows a single related entity already exists.
        /// </summary>
        /// <typeparam name="T2">the related entity's type</typeparam>
        /// <param name="src">the source associated with that related entity</param>
        /// <param name="context">the storage context</param>
        /// <param name="singlePropExpr">a function used to retrieve the associated property from the source</param>
        /// <param name="getCtxSingleProp">a function to retrieve the associated property from the storage context</param>
        /// <param name="setSingleProp">a function to define the associated property on the source</param>
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

        /// <summary>
        /// Update a single related entity from a source to a destination
        /// </summary>
        /// <typeparam name="T2">the related entity's type</typeparam>
        /// <param name="src">the source associated with that related entity</param>
        /// <param name="dest">the destination which related entity will be updated</param>
        /// <param name="context">the storage context</param>
        /// <param name="singlePropExpr">a function used to retrieve the associated property from the source</param>
        /// <param name="getCtxSingleProp">a function to retrieve the associated property from the storage context</param>
        /// <param name="setSingleProp">a function to define the associated property on the source</param>
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