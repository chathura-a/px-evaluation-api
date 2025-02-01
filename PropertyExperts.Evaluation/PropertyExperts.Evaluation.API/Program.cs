using Polly;
using Polly.Retry;
using PropertyExperts.Evaluation.API.ExceptionHandlers;
using PropertyExperts.Evaluation.API.ExternalServices.Classification;
using PropertyExperts.Evaluation.API.Services;
using PropertyExperts.Evaluation.API.Utils.RuleEngine;
using RestSharp;

var builder = WebApplication.CreateBuilder(args);

var retryStrategy = new RetryStrategyOptions
{
    MaxRetryAttempts = 3,
    Delay = TimeSpan.FromSeconds(2),
    BackoffType = DelayBackoffType.Exponential,
    ShouldHandle = new PredicateBuilder().Handle<HttpRequestException>()
};

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IEvaluationService, EvaluationService>();
builder.Services.AddScoped<IClassificationService, ClassificationService>();
builder.Services.AddSingleton<IRuleEngine, RuleEngine>();
builder.Services.AddResiliencePipeline("default", builder => builder.AddRetry(retryStrategy));
builder.Services.AddHttpClient(); 
builder.Services.AddScoped<IRestClient, RestClient>(sp => // Register RestSharp RestClient using the HttpClient
{
    var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
    var httpClient = httpClientFactory.CreateClient(); // Reuse this HttpClient instance
    return new RestClient(httpClient);
});
builder.Services.AddControllers();
builder.Services.AddExceptionHandler<AppExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.MapControllers();

app.Run();

