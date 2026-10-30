using System.Globalization;

namespace DeadLineBetter.Domain.Entities.Questions
{
  public sealed class NumericQuestion : Question
  {
    public decimal? CorrectAnswer { get; private set; }

    private NumericQuestion(Guid id, Guid groupId, Guid creatorUserId, string questionText, DateTimeOffset deadlineInUtc) : base(id, groupId, creatorUserId, questionText, deadlineInUtc) { }

    public NumericQuestion Create(Guid groupId, Guid creatorUserId, string questionText, DateTimeOffset deadlineInUtc)
    {
      return new NumericQuestion(Guid.NewGuid(), groupId, creatorUserId, questionText, deadlineInUtc);
    }

    public void Resolve(decimal correctAnswer)
    {
      CorrectAnswer = correctAnswer;
    }
  }
}
