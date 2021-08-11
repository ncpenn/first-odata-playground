using System;
using OdataTest.Models.ProductService.Models;

namespace OdataTest.Models
{
    public class Accessory
    {
        public int AccessoryId { get; set; }

        public string Name { get; set; }

        public Product Product { get; set; }
    }
}
