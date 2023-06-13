using Developist.Core.Cqrs;

namespace Developist.Samples.Domain.Entities.Common;

/// <summary>
/// Represents the root entity of a collection of related entities and possibly value objects.
/// This concept is derived from Domain-Driven Design (DDD).
/// </summary>
public interface IAggregateRoot
{
    /// <summary>
    /// Gets a read-only collection of domain events that will be raised when this entity is persisted (created, updated, or deleted) in the data store.
    /// </summary>
    IReadOnlyCollection<IEvent> Events { get; }

    /// <summary>
    /// Adds a new domain event to the collection of events associated with this entity.
    /// </summary>
    /// <param name="event">The domain event to be added.</param>
    void AddEvent(IEvent @event);

    /// <summary>
    /// Clears all the domain events collected by this entity.
    /// </summary>
    void ClearEvents();
}
