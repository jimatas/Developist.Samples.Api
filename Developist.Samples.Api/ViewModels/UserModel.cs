using Developist.Samples.Domain.Entities;

namespace Developist.Samples.Api.ViewModels
{
    public record UserModel(string UserName, string? GivenName, string? FamilyName)
    {
        public UserModel(User source) : this(
            source.UserName,
            source.GivenName,
            source.FamilyName)
        { }
    }
}
