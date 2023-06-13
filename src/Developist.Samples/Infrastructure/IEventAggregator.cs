using Developist.Core.Cqrs;
using Developist.Samples.Domain.Entities.Common;

namespace Developist.Samples.Infrastructure;

/// <summary>
/// Defines the interface for an event aggregator.
/// </summary>
public interface IEventAggregator
{
    /// <summary>
    /// Gets a read-only collection of events that have been collected so far.
    /// </summary>
    IReadOnlyCollection<IEvent> Events { get; }

    /// <summary>
    /// Aggregates the events from the specified aggregate root entity and removes them from its events collection.
    /// </summary>
    /// <param name="entity">The aggregate root entity whose events should be aggregated.</param>
    void AggregateEvents(IAggregateRoot entity);

    /// <summary>
    /// Asynchronously dispatches all the events collected so far.
    /// </summary>
    /// <remarks>
    /// Note: After the dispatch operation completes, the events should be cleared from the internal collection,
    /// ensuring that a subsequent call to this method does not dispatch the same events again.
    /// </remarks>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task DispatchEventsAsync(CancellationToken cancellationToken = default);
}
