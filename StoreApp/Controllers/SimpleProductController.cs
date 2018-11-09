using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using StoreApp.Models;
using System.Threading.Tasks;

namespace StoreApp.Controllers
{
    public class SimpleProductController : ApiController
    {
        List<Product> products = new List<Product>();
        public SimpleProductController()
        {

        }
        public SimpleProductController(List<Product> _products)
        {
            products = _products;
        }
        public IEnumerable<Product> GetProducts()
        {
            return products;
        }
        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            return await Task.FromResult(GetProducts());
        }
        public IHttpActionResult GetProduct(int id)
        {
            Product product = products.Find(x => x.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(product);
            }
        }
        public async Task<IHttpActionResult> GetProductAsync(int id)
        {
            return await Task.FromResult(GetProduct(id));
        }
        public HttpResponseMessage Get(int id)
        {
            Product product = products.FirstOrDefault(x => x.Id == id);
            if (product == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            else {
                return Request.CreateResponse(product);
            }
        }
        public HttpResponseMessage Post(Product product)
        {
            products.Add(product);

            HttpResponseMessage httpResponse = Request.CreateResponse(HttpStatusCode.Created, product);
            httpResponse.Headers.Location = new Uri(
                Url.Link("api2", new {
                    action = "Get",
                    id =product.Id
                }));
            return httpResponse;
        }

    }
}