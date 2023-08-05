using AutoMapper;
using QuizAPI.Domain.Entities;
using QuizAPI.Web.ViewModels;

namespace QuizAPI.Web.Profiles;

public class QuizProfile : Profile
{
  public QuizProfile()
  {
    CreateMap<OptionVm, Option>();
    CreateMap<QuestionVm, Question>();
    CreateMap<CreateQuizVm, Quiz>();
  }
}