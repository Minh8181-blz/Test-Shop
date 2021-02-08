using System.Reflection;
using Application.Base.SeedWork;
using Domain.Base.SeedWork;
using Infrastructure.Base;
using Infrastructure.Base.EventBus;
using Infrastructure.Base.EventLog;
using Infrastructure.Base.MessageQueue;
using Infrastructure.Base.RequestManager;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ordering.API.Application;
using Ordering.API.Domain.Interfaces;
using Ordering.API.Domain.Services;
using Ordering.API.Filters;
using Ordering.API.Infrastructure;

namespace Ordering.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(HttpGlobalExceptionFilter));
            });

            services.AddDbContext<OrderingContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("MaktaShop"));
            });

            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IIntegrationEventLogService, IntegrationEventLogService<OrderingContext>>();
            services.AddScoped<IRequestManager, RequestManager<OrderingContext>>();

            services.AddTransient<IDomainEventPublisher, DomainEventPublisher>();

            services.AddSingleton<IQueueConnectionFactory, QueueConnectionFactory>(
                services => new QueueConnectionFactory(Configuration.GetConnectionString("QueueConnection")));

            services.AddSingleton<IQueueProcessor, QueueProcessor>();

            services.AddSingleton<IIntegrationEventTopicMapping, IntegrationEventTopicMapping>();

            services.AddTransient<IOrderDomainService, OrderDomainService>();

            services.AddSingleton<IIntegrationEventBus, IntegrationEventBus>(services => {
                IQueueProcessor queueProcessor = services.GetRequiredService<IQueueProcessor>();
                IIntegrationEventTopicMapping integrationEventTopicMapping = services.GetRequiredService<IIntegrationEventTopicMapping>();
                return new IntegrationEventBus(queueProcessor, services, integrationEventTopicMapping);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            EventSubscriber.RegisterIntegrationEventSubscription(app);
        }
    }
}
