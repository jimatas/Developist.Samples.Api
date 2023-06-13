using Developist.Core.Cqrs;
using Developist.Samples.Domain.Entities.Common;

namespace Developist.Samples.Infrastructure;

/// <summary>
/// Represents the default implementation of the <see cref="IEventAggregator"/> interface.
/// </summary>
public class EventAggregator : IEventAggregator
{
    private readonly IEventDispatcher _eventDispatcher;
    private readonly List<IEvent> _events = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="EventAggregator"/> class with the specified event dispatcher.
    /// </summary>
    /// <param name="eventDispatcher">The event dispatcher to be used for event dispatching.</param>
    public EventAggregator(IEventDispatcher eventDispatcher)
    {
        _eventDispatcher = eventDispatcher ?? throw new ArgumentNullException(nameof(eventDispatcher));
    }

    /// <inheritdoc/>
    public IReadOnlyCollection<IEvent> Events => _events.AsReadOnly();

    /// <inheritdoc/>
    public void AggregateEvents(IAggregateRoot entity)
    {
        _events.AddRange(entity.Events);
        entity.ClearEvents();
    }

    /// <inheritdoc/>
    public Task DispatchEventsAsync(CancellationToken cancellationToken = default)
    {
        var tasks = _events.Select(e => _eventDispatcher.DispatchAsync(e, cancellationToken));
        return Task.WhenAll(tasks).ContinueWith(_ => _events.Clear(), cancellationToken);
    }
}
