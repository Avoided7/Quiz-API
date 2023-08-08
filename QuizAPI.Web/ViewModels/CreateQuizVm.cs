using System.ComponentModel.DataAnnotations;

namespace QuizAPI.Web.ViewModels;

public class CreateQuizVm
{
  [Required] public string Title { get; set; } = null!;

  [Required] public string Description { get; set; } = null!;

  [Required] public List<CreateQuestionVm> Questions { get; set; } = null!;
}

public class CreateQuestionVm
{
  [Required] public string Title { get; set; } = null!;

  [Required] public List<CreateOptionVm> Options { get; set; } = null!;
}

public class CreateOptionVm
{
  [Required] public string Title { get; set; } = null!;
  [Required] public double Value { get; set; }
}