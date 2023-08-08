namespace QuizAPI.Web.ViewModels;

public class QuizVm
{
  public Guid Id { get; set; }
  
  public string Title { get; set; } = string.Empty;
  public string Description { get; set; } = string.Empty;

  public List<QuestionVm> Questions { get; set; } = null!;
}

public class QuestionVm
{
  public Guid Id { get; set; }

  public string Title { get; set; } = string.Empty;

  public List<OptionVm> Options { get; set; } = null!;
}

public class OptionVm
{
  public Guid Id { get; set; }

  public string Title { get; set; } = string.Empty;
}