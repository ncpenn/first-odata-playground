using System;
using System.Collections.Generic;

namespace OdataTest.Models
{
    namespace ProductService.Models
    {
        public class Product
        {
            public Product()
            {
                Accessories = new HashSet<Accessory>();
            }

            public int Id { get; set; }
            public string Name { get; set; }
            public decimal Price { get; set; }
            public string Category { get; set; }

            public ICollection<Accessory> Accessories { get; set; }
        }
    }
}
