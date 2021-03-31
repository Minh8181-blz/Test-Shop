using API.Identity.Domain;
using Infrastructure.Base.EventLog;
using Infrastructure.Base.RequestManager;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Identity.Infrastructure
{
    public class IdentityDbContext : IdentityDbContext<AppUser, AppRole, int>, IIntegrationEventDbContext, IRequestManagerDbContext
    {
        private const string Schema = "ms_identity";

        public IdentityDbContext(DbContextOptions<IdentityDbContext> options)
           : base(options)
        {
        }

        public virtual DbSet<IntegrationEventLogEntry> EventLogEntries { get; set; }
        public virtual DbSet<RequestEntry> RequestEntries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppUser>().ToTable("AppUser", Schema).HasKey(x => x.Id);
            modelBuilder.Entity<AppRole>().ToTable("AppRole", Schema).HasKey(x => x.Id);

            modelBuilder.Entity<IdentityUserClaim<int>>().ToTable("AppUserClaim", Schema);
            modelBuilder.Entity<IdentityUserRole<int>>().ToTable("AppUserRole", Schema).HasKey(x => new { x.UserId, x.RoleId });
            modelBuilder.Entity<IdentityUserLogin<int>>().ToTable("AppUserLogin", Schema).HasKey(x => x.UserId);
            modelBuilder.Entity<IdentityRoleClaim<int>>().ToTable("AppRoleClaim", Schema);
            modelBuilder.Entity<IdentityUserToken<int>>().ToTable("AppUserToken", Schema).HasKey(x => x.UserId);

            var eventLogTableBuilder = modelBuilder.Entity<IntegrationEventLogEntry>().ToTable("IntegrationEventLog", Schema);

            eventLogTableBuilder.HasKey(x => x.EventId);

            var requestEntryTableBuilder = modelBuilder.Entity<RequestEntry>().ToTable("RequestEntry", Schema);

            requestEntryTableBuilder.HasKey(x => x.Id);
        }
    }
}
