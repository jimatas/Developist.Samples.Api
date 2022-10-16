using Developist.Samples.Domain.Entities;

namespace Developist.Samples.Api.ViewModels
{
    public record UserModel(string UserName, string? GivenName, string? FamilyName)
    {
        public static UserModel FromUser(User user)
        {
            if (user is null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return new(user.UserName, user.GivenName, user.FamilyName);
        }

        public static UserModel? FromUserOrNull(User? user) => user is null
            ? null
            : new(user.UserName, user.GivenName, user.FamilyName);
    }
}
