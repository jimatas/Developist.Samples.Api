using Developist.Core.Cqrs.Queries;
using Developist.Core.Persistence;
using Developist.Extensions.Api.Exceptions;
using Developist.Samples.Domain.Entities;

namespace Developist.Samples.Application.Queries
{
    public class GetUsersByNameHandler : IQueryHandler<GetUsersByName, IReadOnlyList<User>>
    {
        private readonly IReadOnlyRepository<User> userRepository;
        public GetUsersByNameHandler(IUnitOfWork unitOfWork) => userRepository = unitOfWork.Repository<User>();

        public async Task<IReadOnlyList<User>> HandleAsync(GetUsersByName query, CancellationToken cancellationToken)
        {
            IReadOnlyList<User> users = await userRepository.FindAsync(
                new UserByNameFilter(
                    query.Name,
                    query.IsCaseSensitive,
                    query.IsSubstringMatch),
                cancellationToken);

            if (!users.Any() && query.ThrowNotFoundException)
            {
                throw new NotFoundException($"No users with the name '{query.Name}' were found.");
            }

            return users;
        }
    }
}
