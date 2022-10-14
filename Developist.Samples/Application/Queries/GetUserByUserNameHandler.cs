using Developist.Core.Cqrs.Queries;
using Developist.Core.Persistence;
using Developist.Extensions.Api.Exceptions;
using Developist.Samples.Domain.Entities;

namespace Developist.Samples.Application.Queries
{
    public class GetUserByUserNameHandler : IQueryHandler<GetUserByUserName, User?>
    {
        private readonly IReadOnlyRepository<User> users;
        public GetUserByUserNameHandler(IUnitOfWork unitOfWork) => users = unitOfWork.Repository<User>();

        public async Task<User?> HandleAsync(GetUserByUserName query, CancellationToken cancellationToken)
        {
            User? user = (await users.FindAsync(
                new UserByUserNameFilter(query.UserName, query.IsCaseSensitive),
                cancellationToken)).SingleOrDefault();

            if (user is null && query.ThrowNotFoundException)
            {
                throw new NotFoundException($"User '{query.UserName}' was not found.");
            }

            return user;
        }
    }
}
