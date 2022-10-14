using Developist.Core.Cqrs.Commands;
using Developist.Core.Persistence;
using Developist.Extensions.Api.Exceptions;
using Developist.Samples.Domain.Entities;

namespace Developist.Samples.Application.Commands
{
    public class AssignRoleToUserHandler : ICommandHandler<AssignRoleToUser>
    {
        private readonly IUnitOfWork unitOfWork;
        public AssignRoleToUserHandler(IUnitOfWork unitOfWork) => this.unitOfWork = unitOfWork;

        public async Task HandleAsync(AssignRoleToUser command, CancellationToken cancellationToken)
        {
            User user = (await unitOfWork.Repository<User>().FindAsync(
                new UserByUserNameFilter(command.UserName, isCaseSensitive: false),
                cancellationToken)).SingleOrDefault() ?? throw new NotFoundException($"User '{command.UserName}' was not found.");

            var success = user.AssignRole(command.RoleName);
            if (!success)
            {
                throw new ConflictException($"User '{user.UserName}' is already in role '{command.RoleName}'.");
            }

            await unitOfWork.CompleteAsync(cancellationToken);
        }
    }
}
