using Domain.Base.SeedWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Base.Database
{
    public class UnitOfWork<T> : IUnitOfWork where T : DbContext
    {
        private readonly DbContext _context;
        private IDbContextTransaction _transaction;
        private readonly IDomainEventPublisher _domainEventPublisher;

        public UnitOfWork(T context, IDomainEventPublisher domainEventPublisher)
        {
            _context = context;
            _domainEventPublisher = domainEventPublisher;
        }

        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            await _transaction.CommitAsync();

            await _transaction.DisposeAsync();

            _transaction = null;
        }

        public async Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            await _transaction.RollbackAsync();

            await _transaction.DisposeAsync();

            _transaction = null;
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
        {
            var domainEvents = GetDomainEvents();

            await _domainEventPublisher.Publish(domainEvents);

            await _context.SaveChangesAsync();

            return true;
        }

        private IEnumerable<DomainEvent> GetDomainEvents()
        {
            var domainEntities = _context.ChangeTracker
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
