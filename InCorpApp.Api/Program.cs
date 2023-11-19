using InCorpApp.Application;
using InCorpApp.Infrastructure;
using Microsoft.OpenApi.Models;
using Serilog.Events;
using Serilog.Formatting.Json;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Reflection;
using InCorpApp.Contracts.Common;
using InCorpApp.Api.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);

builder.Services.AddControllers().AddNewtonsoftJson();
                    //;
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

string xmlCommentFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
string xmlCommentFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentFile);
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "InCorp.Api", Version = "v1" });
    c.IncludeXmlComments(xmlCommentFullPath, true);
    var securitySchema = new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        Reference = new OpenApiReference
        {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
        }
    };
    c.AddSecurityDefinition("Bearer", securitySchema);
    var securityRequirement = new OpenApiSecurityRequirement();
    securityRequirement.Add(securitySchema, new[] { "Bearer" });
    c.AddSecurityRequirement(securityRequirement);
});
builder.Services.AddSwaggerGenNewtonsoftSupport();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy =>
                        policy.RequireClaim("Role", "Admin"));
    options.AddPolicy("RecruiterPolicy", policy =>
                        policy.RequireClaim("Role", "Recruiter"));
    options.AddPolicy("ApplicantPolicy", policy =>
                        policy.RequireClaim("Role", "Applicant"));
});

builder.Services.AddAInfrastructure(builder.Configuration)
                .AddApplication();
builder.Services.AddCors(options => options.AddDefaultPolicy(x => x.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

var logger = new LoggerConfiguration()
                    .WriteTo.Console()
                    .WriteTo.File(new JsonFormatter(), "important-Logs.json", restrictedToMinimumLevel: LogEventLevel.Warning)
                    .MinimumLevel.Information()
                    .CreateLogger();

builder.Services.AddLogging(loggingBuilder =>
{
    loggingBuilder.ClearProviders();
    loggingBuilder.AddSerilog(logger, dispose: true);
});
logger.Information($"Starting Application at ==> {new DateTimeProvider().CurrentDateTime()}");

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(
    c => {
        c.SwaggerEndpoint("../swagger/v1/swagger.json", "InCorp.Api");
        c.DocExpansion(DocExpansion.List);
    });


app.UseHttpsRedirection();
app.UseExceptionHandler(
    new ExceptionHandlerOptions()
    {
        ExceptionHandlingPath = "/error"
    }); 
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
