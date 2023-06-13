using Developist.Core.Cqrs;
using Developist.Core.Persistence;
using Developist.Samples.Application.Commands;
using Developist.Samples.Application.Queries;
using Developist.Samples.Domain.Entities;

namespace Developist.Samples.Application.Interceptors;

public class SeedUserDataInterceptor : ICommandInterceptor<AssignRoleToUser>,
    IQueryInterceptor<GetAllUsers, IPaginatedList<User>>,
    IQueryInterceptor<GetUsersByName, IPaginatedList<User>>
{
    private readonly ICommandDispatcher _commandDispatcher;

    public SeedUserDataInterceptor(ICommandDispatcher commandDispatcher)
    {
        _commandDispatcher = commandDispatcher;
    }

    public async Task InterceptAsync(AssignRoleToUser command, CommandHandlerDelegate<AssignRoleToUser> next, CancellationToken cancellationToken)
    {
        await _commandDispatcher.DispatchAsync(new SeedUserData(), cancellationToken);
        await next(command, cancellationToken);
    }

    public async Task<IPaginatedList<User>> InterceptAsync(GetAllUsers query, QueryHandlerDelegate<GetAllUsers, IPaginatedList<User>> next, CancellationToken cancellationToken)
    {
        await _commandDispatcher.DispatchAsync(new SeedUserData(), cancellationToken);
        return await next(query, cancellationToken);
    }

    public async Task<IPaginatedList<User>> InterceptAsync(GetUsersByName query, QueryHandlerDelegate<GetUsersByName, IPaginatedList<User>> next, CancellationToken cancellationToken)
    {
        await _commandDispatcher.DispatchAsync(new SeedUserData(), cancellationToken);
        return await next(query, cancellationToken);
    }
}
