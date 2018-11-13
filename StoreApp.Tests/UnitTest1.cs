using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StoreApp;
using StoreApp.Controllers;
using StoreApp.Models;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Web.Http;
using StoreApp.Repositorys;


namespace StoreApp.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var products = GetLocalProducts();
            var controller=new StoreApp.Controllers.SimpleProductController(products);
            List<Product> result = controller.GetProducts().ToList();
            Assert.AreEqual(products.Count, result.Count);
        }
        private List<Product> GetLocalProducts()
        {
            List<Product> products = new List<Product> {
                new Product{ Id=1,Name="jiling", Price=2 },
                new Product{ Id=2,Name="",Price=1.2M}
            };
            return products;
        }
        [TestMethod]
        public async Task TestGetProductsAsync()
        {
            var products = GetLocalProducts();
            var controller = new SimpleProductController(products);
            var result =await controller.GetProductsAsync() as List<Product>;
            Assert.AreEqual(products.Count, result.Count);
        }
        [TestMethod]
        public void TestGetProduct()
        {
            var products = GetLocalProducts();
            var controller = new SimpleProductController(products);
            var result = controller.GetProduct(2) as System.Web.Http.Results.OkNegotiatedContentResult<Product>;
            Assert.IsNotNull(result);
            Assert.AreEqual(products[1].Name,result.Content.Name);
        }
        [TestMethod]
        public async Task TestGetProductAsync()
        {
            var products = GetLocalProducts();
            var controller = new SimpleProductController(products);
            var result = await controller.GetProductAsync(2) as System.Web.Http.Results.OkNegotiatedContentResult<Product>;
            Assert.IsNotNull(result);
            Assert.AreEqual(products[1].Name, result.Content.Name);
        }
        [TestMethod]
        public void TestNotFoundProduct()
        {
            var products = GetLocalProducts();
            var controller = new SimpleProductController(products);
            var result= controller.GetProduct(88);
            Assert.IsInstanceOfType(result, typeof(System.Web.Http.Results.NotFoundResult));
        }
        [TestMethod]
        public void TestGet()
        {
            List<Product> products = GetLocalProducts();
            SimpleProductController controller = new SimpleProductController(products);
            controller.Request = new System.Net.Http.HttpRequestMessage();
            controller.Configuration = new System.Web.Http.HttpConfiguration();
            System.Net.Http.HttpResponseMessage httpResponse = controller.Get(1);
            Product product;
            Assert.IsTrue(httpResponse.TryGetContentValue<Product>(out product));
            Assert.AreEqual(1, product.Id);
        }
        [TestMethod]
        public void TestPost()
        {
            List<Product> products = GetLocalProducts();
            SimpleProductController controller = new SimpleProductController(products);
            controller.Request = new HttpRequestMessage(HttpMethod.Post, "http://localhost/api/simpleProduct");
            controller.Configuration = new System.Web.Http.HttpConfiguration();
            controller.Configuration.Routes.MapHttpRoute(
                name: "api2",
                routeTemplate:"api/{controller}/{action}/{id}",
                defaults:new { id=RouteParameter.Optional}
                );
            controller.RequestContext.RouteData =
                new System.Web.Http.Routing.HttpRouteData(
                route: new System.Web.Http.Routing.HttpRoute(),
                values:new System.Web.Http.Routing.HttpRouteValueDictionary { { "controller", "simpleProduct" },{ "action", "post"} }
                 );
            Product product = new Product {  Id=3, Name="hu" };
            HttpResponseMessage httpResponse= controller.Post(product);
            Assert.AreEqual("http://localhost/api/simpleProduct/Get/3", httpResponse.Headers.Location.AbsoluteUri, true);
        }
        [TestMethod]
        public void PostSetsLocationHeader_MockVersion()
        {
            List<Product> products = GetLocalProducts();
            SimpleProductController controller = new SimpleProductController(products);
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            string locationUrl = "http://l/";
            var mockUrlHelper = new Moq.Mock<System.Web.Http.Routing.UrlHelper>();
            mockUrlHelper.Setup(x => x.Link(Moq.It.IsAny<string>(), Moq.It.IsAny<object>())).Returns(locationUrl);
            controller.Url = mockUrlHelper.Object;

            Product product = new Product { Id=4,Name="hahah" };
            var response = controller.Post(product);
            Assert.AreEqual(locationUrl, response.Headers.Location.AbsoluteUri);
        }
        [TestMethod]
        public void GetReturnsProductWithSameId()
        {
            var mockRepository = new Moq.Mock<IProductRepository>();
            mockRepository.Setup(x => x.GetById(42)).Returns(new Product { Id = 42, Name = "huolun" });
            var controller = new Products2Controller(mockRepository.Object);
            var response=controller.Get(42);
            var result = response as System.Web.Http.Results.OkNegotiatedContentResult<Product>;
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
            Assert.AreEqual(42, result.Content.Id);
        }
        [TestMethod]
        public void GetReturnsNotFound()
        {
            var mockRepository = new Moq.Mock<IProductRepository>();
            var controller = new Products2Controller(mockRepository.Object);

            var response = controller.Get(10);
            Assert.IsInstanceOfType(response, typeof(System.Web.Http.Results.NotFoundResult));
        }
        [TestMethod]
        public void DeleteReturnsOk()
        {
            var mockRepository = new Moq.Mock<IProductRepository>();
            var controller = new Products2Controller(mockRepository.Object);

            var response = controller.Delete(10);
            Assert.IsInstanceOfType(response, typeof(System.Web.Http.Results.OkResult));
        }
        [TestMethod]
        public void PostRetursProduct()
        {
            Moq.Mock<IProductRepository> mockRepository = new Moq.Mock<IProductRepository>();
            Products2Controller controller = new Products2Controller(mockRepository.Object);
            Product product = new Product { Id = 1, Name = "keyi" };
            var result= controller.Post(product) as System.Web.Http.Results.CreatedAtRouteNegotiatedContentResult<Product>;
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Content);
            Assert.AreEqual(product.Id, result.Content.Id);
        }
        [TestMethod]
        public void PutReturnsProduct()
        {
            Moq.Mock<IProductRepository> mockRepository = new Moq.Mock<IProductRepository>();
            Products2Controller controller = new Products2Controller(mockRepository.Object);
            Product product = new Product {Id=1,Name="haha" };
            System.Web.Http.Results.NegotiatedContentResult<Product> result = controller.Put(product) as System.Web.Http.Results.NegotiatedContentResult<Product>;
            Assert.IsNotNull(result);
            Assert.AreEqual(result.StatusCode, System.Net.HttpStatusCode.Accepted);
            Assert.IsNotNull(result.Content);
            Assert.AreEqual(product.Id, result.Content.Id);
        }
    }
}
