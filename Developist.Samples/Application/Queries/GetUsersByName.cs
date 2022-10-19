using Developist.Core.Cqrs.Queries;
using Developist.Samples.Domain.Entities;

using Microsoft.AspNetCore.Mvc;

namespace Developist.Samples.Application.Queries
{
    public record GetUsersByName(string Name) : IQuery<IReadOnlyList<User>>
    {
        [FromQuery]
        public bool IsCaseSensitive { get; set; } = true;

        /// <summary>
        /// Match on whole name or partial (substring).
        /// </summary>
        [FromQuery]
        public bool IsSubstringMatch { get; set; } = false;

        /// <summary>
        /// Indicates whether a NotFoundException should be thrown rather than a default (null or empty) value returned in case of a no match.
        /// </summary>
        [FromQuery(Name = "error404IfNotFound")]
        public bool ThrowNotFoundException { get; set; }
    }
}
