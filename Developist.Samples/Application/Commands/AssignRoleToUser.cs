using Developist.Core.Cqrs.Commands;

namespace Developist.Samples.Application.Commands
{
    public record AssignRoleToUser(string UserName, string RoleName) : ICommand;
}
