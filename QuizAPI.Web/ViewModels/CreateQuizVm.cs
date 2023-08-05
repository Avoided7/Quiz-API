using System.ComponentModel.DataAnnotations;

namespace QuizAPI.Web.ViewModels;

public class CreateQuizVm
{
  [Required] public string Title { get; set; } = null!;

  [Required] public string Description { get; set; } = null!;

  [Required] public List<QuestionVm> Questions { get; set; } = null!;
}

public class QuestionVm
{
  [Required] public string Title { get; set; } = null!;

  [Required] public List<OptionVm> Options { get; set; } = null!;
}

public class OptionVm
{
  [Required] public string Title { get; set; } = null!;
  [Required] public double Value { get; set; }
}