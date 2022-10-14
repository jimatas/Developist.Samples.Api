using Developist.Core.Persistence;
using Developist.Core.Persistence.Entities;
using Developist.Extensions.Persistence;
using Developist.Samples.Domain.Entities.Common;

using Microsoft.Extensions.Logging;

namespace Developist.Samples.Infrastructure.Persistence
{
    /// <summary>
    /// A custom <see cref="IUnitOfWork"/> that publishes the domain events of any aggregate root entities that are persisted by it on the call to CompleteAsync.
    /// </summary>
    public partial class CustomUnitOfWork : UnitOfWorkWrapper
    {
        private readonly ICollection<IAggregateRoot> aggregateRoots = new HashSet<IAggregateRoot>();
        private readonly IEventAggregator eventAggregator;
        private readonly ILogger logger;

        public CustomUnitOfWork(IUnitOfWork unitOfWork, IEventAggregator eventAggregator, ILogger<CustomUnitOfWork> logger) : base(unitOfWork)
        {
            unitOfWork.Completed += UnitOfWork_Completed;
            this.eventAggregator = eventAggregator;
            this.logger = logger;
        }

        public override RepositoryWrapper<TEntity> Repository<TEntity>()
        {
            var repository = base.Repository<TEntity>();
            repository.EntitiesRetrieved += Repository_EntitiesRetrieved;
            repository.EntityAdded += Repository_EntityAdded;
            repository.EntityRemoved += Repository_EntityRemoved;

            return repository;
        }

        private void Repository_EntitiesRetrieved<TEntity>(object? sender, EntitiesRetrievedEventArgs<TEntity> e)
            where TEntity : IEntity
        {
            foreach (var aggregateRoot in e.Entities.OfType<IAggregateRoot>())
            {
                aggregateRoots.Add(aggregateRoot);
            }
        }

        private void Repository_EntityAdded<TEntity>(object? sender, EntityAddedEventArgs<TEntity> e)
            where TEntity : IEntity
        {
            if (e.Entity is IAggregateRoot aggregateRoot)
            {
                aggregateRoots.Add(aggregateRoot);
            }
        }

        private void Repository_EntityRemoved<TEntity>(object? sender, EntityRemovedEventArgs<TEntity> e)
            where TEntity : IEntity
        {
            if (e.Entity is IAggregateRoot aggregateRoot)
            {
                aggregateRoots.Add(aggregateRoot);
            }
        }

        private async void UnitOfWork_Completed(object? sender, UnitOfWorkCompletedEventArgs e)
        {
            logger.LogInformation("Completing unit of work.");
            foreach (var aggregateRoot in aggregateRoots)
            {
                eventAggregator.AggregateEvents(aggregateRoot);
            }
            aggregateRoots.Clear();

            if (eventAggregator.Events.Any())
            {
                logger.LogInformation("Dispatching aggregated events.");
                await eventAggregator.DispatchEventsAsync().ConfigureAwait(false);
            }
            else
            {
                logger.LogInformation("No events to dispatch.");
            }
        }
    }
}
