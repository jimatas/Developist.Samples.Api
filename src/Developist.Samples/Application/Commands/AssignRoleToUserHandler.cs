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
        IReadOnlyList<User> filteredUsers = await _uow.Repository<User>().ListAsync(
            new UserByNameFilter(command.UserName, isCaseSensitive: true),
            new SortingPaginator<User>(pageNumber: 1, pageSize: 1),
            cancellationToken);

        var user = filteredUsers.SingleOrDefault(u => u.UserName.Equals(command.UserName, StringComparison.OrdinalIgnoreCase))
            ?? throw new NotFoundException($"User '{command.UserName}' was not found.");

        var success = user.AssignRole(command.RoleName);
        if (!success)
        {
            throw new ConflictException($"User '{user.UserName}' is already in role '{command.RoleName}'.");
        }

        await _uow.CompleteAsync(cancellationToken);
    }
}
