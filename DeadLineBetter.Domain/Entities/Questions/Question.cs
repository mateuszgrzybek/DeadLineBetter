namespace DeadLineBetter.Domain.Entities.Questions
{
  public abstract class Question
  {
    public Guid Id { get; private set; }
    public Guid GroupId { get; private set; }
    public Guid CreatorUserId { get; private set; }
    public string QuestionText { get; private set; } = string.Empty;
    public DateTimeOffset DeadlineInUtc { get; private set; }
    public DateTimeOffset CreatedAtUtc { get; private set; }

    private Question() { }

    internal Question(Guid id, Guid groupId, Guid creatorUserId, string questionText, DateTimeOffset deadlineInUtc)
    {
      if (id == Guid.Empty)
      {
        throw new ArgumentException("Question ID cannot be empty", nameof(id));
      }

      if (groupId == Guid.Empty)
      {
        throw new ArgumentException("Group ID cannot be empty.", nameof(groupId));
      }

      if (creatorUserId == Guid.Empty)
      {
        throw new ArgumentException("Creator user ID cannot be empty.", nameof(creatorUserId));
      }

      Id = id;
      GroupId = groupId;
      CreatorUserId = creatorUserId;

      if (string.IsNullOrWhiteSpace(questionText))
      {
        throw new ArgumentException("Question text cannot be empty or whitespace.", nameof(questionText));
      }

      QuestionText = questionText.Trim();

      ValidateAndSetDeadlineAndCreationTimes(deadlineInUtc);
    }

    public void EnsureCanBeEdited(bool hasPredictions)
    {
      if (hasPredictions)
      {
        throw new InvalidOperationException("Questions with predictions cannot be edited.");
      }

      if (DateTimeOffset.UtcNow >= DeadlineInUtc)
      {
        throw new InvalidOperationException("Questions cannot be edited after their deadline.");
      }
    }
    public void EnsureCanBeDeleted(bool hasPredictions)
    {
      if (hasPredictions) throw new InvalidOperationException("Questions with predictions cannot be deleted.");

      if (DateTimeOffset.UtcNow >= DeadlineInUtc) throw new InvalidOperationException("Questions cannot be deleted after their deadline.");
    }

    private void ValidateAndSetDeadlineAndCreationTimes(DateTimeOffset deadlineInUtc)
    {
      var utcNow = DateTimeOffset.UtcNow;

      if (deadlineInUtc <= utcNow)
      {
        throw new ArgumentException("Question deadline must be in the future from now", nameof(deadlineInUtc));
      }

      DeadlineInUtc = deadlineInUtc;
      CreatedAtUtc = utcNow;
    }
  }
}
