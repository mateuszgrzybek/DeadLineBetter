namespace DeadLineBetter.Domain.Entities.Questions
{
  public sealed class MultipleChoiceQuestion : Question
  {
    public IReadOnlyCollection<AnswerOption> AnswerOptions => _answerOptions.AsReadOnly();
    private readonly List<AnswerOption> _answerOptions = [];

    private MultipleChoiceQuestion(Guid id, Guid groupId, Guid creatorUserId, string questionText, DateTimeOffset deadlineInUtc) : base(id, groupId, creatorUserId, questionText, deadlineInUtc) { }

    public MultipleChoiceQuestion Create(Guid groupId, Guid creatorUserId, string questionText, DateTimeOffset deadlineInUtc, IEnumerable<string> answerOptions)
    {
      ArgumentNullException.ThrowIfNull(answerOptions);

      var options = answerOptions.ToList();

      EnsureAnswerOptionCountAndUniqueness(options);

      var questionId = Guid.NewGuid();

      var mutlipleChoiceQuestion = new MultipleChoiceQuestion(questionId, groupId, creatorUserId, questionText, deadlineInUtc);

      foreach (var answerOption in answerOptions)
      {
        mutlipleChoiceQuestion._answerOptions.Add(new AnswerOption(Guid.NewGuid(), questionId, answerOption));
      }

      return mutlipleChoiceQuestion;
    }

    public void ChangeAnswerOption(Guid answerOptionId, string newAnswerText, bool hasPredictions)
    {
      EnsureAnswerOptionsCanBeModified(hasPredictions);

      if (answerOptionId == Guid.Empty)
      {
        throw new ArgumentException("The ID of the answer option to edit cannot be empty", nameof(answerOptionId));
      }

      ValidateAnswerOptionText(newAnswerText);

      var matchingOption = _answerOptions.FirstOrDefault(answerOption => answerOption.Id == answerOptionId) ?? throw new InvalidOperationException("An answer option with the provided ID has not been found");

      matchingOption.ChangeText(newAnswerText);
    }

    private void EnsureAnswerOptionsCanBeModified(bool hasPredictions)
    {
      if (hasPredictions) throw new InvalidOperationException("Answer options cannot be modified if predictions were already submitted");

      if (DateTimeOffset.UtcNow >= DeadlineInUtc) throw new InvalidOperationException("Answer options cannot be modified after the prediction submission deadline has passed");
    }

    private static void EnsureAnswerOptionCountAndUniqueness(IReadOnlyCollection<string> answerOptions)
    {
      if (answerOptions.Count < 2) //TODO: Move to consts
      {
        throw new ArgumentException("A multiple-choice question requires at least 2 answer options to be provided", nameof(answerOptions));
      }

      foreach (var answerOption in answerOptions)
      {
        ValidateAnswerOptionText(answerOption);
      }

      var distinctAnswerOptionsCount = answerOptions.Select(answerOption => answerOption.Trim()).Distinct(StringComparer.OrdinalIgnoreCase).Count();

      if (distinctAnswerOptionsCount != answerOptions.Count)
      {
        throw new ArgumentException("Each provided answer option must be unique", nameof(answerOptions));
      }
    }

    private static void ValidateAnswerOptionText(string answerOptionText)
    { 
      if (string.IsNullOrWhiteSpace(answerOptionText))
      {
        throw new ArgumentException("Answer option cannot be empty or white-space only", nameof(answerOptionText));
      }
    }
  }
}
