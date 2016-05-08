using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ConceptionDevisWS.Models;
using System.Data.Entity;
using System.Web.Http;
using System.Net;
using ConceptionDevisWS.Services.Utils;
using ConceptionDevisWS.Models.Converters;
using System.Globalization;

namespace ConceptionDevisWS.Services
{
    public class ProductService
    {
        public async static Task<List<Product>> GetAllProducts(string lang)
        {
            CulturalEnumStringConverter.Culture = new CultureInfo(lang);
            using (ModelsDBContext ctx = new ModelsDBContext())
            {
                return await ctx.Products.ToListAsync();
            }
        }

        public async static Task<Product> GetProduct(int id, string lang)
        {
            if (id == 0)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            CulturalEnumStringConverter.Culture = new CultureInfo(lang);
            using (ModelsDBContext ctx = new ModelsDBContext())
            {
                Product product = await ctx.Products
                    .Include(p => p.Model)
                    .FirstOrDefaultAsync(p => p.Id == id);
                if(product == null)
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }
                return product;
            }
        }

        public async static Task<Product> CreateNew(Product newProduct, string lang)
        {
            if(newProduct == null || newProduct.Name == null)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            CulturalEnumStringConverter.Culture = new CultureInfo(lang);
            using (ModelsDBContext ctx = new ModelsDBContext())
            {
                await ServiceHelper<Product>.LoadSingleNavigationProperty(newProduct, ctx, p => p.Model, _getCtxModels, _setModel);
                ctx.Products.Add(newProduct);
                await ctx.SaveChangesAsync();
                return newProduct;
            }
        }

        public async static Task<Product> UpdateProduct(int id, Product newProduct, string lang)
        {
            if(newProduct == null || newProduct.Name == null)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            using (ModelsDBContext ctx = new ModelsDBContext())
            {
                Product seekedProduct = await GetProduct(id, lang);
                ctx.Entry(seekedProduct).State = EntityState.Modified;
                ctx.Entry(seekedProduct).Reference(p => p.Model).EntityEntry.State = EntityState.Unchanged;
                seekedProduct.UpdateNonComposableProperties(newProduct);
                await ctx.SaveChangesAsync();
                return newProduct;
            }
        }

        public async static Task RemoveProduct(int id)
        {
            using (ModelsDBContext ctx = new ModelsDBContext())
            {
                Product seekedProduct = await GetProduct(id, "fr-FR");
                ctx.Entry(seekedProduct).State = EntityState.Deleted;
                ctx.Entry(seekedProduct).Reference(p => p.Model).EntityEntry.State = EntityState.Unchanged;
                // deletes the relation between the removed product and its model
                ctx.Entry(seekedProduct).Collection( p => new Model[] { p.Model }).EntityEntry.State = EntityState.Deleted;
                await ctx.SaveChangesAsync();
            }
            
        }

        private static void _setModel(Product product, Model model)
        {
            product.Model = model;
        }

        private static DbSet<Model> _getCtxModels(DbContext context)
        {
            return ((ModelsDBContext)context).Models;
        }
    }
}