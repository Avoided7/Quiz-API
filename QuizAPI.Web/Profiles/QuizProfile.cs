using AutoMapper;
using QuizAPI.Domain.Entities;
using QuizAPI.Web.ViewModels;

namespace QuizAPI.Web.Profiles;

public class QuizProfile : Profile
{
  public QuizProfile()
  {
    CreateMap<CreateOptionVm, Option>();
    CreateMap<CreateQuestionVm, Question>();
    CreateMap<CreateQuizVm, Quiz>();

    CreateMap<Option, OptionVm>();
    CreateMap<Question, QuestionVm>();
    CreateMap<Quiz, QuizVm>();

    CreateMap<MemberAnswerVm, MemberAnswer>();
    CreateMap<PassedQuizVm, PassedQuiz>();
  }
}