using Developist.Core.Persistence;

namespace Developist.Samples.Domain.Entities
{
    public class UserByUserNameFilter : IQueryableFilter<User>
    {
        private readonly string userName;
        private readonly bool isCaseSensitive;

        public UserByUserNameFilter(string userName, bool isCaseSensitive = true)
        {
            this.userName = userName;
            this.isCaseSensitive = isCaseSensitive;
        }

        public IQueryable<User> Filter(IQueryable<User> query)
        {
            return query.Where(user => isCaseSensitive ? user.UserName.Equals(userName) : user.UserName.ToLower().Equals(userName.ToLower()));
        }
    }
}
