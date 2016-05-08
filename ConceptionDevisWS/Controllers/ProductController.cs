using ConceptionDevisWS.Models;
using ConceptionDevisWS.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace ConceptionDevisWS.Controllers
{
    /// <summary>
    /// Controller to manager <see cref="ConceptionDevisWS.Models.Product"/>s.
    /// </summary>
    public class ProductController : ApiController
    {
        /// <summary>
        /// Retrieve all <see cref="ConceptionDevisWS.Models.Product"/>s
        /// </summary>
        /// <param name="lang">the culture to retrieve Products into (fr-FR or en-US)</param>
        /// <returns>a list of products</returns>
        [Authorize]
        [Route("api/products")]
        public async Task<List<Product>> GetAllProducts([FromUri]string lang="fr-FR")
        {
            return await ProductService.GetAllProducts(lang);
        }

        /// <summary>
        /// Retrieve the given <see cref="ConceptionDevisWS.Models.Product"/>
        /// </summary>
        /// <param name="id">the product's identity</param>
        /// <param name="lang">the culture to retrieve the product into (fr-FR or en-US)</param>
        /// <returns>the given product</returns>
        /// <exception cref="HttpResponseException">In case something went wrong (for example when the requested product does not exists anymore).</exception>
        [Authorize]
        [Route("api/products/{id}")]
        public async Task<Product> GetProduct(int id, [FromUri]string lang="fr-FR")
        {
            return await ProductService.GetProduct(id, lang);
        }
        
        /// <summary>
        /// Create a new <see cref="ConceptionDevisWS.Models.Product"/>.
        /// </summary>
        /// <param name="newProduct">the product to store</param>
        /// <param name="lang">the culture to create the product into (fr-FR or en-US)</param>
        /// <returns>the created product</returns>
        /// <exception cref="HttpResponseException">In case something went wrong (for example when the newProduct is null).</exception>
        [Authorize]
        [Route("api/products")]
        [AcceptVerbs("POST")]
        public async Task<Product> PostProduct(Product newProduct, [FromUri] string lang="fr-FR")
        {
            return await ProductService.CreateNew(newProduct, lang);
        }

        /// <summary>
        /// Update a given <see cref="ConceptionDevisWS.Models.Product"/> (do not change its model as that'd change the range).
        /// </summary>
        /// <param name="id">the product's identity</param>
        /// <param name="newProduct">the updated product to store</param>
        /// <param name="lang">the culture to update the product into (fr-FR, en-US)</param>
        /// <returns>the updated product</returns>
        /// <exception cref="HttpResponseException">In case something went wrong (for example if the new product is null or the requested product does not exists anymore).</exception>
        /// <remarks>To change the model : delete the product and creates a new one.</remarks>
        [Authorize]
        [Route("api/products/{id}")]
        [AcceptVerbs("PUT")]
        public async Task<Product> PutProduct(int id, Product newProduct, [FromUri]string lang="fr-FR")
        {
            return await ProductService.UpdateProduct(id, newProduct, lang);
        }

        /// <summary>
        /// Remove a given product
        /// </summary>
        /// <param name="id">the product's identity</param>
        /// <returns>the request's Http status code</returns>
        /// <exception cref="HttpResponseException">In case something went wrong (for example when the requested product does not exist anymore).</exception>
        [Authorize]
        [Route("api/products/{id}")]
        [AcceptVerbs("DELETE")]
        public async Task<IHttpActionResult> DeleteProduct(int id)
        {
            IHttpActionResult result = Ok();
            await ProductService.RemoveProduct(id);
            return result;
        }
    }
}
