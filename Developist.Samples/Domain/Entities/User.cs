using Developist.Samples.Domain.Entities.Common;
using Developist.Samples.Domain.Events;

namespace Developist.Samples.Domain.Entities
{
    public class User : AggregateRootBase<Guid>
    {
        private readonly ICollection<string> roles = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        public User(Guid id, string userName, string? givenName = null, string? familyName = null) : base(id)
        {
            UserName = userName;
            GivenName = givenName;
            FamilyName = familyName;
        }

        public string UserName { get; }
        public string? GivenName { get; }
        public string? FamilyName { get; }

        public IReadOnlyCollection<string> Roles => roles.ToList().AsReadOnly();

        /// <summary>
        /// Assigns a new role to this user by adding it to the <see cref="Roles"/> collection, but only if it was not already in there.
        /// </summary>
        /// <param name="role"></param>
        /// <returns>true if the role was assigned; false if not.</returns>
        public bool AssignRole(string role)
        {
            if (roles.Contains(role))
            {
                return false;
            }

            roles.Add(role);
            AddEvent(new RoleAssignedToUser(UserName, role));

            return true;
        }

        /// <summary>
        /// Returns a string representation of this <see cref="User"/> object.
        /// </summary>
        /// <returns>The user's display name formatted as <c>"FamilyName, GivenName (UniqueName)"</c>.</returns>
        public override string ToString()
        {
            string? displayName = FamilyName;
            if (!string.IsNullOrEmpty(GivenName) && !string.IsNullOrEmpty(displayName))
            {
                displayName += ", ";
            }
            displayName += GivenName;

            if (!string.IsNullOrEmpty(displayName))
            {
                displayName += $" ({UserName})";
            }
            else
            {
                displayName += UserName;
            }
            return displayName;
        }
    }
}
