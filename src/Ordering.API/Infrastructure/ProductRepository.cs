using Domain.Base.SeedWork;
using Microsoft.EntityFrameworkCore;
using Ordering.API.Domain.Entities;
using Ordering.API.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ordering.API.Infrastructure
{
    public class ProductRepository : IProductRepository
    {
        private readonly OrderingContext _context;

        public IUnitOfWork<int> UnitOfWork
        {
            get
            {
                return _context;
            }
        }

        public ProductRepository(OrderingContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Product Add(Product product)
        {
            return _context.Products.Add(product).Entity;
        }

        public async Task<Product> GetAsync(int id)
        {
            var product = await _context
                .Products
                .FirstOrDefaultAsync(o => o.Id == id);

            return product;
        }

        public void Update(Product product)
        {
            _context.Entry(product).State = EntityState.Modified;
        }

        public async Task<IEnumerable<Product>> GetByIdsAsync(IEnumerable<int> ids)
        {
            var products = await _context.Products
                .Where(x => ids.Contains(x.Id))
                .ToListAsync();

            return products;
        }
    }
}
