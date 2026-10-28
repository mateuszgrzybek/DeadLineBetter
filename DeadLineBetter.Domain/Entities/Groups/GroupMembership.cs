using DeadLineBetter.Domain.Enums;

namespace DeadLineBetter.Domain.Entities.Groups
{
  public sealed class GroupMembership
  {
    public Guid GroupId { get; private set; }
    public Guid UserId { get; private set; }
    public GroupRole Role { get; private set; }

    private GroupMembership() { }

    internal GroupMembership(Guid groupId, Guid userId, GroupRole role)
    {
      if (groupId == Guid.Empty)
      {
        throw new ArgumentException("Group ID cannot be empty", nameof(groupId));   
      }

      if (userId == Guid.Empty)
      {
        throw new ArgumentException("User ID cannot be empty", nameof(userId));
      }

      if (!Enum.IsDefined(role))
      {
        throw new ArgumentOutOfRangeException(nameof(role), role, "Invalid group role was provided");
      }

      GroupId = groupId;
      UserId = userId;
      Role = role;
    }

    internal void ChangeRole(GroupRole role)
    { 
      if (!Enum.IsDefined(role))
      {
        throw new ArgumentOutOfRangeException(nameof(role), role, "Invalid group role was provided");
      }

      Role = role;
    }
  }
}