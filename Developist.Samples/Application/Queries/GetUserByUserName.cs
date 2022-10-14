using Developist.Core.Cqrs.Queries;
using Developist.Samples.Domain.Entities;

using Microsoft.AspNetCore.Mvc;

namespace Developist.Samples.Application.Queries
{
    public record GetUserByUserName(string UserName) : IQuery<User?>
    {
        [FromQuery]
        public bool IsCaseSensitive { get; set; } = true;

        /// <summary>
        /// Indicates whether a NotFoundException should be thrown rather than a default (null) value returned in case of a no match.
        /// </summary>
        [FromQuery(Name = "error404IfNotFound")]
        public bool ThrowNotFoundException { get; set; }
    }
}
