using Developist.Core.Api.Exceptions;
using Developist.Core.Cqrs;
using Developist.Core.Persistence;
using Developist.Samples.Domain.Entities;

namespace Developist.Samples.Application.Commands;

/// <summary>
/// Represents a command handler for the <see cref="AssignRoleToUser"/> command.
/// </summary>
public class AssignRoleToUserHandler : ICommandHandler<AssignRoleToUser>
{
    private readonly IUnitOfWork _uow;

    public AssignRoleToUserHandler(IUnitOfWork uow) => _uow = uow;

    public async Task HandleAsync(AssignRoleToUser command, CancellationToken cancellationToken)
    {
        var user = await _uow.Repository<User>()
            .SingleOrDefaultAsync(
                predicate: user => user.UserName.Equals(command.UserName, StringComparison.OrdinalIgnoreCase),
                cancellationToken)
            ?? throw new NotFoundException($"User '{command.UserName}' was not found.");

        var success = user.AssignRole(command.RoleName);
        if (!success)
        {
            throw new ConflictException($"User '{user.UserName}' is already in role '{command.RoleName}'.");
        }

        await _uow.CompleteAsync(cancellationToken);
    }
}
