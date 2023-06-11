using Developist.Core.Persistence;
using Developist.Core.Persistence.Extensions;
using Developist.Samples.Domain.Entities.Common;
using Microsoft.Extensions.Logging;

namespace Developist.Samples.Infrastructure.Persistence;

/// <summary>
/// Custom implementation of <see cref="IUnitOfWork"/> that publishes domain events of persisted aggregate root entities during the completion process triggered by the <c>CompleteAsync</c> method.
/// </summary>
public partial class CustomUnitOfWork : UnitOfWorkWrapper
{
    private readonly ICollection<IAggregateRoot> _aggregateRoots = new HashSet<IAggregateRoot>();
    private readonly IEventAggregator _eventAggregator;
    private readonly ILogger _logger;

    public CustomUnitOfWork(
        IUnitOfWork unitOfWork,
        IEventAggregator eventAggregator,
        ILogger<CustomUnitOfWork> logger) : base(unitOfWork)
    {
        unitOfWork.Completed += UnitOfWork_Completed;

        _eventAggregator = eventAggregator;
        _logger = logger;
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
    {
        foreach (var aggregateRoot in e.Entities.OfType<IAggregateRoot>())
        {
            _aggregateRoots.Add(aggregateRoot);
        }
    }

    private void Repository_EntityAdded<TEntity>(object? sender, EntityAddedEventArgs<TEntity> e)
    {
        if (e.Entity is IAggregateRoot aggregateRoot)
        {
            _aggregateRoots.Add(aggregateRoot);
        }
    }

    private void Repository_EntityRemoved<TEntity>(object? sender, EntityRemovedEventArgs<TEntity> e)
    {
        if (e.Entity is IAggregateRoot aggregateRoot)
        {
            _aggregateRoots.Add(aggregateRoot);
        }
    }

    private async void UnitOfWork_Completed(object? sender, UnitOfWorkCompletedEventArgs e)
    {
        _logger.LogInformation("Completing unit of work.");

        foreach (var aggregateRoot in _aggregateRoots)
        {
            _eventAggregator.AggregateEvents(aggregateRoot);
        }
        _aggregateRoots.Clear();

        if (_eventAggregator.Events.Any())
        {
            _logger.LogInformation("Dispatching aggregated events.");

            await _eventAggregator.DispatchEventsAsync().ConfigureAwait(false);
        }
        else
        {
            _logger.LogInformation("No events to dispatch.");
        }
    }
}
