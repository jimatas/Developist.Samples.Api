using Developist.Core.Cqrs;

namespace Developist.Samples.Domain.Events;

/// <summary>
/// Event representing the assignment of a role to a user.
/// </summary>
/// <param name="UserName">The name of the user to whom the role was assigned.</param>
/// <param name="RoleName">The name of the role that was assigned.</param>
public readonly record struct RoleAssignedToUser(string UserName, string RoleName) : IEvent;
