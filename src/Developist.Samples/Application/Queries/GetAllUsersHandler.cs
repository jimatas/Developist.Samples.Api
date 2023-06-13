using Developist.Core.Cqrs;
using Developist.Core.Persistence;
using Developist.Samples.Domain.Entities;

namespace Developist.Samples.Application.Queries;

public class GetAllUsersHandler : IQueryHandler<GetAllUsers, IPaginatedList<User>>
{
    private readonly IRepository<User> _users;

    public GetAllUsersHandler(IUnitOfWork uow)
    {
        _users = uow.Repository<User>();
    }

    public Task<IPaginatedList<User>> HandleAsync(GetAllUsers query, CancellationToken cancellationToken)
    {
        return _users.ListAsync(
            new SortingPaginator<User>()
                .StartingAtPage(query.PageNumber)
                .WithPageSize(query.PageSize)
                .SortedByString(query.SortBy),
            cancellationToken);
    }
}
