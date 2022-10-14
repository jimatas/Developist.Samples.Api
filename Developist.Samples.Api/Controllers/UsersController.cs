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
    public class UsersController : ControllerBase
    {
        private readonly DispatcherDelegate<GetAllUsers, IReadOnlyList<User>> getAllUsersAsync;
        private readonly DispatcherDelegate<GetUserByUserName, User?> getUserByUniqueNameAsync;
        private readonly DispatcherDelegate<AssignRoleToUser> assignRoleToUserAsync;

        public UsersController(ICompositeDispatcher dispatcher)
        {
            getAllUsersAsync = dispatcher.CreateDelegate<GetAllUsers, IReadOnlyList<User>>();
            getUserByUniqueNameAsync = dispatcher.CreateDelegate<GetUserByUserName, User?>();
            assignRoleToUserAsync = dispatcher.CreateDelegate<AssignRoleToUser>();
        }

        [HttpGet]
        public async Task<IEnumerable<UserModel>> GetAsync([FromQuery] GetAllUsers query, CancellationToken cancellationToken)
        {
            return (await getAllUsersAsync(query, cancellationToken)).Select(user => new UserModel(user));
        }

        [HttpGet("{userName}")]
        public async Task<UserModel?> GetAsync([FromRoute] GetUserByUserName query, CancellationToken cancellationToken)
        {
            var user = await getUserByUniqueNameAsync(query, cancellationToken);
            return user is not null ? new UserModel(user) : null;
        }

        [HttpPost("{userName}/roles/{roleName}")]
        public async Task PostAsync([FromRoute] AssignRoleToUser command, CancellationToken cancellationToken)
        {
            await assignRoleToUserAsync(command, cancellationToken);
        }
    }
}
