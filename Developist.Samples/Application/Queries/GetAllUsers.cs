using Developist.Core.Cqrs.Queries;
using Developist.Core.Persistence.Pagination;
using Developist.Samples.Domain.Entities;

namespace Developist.Samples.Application.Queries
{
    public record GetAllUsers : IQuery<IReadOnlyList<User>>
    {
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = SortingPaginator<User>.DefaultPageSize;
        public string SortBy { get; init; } = "FamilyName, GivenName";
    }
}
