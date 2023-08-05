using QuizAPI.Infrastructure.Extensions;
using QuizAPI.Services.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// CORS
builder.Services.AddCors();

// Mapper
builder.Services.AddAutoMapper(typeof(Program).Assembly);

// DbContext
string connectionString = builder.Configuration.GetConnectionString("QuizContext")!;
builder.Services.RegisterDbContext(connectionString);

// Repositories
builder.Services.RegisterRepositories();

// Services
builder.Services.RegisterServices();

var app = builder.Build();

app.UseCors(options => options.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
