using Developist.Core.Cqrs;

namespace Developist.Samples.Application.Commands;

/// <summary>
/// Represents a command to assign a role to a user.
/// </summary>
/// <param name="RoleName">The name of the role to be assigned.</param>
/// <param name="UserName">The name of the user to whom the role will be assigned.</param>
public record AssignRoleToUser(string RoleName, string UserName) : ICommand;
