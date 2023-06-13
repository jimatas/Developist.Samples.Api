using Developist.Core.Cqrs;

namespace Developist.Samples.Domain.Entities.Common;

/// <summary>
/// A convenient base implementation of the <see cref="IAggregateRoot"/> interface to derive your aggregate root entities from.
/// </summary>
/// <typeparam name="TIdentifier">The type of identifier used for the aggregate root.</typeparam>
public abstract class AggregateRootBase<TIdentifier> : IAggregateRoot
    where TIdentifier : IEquatable<TIdentifier>
{
    private readonly List<IEvent> _events = new();

    /// <summary>
    /// Initializes a new instance of the <see cref="AggregateRootBase{TIdentifier}"/> class.
    /// </summary>
    protected AggregateRootBase() : this(default!) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="AggregateRootBase{TIdentifier}"/> class with the specified identifier.
    /// </summary>
    /// <param name="id">The identifier for the aggregate root.</param>
    protected AggregateRootBase(TIdentifier id) => Id = id;

    /// <summary>
    /// Gets the identifier for the aggregate root.
    /// </summary>
    public TIdentifier Id { get; }
    
    /// <inheritdoc/>
    public IReadOnlyCollection<IEvent> Events => _events.AsReadOnly();

    /// <inheritdoc/>
    public void AddEvent(IEvent @event) => _events.Add(@event);

    /// <inheritdoc/>
    public void ClearEvents() => _events.Clear();
}
