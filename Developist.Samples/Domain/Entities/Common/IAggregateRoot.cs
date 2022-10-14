using Developist.Core.Cqrs.Events;
using Developist.Core.Persistence.Entities;

namespace Developist.Samples.Domain.Entities.Common
{
    /// <summary>
    /// Represents the root entity of a collection of related entities and possibly value objects.
    /// This is a Domain Driven Design (DDD) concept.
    /// </summary>
    public interface IAggregateRoot : IEntity
    {
        /// <summary>
        /// A read-only view of the domain events that will be raised when this entity is persisted (i.e., created, updated, deleted) in the data store
        /// </summary>
        IReadOnlyCollection<IEvent> Events { get; }

        /// <summary>
        /// Add a new domain event to the collection of events.
        /// </summary>
        /// <param name="event"></param>
        void AddEvent(IEvent @event);

        /// <summary>
        /// Removes all the domain events collected by this entity.
        /// </summary>
        void ClearEvents();
    }
}
