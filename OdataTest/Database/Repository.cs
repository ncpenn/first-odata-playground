using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using OdataTest.Database;
using OdataTest.Models.ProductService.Models;

namespace OdataTest
{
    public class MyRepository
    {
        private readonly NathanContext dbContext;

        public MyRepository(NathanContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IQueryable<Product> GetAllProducts()
        {
            return dbContext.Products.AsQueryable();
        }

        public void SaveProduct(Product product)
        {
            dbContext.Products.Add(product);
            dbContext.SaveChanges();
        }
    }
}
