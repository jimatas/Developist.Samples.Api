using Developist.Core.Persistence;

using System.Linq.Expressions;

namespace Developist.Samples.Domain.Entities
{
    public class UserByNameFilter : IQueryableFilter<User>
    {
        private readonly string name;
        private readonly bool isCaseSensitive;
        private readonly bool isSubstringMatch;

        public UserByNameFilter(string name, bool isCaseSensitive = true, bool isSubstringMatch = false)
        {
            this.name = name;
            this.isCaseSensitive = isCaseSensitive;
            this.isSubstringMatch = isSubstringMatch;
        }

        public IQueryable<User> Filter(IQueryable<User> query)
        {
            Expression<Func<User, bool>> predicate;
            if (isCaseSensitive)
            {
                predicate = user => isSubstringMatch ? user.UserName.Contains(name) : user.UserName.Equals(name);
                predicate = predicate.OrElse(user => isSubstringMatch ? user.GivenName!.Contains(name) : user.GivenName!.Equals(name));
                predicate = predicate.OrElse(user => isSubstringMatch ? user.FamilyName!.Contains(name) : user.FamilyName!.Equals(name));
            }
            else
            {
                predicate = user => isSubstringMatch ? user.UserName.ToLower().Contains(name.ToLower()) : user.UserName.ToLower().Equals(name.ToLower());
                predicate = predicate.OrElse(user => isSubstringMatch ? user.GivenName!.ToLower().Contains(name.ToLower()) : user.GivenName!.ToLower().Equals(name.ToLower()));
                predicate = predicate.OrElse(user => isSubstringMatch ? user.FamilyName!.ToLower().Contains(name.ToLower()) : user.FamilyName!.ToLower().Equals(name.ToLower()));
            }
            return query.Where(predicate);
        }
    }
}
