using Developist.Core.Cqrs;
using Developist.Core.Persistence;
using Developist.Samples.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Developist.Samples.Application.Queries;

/// <summary>
/// Represents a query to retrieve users by name.
/// </summary>
/// <param name="Name">The name to search for, which can match any of the username, family name, or given name.</param>
/// <param name="IsCaseSensitive">Specifies whether the search is case-sensitive (default is <c>true</c>).</param>
/// <param name="IsSubstringMatch">Specifies whether the search is a partial, aka substring, match (default is <c>false</c>).</param>
/// <param name="ThrowNotFoundException">Specifies whether to throw a <see cref="Core.Api.Exceptions.NotFoundException"/> if no users are found (default is <c>false</c>).</param>
public record GetUsersByName(
    [FromQuery] string Name,
    [FromQuery] bool IsCaseSensitive = true,
    [FromQuery] bool IsSubstringMatch = false,
    [FromQuery(Name = "error404IfNotFound")] bool ThrowNotFoundException = false) : IQuery<IPaginatedList<User>>;
