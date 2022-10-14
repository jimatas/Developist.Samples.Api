using Developist.Core.Cqrs.Events;

namespace Developist.Samples.Domain.Events
{
    public record RoleAssignedToUser(string UserName, string Role) : IEvent;
}
