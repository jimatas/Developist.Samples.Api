using Developist.Core.Cqrs;
using Developist.Samples.Domain.Events;

namespace Developist.Samples.Application.Events;

/// <summary>
/// Represents a basic event handler implementation for the <see cref="RoleAssignedToUser"/> event.
/// </summary>
public class RoleAssignedToUserHandler : IEventHandler<RoleAssignedToUser>
{
    public Task HandleAsync(RoleAssignedToUser e, CancellationToken cancellationToken)
    {
        Console.WriteLine("The following role was assigned to the following user:");
        Console.WriteLine("Role: {0}", e.RoleName);
        Console.WriteLine("User: {0}", e.UserName);

        return Task.CompletedTask;
    }
}
