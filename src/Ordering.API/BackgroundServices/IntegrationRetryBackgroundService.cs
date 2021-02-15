using Application.Base.SeedWork;
using Infrastructure.Base.EventBus;
using Infrastructure.Base.EventLog;
using Infrastructure.Base.MessageQueue;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ordering.API.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.API.BackgroundServices
{
    public class IntegrationRetryBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _services;
        private readonly IQueueProcessor _queueProcessor;
        private readonly IIntegrationEventTopicMapping _topicMapping;
        private const int RetryTimes = 5;
        private OrderingContext _context;

        public IntegrationRetryBackgroundService(
            IServiceProvider services,
            IQueueProcessor queueProcessor,
            IIntegrationEventTopicMapping topicMapping)
        {
            _services = services;
            _queueProcessor = queueProcessor;
            _topicMapping = topicMapping;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(300000);
                using var scope = _services.CreateScope();
                _context = scope.ServiceProvider.GetRequiredService<OrderingContext>();
                await RepublishEvents();
            }

            await Task.CompletedTask;
        }

        protected async Task RepublishEvents()
        {
            var entries = await GetNotSucceededEventEntries();

            if (entries == null || entries.Count() == 0)
                return;

            foreach(var entry in entries)
            {
                await Publish(entry);
            }
        }

        protected async Task<IEnumerable<IntegrationEventLogEntry>> GetNotSucceededEventEntries()
        {
            return await _context.EventLogEntries
                .Where(x => x.State != EventStateEnum.Published.ToString())
                .OrderBy(x => x.CreatedAt)
                .ToListAsync();
        }

        protected async Task<bool> Publish(IntegrationEventLogEntry entry)
        {
            int count = 0;
            bool publishSucceeded = false;

            var eventTypeName = GetRelativeEventTypeName(entry.EventTypeName);

            var topicModel = _topicMapping.GetPublishedTopic(eventTypeName);

            if (topicModel == null)
                return false;

            entry.UpdateState(EventStateEnum.InProgress);
            await _context.SaveChangesAsync();

            while (!publishSucceeded && count <= RetryTimes)
            {
                try
                {
                    publishSucceeded = _queueProcessor.PublishPlainMessageToExchange(topicModel.Exchange, topicModel.RoutingKey, entry.Content);
                }
                catch (Exception ex)
                {
                    publishSucceeded = false;
                }
            }

            if (publishSucceeded)
            {
                entry.UpdateState(EventStateEnum.Published);
                await _context.SaveChangesAsync();
            }
            else
            {
                entry.UpdateState(EventStateEnum.PublishedFailed);
                await _context.SaveChangesAsync();
            }

            return publishSucceeded;
        }

        private string GetRelativeEventTypeName(string absoluteTypeName)
        {
            var parts = absoluteTypeName.Split('.');

            if (parts.Length == 0)
                return null;

            return parts[^1];
        }
    }
}
