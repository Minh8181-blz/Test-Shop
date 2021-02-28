using Domain.Base.SeedWork;
using Infrastructure.Base.EventLog;
using Infrastructure.Base.RequestManager;
using Microsoft.EntityFrameworkCore;
using Ordering.API.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.API.Infrastructure
{
    public class OrderingContext : DbContext, IIntegrationEventDbContext, IRequestManagerDbContext
    {
        private const string Schema = "ms_ordering";

        public OrderingContext(DbContextOptions<OrderingContext> options)
           : base(options)
        {
        }

        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderItem> OrderItems { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<IntegrationEventLogEntry> EventLogEntries { get; set; }
        public virtual DbSet<RequestEntry> RequestEntries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var orderTableBuider = modelBuilder.Entity<Order>().ToTable("Orders", Schema);

            orderTableBuider
                .Property(p => p.RowVersion)
                .IsConcurrencyToken();

            orderTableBuider
                .Property(p => p.Amount)
                .HasColumnType("decimal(20,5)");

            modelBuilder.Entity<OrderItem>().ToTable("OrderItems", Schema);    
            
            var productTableBuilder = modelBuilder.Entity<Product>().ToTable("Products", Schema);

            productTableBuilder
                .Property(p => p.Price)
                .HasColumnType("decimal(20,5)");

            productTableBuilder
                .Property(p => p.RowVersion)
                .IsConcurrencyToken();

            var eventLogTableBuilder = modelBuilder.Entity<IntegrationEventLogEntry>().ToTable("IntegrationEventLog", Schema);

            eventLogTableBuilder.HasKey(x => x.EventId);

            var requestEntryTableBuilder = modelBuilder.Entity<RequestEntry>().ToTable("RequestEntry", Schema);

            requestEntryTableBuilder.HasKey(x => x.Id);
        }
    }
}
