using Developist.Core.Cqrs;
using Developist.Samples.Api.ViewModels;
using Developist.Samples.Application.Commands;
using Developist.Samples.Application.Queries;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace Developist.Samples.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[ProducesResponseType(StatusCodes.Status200OK), ProducesResponseType(StatusCodes.Status500InternalServerError)]
public class UsersController : ControllerBase
{
    private readonly IQueryDispatcher _queryDispatcher;
    private readonly ICommandDispatcher _commandDispatcher;

    public UsersController(IQueryDispatcher queryDispatcher, ICommandDispatcher commandDispatcher)
    {
        _queryDispatcher = queryDispatcher;
        _commandDispatcher = commandDispatcher;
    }

    [HttpGet(Name = nameof(GetAllUsers))]
    public async Task<IEnumerable<UserViewModel>> GetAsync([FromQuery] GetAllUsers query, CancellationToken cancellationToken)
    {
        return (await _queryDispatcher.DispatchAsync(query, cancellationToken)).Select(UserViewModel.FromUser);
    }

    [HttpGet("{name}", Name = nameof(GetUsersByName))]
    [ProducesResponseType(StatusCodes.Status204NoContent), ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IEnumerable<UserViewModel>> GetAsync([FromRoute] GetUsersByName query, CancellationToken cancellationToken)
    {
        return (await _queryDispatcher.DispatchAsync(query, cancellationToken)).Select(UserViewModel.FromUser);
    }

    [HttpPost("{userName}/roles/{roleName}", Name = nameof(AssignRoleToUser))]
    [ProducesResponseType(StatusCodes.Status404NotFound), ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task PostAsync([FromRoute] AssignRoleToUser command, CancellationToken cancellationToken)
    {
        await _commandDispatcher.DispatchAsync(command, cancellationToken);
    }
}
