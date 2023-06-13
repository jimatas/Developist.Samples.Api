using Developist.Core.Api.Exceptions;
using Developist.Core.Cqrs;
using Developist.Core.Persistence;
using Developist.Samples.Domain.Entities;

namespace Developist.Samples.Application.Queries;

public class GetUsersByNameHandler : IQueryHandler<GetUsersByName, IPaginatedList<User>>
{
    private readonly IRepository<User> _users;

    public GetUsersByNameHandler(IUnitOfWork uow)
    {
        _users = uow.Repository<User>();
    }

    public async Task<IPaginatedList<User>> HandleAsync(GetUsersByName query, CancellationToken cancellationToken)
    {
        var users = await _users.ListAsync(
            new UserByNameFilter(
                query.Name,
                query.IsCaseSensitive,
                query.IsSubstringMatch),
            new SortingPaginator<User>(),
            cancellationToken);

        if (!users.Any() && query.ThrowNotFoundException)
        {
            throw new NotFoundException($"No users with the name '{query.Name}' were found.");
        }

        return users;
    }
}
