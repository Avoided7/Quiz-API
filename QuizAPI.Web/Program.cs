using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
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

// Identity
builder.Services.RegisterIdentity();

// Repositories
builder.Services.RegisterRepositories();

// Services
builder.Services.RegisterServices();

// Authentication & Authorization
builder.Services
  .AddAuthentication(options =>
  {
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
  }).AddJwtBearer(options =>
  {
    options.SaveToken = true;
    options.RequireHttpsMetadata = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
      ValidateIssuer = true,
      ValidateAudience = true,
      ValidateLifetime = true,
      ValidateIssuerSigningKey = true,
      ValidAudience = builder.Configuration["JwtOptions:Audience"],
      ValidIssuer = builder.Configuration["JwtOptions:Issuer"],
      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtOptions:Secret"]!))
    };
  });

var app = builder.Build();

app.UseRouting();

app.UseCors(options => options.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers()
  .RequireAuthorization();

app.Run();
