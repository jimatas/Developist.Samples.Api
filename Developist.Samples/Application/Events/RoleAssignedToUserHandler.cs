using Developist.Core.Cqrs.Events;
using Developist.Samples.Domain.Events;

namespace Developist.Samples.Application.Events
{
    /// <summary>
    /// A sample event handler for the <see cref="RoleAssignedToUser"/> event.
    /// </summary>
    public class RoleAssignedToUserHandler : IEventHandler<RoleAssignedToUser>
    {
        public Task HandleAsync(RoleAssignedToUser e, CancellationToken cancellationToken)
        {
            Console.WriteLine("The following role was assigned to the following user:");
            Console.WriteLine("Role: {0}", e.Role);
            Console.WriteLine("User: {0}", e.UserName);

            return Task.CompletedTask;
        }
    }
}
