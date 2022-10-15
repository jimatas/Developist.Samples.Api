using Developist.Core.Cqrs.Queries;
using Developist.Core.Persistence.Pagination;
using Developist.Samples.Domain.Entities;

namespace Developist.Samples.Application.Queries
{
    public record GetAllUsers : IQuery<IReadOnlyList<User>>
    {
        /// <summary>
        /// The 1-based page number of the desired page of results.
        /// </summary>
        public int PageNumber { get; init; } = 1;

        /// <summary>
        /// The desired number of results per page. Defaults to 20.
        /// </summary>
        public int PageSize { get; init; } = SortingPaginator<User>.DefaultPageSize;

        /// <summary>
        /// The user attributes to sort the result set by. 
        /// Defaults to sorting in ascending order by <see cref="User.FamilyName"/> and then <see cref="User.GivenName"/>.
        /// </summary>
        public string SortBy { get; init; } = "FamilyName, GivenName";
    }
}
