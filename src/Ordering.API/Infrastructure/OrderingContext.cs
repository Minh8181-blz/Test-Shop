using Domain.Base.SeedWork;
using Microsoft.EntityFrameworkCore;
using Ordering.API.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.API.Infrastructure
{
    public class OrderingContext : DbContext, IUnitOfWork<Order>
    {
        public OrderingContext(DbContextOptions<OrderingContext> options)
           : base(options)
        {
        }

        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderItem> OrderItems { get; set; }

        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
        {
            await base.SaveChangesAsync();
            return true;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>().ToTable("Orders")
                 .Property(p => p.Amount)
            .HasColumnType("decimal(10,5)");

            modelBuilder.Entity<OrderItem>().ToTable("OrderItems");       
        }
    }
}
