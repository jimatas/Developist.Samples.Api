using Developist.Core.Cqrs.Events;
using Developist.Core.Persistence.Entities;

namespace Developist.Samples.Domain.Entities.Common
{
    /// <summary>
    /// Convenient base implementation of the <see cref="IAggregateRoot"/> interface to derive your aggregate root entities from.
    /// </summary>
    /// <typeparam name="TIdentifier"></typeparam>
    public abstract class AggregateRootBase<TIdentifier> : EntityBase<TIdentifier>, IAggregateRoot
        where TIdentifier : IEquatable<TIdentifier>
    {
        private readonly List<IEvent> events = new();

        protected AggregateRootBase() { }
        protected AggregateRootBase(TIdentifier id) : base(id) { }

        public IReadOnlyCollection<IEvent> Events => events.AsReadOnly();
        public void AddEvent(IEvent @event) => events.Add(@event);
        public void ClearEvents() => events.Clear();
    }
}
