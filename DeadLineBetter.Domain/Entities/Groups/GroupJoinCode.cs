namespace DeadLineBetter.Domain.Entities.Groups
{
  public sealed class GroupJoinCode
  {
    public Guid GroupId { get; private set; }
    public string CodeHash { get; private set; } = string.Empty;
    public string EncryptedCode { get; private set; } = string.Empty;
    public DateTimeOffset CreatedAtUtc { get; private set; }

    private GroupJoinCode() { }

    internal GroupJoinCode(Guid groupId, string codeHash, string encryptedCode, DateTimeOffset createdAtUtc)
    {
      if (groupId == Guid.Empty)
      {
        throw new ArgumentException("Group ID cannot be empty", nameof(groupId));
      }

      GroupId = groupId;
      CodeHash = codeHash;
      EncryptedCode = encryptedCode;
      CreatedAtUtc = createdAtUtc;
    }

    internal void UpdateJoinCode(string codeHash, string encryptedCode, DateTimeOffset createdAtUtc)
    {
      ArgumentException.ThrowIfNullOrWhiteSpace(codeHash);
      ArgumentException.ThrowIfNullOrWhiteSpace(encryptedCode);

      if (createdAtUtc.Offset != TimeSpan.Zero)
      {
        throw new ArgumentException("The supplied creation date must be in UTC.", nameof(createdAtUtc));
      }

      CodeHash = codeHash;
      EncryptedCode = encryptedCode;
      CreatedAtUtc = createdAtUtc;
    }
  }
}
