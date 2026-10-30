namespace DeadLineBetter.Domain.Entities.Questions
{
  public sealed class AnswerOption
  {
    public Guid Id { get; private set; }
    public Guid QuestionId { get; private set; }
    public string Text { get; private set; } = string.Empty;

    private AnswerOption() { }

    internal AnswerOption(Guid id, Guid questionId, string text)
    {
      if (id == Guid.Empty)
      {
        throw new ArgumentException("Answer option ID cannot be empty.", nameof(id));
      }

      if (questionId == Guid.Empty)
      {
        throw new ArgumentException("Question ID cannot be empty.", nameof(questionId));
      }

      if (string.IsNullOrWhiteSpace(text))
      {
        throw new ArgumentException("Answer option text cannot be empty or white-space only.", nameof(text));
      }

      Id = id;
      QuestionId = questionId;
      Text = text.Trim();
    }

    internal void ChangeText(string newText)
    {
      if (string.IsNullOrWhiteSpace(newText))
      {
        throw new ArgumentException("The new answer option text cannot be empty or white-space only.")
      }

      Text = newText.Trim();
    }
  }
}
