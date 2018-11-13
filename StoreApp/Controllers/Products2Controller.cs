using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using StoreApp.Repositorys;
using StoreApp.Models;

namespace StoreApp.Controllers
{
    public class Products2Controller : ApiController
    {
        IProductRepository _repository;
        public Products2Controller(IProductRepository repository)
        {
            _repository = repository;
        }
        // GET: api/Products2
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Products2/5
        public IHttpActionResult Get(int id)
        {
            var product = _repository.GetById(id);
            if (product==null)
            {
                return NotFound();
            }
            else {
                return Ok(product);
            }
        }

        // POST: api/Products2
        public IHttpActionResult Post(Product product)
        {
            _repository.Add(product);
            return CreatedAtRoute("DefaultApi", new { id = product.Id }, product);
        }

        // PUT: api/Products2/5
        public IHttpActionResult Put(Product product)
        {
            return Content(HttpStatusCode.Accepted, product);
        }

        // DELETE: api/Products2/5
        public IHttpActionResult Delete(int id)
        {
            _repository.Delete(id);
            return Ok();
        }
    }
}
