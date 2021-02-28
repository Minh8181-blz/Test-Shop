using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Catalog.Application.Dto
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}
