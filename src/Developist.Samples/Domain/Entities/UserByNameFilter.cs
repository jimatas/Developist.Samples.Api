using Developist.Core.Persistence;

namespace Developist.Samples.Domain.Entities;

public class UserByNameFilter : IFilterCriteria<User>
{
    private readonly string _name;
    private readonly bool _isCaseSensitive;
    private readonly bool _isSubstringMatch;

    public UserByNameFilter(string name, bool isCaseSensitive = true, bool isSubstringMatch = false)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(
                message: "Value cannot be null, empty, or contain only whitespace characters.",
                paramName: nameof(name));
        }

        _name = name;
        _isCaseSensitive = isCaseSensitive;
        _isSubstringMatch = isSubstringMatch;
    }

    public IQueryable<User> Filter(IQueryable<User> query)
    {
        return _isCaseSensitive
            ? query.Where(user =>
                _isSubstringMatch
                    ? user.UserName.Contains(_name) || user.FamilyName!.Contains(_name) || user.GivenName!.Contains(_name)
                    : user.UserName.Equals(_name) || user.FamilyName!.Equals(_name) || user.GivenName!.Equals(_name))
            : query.Where(user =>
                _isSubstringMatch
                    ? user.UserName.ToLower().Contains(_name.ToLower()) || user.FamilyName!.ToLower().Contains(_name.ToLower()) || user.GivenName!.ToLower().Contains(_name.ToLower())
                    : user.UserName.ToLower().Equals(_name.ToLower()) || user.FamilyName!.ToLower().Equals(_name.ToLower()) || user.GivenName!.ToLower().Equals(_name.ToLower()));
    }
}
