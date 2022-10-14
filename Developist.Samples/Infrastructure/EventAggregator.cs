using Developist.Core.Cqrs.Events;
using Developist.Samples.Domain.Entities.Common;

namespace Developist.Samples.Infrastructure
{
    public class EventAggregator : IEventAggregator
    {
        private readonly IDynamicEventDispatcher eventDispatcher;
        private readonly List<IEvent> events = new();

        public EventAggregator(IDynamicEventDispatcher eventDispatcher)
        {
            this.eventDispatcher = eventDispatcher ?? throw new ArgumentNullException(nameof(eventDispatcher));
        }

        public IReadOnlyCollection<IEvent> Events => events.AsReadOnly();

        public void AggregateEvents(IAggregateRoot entity)
        {
            events.AddRange(entity.Events);
            entity.ClearEvents();
        }

        public Task DispatchEventsAsync(CancellationToken cancellationToken = default)
        {
            var tasks = events.Select(e => eventDispatcher.DispatchAsync(e, cancellationToken));
            return Task.WhenAll(tasks).ContinueWith(_ => events.Clear(), cancellationToken);
        }
    }
}
