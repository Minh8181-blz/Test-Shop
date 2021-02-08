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
    public class OrderingContext : DbContext, IUnitOfWork<int>, IIntegrationEventDbContext, IRequestManagerDbContext
    {
        private readonly IDomainEventPublisher _domainEventPublisher;

        public OrderingContext(DbContextOptions<OrderingContext> options, IDomainEventPublisher domainEventPublisher)
           : base(options)
        {
            _domainEventPublisher = domainEventPublisher;
        }

        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderItem> OrderItems { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<IntegrationEventLogEntry> EventLogEntries { get; set; }
        public DbSet<RequestEntry> RequestEntries { get; set; }

        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
        {
            var domainEvents = GetDomainEvents();

            await _domainEventPublisher.Publish(domainEvents);

            await base.SaveChangesAsync();

            return true;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var orderTableBuider = modelBuilder.Entity<Order>().ToTable("Orders");

            orderTableBuider
                .Property(p => p.RowVersion)
                .IsConcurrencyToken();

            orderTableBuider
                .Property(p => p.Amount)
                .HasColumnType("decimal(20,5)");

            modelBuilder.Entity<OrderItem>().ToTable("OrderItems");    
            
            var productTableBuilder = modelBuilder.Entity<Product>().ToTable("Products");

            productTableBuilder
                .Property(p => p.Price)
                .HasColumnType("decimal(20,5)");

            productTableBuilder
                .Property(p => p.RowVersion)
                .IsConcurrencyToken();

            var eventLogTableBuilder = modelBuilder.Entity<IntegrationEventLogEntry>().ToTable("IntegrationEventLog");

            eventLogTableBuilder.HasKey(x => x.EventId);

            var requestEntryTableBuilder = modelBuilder.Entity<RequestEntry>().ToTable("RequestEntry");

            requestEntryTableBuilder.HasKey(x => x.Id);
        }

        private IEnumerable<DomainEvent> GetDomainEvents()
        {
            var domainEntities = ChangeTracker
                .Entries<Entity<int>>()
                .Where(x => {
                    var events = x.Entity.GetDomainEvents();
                    return events != null && events.Any();
                });

            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.GetDomainEvents())
                .ToList();

            domainEntities.ToList()
                .ForEach(entity => entity.Entity.ClearDomainEvents());

            return domainEvents;
        }
    }
}
