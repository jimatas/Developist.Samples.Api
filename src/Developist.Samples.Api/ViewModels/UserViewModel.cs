using Developist.Samples.Domain.Entities;

namespace Developist.Samples.Api.ViewModels;

public readonly record struct UserViewModel(string UserName, string? GivenName, string? FamilyName)
{
    public static UserViewModel FromUser(User user)
    {
        if (user is null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        return new(user.UserName, user.GivenName, user.FamilyName);
    }

    public static UserViewModel? FromUserOrNull(User? user)
    {
        return user is null
            ? null
            : new(user.UserName, user.GivenName, user.FamilyName);
    }
}
