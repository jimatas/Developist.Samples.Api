using Developist.Core.Cqrs;
using Developist.Core.Persistence;
using Developist.Samples.Domain.Entities;

namespace Developist.Samples.Application.Commands;

public class SeedUserDataHandler : ICommandHandler<SeedUserData>
{
    private readonly IUnitOfWork _uow;

    public SeedUserDataHandler(IUnitOfWork uow) => _uow = uow;

    public Task HandleAsync(SeedUserData _, CancellationToken cancellationToken)
    {
        var users = _uow.Repository<User>();
        
        users.Add(new(Guid.NewGuid(), "WelsD", "Dwayne", "Welsh"));
        users.Add(new(Guid.NewGuid(), "StuaE", "Ed", "Stuart"));
        users.Add(new(Guid.NewGuid(), "MariH", "Hollie", "Marin"));
        users.Add(new(Guid.NewGuid(), "RandB", "Randall", "Bloom"));
        users.Add(new(Guid.NewGuid(), "HensG", "Glenn", "Hensley"));
        users.Add(new(Guid.NewGuid(), "ConPh", "Phillipa", "Connor"));
        users.Add(new(Guid.NewGuid(), "ConPe", "Peter", "Connor"));
        users.Add(new(Guid.NewGuid(), "BryaA", "Ana", "Bryan"));
        users.Add(new(Guid.NewGuid(), "BernE", "Edgar", "Bernard"));
        
        return _uow.CompleteAsync(cancellationToken);
    }
}
