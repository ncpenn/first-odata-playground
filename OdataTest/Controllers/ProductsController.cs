using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using OdataTest.Database;
using OdataTest.Models.ProductService.Models;
using OdataTest.ODataSecurity;

namespace OdataTest.Controllers
{
    public class ProductsController : ODataController
    {
        private readonly MyRepository repository;
        private readonly NathanContext context;

        public ProductsController(MyRepository repository, NathanContext context)
        {
            this.repository = repository;
            this.context = context;
        }

        [EnableQuery]
        [Swashbuckle.AspNetCore.Annotations.SwaggerOperation(OperationId = "get")]
        public IActionResult Get()
        {
            return Ok(repository.GetAllProducts());
        }

        [HttpPost]
        [Swashbuckle.AspNetCore.Annotations.SwaggerOperation(OperationId = "post")]
        public IActionResult Post([FromBody]Product product)
        {
            if (product != null)
            {
                repository.SaveProduct(product);
            }

            return Ok();
        }

        [HttpPatch]
        [Swashbuckle.AspNetCore.Annotations.SwaggerOperation(OperationId = "patch")]
        public IActionResult Patch([FromODataUri] int key, Delta<Product> product)
        {
            var p = context.Products.Find(key);

            product.Patch(p);

            context.SaveChanges();

            return Ok();
        }

        [HttpPut]
        [Swashbuckle.AspNetCore.Annotations.SwaggerOperation(OperationId = "patch")]
        public IActionResult Put([FromODataUri] int key, Delta<Product> product)
        {
            var p = context.Products.Find(key);

            product.Patch(p);

            context.SaveChanges();

            return Ok();
        }

        [HttpDelete]
        public IActionResult Delete([FromODataUri] int key)
        {
            var product = context.Products.Find(key);
            if (product == null)
            {
                return NotFound();
            }

            context.Products.Remove(product);
            context.SaveChanges();

            return Ok();
        }
    }
}
