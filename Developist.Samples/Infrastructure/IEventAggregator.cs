using Developist.Core.Cqrs.Events;
using Developist.Samples.Domain.Entities.Common;

namespace Developist.Samples.Infrastructure
{
    /// <summary>
    /// Defines the interface for an event aggregator.
    /// </summary>
    public interface IEventAggregator
    {
        /// <summary>
        /// A read-only view of the events that have been collected so far.
        /// </summary>
        IReadOnlyCollection<IEvent> Events { get; }

        /// <summary>
        /// Aggregates the events from the specified aggregate root entity and removes them from its events collection.
        /// </summary>
        /// <param name="entity"></param>
        void AggregateEvents(IAggregateRoot entity);

        /// <summary>
        /// Asynchronously dispatches all the events collected so far.
        /// </summary>
        /// <remarks>
        /// Note: A subsequent call to this method should not dispatch the same events again, meaning 
        /// they should be cleared from the internal collection after the dispatch operation completes.
        /// </remarks>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task DispatchEventsAsync(CancellationToken cancellationToken = default);
    }
}
