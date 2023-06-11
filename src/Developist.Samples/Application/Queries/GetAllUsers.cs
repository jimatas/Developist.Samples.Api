using Developist.Core.Cqrs;
using Developist.Core.Persistence;
using Developist.Samples.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Developist.Samples.Application.Queries;

/// <summary>
/// Represents a query to retrieve all users.
/// </summary>
/// <param name="PageNumber">The page number of the results (default is 1).</param>
/// <param name="PageSize">The number of users per page (default is 20).</param>
/// <param name="SortBy">The sorting criteria for the users (default is "FamilyName, GivenName").</param>
public record GetAllUsers(
    [FromQuery] int PageNumber = 1,
    [FromQuery] int PageSize = SortingPaginator<User>.DefaultPageSize,
    [FromQuery] string SortBy = "FamilyName, GivenName") : IQuery<IPaginatedList<User>>;
