using Developist.Core.Cqrs.Queries;
using Developist.Core.Persistence;
using Developist.Core.Persistence.Pagination;
using Developist.Samples.Domain.Entities;

namespace Developist.Samples.Application.Queries
{
    public class GetAllUsersHandler : IQueryHandler<GetAllUsers, IReadOnlyList<User>>
    {
        private readonly IReadOnlyRepository<User> users;
        public GetAllUsersHandler(IUnitOfWork unitOfWork) => users = unitOfWork.Repository<User>();

        public async Task<IReadOnlyList<User>> HandleAsync(GetAllUsers query, CancellationToken cancellationToken)
        {
            return await users.AllAsync(paginator
                => paginator.StartingAtPage(query.PageNumber)
                    .WithPageSizeOf(query.PageSize)
                    .SortedBy(query.SortBy), cancellationToken);
        }
    }
}
