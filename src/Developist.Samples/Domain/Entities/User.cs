using Developist.Samples.Domain.Entities.Common;
using Developist.Samples.Domain.Events;

namespace Developist.Samples.Domain.Entities;

/// <summary>
/// Represents a user entity.
/// </summary>
public class User : AggregateRootBase<Guid>
{
    private readonly ICollection<string> _roleNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Initializes a new instance of the <see cref="User"/> class.
    /// </summary>
    /// <param name="id">The identifier of the user.</param>
    /// <param name="userName">The username of the user.</param>
    /// <param name="givenName">The given name of the user (optional).</param>
    /// <param name="familyName">The family name of the user (optional).</param>
    public User(
        Guid id,
        string userName,
        string? givenName = default,
        string? familyName = default) : base(id)
    {
        UserName = userName;
        GivenName = givenName;
        FamilyName = familyName;
    }

    /// <summary>
    /// Gets the username of the user.
    /// </summary>
    public string UserName { get; }

    /// <summary>
    /// Gets the given name of the user (optional).
    /// </summary>
    public string? GivenName { get; }

    /// <summary>
    /// Gets the family name of the user (optional).
    /// </summary>
    public string? FamilyName { get; }

    /// <summary>
    /// Gets the roles assigned to the user.
    /// </summary>
    public IReadOnlyCollection<string> Roles => _roleNames.ToList().AsReadOnly();

    /// <summary>
    /// Assigns a new role to this user by adding it to the <see cref="Roles"/> collection, but only if it was not already assigned.
    /// </summary>
    /// <param name="roleName">The name of the role to be assigned.</param>
    /// <returns><c>true</c> if the role was assigned; otherwise, <c>false</c> if it was already assigned.</returns>
    public bool AssignRole(string roleName)
    {
        if (_roleNames.Contains(roleName))
        {
            return false;
        }

        _roleNames.Add(roleName);
        AddEvent(new RoleAssignedToUser(UserName, roleName));

        return true;
    }

    /// <summary>
    /// Returns a string representation of this <see cref="User"/> object.
    /// </summary>
    /// <returns>The user's display name formatted as <c>FamilyName, GivenName (UserName)</c>.</returns>
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
