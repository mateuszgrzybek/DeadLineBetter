using DeadLineBetter.Domain.Enums;
using System.Data;

namespace DeadLineBetter.Domain.Entities.Groups
{
  public sealed class Group
  {
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public GroupJoinCode JoinCode { get; private set; } = null!;
    public IReadOnlyCollection<GroupMembership> Memberships => _memberships.AsReadOnly();
    private readonly List<GroupMembership> _memberships = [];

    private Group() { }

    private Group(Guid id, string name)
    {
      if (id == Guid.Empty)
      {
        throw new ArgumentException("Group ID cannot be empty", nameof(id));
      }

      Id = id;
      Name = ValidateName(name);
    }

    public static Group Create(string name, Guid creatorUserId, string codeHash, string encryptedCode)
    {
      if (creatorUserId  == Guid.Empty)
      {
        throw new ArgumentException("Group creator's User ID cannot be empty", nameof(creatorUserId));
      }

      var group = new Group(Guid.NewGuid(), name);

      group._memberships.Add(
        new GroupMembership(group.Id, creatorUserId, GroupRole.Organizer)
      );

      group.JoinCode = new GroupJoinCode(group.Id, codeHash, encryptedCode);

      return group;
    }

    public void RegenerateJoinCode(string codeHash, string encryptedCode)
    {
      JoinCode.UpdateJoinCode(codeHash, encryptedCode);
    }

    public void Rename(string newName)
    {
      Name = ValidateName(newName);
    }

    public void AddMember(Guid newMemberId)
    {
      if (newMemberId == Guid.Empty)
      {
        throw new ArgumentException("New member's User ID cannot be empty", nameof(newMemberId));
      }

      if (_memberships.Any(membership => membership.UserId == newMemberId))
      {
        throw new InvalidOperationException("User with the provided User ID is already a member of the group");
      }

      _memberships.Add(
        new GroupMembership(Id, newMemberId, GroupRole.Member)
      );
    }

    public void RemoveMember(Guid memberToRemoveId)
    {
      if (memberToRemoveId == Guid.Empty)
      {
        throw new ArgumentException("Removed member's User ID cannot be empty", nameof(memberToRemoveId));
      }

      var membership = FindMembership(memberToRemoveId);

      if (membership.Role == GroupRole.Organizer)
      {
        throw new InvalidOperationException("Transfer the organizer role to another group member, before attempting the removal of the current organizer");
      }

      _memberships.Remove(membership);
    }

    public void TransferOrganizerRole(Guid currentOrganizerId, Guid newOrganizerId)
    {
      var currentOrganizerMembership = FindMembership(currentOrganizerId);
      var newOrganizerMembership = FindMembership(newOrganizerId);

      if (currentOrganizerMembership.Role != GroupRole.Organizer)
      {
        throw new InvalidOperationException("The user with the provided organizer User ID is not the group's organizer");
      }

      if (newOrganizerMembership.Role != GroupRole.Member)
      {
        throw new InvalidOperationException("The user with the provided new organizer User ID is not an ordinary member of the group");
      }

      currentOrganizerMembership.ChangeRole(GroupRole.Member);
      newOrganizerMembership.ChangeRole(GroupRole.Organizer);
    }

    private GroupMembership FindMembership(Guid userId)
    {
      if (userId == Guid.Empty)
      {
        throw new ArgumentException("The User ID for membership lookup cannot be empty", nameof(userId));
      }

      return _memberships.SingleOrDefault(membership => membership.UserId == userId) ?? throw new ArgumentNullException("The provided user is not a member of the group");
    }

    private static string ValidateName(string name)
    {
      ArgumentNullException.ThrowIfNull(name, nameof(name));

      var trimmedName = name.Trim();

      if (trimmedName.Length < 1 || trimmedName.Length > 100)
      {
        throw new ArgumentException("Group name has to be between 1 and 100 characters long.", nameof(name));
      }

      return trimmedName;
    }
  }
}
