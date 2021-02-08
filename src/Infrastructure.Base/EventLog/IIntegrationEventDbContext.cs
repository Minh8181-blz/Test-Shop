using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Base.EventLog
{
    public interface IIntegrationEventDbContext
    {
        DbSet<IntegrationEventLogEntry> EventLogEntries { get; set; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
