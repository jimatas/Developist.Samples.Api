using Developist.Core.Cqrs;
using Developist.Extensions.Cqrs;
using Developist.Samples.Api.ViewModels;
using Developist.Samples.Application.Commands;
using Developist.Samples.Application.Queries;
using Developist.Samples.Domain.Entities;

using Microsoft.AspNetCore.Mvc;

using System.Net.Mime;

namespace Developist.Samples.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public class UsersController : ControllerBase
    {
        private readonly DispatcherDelegate<GetAllUsers, IReadOnlyList<User>> getAllUsersAsync;
        private readonly DispatcherDelegate<GetUsersByName, IReadOnlyList<User>> getUsersByNameAsync;
        private readonly DispatcherDelegate<AssignRoleToUser> assignRoleToUserAsync;

        public UsersController(ICompositeDispatcher dispatcher)
        {
            getAllUsersAsync = dispatcher.CreateDelegate<GetAllUsers, IReadOnlyList<User>>();
            getUsersByNameAsync = dispatcher.CreateDelegate<GetUsersByName, IReadOnlyList<User>>();
            assignRoleToUserAsync = dispatcher.CreateDelegate<AssignRoleToUser>();
        }

        [HttpGet(Name = nameof(GetAllUsers))]
        public async Task<IEnumerable<UserModel>> GetAsync([FromQuery] GetAllUsers query, CancellationToken cancellationToken)
        {
            return (await getAllUsersAsync(query, cancellationToken)).Select(UserModel.FromUser);
        }

        [HttpGet("{name}", Name = nameof(GetUsersByName))]
        [ProducesResponseType(StatusCodes.Status204NoContent), ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IEnumerable<UserModel>> GetAsync([FromRoute] GetUsersByName query, CancellationToken cancellationToken)
        {
            return (await getUsersByNameAsync(query, cancellationToken)).Select(UserModel.FromUser);
        }

        [HttpPost("{userName}/roles/{roleName}", Name = nameof(AssignRoleToUser))]
        [ProducesResponseType(StatusCodes.Status404NotFound), ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task PostAsync([FromRoute] AssignRoleToUser command, CancellationToken cancellationToken)
        {
            await assignRoleToUserAsync(command, cancellationToken);
        }
    }
}
